// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters.Domains
{
    using NUnit.Framework;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.Domains;
    using SilverNeedle.Characters.SpecialAbilities;
    using SilverNeedle.Serialization;

    public class GoodTests : DomainTestBase<Good>
    {
        [SetUp]
        public void Configure()
        {
            base.InitializeDomain("good");
        }

        [Test]
        public void TouchOfGood()
        {
            var touch = character.Get<TouchOfGood>();
            Assert.That(touch, Is.Not.Null); 
            Assert.That(touch.UsesPerDay, Is.EqualTo(6));
        }

        [Test]
        public void HolyLance()
        {
            character.SetLevel(8);
            domain.LeveledUp(character.Components);
            var evilScythe = character.Get<HolyLance>();
            Assert.That(evilScythe, Is.Not.Null); 
            Assert.That(evilScythe.UsesPerDay, Is.EqualTo(1));
            character.SetLevel(16);

            Assert.That(evilScythe.UsesPerDay, Is.EqualTo(3));}
    }
}