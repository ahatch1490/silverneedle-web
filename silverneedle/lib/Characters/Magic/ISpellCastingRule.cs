// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters.Magic
{
    using SilverNeedle.Spells;
    public interface ISpellCastingRule
    {
        bool CanCastSpell(Spell spell);
    }
}