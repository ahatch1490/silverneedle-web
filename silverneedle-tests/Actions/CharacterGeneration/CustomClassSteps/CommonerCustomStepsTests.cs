// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGeneration.CustomClassSteps
{
    using System.Linq;
    using Xunit;
    using SilverNeedle.Actions.CharacterGeneration.CustomClassSteps;
    using SilverNeedle.Characters;

    
    public class CommonerCustomStepsTests : RequiresDataFiles
    {
        [Fact]
        public void SelectsASingleSimpleWeaponProficiency()
        {
            var character = CharacterTestTemplates.AverageBob();

            var subject = new CommonerCustomSteps();
            subject.Execute(character.Components);
            Assert.Equal(character.Offense.WeaponProficiencies.Count(), 1);
        }
    }
}