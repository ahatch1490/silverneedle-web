// Copyright (c) 2016 Trevor Redfern
// 
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

namespace Characters
{
    using System.Linq;
    using NUnit.Framework;
    using SilverNeedle.Characters;
    using SilverNeedle.Serialization;
    using SilverNeedle.Utility;

    [TestFixture]
    public class CharacterBuildStrategyTests
    {
        EntityGateway<CharacterBuildStrategy> strategies;

        [SetUp]
        public void SetUp()
        {
            var list = CharacterBuildYaml.ParseYaml().Load<CharacterBuildStrategy>();
            strategies = new EntityGateway<CharacterBuildStrategy>(list);
        }

        [Test]
        public void DefaultTargetLevelIsOne()
        {
            var strat = new CharacterBuildStrategy();
            Assert.AreEqual(1, strat.TargetLevel);
        }

        [Test]
        public void LoadsWeightedTablesForRacesAndClasses()
        {
            var archer = strategies.Find("Archer");
            Assert.AreEqual(10, archer.Races.All.First().MaximumValue);
            Assert.AreEqual("elf", archer.Races.All.First().Option);
            Assert.AreEqual(10, archer.Classes.All.First().MaximumValue);
            Assert.AreEqual("Fighter", archer.Classes.All.First().Option);
        }  
        
        [Test]
        public void StrategyProvidesGuidanceOnFavoringClassSkills()
        {
            var tank = strategies.Find("tank");
            Assert.AreEqual(3.2f, tank.ClassSkillMultiplier);
        }

        [Test]
        public void IgnoreCaseMatching()
        {
            var tank = strategies.Find("tank");
            Assert.IsNotNull(tank);
        }

        [Test]
        public void StrategyProvidesBaseForAllSkills()
        {
            var archer = strategies.Find("archer");
            Assert.AreEqual(1, archer.BaseSkillWeight);
        }

        [Test]
        public void StrategyProvidesSpecificationOnSkills()
        {
            var archer = strategies.Find("archer");
            Assert.IsNotNull(archer.FavoredSkills);
            Assert.AreEqual("survival", archer.FavoredSkills.All.First().Option);
            Assert.AreEqual(20, archer.FavoredSkills.All.First().MaximumValue);
        }

        [Test]
        public void StrategyProvidesRecommendationsOnFeats()
        {
            var tank = strategies.Find("tank");
            var feats = tank.FavoredFeats.All;
            
            Assert.AreEqual("power attack", feats.First().Option);
            Assert.AreEqual(100, feats.First().MaximumValue);

            Assert.AreEqual(3, feats.Count());
        }

        [Test]
        public void StrategyFavorsSomeAttributesAheadOfOthers() 
        {
            var tank = strategies.Find("tank");
            var abilities = tank.FavoredAbilities.All;
            Assert.AreEqual(AbilityScoreTypes.Strength, abilities.First().Option);
            Assert.AreEqual(100, abilities.First().MaximumValue);
            Assert.AreEqual(6, abilities.Count());
        }

        [Test]
        public void StrategyProvidesDefaultsToAbilitiesNotSpecifiedOfOne()
        {
            var archer = strategies.Find("archer");
            var abilities = archer.FavoredAbilities.All;
            Assert.AreEqual(AbilityScoreTypes.Strength, abilities.First().Option);
            Assert.AreEqual(AbilityScoreTypes.Charisma, abilities.Last().Option);
            Assert.AreEqual(1, abilities.First().MaximumValue);
            Assert.AreEqual(6, abilities.Last().MaximumValue);
            Assert.AreEqual(6, abilities.Count());
        }

        [Test]
        public void AnEmptyStrategySelectsAllAttributesEvenly()
        {
            var strategy = new CharacterBuildStrategy();
            Assert.AreEqual(6, strategy.FavoredAbilities.All.Count());
        }

        [Test]
        public void SpecifiesTheDesignerToUseCreatingCharacter()
        {
            var archer = strategies.Find("archer");
            Assert.AreEqual("create-default", archer.Designer);
        }

        [Test]
        public void DefaultAbilityScoreGeneratorToAverageAbilities()
        {
            var strategy = new CharacterBuildStrategy();
            Assert.That(strategy.AbilityScoreRoller, Contains.Substring("AverageAbilityScore"));
        }

        [Test]
        public void CanSpecifyDifferentAbilityScoreGeneratorsInStrategy()
        {
            var archer = strategies.Find("archer");
            Assert.That(archer.AbilityScoreRoller, Contains.Substring("HeroicAbilityScore"));
        }

        private const string CharacterBuildYaml = @"--- 
- build:
  name: Archer
  ability-score-roller: SilverNeedle.Actions.CharacterGenerator.Abilities.HeroicAbilityScoreGenerator
  races:
    - name: elf
      weight: 10
    - name: human
      weight: 12
  classes:
    - name: Fighter
      weight: 10
    - name: Ranger
      weight: 15
    - name: Rogue
      weight: 5
  classskillmultiplier: 2
  baseskillweight: 1
  skills:
    - name: survival
      weight: 20
    - name: climb
      weight: 16
    - name: perception
      weight: 16    
  feats:
    - name: point-blank shot
      weight: 20
    - name: quick draw
      weight: 10
  designer: create-default
- build:
  name: Tank
  races:
    - name: halfling
      weight: 12
    - name: half-orc
      weight: 10
  classes:
    - name: Fighter
      weight: 10
    - name: Paladin
      weight: 8
    - name: Ranger
      weight: 6
    - name: Monk
      weight: 6
  classskillmultiplier: 3.2
  baseskillweight: 1
  skills:
    - name: survival
      weight: 20
    - name: climb
      weight: 16
    - name: perception
      weight: 16    
  feats:
    - name: power attack
      weight: 100
    - name: cleave
      weight: 40
    - name: shield focus
      weight: 10
  abilities:
    - name: strength
      weight: 100
    - name: dexterity
      weight: 50
  designer: equip-adventurer
";
    }
}