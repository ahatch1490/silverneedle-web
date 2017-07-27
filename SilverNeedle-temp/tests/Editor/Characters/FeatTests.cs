﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters {
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.Prerequisites;
    using SilverNeedle.Utility;
    
    [TestFixture]
    public class FeatTests {
        Feat Acrobatic;
        Feat CombatExpertise;
        Feat PowerAttack;
        Feat CraftWand;


        [SetUp]
        public void SetUp() {
            var data = FeatYamlFile.ParseYaml();
            var list = new List<Feat>();
            foreach(var c in data.Children)
            {
                list.Add(new Feat(c));
            }

            Acrobatic = list.First(x => x.Name == "Acrobatic");
            CombatExpertise = list.First(x => x.Name == "Combat Expertise");
            PowerAttack = list.First(x => x.Name == "Power Attack");
            CraftWand = list.First(x => x.Name == "Craft Wand");
        }
            

        [Fact]
        public void FeatsKnowWhetherYouQualify() {
            var smartCharacter = new CharacterSheet (new List<Skill>());
            smartCharacter.AbilityScores.SetScore (AbilityScoreTypes.Intelligence, 15);
            var dumbCharacter = new CharacterSheet (new List<Skill>());
            dumbCharacter.AbilityScores.SetScore (AbilityScoreTypes.Intelligence, 5);

            var CombatExpertise = new Feat();
            CombatExpertise.Prerequisites.Add(new AbilityPrerequisite(AbilityScoreTypes.Intelligence, 13));

            Assert.IsTrue (CombatExpertise.IsQualified (smartCharacter));
            Assert.IsFalse (CombatExpertise.IsQualified (dumbCharacter));
        }		

        [Fact]
        public void IfFeatIsAlreadySelectedItCannotBeSelectedAgain()
        {
            var character = new CharacterSheet (new List<Skill>());
            var basicFeat = new Feat();
            character.Add(basicFeat);
            Assert.IsFalse(basicFeat.IsQualified(character));
        }

        [Fact]
        public void MatchesByName()
        {
            Assert.IsTrue(Acrobatic.Matches("Acrobatic"));
            Assert.IsTrue(Acrobatic.Matches("acrobatic"));
        }
        
        [Fact]
        public void FeatsHaveADescription() {
            Assert.AreEqual ("Move good", Acrobatic.Description);
            Assert.AreEqual ("Hit Stuff Hard", PowerAttack.Description);
        }

        [Fact]
        public void FeatsCanHaveSkillAdjustments() {
            var modifiers = Acrobatic.Modifiers;
            Assert.AreEqual (2, modifiers.Count);
            var skillAdj = modifiers.First ();
            Assert.AreEqual ("Acrobatics", skillAdj.StatisticName);
            Assert.AreEqual("bonus", skillAdj.Type);
            Assert.AreEqual ("Acrobatic (feat)", skillAdj.Reason);
            Assert.AreEqual (2, skillAdj.Modifier);

            var flyAdj = modifiers.Last ();
            Assert.AreEqual ("Fly", flyAdj.StatisticName);
            Assert.AreEqual("bonus", skillAdj.Type);
            Assert.AreEqual ("Acrobatic (feat)", skillAdj.Reason);
            Assert.AreEqual (4, flyAdj.Modifier);
        }

        [Fact]
        public void FeatsCanHaveAbilityPrerequisites() {
            var prereq = CombatExpertise.Prerequisites;
            var abilityCheck = prereq.First() as AbilityPrerequisite;
                  Assert.IsInstanceOf<AbilityPrerequisite> (abilityCheck);
            Assert.AreEqual (AbilityScoreTypes.Intelligence, abilityCheck.Ability);
            Assert.AreEqual (13, abilityCheck.Minimum);
        }

        [Fact]
        public void SomeFeatsAreCombatFeats() {
            Assert.IsTrue (CombatExpertise.IsCombatFeat);
            Assert.IsFalse (Acrobatic.IsCombatFeat);
        }

        [Fact]
        public void SomeFeatsAreCriticalFeats() {
            Assert.IsTrue (PowerAttack.IsCriticalFeat);
            Assert.IsFalse (Acrobatic.IsCriticalFeat);
        }

        [Fact]
        public void SomeFeatsAreItemCreationFeats() {
            Assert.IsTrue (CraftWand.IsItemCreation);
            Assert.IsFalse (Acrobatic.IsItemCreation);
        }

        private const string FeatYamlFile = @"--- 
- feat: 
  name: Acrobatic
  description: Move good
  modifiers:
    - type: bonus
      stat: Acrobatics
      modifier: 2
    - type: bonus
      stat: Fly
      modifier: 4
- feat:
  name: Combat Expertise
  description: Dodge stuff better
  tags: combat
  prerequisites:
    - intelligence: 13
- feat:
  name: Power Attack
  description: Hit Stuff Hard
  tags: combat, critical
- feat:
  name: Craft Wand
  tags: itemcreation
  description: Make Wands
...";
    }
}
