// Copyright (c) 2016 Trevor Redfern
// 
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

namespace Tests.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using SilverNeedle.Actions.CharacterGeneration;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.Prerequisites;
    using SilverNeedle.Serialization;
    using SilverNeedle.Utility;

    
    public class FeatSelectorTests
    {
        Feat powerattack;
        Feat cleave;
        Feat empowerspell;

        FeatSelector selector;
        EntityGateway<Feat> gateway;

        public FeatSelectorTests()
        {
            powerattack = Feat.Named("Power Attack");

            cleave = Feat.Named("Cleave");
            cleave.Prerequisites.Add(new SpecialAbilityPrerequisite("power attack"));

            empowerspell = Feat.Named("Empower Spell");
            empowerspell.Tags.Add("metamagic");

            var list = new List<Feat>();
            list.Add(powerattack);
            list.Add(cleave);
            list.Add(empowerspell);
            gateway = EntityGateway<Feat>.LoadFromList(list);
            selector = new FeatSelector(gateway);
        }
        [Fact]
        public void ChooseFeatBasedOnStrategy()
        {
            
            var buildStrategy = new CharacterStrategy();
            buildStrategy.FavoredFeats.AddEntry("power attack", 5000000);
            buildStrategy.FavoredFeats.AddEntry("cleave", 1);
            var character = new CharacterSheet(buildStrategy);
            character.Add(new FeatToken());
            
            selector.ExecuteStep(character);
            Assert.Equal(powerattack, character.Feats.First()); 

        }

        [Fact]
        public void OnlySelectsFeatsThatCharacterQualifiesFor()
        {
            var buildStrategy = new CharacterStrategy();
            buildStrategy.FavoredFeats.AddEntry("power attack", 1);
            buildStrategy.FavoredFeats.AddEntry("cleave", 5000000);
            for(int i = 0; i < 1000; i++)
            {
                var character = new CharacterSheet(buildStrategy);
                character.Add(new FeatToken());

                selector.ExecuteStep(character);
                Assert.Equal(powerattack, character.Feats.First());
            }            
        } 

        [Fact]
        public void SelectFeatsBasedOnTokensAvailable() 
        {
            var buildStrategy = new CharacterStrategy();
            buildStrategy.FavoredFeats.AddEntry("power attack", 5000000);
            buildStrategy.FavoredFeats.AddEntry("empower spell", 1);

            for(int i = 0; i < 1000; i++)
            {
                var character = new CharacterSheet(buildStrategy);
                character.Add(new FeatToken("metamagic"));

                selector.ExecuteStep(character);
                Assert.Equal(empowerspell, character.Feats.First());
            }            
        }

        [Fact]
        public void SelectsAFeatForEachOutstandingToken() {
            var buildStrategy = new CharacterStrategy();
            buildStrategy.FavoredFeats.AddEntry("power attack", 5000000);
            buildStrategy.FavoredFeats.AddEntry("empower spell", 1);

            for(int i = 0; i < 1000; i++)
            {
                var character = new CharacterSheet(buildStrategy);
                character.Add(new FeatToken("metamagic"));
                character.Add(new FeatToken());

                selector.ExecuteStep(character);
                Assert.True(character.Feats.Contains(empowerspell));
                Assert.True(character.Feats.Contains(powerattack));
            }        
        }

        [Fact]
        public void FeatTokensAreUsedUpAfterSelection() {
            var buildStrategy = new CharacterStrategy();
            buildStrategy.FavoredFeats.AddEntry("power attack", 5000000);

            var character = new CharacterSheet(buildStrategy);
            character.Add(new FeatToken());

            selector.ExecuteStep(character);
            Assert.Equal(0, character.FeatTokens.Count());
        }

        [Fact]
        public void IfNoPreferredFeatsArePossibleJustSelectRandomlyFromAnyPossible()
        {
            var buildStrategy = new CharacterStrategy();
            var character = new CharacterSheet(buildStrategy);
            character.Add(new FeatToken());
            selector.ExecuteStep(character);
            Assert.True(character.Feats.First().Equals(powerattack) || 
                character.Feats.First().Equals(empowerspell));
        }

        [Fact]
        public void FeatTokensCanSpecifyAFeatByName()
        {
            var buildStrategy = new CharacterStrategy();
            buildStrategy.FavoredFeats.AddEntry("power attack", 5000000);

            var character = new CharacterSheet(buildStrategy);
            character.Add(new FeatToken("Empower Spell"));

            selector.ExecuteStep(character);
            Assert.Contains(empowerspell, character.Feats);
        }

        [Fact]
        public void IgnorePrerequisitesIfTokenSaysSo()
        {
            Assert.True(cleave.Prerequisites.Count > 0);
            var bob = CharacterTestTemplates.AverageBob();
            bob.Add(new FeatToken(new string[] { "cleave" } , true));
            selector.ExecuteStep(bob);
            Assert.Contains(cleave, bob.Feats);
        }

        [Fact]
        public void ThrowExceptionIfAFeatTokenCannotResolveProperly()
        {
            var bob = CharacterTestTemplates.AverageBob();
            bob.Add(new FeatToken(new string[] { "impossible" }, false));
            Assert.Throws(
                typeof(CharacterGeneratorStepFailedException), 
                () => selector.ExecuteStep(bob)
            ); 
        }
    }
}