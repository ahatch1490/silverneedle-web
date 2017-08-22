﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Actions.CharacterGeneration
{
    using SilverNeedle.Dice;
    using SilverNeedle.Characters;
    
    /// <summary>
    /// Hit point generator rolls hitpoints for a character
    /// </summary>
    public class HitPointRoller : ICharacterDesignStep
    {
        public void ExecuteStep(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            var cup = new Cup();
            cup.AddDie(new Die(character.Class.HitDice));
            cup.Modifier = character.AbilityScores.GetModifier(AbilityScoreTypes.Constitution);
            var roll = cup.Roll();
            
            character.IncreaseHitPoints(roll);            
        }
    }
}