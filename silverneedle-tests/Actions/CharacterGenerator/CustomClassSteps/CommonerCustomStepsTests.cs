// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGenerator.CustomClassSteps
{
    using System.Linq;
    using Xunit;
    using SilverNeedle.Actions.CharacterGenerator.CustomClassSteps;
    using SilverNeedle.Characters;

    
    public class CommonerCustomStepsTests
    {
        [Fact]
        public void SelectsASingleSimpleWeaponProficiency()
        {
            var character = new CharacterSheet();

            var subject = new CommonerCustomSteps();
            subject.Process(character, new CharacterBuildStrategy());
            Assert.Equal(character.Offense.WeaponProficiencies.Count, 1);
        }
    }
}