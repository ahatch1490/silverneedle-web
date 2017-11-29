// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters.SpecialAbilities.BloodlinePowers
{
    using SilverNeedle.Utility;

    public class DragonResistances : SpecialAbility, IBloodlinePower, IComponent, IImprovesWithLevels
    {
        private DamageResistance damageResistance;
        private IStatModifier naturalArmor;
        private ClassLevel sorcererLevels;

        public void Initialize(ComponentBag components)
        {
            var dragon = components.Get<IDraconicBloodline>();
            var energyType = dragon.DragonType.EnergyType;
            damageResistance = new DamageResistance(5, energyType);
            var defense = components.Get<DefenseStats>();
            defense.AddDamageResistance(damageResistance);

            sorcererLevels = components.Get<ClassLevel>();

            naturalArmor = new DelegateStatModifier(
                StatNames.ArmorClass,
                "natural",
                this.Name,
                GetNaturalArmorBonus
            );
            components.ApplyStatModifier(naturalArmor);
        }

        public void LeveledUp(ComponentBag components)
        {
            if(sorcererLevels.Level == 9)
            {
                damageResistance.Amount = 10;
            }
        }

        private float GetNaturalArmorBonus()
        {
            if(sorcererLevels.Level >=15)
                return 4;
            
            if(sorcererLevels.Level >= 9)
                return 2;

            return 1;
        }
    }
}