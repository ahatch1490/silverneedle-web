// Copyright (c) 2016 Trevor Redfern
// 
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

namespace Tests.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using SilverNeedle.Actions.CharacterGenerator;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.Prerequisites;
    using SilverNeedle.Serialization;
    using SilverNeedle.Utility;

    [TestFixture]
    public class FeatSelectorTests
    {
        Feat powerattack;
        Feat cleave;
        Feat empowerspell;

        FeatSelector selector;
        EntityGateway<Feat> gateway;

        [SetUp]
        public void SetUp()
        {
            powerattack = new Feat();
            powerattack.Name = "Power Attack";

            cleave = new Feat();
            cleave.Name = "Cleave";
            cleave.Prerequisites.Add(new FeatPrerequisite("power attack"));

            empowerspell = new Feat();
            empowerspell.Name = "Empower Spell";
            empowerspell.Tags.Add("metamagic");

            var list = new List<Feat>();
            list.Add(powerattack);
            list.Add(cleave);
            list.Add(empowerspell);
            gateway = new EntityGateway<Feat>(list);
            selector = new FeatSelector(gateway);
        }
        [Fact]
        public void ChooseFeatBasedOnStrategy()
        {
            var strategy = new WeightedOptionTable<string>();
            strategy.AddEntry("power attack", 5000000);
            strategy.AddEntry("cleave", 1);
            
            var character = new CharacterSheet();
            character.FeatTokens.Add(new FeatToken());
            
            selector.SelectFeats(character, strategy);
            Assert.AreEqual(powerattack, character.Feats.First()); 

        }

        [Fact]
        public void OnlySelectsFeatsThatCharacterQualifiesFor()
        {
            var strategy = new WeightedOptionTable<string>();
            strategy.AddEntry("power attack", 1);
            strategy.AddEntry("cleave", 5000000);
            
            for(int i = 0; i < 1000; i++)
            {
                var character = new CharacterSheet();
                character.FeatTokens.Add(new FeatToken());

                selector.SelectFeats(character, strategy);
                Assert.AreEqual(powerattack, character.Feats.First());
            }            
        } 

        [Fact]
        public void SelectFeatsBasedOnTokensAvailable() 
        {
            var strategy = new WeightedOptionTable<string>();
            strategy.AddEntry("power attack", 5000000);
            strategy.AddEntry("empower spell", 1);


            for(int i = 0; i < 1000; i++)
            {
                var character = new CharacterSheet();
                character.FeatTokens.Add(new FeatToken("metamagic"));

                selector.SelectFeats(character, strategy);
                Assert.AreEqual(empowerspell, character.Feats.First());
            }            
        }

        [Fact]
        public void SelectsAFeatForEachOutstandingToken() {
            var strategy = new WeightedOptionTable<string>();
            strategy.AddEntry("power attack", 5000000);
            strategy.AddEntry("empower spell", 1);
        
            for(int i = 0; i < 1000; i++)
            {
                var character = new CharacterSheet();
                character.FeatTokens.Add(new FeatToken("metamagic"));
                character.FeatTokens.Add(new FeatToken());

                selector.SelectFeats(character, strategy);
                Assert.IsTrue(character.Feats.Contains(empowerspell));
                Assert.IsTrue(character.Feats.Contains(powerattack));
            }        
        }

        [Fact]
        public void FeatTokensAreUsedUpAfterSelection() {
            var strategy = new WeightedOptionTable<string>();
            strategy.AddEntry("power attack", 5000000);
            var character = new CharacterSheet();
            character.FeatTokens.Add(new FeatToken());
            selector.SelectFeats(character, strategy);
            Assert.AreEqual(0, character.FeatTokens.Count);
        }

        [Fact]
        public void IfNoPreferredFeatsArePossibleJustSelectRandomlyFromAnyPossible()
        {
            var strategy = new WeightedOptionTable<string>();
            var character = new CharacterSheet();
            character.FeatTokens.Add(new FeatToken());
            selector.SelectFeats(character, strategy);
            Assert.IsTrue(character.Feats.First() == powerattack || character.Feats.First() == empowerspell);
        }

        [Fact]
        public void FeatTokensCanSpecifyAFeatByName()
        {
            var strategy = new WeightedOptionTable<string>();
            strategy.AddEntry("power attack", 5000000);
            var character = new CharacterSheet();
            character.FeatTokens.Add(new FeatToken("Empower Spell"));
            selector.SelectFeats(character, strategy);
            Assert.That(character.Feats, Contains.Item(empowerspell));
        }
    }
}