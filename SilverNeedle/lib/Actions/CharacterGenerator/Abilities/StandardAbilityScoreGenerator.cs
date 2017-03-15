﻿//-----------------------------------------------------------------------
// <copyright file="StandardAbilityScoreGenerator.cs" company="Short Leg Studio, LLC">
//     Copyright (c) Short Leg Studio, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SilverNeedle.Actions.CharacterGenerator.Abilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SilverNeedle.Dice;
    using SilverNeedle.Characters;
    using SilverNeedle.Utility;
    
    /// <summary>
    /// Generates ability scores by rolling 4d6 for each and selecting the top 3 dice
    /// </summary>
    public class StandardAbilityScoreGenerator : ICharacterDesignStep
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SilverNeedle.Actions.CharacterGenerator.Abilities.StandardAbilityScoreGenerator"/> class.
        /// </summary>
        public StandardAbilityScoreGenerator()
        {
        }

        public void Process(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            CreateAndAssignScores(character.AbilityScores, strategy.FavoredAbilities);
        }
        
        private void CreateAndAssignScores(AbilityScores abilities, WeightedOptionTable<AbilityScoreTypes> abilityPref)
        {
            //Roll up 6 scores
            List<int> scores = new List<int>();
            IEnumerable<AbilityScoreTypes> weightedAbilities = abilityPref.UniqueList();
            
            //Create sorted list of items
            for(int i = 0; i < 6; i++) {
                scores.Add(Roll4d6());
            }            
            scores.Sort();

            for(int i = 0; i < 6; i++)
            {
                var ability = weightedAbilities.ElementAt(i);
                var score = scores.ElementAt(5 - i);
                abilities.SetScore(ability, score);
            }            
        }

        /// <summary>
        /// Roll4d6 To get ability Score
        /// </summary>
        /// <returns>Top 3 dice summed</returns>
        private int Roll4d6()
        {
            var diceCup = new Cup(Die.GetDice(DiceSides.d6, 4));
            diceCup.Roll();
            return diceCup.SumTop(3);
        }
    }
}