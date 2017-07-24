// Copyright (c) 2016 Trevor Redfern
// 
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

namespace Tests.Actions
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using SilverNeedle.Actions.CharacterGenerator;
    using SilverNeedle.Characters;
    using SilverNeedle.Dice;
    using SilverNeedle.Serialization;
    using SilverNeedle.Utility;

    [TestFixture]
    public class ClassSelectorTests
    {
        public EntityGateway<Class> classGateway;

        public ClassSelector subject;

        [SetUp]
        public void SetUp()
        {
            var classes = new List<Class>();
            var hero = new Class();
            hero.Name = "Fighter";
            hero.HitDice = DiceSides.d10;
            var bartender = new Class();
            bartender.Name = "Bartender";
            bartender.HitDice = DiceSides.d10;

            classes.Add(hero);
            classes.Add(bartender);

            classGateway = new EntityGateway<Class>(classes);
            subject = new ClassSelector(classGateway);
        }

        [Test]
        public void SelectARandomClassForACharacter()
        {
            var character = new CharacterSheet();
            subject.ChooseAny(character);
            Assert.That(character.Class.Name, Is.Not.Null);
        }

        [Test]
        public void ChoosingClassFromWeightedOptionTableSelectsFromThoseClasses()
        {
            var character = new CharacterSheet();
            var choices = new WeightedOptionTable<string>();
            choices.AddEntry("Fighter", 10);

            subject.ChooseClass(character, choices);

            Assert.AreEqual("Fighter", character.Class.Name);
        }

        [Test]
        public void EmptyOptionTableChoosesFromAnyOfTheClasses()
        {
            var character = new CharacterSheet();
            var choices = new WeightedOptionTable<string>();
            
            Assert.That(character.Class, Is.Null);
            subject.ChooseClass(character, choices);
            Assert.That(character.Class.Name, Is.Not.Null);
        }

        [Test]
        public void SettingClassAssignsClassSkills()
        {
            var skills = new List<Skill>();
            skills.Add(new Skill("Climb", AbilityScoreTypes.Strength, false));
            var character = new CharacterSheet(skills);
            
            var cls = new Class();
            cls.AddClassSkill("Climb");

            subject.AssignClass(character, cls);

            Assert.IsTrue(character.GetSkill("Climb").ClassSkill);

        }

        [Test]
        public void AddSpecialAbilitiesFromFirstLevelForClass()
        {
            var cls = new Class();
            var lvl1 = new Level(1);
            lvl1.FeatTokens.Add(new FeatToken());
            cls.Levels.Add(lvl1);
            var character = new CharacterSheet();
            subject.AssignClass(character, cls);
            Assert.AreEqual(1, character.FeatTokens.Count);
        }
    }
}