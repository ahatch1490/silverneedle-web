// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGeneration.ClassFeatures
{
    using Xunit;
    using SilverNeedle.Actions.CharacterGeneration.ClassFeatures;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.SpecialAbilities;
    
    
    public class ConfigureWeaponMasteryTests : RequiresDataFiles
    {
        [Fact]
        public void ChoosesAWeaponForMastery()
        {
            var configure = new ConfigureWeaponMastery();
            var character = new CharacterSheet();
            configure.ExecuteStep(character, new CharacterBuildStrategy());

            var mastery = character.Components.Get<WeaponMastery>();
            Assert.NotNull(mastery.Weapon);
        }
    }
}