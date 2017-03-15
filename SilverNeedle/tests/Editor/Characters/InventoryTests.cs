﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters {
    using System.Linq;
    using NUnit.Framework;
    using SilverNeedle.Characters;
    using SilverNeedle.Equipment;
    using SilverNeedle.Treasure;

	[TestFixture]
	public class InventoryTests {
		[Test]
		public void InventoryTracksWeapons() {
			var inv = new Inventory ();
			var wpn1 = new Weapon ();
			var gear = new PieceOfJunk ();
			inv.AddGear (wpn1);
			inv.AddGear (gear);

			Assert.AreEqual (wpn1, inv.Weapons.First ());
		}

		[Test]
		public void InventoryWillReturnCurrentArmor() {
			var inv = new Inventory ();
			var armor = new Armor ();
			inv.AddGear (armor);

			Assert.AreEqual (armor, inv.Armor.First());
		}

		[Test]
		public void InventoryItemsCanBeMarkedAsEquipped() {
			var inv = new Inventory ();
			var armor = new Armor ();
			inv.EquipItem (armor);

			Assert.AreEqual (1, inv.EquippedItems.Count());

		}

		[Test]
		public void CanGetItemOfSpecificType() {
			var inv = new Inventory ();
			var armor = new Armor ();
			var junk = new PieceOfJunk ();
			var wpn = new Weapon ();
			inv.AddGear (armor);
			inv.AddGear (junk);
			inv.AddGear (wpn);

			Assert.AreEqual (junk, inv.GearOfType<PieceOfJunk> ().First());
		}

		[Test]
		public void EquippingItemAddsItToInventoryIfNotAlreadyThere() {
			var inv = new Inventory ();
			var armor = new Armor ();
			inv.EquipItem (armor);
			Assert.AreEqual (armor, inv.Armor.First ());
		}

		[Test]
		public void AddingTheSameItemTwiceIncrementsQuantity() {
			var inv = new Inventory ();
			var j = new PieceOfJunk ();
			inv.AddGear (j);
			inv.AddGear (j);
            var item = inv.Find(j);
            Assert.That(item.Quantity, Is.EqualTo(2));
		}

        [Test]
        public void PurchasingAnItemDeductsTheValueFromPurse()
        {
            var inv = new Inventory();
            var j = new PieceOfJunk();
            j.Value = 34;
            inv.CoinPurse.SetValue(40);
            inv.Purchase(j);
            Assert.AreEqual(6, inv.CoinPurse.Value);
            Assert.AreEqual(1, inv.All.Count());
        }

        [Test]
        public void ProvideMethodForGettingEquippedItemsOfType()
        {
            var inv = new Inventory();
            inv.EquipItem(new Armor());
            inv.EquipItem(new Weapon());
            inv.EquipItem(new Weapon());

            var equippedArmors = inv.Equipped<Armor>();
            Assert.That(equippedArmors.Count(), Is.EqualTo(1));
        }

		class PieceOfJunk : IGear {
			public string Name { get { return "Junk"; } }
			public float Weight { get { return 0.5f; } }

            public int Value { get; set; }

            public bool GroupSimilar { get; set; }
		}
	}
}
