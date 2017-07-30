﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters {
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using SilverNeedle;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.SpecialAbilities;
    using SilverNeedle.Equipment;
    using SilverNeedle.Dice;
    using SilverNeedle.Utility;

    
    public class OffenseStatsTests {
        OffenseStats smallStats;
        Inventory inventory;

        public OffenseStatsTests() {
            var abilities = new AbilityScores ();
            abilities.SetScore (AbilityScoreTypes.Strength, 16);
            abilities.SetScore (AbilityScoreTypes.Dexterity, 14);
            var size = new SizeStats (CharacterSize.Small, 1,1);

            inventory = new Inventory();
            smallStats = new OffenseStats ();

            var components = new ComponentBag();
            components.Add(abilities);
            components.Add(size);
            components.Add(smallStats);
            components.Add(inventory);
            smallStats.Initialize(components);
        }

        [Fact]
        public void BaseAttackBonusIsAStat() {
            Assert.IsType<BasicStat> (smallStats.BaseAttackBonus);
        }

        [Fact]
        public void BaseMeleeBonusIsBABAndStrengthAndSize() {
            smallStats.BaseAttackBonus.SetValue (3);
            Assert.Equal (7, smallStats.MeleeAttackBonus.TotalValue);
        }

        [Fact]
        public void BaseRangeBonusIsBABAndDexterityAndSize() {
            smallStats.BaseAttackBonus.SetValue (3);
            Assert.Equal (6, smallStats.RangeAttackBonus.TotalValue);
        }

        [Fact]
        public void CMBIsBABAndStrengthAndSize() {
            smallStats.BaseAttackBonus.SetValue (3);
            Assert.Equal (5, smallStats.CombatManeuverBonus.TotalValue);
        }

        [Fact]
        public void CMDIsBABStrengthAndDexterityAndSize() {
            smallStats.BaseAttackBonus.SetValue (3);
            Assert.Equal (17, smallStats.CombatManeuverDefense.TotalValue);
        }

        [Fact]
        public void ModifiersCanBeAppliedToCombatManeuverDefense() {
            var mods = new MockMod();
            var oldCMD = smallStats.CombatManeuverDefense.TotalValue;
            smallStats.ProcessModifier(mods);
            Assert.Equal(oldCMD + 1, smallStats.CombatManeuverDefense.TotalValue);
        }

        [Fact]
        public void ModifiersCanBeAppliedToCombatManeuverBonus() {
            var mods = new MockMod();
            var oldCMB = smallStats.CombatManeuverBonus.TotalValue;
            smallStats.ProcessModifier(mods);
            Assert.Equal(oldCMB + 1, smallStats.CombatManeuverBonus.TotalValue);
        }

        [Fact]
        public void ContainsAListOfAllWeaponsAvailableAndTheirStats() {
            var longsword = Longsword();
            inventory.AddGear(longsword);
            Assert.Equal(1, smallStats.Attacks().Count);
            Assert.Equal("Longsword", smallStats.Attacks().First().Name);
            Assert.Equal(longsword, smallStats.Attacks().First().Weapon);
        }

        [Fact]
        public void MeleeWeaponAttacksCalculateDamageBonuses() {
            inventory.AddGear(Longsword());
            smallStats.AddWeaponProficiency("martial");

            var atk = smallStats.Attacks().First();
            Assert.NotNull(atk);
            var diceRoll = atk.Damage;
            Assert.Equal(3, diceRoll.Modifier);

            //Should convert damage based on size
            Assert.Equal(DiceSides.d6, diceRoll.Dice.First().Sides);
            Assert.Equal(smallStats.MeleeAttackBonus.TotalValue, atk.AttackBonus);
        }

        [Fact]
        public void RangeAttackBonusHaveAttackBonusButNotDamage() {
            inventory.AddGear(ShortBow());
            smallStats.AddWeaponProficiency("martial");
            var atk = smallStats.Attacks().First();
            Assert.NotNull(atk);
            var diceRoll = atk.Damage;
            Assert.Equal(0, diceRoll.Modifier);
            Assert.Equal(DiceSides.d4, diceRoll.Dice.First().Sides);
            Assert.Equal(smallStats.RangeAttackBonus.TotalValue, atk.AttackBonus);
        }

        [Fact]
        public void CanAddWeaponProficiencies() {
            smallStats.AddWeaponProficiency("Shortbow");
            Assert.True(smallStats.IsProficient(ShortBow()));
        }

        [Fact]
        public void CanAddAnArrayOfWeaponProficiencies() {
            smallStats.AddWeaponProficiencies(new string[] {"simple", "martial"});
            Assert.True(smallStats.IsProficient(Longsword()));	
        }

        [Fact]
        public void AttacksWithoutProficiencyAreAtMinus4() {
            inventory.AddGear(Nunchaku());
            var atk = smallStats.Attacks().First();
            Assert.NotNull(atk);
            Assert.Equal(smallStats.MeleeAttackBonus.TotalValue + OffenseStats.UnproficientWeaponModifier, atk.AttackBonus);
        }

        [Fact]
        public void ThrownWeaponsAreReturnedForBothMeleeAndRangedVersions()
        {
            inventory.AddGear(Dagger());
            var atks = smallStats.Attacks();
            Assert.True(atks.Any(x => x.AttackType == AttackTypes.Melee));
            Assert.True(atks.Any(x => x.AttackType == AttackTypes.Ranged));
        }

        [Fact]
        public void TracksSpecialAttackAbilities()
        {
            var special = new SpecialAttacks();
            smallStats.ProcessSpecialAbilities(special);
            Assert.True(smallStats.OffensiveAbilities.Count() > 0);
        }

        [Fact]
        public void LevelsUpCombatStatsBasedOnClass()
        {
            var cls = new Class();
            cls.BaseAttackBonusRate = 1;
            smallStats.LevelUp(cls);
            Assert.Equal(1, smallStats.BaseAttackBonus.TotalValue);
            smallStats.LevelUp(cls);
            Assert.Equal(2, smallStats.BaseAttackBonus.TotalValue);            
        }

        [Fact]
        public void ReturnsStatsForCombat()
        {
            var stats = smallStats.Statistics;
            Assert.Equal(stats.Count(), 3);
            Assert.NotStrictEqual(stats, new BasicStat[] { 
                smallStats.BaseAttackBonus, 
                smallStats.CombatManeuverBonus, 
                smallStats.CombatManeuverDefense,
            });
        }

        [Fact]
        public void AllowCustomModifiersToAttackBonusForSpecificWeapons()
        {
            //This allows things like WeaponFocus feats
            var attackMod = new WeaponAttackModifier("Weapon Focus",  1, x => { return x.Name.EqualsIgnoreCase("Longsword"); });
            var damageMod = new WeaponDamageModifier("Weapon Training", 2, x => { return x.Group == WeaponGroup.HeavyBlades; });
            smallStats.AddWeaponProficiency("simple");
            smallStats.AddWeaponProficiency("martial");
            smallStats.AddWeaponModifier(attackMod);
            smallStats.AddWeaponModifier(damageMod);
            inventory.AddGear(Longsword());
            inventory.AddGear(Dagger());

            //Longsword should have bonuses while dagger should not
            var lswordAttack = smallStats.Attacks().First(x => x.Name == "Longsword");
            var dAttack = smallStats.Attacks().First(x => x.Name == "Dagger");

            Assert.Equal(lswordAttack.AttackBonus, smallStats.MeleeAttackBonus.TotalValue + 1);
            Assert.Equal(lswordAttack.Damage.Modifier, 5);
            Assert.Equal(dAttack.AttackBonus, smallStats.MeleeAttackBonus.TotalValue);
            Assert.Equal(dAttack.Damage.Modifier, 3);
        }

        [Fact]
        public void CanAddSpecialAttacksToStats()
        {
            var attack = new AttackStatistic();
            smallStats.AddAttack(attack);
            Assert.Contains(attack, smallStats.Attacks());
        }

        [Fact]
        public void MasterworkWeaponsProvideImprovedAttackBonus()
        {
            var mwkLongsword = new MasterworkWeapon(Longsword());
            inventory.AddGear(mwkLongsword);
            smallStats.AddWeaponProficiency("martial");

            var atk = smallStats.GetAttack(mwkLongsword);
            // Small Size (1) + Str16 (3) + Mwk (1)
            Assert.Equal(atk.AttackBonus, 5);
        }

        private Weapon Longsword() {
            return new Weapon("Longsword", 0, "1d8", DamageTypes.Slashing, 19, 2, 0, WeaponType.OneHanded, WeaponGroup.HeavyBlades, WeaponTrainingLevel.Martial);
        }

        private Weapon ShortBow() {
            return new Weapon("Shortbow", 0, "1d6", DamageTypes.Piercing, 19, 2, 0, WeaponType.Ranged, WeaponGroup.Bows, WeaponTrainingLevel.Martial);
        }

        private Weapon Nunchaku() {
            return new Weapon("Nunchaku", 0, "1d6", DamageTypes.Bludgeoning, 20, 2, 0, WeaponType.OneHanded, WeaponGroup.Monk, WeaponTrainingLevel.Exotic);
        }

        private Weapon Dagger() {
            return new Weapon("Dagger", 0, "1d4", DamageTypes.Piercing, 20, 2, 10, WeaponType.Light, WeaponGroup.Thrown, WeaponTrainingLevel.Simple);
        }

        class MockMod : IModifiesStats {
            public IList<IStatModifier> Modifiers { get; set;  }

            public MockMod() {
                Modifiers = new List<IStatModifier>();
                Modifiers.Add(new ValueStatModifier("CMD", 1, "racial", "Trait"));
                Modifiers.Add(new ValueStatModifier("CMB", 1, "racial", "Trait"));
            }
        }

        class SpecialAttacks : IProvidesSpecialAbilities 
        {
            public IList<SpecialAbility> SpecialAbilities { get; set; }

            public SpecialAttacks() {
                SpecialAbilities = new List<SpecialAbility>();
                SpecialAbilities.Add(new SpecialAbility("Sneak Attack 1d6", "Offensive"));

            }
        }
    }
}