﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters
{
    using System.Collections.Generic;
    using System.Linq;
    using SilverNeedle.Dice;
    using SilverNeedle.Equipment;

    /// <summary>
    /// Offense stats for a character. Keeps track of attacks and abilities
    /// for attacking
    /// </summary>
    public class OffenseStats : IStatTracker
    {
        /// <summary>
        /// The unproficient weapon modifier.
        /// </summary>
        public const int UnproficientWeaponModifier = -4;

        /// <summary>
        /// The name of the combat manuever defense stat.
        /// </summary>
        private const string CombatManeuverDefenseStatName = "CMD";   // TODO: Does this belong in Defense Stats?

        /// <summary>
        /// The name of the combat maneuver bonus stat.
        /// </summary>
        private const string CombatManeuverBonusStatName = "CMB";

        private const string OffensiveAbilitiesName = "Offensive";

        /// <summary>
        /// Gets or sets the CombatManeuverDefense.
        /// </summary>
        private BasicStat combatManeuverDefense;

        /// <summary>
        /// Gets or sets the combat maneuver bonus.
        /// </summary>
        private BasicStat combatManeuverOffense;

        /// <summary>
        /// The inventory for the character.
        /// </summary>
        private Inventory inventory;

        private List<SpecialAbility> offensiveAbilities;

        /// <summary>
        /// Initializes a new instance of the <see cref="SilverNeedle.Characters.OffenseStats"/> class.
        /// </summary>
        /// <param name="scores">Ability scores of the character.</param>
        /// <param name="size">Size of the character.</param>
        /// <param name="inventory">Inventory and gear of the character.</param>
        public OffenseStats(AbilityScores scores, SizeStats size, Inventory inventory)
        {
            this.BaseAttackBonus = new BasicStat();
            this.combatManeuverDefense = new BasicStat(10);
            this.combatManeuverOffense = new BasicStat();
            this.AbilityScores = scores;
            this.Size = size;
            this.inventory = inventory;
            this.WeaponProficiencies = new List<WeaponProficiency>();
            this.offensiveAbilities = new List<SpecialAbility>();
        }

        /// <summary>
        /// Gets the weapon proficiencies.
        /// </summary>
        /// <value>The weapon proficiencies for the character.</value>
        public IList<WeaponProficiency> WeaponProficiencies { get; private set; }

        /// <summary>
        /// Gets the base attack bonus.
        /// </summary>
        /// <value>The base attack bonus.</value>
        public BasicStat BaseAttackBonus { get; private set; }

        public IEnumerable<SpecialAbility> OffensiveAbilities
        {
            get
            {
                return offensiveAbilities;
            }
        }

        /// <summary>
        /// Gets or sets the ability scores.
        /// </summary>
        /// <value>The ability scores.</value>
        private AbilityScores AbilityScores { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        private SizeStats Size { get; set; }

        /// <summary>
        /// Calculates the melee attack bonus.
        /// </summary>
        /// <returns>The attack bonus.</returns>
        public int MeleeAttackBonus()
        {
            return this.BaseAttackBonus.TotalValue + 
                this.AbilityScores.GetModifier(AbilityScoreTypes.Strength) + 
                this.Size.SizeModifier;
        }

        /// <summary>
        /// Calculates the range attack bonus.
        /// </summary>
        /// <returns>The attack bonus.</returns>
        public int RangeAttackBonus()
        {
            return this.BaseAttackBonus.TotalValue + 
                this.AbilityScores.GetModifier(AbilityScoreTypes.Dexterity) + 
                this.Size.SizeModifier;
        }

        /// <summary>
        /// Calculates the combat manuever bonus.
        /// </summary>
        /// <returns>The manuever bonus.</returns>
        public int CombatManeuverBonus()
        {
            return this.combatManeuverOffense.TotalValue + 
                this.BaseAttackBonus.TotalValue + 
                this.AbilityScores.GetModifier(AbilityScoreTypes.Strength) - 
                this.Size.SizeModifier;
        }

        /// <summary>
        /// Calculate the combats manuever defense.
        /// </summary>
        /// <returns>The manuever defense.</returns>
        public int CombatManeuverDefense()
        {
            return this.combatManeuverDefense.TotalValue + 
                this.BaseAttackBonus.TotalValue + 
                this.AbilityScores.GetModifier(AbilityScoreTypes.Strength) + 
                this.AbilityScores.GetModifier(AbilityScoreTypes.Dexterity) - 
                this.Size.SizeModifier;
        }

        /// <summary>
        /// The implementing class must handle modifiers to stats under its control
        /// </summary>
        /// <param name="modifier">Modifier for stats</param>
        public void ProcessModifier(IModifiesStats modifier)
        {
            foreach (var m in modifier.Modifiers)
            {
                switch (m.StatisticName)
                {
                    case CombatManeuverDefenseStatName:
                        this.combatManeuverDefense.AddModifier(m);
                        break;
                    case CombatManeuverBonusStatName:
                        this.combatManeuverOffense.AddModifier(m);
                        break;
                }
            }
        }

        public void ProcessSpecialAbilities(IProvidesSpecialAbilities abilities)
        {
            foreach (var ability in abilities.SpecialAbilities)
            {
                switch (ability.Type)
                {
                    case OffensiveAbilitiesName:
                        offensiveAbilities.Add(ability);
                        break;
                }
            }
        }


        /// <summary>
        /// Adds a weapon proficiencies.
        /// </summary>
        /// <param name="proficiencies">Proficiencies to add</param>
        public void AddWeaponProficiencies(IEnumerable<string> proficiencies)
        {
            foreach (var p in proficiencies)
            {
                this.AddWeaponProficiency(p);
            }
        }

        /// <summary>
        /// Adds the weapon proficiency.
        /// </summary>
        /// <param name="proficiency">Proficiency to add.</param>
        public void AddWeaponProficiency(string proficiency)
        {
            this.WeaponProficiencies.Add(new WeaponProficiency(proficiency));
        }

        /// <summary>
        /// Determines whether this instance is proficient the specified weapon.
        /// </summary>
        /// <returns><c>true</c> if this instance is proficient in the specified weapon; otherwise, <c>false</c>.</returns>
        /// <param name="weapon">Weapon to check.</param>
        public bool IsProficient(Weapon weapon)
        {
            return this.WeaponProficiencies.IsProficient(weapon);
        }

        /// <summary>
        /// Calculates what attacks are available to this instance
        /// </summary>
        /// <returns>List of attacks that are available</returns>
        public IList<AttackStatistic> Attacks()
        {
            var attacks = new List<AttackStatistic>();
            
            // Get all the melee weapons
            foreach (var weapon in this.inventory.Weapons.Where(x => x.IsMelee))
            {
                attacks.Add(
                    CreateAttack(AttackTypes.Melee, weapon)
                );
            }

            // Get all the ranged weapons
            foreach (var weapon in this.inventory.Weapons.Where(x => x.IsRanged))
            {
                attacks.Add(
                    CreateAttack(AttackTypes.Ranged, weapon)
                );
            }

            return attacks;
        }

        public void LevelUp(Class characterClass)
        {
            BaseAttackBonus.AddModifier(new BasicStatModifier(characterClass.BaseAttackBonusRate, string.Format("{0} Level", characterClass.Name)));            
        }

        private AttackStatistic CreateAttack(AttackTypes attackType, Weapon weapon) 
        {
            var atk = new AttackStatistic();
            atk.AttackType = attackType;
            atk.Name = weapon.Name;
            atk.Weapon = weapon;
            atk.Damage = DiceStrings.ParseDice(DamageTables.ConvertDamageBySize(weapon.Damage, this.Size.Size));
            atk.AttackBonus = this.MeleeAttackBonus();
            
            // Figure out to apply damage modifier
            if (attackType == AttackTypes.Melee)
            {
                atk.Damage.Modifier = AbilityScores.GetModifier(AbilityScoreTypes.Strength);
            }
            
            // If not proficient, add a penalty to the attack bonus
            if (!this.IsProficient(weapon))
            {
                atk.AttackBonus += UnproficientWeaponModifier;
            }
            return atk;
        }

        /// <summary>
        /// Attack statistics for offense
        /// </summary>
        public struct AttackStatistic
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the weapon.
            /// </summary>
            /// <value>The weapon.</value>
            public Weapon Weapon { get; set; }

            /// <summary>
            /// Gets or sets the damage.
            /// </summary>
            /// <value>The dice needed to roll damage.</value>
            public Cup Damage { get; set; }

            /// <summary>
            /// Gets or sets the attack bonus.
            /// </summary>
            public int AttackBonus { get; set; }

            public AttackTypes AttackType { get; set; }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents the current <see cref="SilverNeedle.Characters.OffenseStats+AttackStatistic"/>.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents the current <see cref="SilverNeedle.Characters.OffenseStats+AttackStatistic"/>.</returns>
            public override string ToString()
            {
                return string.Format(
                    "{0} {1} ({2} / {3}x{4})", 
                    this.Name, 
                    this.AttackBonus.ToModifierString(), 
                    this.Damage, 
                    this.Weapon.CriticalThreat, 
                    this.Weapon.CriticalModifier);
            }
        }
    }
}