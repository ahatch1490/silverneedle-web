// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters.Attacks
{
    public interface IAttackBonus
    {
        int NumberOfAttacks { get; }
        int GetAttackBonus(int attack);
    }
}