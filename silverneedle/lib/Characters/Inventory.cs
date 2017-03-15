﻿//-----------------------------------------------------------------------
// <copyright file="Inventory.cs" company="Short Leg Studio, LLC">
//     Copyright (c) Short Leg Studio, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SilverNeedle.Characters
{
    using System.Collections.Generic;
    using System.Linq;
    using SilverNeedle.Equipment;
    using SilverNeedle.Treasure;

    /// <summary>
    /// Inventory for a character. This holds all the gear and keeps track
    /// of what they have equipped.
    /// </summary>
    public class Inventory
    {
        /// <summary>
        /// The gear a character has
        /// </summary>
        private IList<IInventoryItem> gear;

        /// <summary>
        /// The equipped gear.
        /// </summary>
        private IList<IInventoryItem> equippedGear;

        /// <summary>
        /// Initializes a new instance of the <see cref="SilverNeedle.Characters.Inventory"/> class.
        /// </summary>
        public Inventory()
        {
            this.gear = new List<IInventoryItem>();  
            this.equippedGear = new List<IInventoryItem>();
            this.CoinPurse = new CoinPurse();
        }

        public CoinPurse CoinPurse { get; private set; }

        /// <summary>
        /// Gets all of the gear
        /// </summary>
        /// <value>All of the equipment in inventory.</value>
        public IEnumerable<IInventoryItem> All 
        { 
            get { return this.gear; } 
        }

        /// <summary>
        /// Gets all of the weapons 
        /// </summary>
        /// <value>The weapons.</value>
        public IEnumerable<Weapon> Weapons 
        { 
            get { return this.gear.OfType<Weapon>(); } 
        }

        /// <summary>
        /// Gets all of the armor
        /// </summary>
        /// <value>The armor.</value>
        public IEnumerable<Armor> Armor 
        { 
            get { return this.gear.OfType<Armor>(); } 
        }

        /// <summary>
        /// Gets the equipped items.
        /// </summary>
        /// <value>The equipped items.</value>
        public IEnumerable<IInventoryItem> EquippedItems 
        { 
            get { return this.equippedGear; } 
        }

        /// <summary>
        /// Adds the gear to the character.
        /// </summary>
        /// <param name="equip">Equipment to add.</param>
        public void AddGear(IInventoryItem equip)
        {
            if (!this.gear.Contains(equip))
            {
                this.gear.Add(equip);
            }
        }

        /// <summary>
        /// Equips the item.
        /// </summary>
        /// <param name="item">Item to equip. Adds gear if not already added</param>
        public void EquipItem(IInventoryItem item)
        {
            this.AddGear(item);
            this.equippedGear.Add(item);
        }

        /// <summary>
        /// Finds gear of specific type
        /// </summary>
        /// <returns>The type.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public IEnumerable<T> GearOfType<T>()
        {
            return this.gear.OfType<T>();
        }

        public void Purchase(IInventoryItem item)
        {
            CoinPurse.Spend(item.Value);
            AddGear(item);
        }
    }
}