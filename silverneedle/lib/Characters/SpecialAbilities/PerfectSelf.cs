// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters.SpecialAbilities
{
    using SilverNeedle.Utility;
    public class PerfectSelf : SpecialAbility, IComponent
    {
        public void Initialize(ComponentBag components)
        {
            var def = components.Get<DefenseStats>();
            def.AddDamageResistance(new DamageResistance(10, "chaotic"));
        }
    }
}