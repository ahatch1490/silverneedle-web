﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using SilverNeedle;
    using SilverNeedle.Actions.CharacterGenerator;
    using SilverNeedle.Characters;
    using SilverNeedle.Dice;
    using SilverNeedle.Serialization;
    using SilverNeedle.Utility;

    [TestFixture]
    public class RaceSelectorTests
    {
        private EntityGateway<Race> raceGateway;
        private EntityGateway<Trait> traitGateway;

        private Race elf;

        private Race human;
        private Trait elfTrait;
        private RaceSelector raceSelectorSubject;

        [SetUp]
        public void SetUp()
        {
            // Create a race
            elf = new Race();
            elf.Name = "Elfy";
            elf.Traits.Add("Elfy");
            elf.SizeSetting = CharacterSize.Medium;
            elf.HeightRange = DiceStrings.ParseDice("10d6");
            elf.WeightRange = DiceStrings.ParseDice("20d8");
            elf.KnownLanguages.Add("Common");
            elf.KnownLanguages.Add("Elvish");
            elf.AvailableLanguages.Add("Draconic");
            elf.AvailableLanguages.Add("Celestial");

            human = new Race();
            human.Name = "Human";
            human.SizeSetting = CharacterSize.Medium;
            human.HeightRange = DiceStrings.ParseDice("2d8+30");
            human.WeightRange = DiceStrings.ParseDice("3d6+100");
            human.KnownLanguages.Add("Common");


            var list = new List<Race>();
            list.Add(elf);
            list.Add(human);

            //Set up the trait
            elfTrait = new Trait();
            elfTrait.Name = "Elfy";
            var traitList = new List<Trait>();
            traitList.Add(elfTrait);

            // Configure Gateways
            raceGateway = new EntityGateway<Race>(list);
            traitGateway = new EntityGateway<Trait>(traitList);
            
            raceSelectorSubject = new RaceSelector(raceGateway, traitGateway);
        }

        [Test]
        public void SettingRaceLoadsTraits()
        {
            var sheet = new CharacterSheet();

            raceSelectorSubject.SetRace(sheet, elf);
            Assert.AreEqual(elf, sheet.Race);
            Assert.IsTrue(sheet.Traits.Any(x => x == elfTrait));
        }

        [Test]
        public void SettingRaceCalculatesSize()
        {
            var sheet = new CharacterSheet();

            var smallGuy = new Race();
            smallGuy.SizeSetting = CharacterSize.Small;
            smallGuy.HeightRange = DiceStrings.ParseDice("2d4+10");
            smallGuy.WeightRange = DiceStrings.ParseDice("2d4+100");

            var assign = new RaceSelector(raceGateway, traitGateway);
            raceSelectorSubject.SetRace(sheet, smallGuy);
            Assert.AreEqual(CharacterSize.Small, sheet.Size.Size);
            Assert.GreaterOrEqual(sheet.Size.Height, 12);
            Assert.GreaterOrEqual(sheet.Size.Weight, 102);
        }


        [Test]
        public void SettingRaceAssignsMovement()
        {
            var sheet = new CharacterSheet();
            var fastGuy = new Race();
            fastGuy.SizeSetting = CharacterSize.Small;
            fastGuy.HeightRange = DiceStrings.ParseDice("2d4+10");
            fastGuy.WeightRange = DiceStrings.ParseDice("2d4+100");
            fastGuy.BaseMovementSpeed = 45;

            raceSelectorSubject.SetRace(sheet, fastGuy);
            Assert.AreEqual(45, sheet.Movement.BaseMovement.TotalValue);
        }

        [Test]
        public void OptionTableLimitsSelectionOfRace()
        {
            var sheet = new CharacterSheet();
            var options = new WeightedOptionTable<string>();
            var strategy = new CharacterBuildStrategy();
            strategy.Races.AddEntry("Human", 12);

            //Run it 1000 times, should always be human
            for (int x = 0; x < 1000; x++)
            {
                raceSelectorSubject.Process(sheet, strategy);
                Assert.AreEqual(human, sheet.Race);
            }
        }

        [Test]
        public void IfChoiceListIsEmptyChooseAnyRace() 
        {	
            var sheet = new CharacterSheet();
            var strategy = new CharacterBuildStrategy();

            raceSelectorSubject.Process(sheet, strategy);
            Assert.IsNotNull(sheet.Race);
        }

        [Test]
        public void AddLanguagesKnownToStrategy()
        {
            var character = new CharacterSheet();
            var strategy = new CharacterBuildStrategy();
            strategy.Races.AddEntry("Elfy", 1000);
            raceSelectorSubject.Process(character, strategy);
            Assert.That(character.Race.Name, Is.EqualTo("Elfy"));
            Assert.That(strategy.LanguagesKnown, Is.EquivalentTo(new string[] {"Common", "Elvish"}));
            Assert.That(strategy.LanguageChoices, Is.EquivalentTo(new string[] {"Draconic", "Celestial"}));
        }
    }

    class TestTraitGateway : IEntityGateway<Trait>
    {
        public List<Trait> Traits = new List<Trait>();

        public IEnumerable<Trait> All()
        {
            return Traits;
        }
    }
}
