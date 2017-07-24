// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters.SpecialAbilities
{
    using SilverNeedle.Utility;

    public class AuraOfRighteousness : SpecialAbility, IComponent
    {
        public void Initialize(ComponentBag components)
        {
            var defense = components.Get<DefenseStats>();
            defense.AddImmunity("Compulsion");
            defense.AddDamageResistance(new DamageResistance(5, "evil"));
        }
    }
}