// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGenerator.Abilities
{
    using Xunit;
    using SilverNeedle.Actions.CharacterGenerator.Abilities;
    using SilverNeedle.Characters;

    
    public class CreateAbilityScoreTests
    {
        [Fact]
        public void UsesAbilityScoreGeneratorSpecifiedByStrategy()
        {
            var strategy = new CharacterBuildStrategy();
            strategy.AbilityScoreRoller = "SilverNeedle.Actions.CharacterGenerator.Abilities.AverageAbilityScoreGenerator";
            var character = new CharacterSheet();
            var step = new CreateAbilityScores();
            step.Process(character, strategy);
            Assert.Equal(character.AbilityScores.GetScore(AbilityScoreTypes.Wisdom), 10);
            Assert.Equal(character.AbilityScores.GetScore(AbilityScoreTypes.Charisma), 10);
            Assert.Equal(character.AbilityScores.GetScore(AbilityScoreTypes.Strength), 10);
        }
    }
}