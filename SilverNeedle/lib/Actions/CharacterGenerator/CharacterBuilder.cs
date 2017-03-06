﻿//-----------------------------------------------------------------------
// <copyright file="CharacterGenerator.cs" company="Short Leg Studio, LLC">
//     Copyright (c) Short Leg Studio, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SilverNeedle.Actions.CharacterGenerator
{
    // TODO: This class design is kind of all over the place. Is it trying to do everything or is it driven by an outside source?
    using System.Linq;
    using SilverNeedle;
    using SilverNeedle.Actions.CharacterGenerator.Abilities;
    using SilverNeedle.Actions.CharacterGenerator.Appearance;
    using SilverNeedle.Actions.CharacterGenerator.Background;
    using SilverNeedle.Actions.NamingThings;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.Background;
    using SilverNeedle.Equipment;
    
    /// <summary>
    /// Character generator generates a new character. By specifying different generators different character
    /// creation schemes are possible
    /// </summary>
    public class CharacterBuilder
    {
        /// <summary>
        /// The ability generator handles the ability score creation
        /// </summary>
        private IAbilityScoreGenerator abilityGenerator;

        /// <summary>
        /// The language selector selects what languages the character can speak
        /// </summary>
        private LanguageSelector languageSelector;

        /// <summary>
        /// The race selector chooses which race the character will be
        /// </summary>
        private RaceSelector raceAssigner;

        /// <summary>
        /// The name generator selects the name for the character
        /// </summary>
        private INameCharacter nameGenerator;

        private GatewayProvider gateways;

        private FeatSelector featSelector;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SilverNeedle.Actions.CharacterGenerator.CharacterBuilder"/> class.
        /// </summary>
        /// <param name="abilities">Ability score generator to use.</param>
        /// <param name="langs">Language selector to use.</param>
        /// <param name="races">Race selector to use.</param>
        /// <param name="names">Name selector to use.</param>
        public CharacterBuilder(
            IAbilityScoreGenerator abilities,
            LanguageSelector langs,
            RaceSelector races,
            INameCharacter names,
            FeatSelector feats,
            GatewayProvider gateways)
        {
            this.abilityGenerator = abilities;
            this.languageSelector = langs;
            this.raceAssigner = races;
            this.nameGenerator = names;
            this.gateways = gateways;
            this.featSelector = feats;
        }

        /// <summary>
        /// Generates the random character.
        /// </summary>
        /// <returns>The random character.</returns>
        public CharacterSheet GenerateRandomCharacter()
        {
            return GenerateCharacter(new CharacterBuildStrategy(), 1);
        }

        public CharacterSheet GenerateCharacter(CharacterBuildStrategy strategy, int level)
        {
            // Create Character
            var character = new CharacterSheet(this.gateways.Skills.All());
            character.FeatTokens.Add(new FeatToken());
            
            // Assign Basic Attributes
            this.AssignAttributes(character, strategy);            
            this.ChooseRace(character, strategy);
            this.CreateDescription(character);
            this.CreateName(character);
            this.ChooseLanguages(character);
            this.GenerateHistory(character);
            this.CreatePersonality(character, strategy);
            

            // Choose the class
            this.SelectClass(character, strategy);

            // Select feats
            this.AssignAbilityPoints(character, strategy);
            this.ChooseFeats(character, strategy);
            this.AddHitPoints(character);
            this.CalculateAge(character);
            this.GenerateBackground(character);
            this.AssignSkillPoints(character, strategy);
            this.EquipWeapons(character);
            this.EquipArmor(character);

            for(int i = 1; i < level; i++)
            {
                this.LevelUp(character, strategy);
                this.AssignAbilityPoints(character, strategy);
                this.AssignSkillPoints(character, strategy);
                this.ChooseFeats(character, strategy);
            }
            return character;
        }

        private void AssignSkillPoints(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            ShortLog.Debug("Assigning Skill Points");
            // Assign Skill Points
            var skillGen = new SkillPointDistributor();
            skillGen.AssignSkillPoints(character.SkillRanks, strategy.FavoredSkills, character.GetSkillPointsPerLevel(), character.Level);
        }

        /// <summary>
        /// Creates a Level 0 character. A level 0 character has no class but has the basic attributes selected
        /// Think of this as a young character before identifying their professions
        /// </summary>
        /// <returns>The level0.</returns>
        private CharacterSheet AssignAttributes(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            ShortLog.Debug("Assigning Attributes");
            character.Gender = EnumHelpers.ChooseOne<Gender>();
            character.Alignment = EnumHelpers.ChooseOne<CharacterAlignment>();
            this.abilityGenerator.StrategyScores(character.AbilityScores, strategy.FavoredAbilities);
            
            return character;
        }

        private void GenerateHistory(CharacterSheet character)
        {
            ShortLog.Debug("Creating History");
            
            var history = new History();

            //Homeland
            var homelandSelector = new HomelandSelector(new HomelandGateway());
            history.Homeland = homelandSelector.SelectHomelandByRace(character.Race.Name);

            // Family
            var familyHistory = new FamilyHistoryCreator(this.nameGenerator);
            history.FamilyTree = familyHistory.CreateFamilyTree(character.Race.Name);

            // Drawback
            var drawback = new CharacterDrawbackSelector(new DrawbackGateway());
            history.Drawback = drawback.SelectDrawback();
            character.History = history;            
        }

        /// <summary>
        /// Selects the class for the character
        /// </summary>
        /// <returns>The class that was selected.</returns>
        /// <param name="character">Character to assign class to.</param>
        private void SelectClass(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            ShortLog.Debug("Selecting Class");
            
            var classSelector = new ClassSelector(gateways.Classes);
            classSelector.ChooseClass(character, strategy.Classes);
        }

        private void AddHitPoints(CharacterSheet character) 
        {
            ShortLog.Debug("Adding Hit Points");
            
            var hp = new HitPointRoller();
            hp.AddMaxHitPoints(character);
        }

        private void CalculateAge(CharacterSheet character)
        {
            ShortLog.Debug("Calculating Age");
            
            // Assign Age based on class
            var assignAge = new AssignAge();
            assignAge.RandomAge(character.Class.ClassDevelopmentAge, gateways.Maturity.Get(character.Race));
        }

        private void GenerateBackground(CharacterSheet character)
        {
            ShortLog.Debug("Generating Background");
            
            // Figure out how this class came about
            var classOrigin = new ClassOriginStoryCreator(new ClassOriginGateway());
            character.History.ClassOriginStory = classOrigin.CreateStory(character.Class.Name);
        }

        private void ChooseLanguages(CharacterSheet character)
        {
            ShortLog.Debug("Choosing Languages");
            
            character.Languages.Add(
                this.languageSelector.PickLanguages(
                    character.Race, 
                    character.AbilityScores.GetModifier(AbilityScoreTypes.Intelligence)));
        }

        private void ChooseFeats(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            ShortLog.Debug("Choosing Feats");
            
            featSelector.SelectFeats(character, strategy.FavoredFeats);
        }

        private void EquipWeapons(CharacterSheet character)
        {
            ShortLog.Debug("Equiping Weapons");
            
            // Get some gear!
            var equip = new EquipMeleeAndRangedWeapon(this.gateways.Weapons);
            equip.AssignWeapons(character.Inventory, character.Offense.WeaponProficiencies);
        }

        private void EquipArmor(CharacterSheet character)
        {
            ShortLog.Debug("Equiping Armor");
            
            var equipArmor = new PurchaseInitialArmor(this.gateways.Armors);
            equipArmor.PurchaseArmorAndShield(character.Inventory, character.Defense.ArmorProficiencies);
        }

        private void ChooseRace(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            ShortLog.Debug("Choosing Race");
            
            this.raceAssigner.ChooseRace(character, strategy.Races);
            
            // Assign Age to adult
            character.Age = this.gateways.Maturity.Get(character.Race).Adulthood;
        }

        private void CreateDescription(CharacterSheet character)
        {
            ShortLog.Debug("Creating Description");
            
            //Generate a facial description
            var facials = new CreateFacialFeatures();
            character.FacialDescription = facials.CreateFace(character.Gender);
        }

        private void CreateName(CharacterSheet character)
        {
            ShortLog.Debug("Creating Name");
            
            // Names come last
            character.Name = this.nameGenerator.CreateFullName(character.Gender, character.Race.Name);
        }

        private void LevelUp(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            ShortLog.Debug("Leveling Up");
            
            var levelUp = new LevelUpCharacter(new HitPointRoller());
            levelUp.LevelUp(character);
        }

        private void AssignAbilityPoints(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            ShortLog.Debug("Assigning Ability Points");
            
            var abilityAssigner = new AbilityPointAssigner();
            abilityAssigner.AssignByStrategy(character, strategy.FavoredAbilities);
        }

        private void CreatePersonality(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            ShortLog.Debug("Creating Personality");
            
            var builder = new PersonalityBuilder();
            builder.Random(character);
        }
    }
}