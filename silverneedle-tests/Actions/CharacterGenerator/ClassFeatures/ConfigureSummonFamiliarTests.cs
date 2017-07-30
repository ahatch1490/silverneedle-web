// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGenerator.ClassFeatures
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using SilverNeedle;
    using SilverNeedle.Actions.CharacterGenerator.ClassFeatures;
    using SilverNeedle.Bestiary;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.SpecialAbilities;
    using SilverNeedle.Serialization;

    
    public class ConfigureSummonFamiliarTests
    {
        Familiar bat;
        ConfigureSummonFamiliar subject;
        public ConfigureSummonFamiliarTests()
        {
            var familiars = new List<Familiar>();
            bat = new Familiar("Bat");
            familiars.Add(bat);
            subject = new ConfigureSummonFamiliar(new EntityGateway<Familiar>(familiars));
        }

        [Fact]
        public void ChoosesAFamiliarForTheCharacter()
        {
            var character = new CharacterSheet();
            character.InitializeComponents();
            subject.Process(character, new CharacterBuildStrategy());

            var summon = character.SpecialQualities.SpecialAbilities.First() as SummonFamiliar;
            Assert.Equal(summon.Familiar, bat);
        }

        [Fact]
        public void SummonFamiliarModifiesTheCharacterStats()
        {
            bat.Modifiers.Add(new ValueStatModifier("Perception", 5, "bonus", "Familiar (Bat)"));
            var character = new CharacterSheet(new Skill[] { new Skill("Perception", AbilityScoreTypes.Wisdom, false) });
            character.InitializeComponents();
            var baseValue = character.GetSkillValue("Perception");
            subject.Process(character, new CharacterBuildStrategy());
            Assert.Equal(character.GetSkillValue("Perception"), baseValue + 5);
            
        }
    }
}