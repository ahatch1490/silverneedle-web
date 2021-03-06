﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle
{
    using SilverNeedle.Serialization;
    /// <summary>
    /// A Conditional stat modifier only modifies a statistic when a condition is met
    /// </summary>
    public class ConditionalStatModifier : IValueStatModifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SilverNeedle.ConditionalStatModifier"/> class.
        /// </summary>
        /// <param name="baseModifier">Modifier to decorate</param>
        /// <param name="condition">Condition of the modifier.</param>
        public ConditionalStatModifier(IValueStatModifier baseModifier, string condition)
        {
            this.baseModifier = baseModifier;
            this.Condition = condition;
        }

        public ConditionalStatModifier(IObjectStore configuration)
        {
            this.baseModifier = new ValueStatModifier(configuration);
            this.Condition = configuration.GetString("condition");
        }

        private IValueStatModifier baseModifier;

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition name for the modifier.</value>
        public string Condition { get; set; }

        public float Modifier { get { return baseModifier.Modifier; } }

        public string ModifierType { get { return baseModifier.ModifierType; } }

        public string StatisticName { get { return baseModifier.StatisticName; } }
        public string StatisticType { get { return baseModifier.StatisticType; } }
    }
}