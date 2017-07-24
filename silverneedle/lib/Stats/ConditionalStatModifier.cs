﻿//-----------------------------------------------------------------------
// <copyright file="ConditionalStatModifier.cs" company="Short Leg Studio, LLC">
//     Copyright (c) Short Leg Studio, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SilverNeedle
{
    /// <summary>
    /// A Conditional stat modifier only modifies a statistic when a condition is met
    /// </summary>
    public class ConditionalStatModifier : IStatModifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SilverNeedle.ConditionalStatModifier"/> class.
        /// </summary>
        /// <param name="baseModifier">Modifier to decorate</param>
        /// <param name="condition">Condition of the modifier.</param>
        public ConditionalStatModifier(IStatModifier baseModifier, string condition)
        {
            this.baseModifier = baseModifier;
            this.Condition = condition;
        }

        private IStatModifier baseModifier;

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition name for the modifier.</value>
        public string Condition { get; set; }

        public float Modifier { get { return baseModifier.Modifier; } }

        public string Reason { get { return baseModifier.Reason; } }

        public string Type { get { return baseModifier.Type; } }

        public string StatisticName { get { return baseModifier.StatisticName; } }
    }
}