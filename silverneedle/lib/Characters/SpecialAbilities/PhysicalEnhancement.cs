// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters.SpecialAbilities
{
    using SilverNeedle.Utility;

    public class PhysicalEnhancement : SpecialAbility, IComponent
    {
        private IStatModifier modifier;
        private ClassLevel sourceLevel;
        public void Initialize(ComponentBag components)
        {
            this.sourceLevel = components.Get<ClassLevel>();
            modifier = new DelegateStatModifier(
                "Physical Attribute",
                "enhancement",
                this.Name,
                CalculateBonus
            );

            var abilityType = new AbilityScoreTypes[] {
                AbilityScoreTypes.Strength, 
                AbilityScoreTypes.Dexterity, 
                AbilityScoreTypes.Constitution
            }.ChooseOne();

            var ability = components.Get<AbilityScores>().GetAbility(abilityType);
            ability.AddModifier(modifier);
        }

        private float CalculateBonus()
        {
            return 1 + sourceLevel.Level / 5;
        }
    }
}