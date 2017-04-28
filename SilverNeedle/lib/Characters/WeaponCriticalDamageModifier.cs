// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

using System;
using SilverNeedle.Equipment;

namespace SilverNeedle.Characters
{
    public class WeaponCriticalDamageModifier : IStatModifier, IWeaponModifier
    {

        public WeaponCriticalDamageModifier(string reason, float modifier, Func<Weapon, bool> qualify)
        {
            this.Reason = reason;
            this.Modifier = modifier;
            this.WeaponQualifies = qualify;
        }
        public Func<Weapon, bool> WeaponQualifies { get; private set; }

        public float Modifier { get; set; }

        public string Reason { get; private set; }

        public string Type { get { return "Bonus"; } }

        public string StatisticName { get { return "Weapon Critical"; } }

        public void ApplyModifier(OffenseStats.AttackStatistic attack)
        {
            attack.CriticalModifier += (int)Modifier;
        }
    }
}