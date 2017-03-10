// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Actions.CharacterGenerator
{
    using SilverNeedle.Characters;
    
    /// <summary>
    /// Hit point generator rolls hitpoints for a character
    /// </summary>
    public class InitialHitPoints : ICreateStep
    {
        public void Process(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            int hp = (int)character.Class.HitDice + character.AbilityScores.GetModifier(AbilityScoreTypes.Constitution);
            character.IncreaseHitPoints(hp);
        }
    }
}