﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters
{
    using Inflector;
    using SilverNeedle.Equipment;

    /// <summary>
    /// Weapon proficiency is the ability to use a weapon
    /// </summary>
    public class WeaponProficiency
    {
        /// <summary>
        /// Whether this represents a whole training level of weaponry.
        /// </summary>
        private bool isLevel;

        /// <summary>
        /// The training level if this is for weapon levels
        /// </summary>
        private WeaponTrainingLevel trainingLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SilverNeedle.Characters.WeaponProficiency"/> class.
        /// </summary>
        /// <param name="proficiency">Proficiency weapon to add</param>
        public WeaponProficiency(string proficiency)
        {
            this.Name = Inflector.Humanize(proficiency);
            this.isLevel = EnumHelpers.TryParse<WeaponTrainingLevel>(proficiency, true, out this.trainingLevel);

            // Append a descriptive string so we know this is a group of weapons
            if (this.isLevel)
            {
                this.Name += " weapons";
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Determines whether this instance is proficient the specified wpn.
        /// </summary>
        /// <returns><c>true</c> if this instance is proficient the specified wpn; otherwise, <c>false</c>.</returns>
        /// <param name="weapon">Weapon to validate proficiency.</param>
        public bool IsProficient(IWeaponAttackStatistics weapon)
        {
            if (this.isLevel)
            {
                return weapon.Level == this.trainingLevel;
            }
            return weapon.ProficiencyName.EqualsIgnoreCase(this.Name);
        }
    }
}