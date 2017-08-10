// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT


namespace SilverNeedle.Shops
{
    using System;
    using System.Collections.Generic;
    using SilverNeedle.Actions.CreateMagicItems; //TODO: Move this out. Magic Shops should not generate inventory
    using SilverNeedle.Equipment;
    using SilverNeedle.Utility;

    public class MagicShop : Shop
    {
        public MagicShop()
        {
            StockShop();
        }

        public MagicShop(IEnumerable<IGear> magicGear)
        {
            StockShop(magicGear);
        }

        private void StockShop()
        {
            var wandBuilder = new CreateWands();
            Repeat.Times(100, () => this.inventory.Add(wandBuilder.Process()));
        }
    }
}