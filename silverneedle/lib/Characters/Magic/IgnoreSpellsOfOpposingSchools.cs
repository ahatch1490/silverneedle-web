// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT


namespace SilverNeedle.Characters.Magic
{
    using System.Linq;
    using SilverNeedle.Spells;
    using SilverNeedle.Utility;

    public class IgnoreSpellsOfOpposingSchools : ISpellCastingRule, IComponent
    {
        private WizardCasting casting;
        public bool CanCastSpell(Spell spell)
        {
            return !(casting.OppositionSchools.Any(x => x.Name.EqualsIgnoreCase(spell.School)));
        }

        public void Initialize(ComponentBag components)
        {
            casting = components.Get<WizardCasting>();
        }
    }
}