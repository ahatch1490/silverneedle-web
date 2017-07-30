// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGenerator
{
    using Xunit;
    using System.Collections.Generic;
    using SilverNeedle.Actions.CharacterGenerator;
    using SilverNeedle.Characters;
    using SilverNeedle.Serialization;
    
    
    public class StartingWealthTests 
    {
        EntityGateway<CharacterWealth> wealthGateway;

        public StartingWealthTests()
        {
            var list = new List<CharacterWealth>();
            var wealth = new CharacterWealth();
            wealth.Name = "adventurer";
            wealth.Levels.Add(new CharacterWealth.CharacterWealthLevel(1, 0));
            wealth.Levels.Add(new CharacterWealth.CharacterWealthLevel(2, 2000));
            list.Add(wealth);
            wealthGateway = new EntityGateway<CharacterWealth>(list);
        }

        [Fact]
        public void DoesNothingIfStartingWealthIsNullForClass()
        {
            var cls = new Class();
            var character = new CharacterSheet();
            character.SetClass(cls);
            var action = new StartingWealth();
            action.Process(character, new CharacterBuildStrategy());
        }

        [Fact]
        public void CalculatesWealthBasedOnTheDiceInGoldPiecesTimesTen()
        {
            var cls = new Class();
            cls.StartingWealthDice = SilverNeedle.Dice.DiceStrings.ParseDice("2d6");
            var character = new CharacterSheet();
            character.SetClass(cls);

            var action = new StartingWealth();
            action.Process(character, new CharacterBuildStrategy());

            Assert.True(character.Inventory.CoinPurse.Gold.Pieces > 19);
            Assert.True(character.Inventory.CoinPurse.Gold.Pieces < 121);
        }

        [Fact]
        public void IfAfterFirstLevelPickStartValueFromWealthList()
        {
            var character = new CharacterSheet();
            character.SetClass(new Class());
            character.SetLevel(2);
            var action = new StartingWealth(wealthGateway);
            action.Process(character, new CharacterBuildStrategy());

            Assert.Equal(character.Inventory.CoinPurse.Value, 2000);

        }
    }
}