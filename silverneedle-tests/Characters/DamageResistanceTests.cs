// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters
{
    using Xunit;
    using SilverNeedle.Characters;
    using SilverNeedle.Core;

    public class DamageResistanceTests
    {
        [Fact]
        public void DamageResistanceCanBeCalculated()
        {
            var energyType = new EnergyType();
            energyType.Name = "Fire";
            var dr = new DamageResistance(
                energyType,
                () => { return 6 * 10; }
            );
            Assert.Equal("Fire", dr.DamageType);
            Assert.Equal(60, dr.Amount);
        }

        [Fact]
        public void CanBeMarkedAsImmunity()
        {
            var dr = new DamageResistance(5, "fire");
            dr.SetToImmunity();
            var character = CharacterTestTemplates.AverageBob();
            character.Defense.AddDamageResistance(dr);
            AssertCharacter.IsImmuneTo("fire", character);
            Assert.Empty(character.Defense.DamageResistance);
        }

        [Fact]
        public void DamageResistanceOverTenThousandIsEqualToImmunity()
        {
            var dr = new DamageResistance(10000, "fire");
            var character = CharacterTestTemplates.AverageBob();
            character.Defense.AddDamageResistance(dr);
            AssertCharacter.IsImmuneTo("fire", character);
        }
    }
}