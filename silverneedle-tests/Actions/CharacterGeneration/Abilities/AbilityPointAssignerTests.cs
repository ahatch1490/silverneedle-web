// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGeneration
{
    using Xunit;
    using SilverNeedle.Actions.CharacterGeneration;
    using SilverNeedle.Characters;

    public class AbilityPointAssignerTests
    {
        [Fact]
        public void AssignsAbilityPointsBasedOnStrategy()
        {
            var character = CharacterTestTemplates.AverageBob();
            character.Add(new AbilityScoreToken(2, "Racial Choice"));

            var strategy = character.Strategy;
            strategy.FavoredAbilities.AddEntry(AbilityScoreTypes.Strength, 100000);

            var abilityPointAssigner = new AbilityPointAssigner();
            abilityPointAssigner.ExecuteStep(character);

            Assert.Equal(12, character.AbilityScores.GetScore(AbilityScoreTypes.Strength));
        }
    }
}