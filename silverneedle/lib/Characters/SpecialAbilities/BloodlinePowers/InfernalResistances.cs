// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters.SpecialAbilities.BloodlinePowers
{
    using SilverNeedle.Utility;

    public class InfernalResistances : SpecialAbility, IBloodlinePower, IComponent, IImprovesWithLevels
    {
        private DamageResistance fireResistance;
        private ConditionalStatModifier saveModifier;
        private ClassLevel sorcererLevels;
        public void Initialize(ComponentBag components)
        {
            sorcererLevels = components.Get<ClassLevel>();
            fireResistance = new DamageResistance(5, "fire");
            components.Get<DefenseStats>().AddDamageResistance(fireResistance);
            saveModifier = new ConditionalStatModifier
            (
                new DelegateStatModifier("Saves",
                    "bonus",
                    this.Name,
                    () => { return sorcererLevels.Level >= 9 ? 4 : 2; }
                ), "poison"
            );
            var defense = components.Get<DefenseStats>();
            defense.FortitudeSave.AddModifier(saveModifier);
            defense.ReflexSave.AddModifier(saveModifier);
            defense.WillSave.AddModifier(saveModifier);
        }

        public void LeveledUp(ComponentBag components)
        {
            if(sorcererLevels.Level == 9)
                fireResistance.Amount = 10;
        }
    }
}