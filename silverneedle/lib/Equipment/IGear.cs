﻿//-----------------------------------------------------------------------
// <copyright file="IEquipment.cs" company="Short Leg Studio, LLC">
//     Copyright (c) Short Leg Studio, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SilverNeedle.Equipment
{
    /// <summary>
    /// Piece of equipment
    /// </summary>
    public interface IGear : INamedEntity
    {
        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <value>The weight of the equipment.</value>
        float Weight { get; }

        int Value { get; }

        bool GroupSimilar { get; }
    }
}