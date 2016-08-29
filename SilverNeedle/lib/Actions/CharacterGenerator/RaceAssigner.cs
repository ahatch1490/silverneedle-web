﻿//-----------------------------------------------------------------------
// <copyright file="RaceSelector.cs" company="Short Leg Studio, LLC">
//     Copyright (c) Short Leg Studio, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SilverNeedle.Actions.CharacterGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SilverNeedle;
    using SilverNeedle.Characters;

    /// <summary>
    /// Race selector chooses a race for a charactor
    /// </summary>
    public class RaceAssigner
    {
        /// <summary>
        /// The trait gateway provides access to all traits
        /// </summary>
        private IEntityGateway<Trait> traitGateway;

        /// <summary>
        /// Initializes a new instance of the <see cref="SilverNeedle.Actions.CharacterGenerator.RaceSelector"/> class.
        /// </summary>
        /// <param name="races">Races gateway to load from.</param>
        /// <param name="traitGateway">Trait gateway.</param>
        public RaceAssigner(IEntityGateway<Trait> traitGateway)
        {
            this.traitGateway = traitGateway;
        }

        /// <summary>
        /// Sets the race.
        /// </summary>
        /// <param name="character">Character to set the race to.</param>
        /// <param name="race">Race to assign.</param>
        public void SetRace(CharacterSheet character, Race race)
        {
            character.SetRace(race);

            this.SetSpeedForRace(character, race);
            this.SetTraitsForRace(character, race);
            this.SetAbilityScoresForRace(character.AbilityScores, race);
            this.SetSizeForRace(character.Size, race);

            // TODO: Should not trigger events from here, that should be handled by the data object
            character.OnModified();
        }

        /// <summary>
        /// Sets the speed for race.
        /// </summary>
        /// <param name="character">Character to assign.</param>
        /// <param name="race">Race selected.</param>
        private void SetSpeedForRace(CharacterSheet character, Race race)
        {
            // Update Speed
            character.Movement.SetBaseSpeed(race.BaseMovementSpeed);
        }

        /// <summary>
        /// Sets the traits for race.
        /// </summary>
        /// <param name="character">Charactet to assign traits to.</param>
        /// <param name="race">Race selected.</param>
        private void SetTraitsForRace(CharacterSheet character, Race race)
        {
            // Add Traits
            foreach (var trait in race.Traits)
            {
                var t = this.traitGateway.All().First(x => x.Name == trait);
                character.AddTrait(t, false);
            }
        }

        /// <summary>
        /// Sets the ability scores for race.
        /// </summary>
        /// <param name="abilities">Abilities for adjustments.</param>
        /// <param name="race">Race selected.</param>
        private void SetAbilityScoresForRace(AbilityScores abilities, Race race)
        {
            // Add Ability Modifiers
            foreach (var adj in race.AbilityModifiers)
            {
                if (adj.RacialModifier)
                {
                    var ability = EnumHelpers.ChooseOne<AbilityScoreTypes>();
                    var a = abilities.GetAbility(ability);
                    a.AddModifier(adj);
                }
                else
                {
                    var a = abilities.GetAbility(adj.AbilityName);
                    a.AddModifier(adj);
                }
            }
        }

        /// <summary>
        /// Sets the size for race.
        /// </summary>
        /// <param name="size">Size to assign to.</param>
        /// <param name="race">Race selected.</param>
        private void SetSizeForRace(ISizeStats size, Race race)
        {
            // Update Size
            size.SetSize(race.SizeSetting, race.HeightRange.Roll(), race.WeightRange.Roll());
        }
    }
}