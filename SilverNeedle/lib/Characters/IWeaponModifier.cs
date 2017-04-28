// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters
{
    using System;
    using SilverNeedle.Equipment;
    public interface IWeaponModifier
    {
        Func<Weapon, bool> WeaponQualifies { get; }
        void ApplyModifier(OffenseStats.AttackStatistic attack);
    }
}