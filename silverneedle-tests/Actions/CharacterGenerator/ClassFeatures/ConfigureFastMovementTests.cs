// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGenerator.ClassFeatures
{
    using Xunit;
    using SilverNeedle.Actions.CharacterGenerator.ClassFeatures;
    using SilverNeedle.Characters;
    using SilverNeedle.Equipment;
    using SilverNeedle.Serialization;

    
    public class ConfigureFastMovementTests
    {
        [Fact]
        public void SetsUpAModifierThatIncreasesMovementSpeedWhenNotWearingHeavyArmor()
        {
            var data = new MemoryStore();
            data.SetValue("speed", 10);
            var character = new CharacterSheet();
            var action = new ConfigureFastMovement(data);

            action.Process(character, new CharacterBuildStrategy());
            Assert.Equal(character.Movement.MovementSpeed, 40);
            var heavyArmor = new Armor();
            heavyArmor.ArmorType = ArmorType.Heavy;
            character.Inventory.EquipItem(heavyArmor);
            Assert.Equal(character.Movement.MovementSpeed, 30);

        }
    }
}