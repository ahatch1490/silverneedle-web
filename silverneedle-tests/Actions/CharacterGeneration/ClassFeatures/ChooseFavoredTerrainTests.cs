// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGeneration.ClassFeatures
{
    using Xunit;
    using SilverNeedle.Actions.CharacterGeneration.ClassFeatures;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.SpecialAbilities;
    using SilverNeedle.Maps;
    using SilverNeedle.Serialization;

    
    public class ChooseFavoredTerrainTests
    {
        [Fact]
        public void ChooseFromFavoredTerrainList()
        {
            var forest = new TerrainType("Forest"); 
            var ctGateway = EntityGateway<TerrainType>.LoadWithSingleItem(forest);
            var configureStep = new ChooseFavoredTerrain(ctGateway);
            var character = new CharacterSheet();
            configureStep.ExecuteStep(character, new CharacterStrategy());
            
            var favTerrain = character.Get<FavoredTerrain>();
            Assert.Contains(forest, favTerrain.TerrainTypes);
        }
        
        [Fact]
        public void AddingASecondTerrainAppendsADifferentOneToTheList()
        {
            var forest = new TerrainType("Forest"); 
            var jungle = new TerrainType("Jungle");
            var terrainTypes = new TerrainType[] { forest, jungle }; 
            var ctGateway = EntityGateway<TerrainType>.LoadFromList(terrainTypes);
            var configureStep = new ChooseFavoredTerrain(ctGateway);
            var character = new CharacterSheet();
            configureStep.ExecuteStep(character, new CharacterStrategy());
            configureStep.ExecuteStep(character, new CharacterStrategy());
            
            var favTerrain = character.Get<FavoredTerrain>();
            Assert.Contains(forest, favTerrain.TerrainTypes);
            Assert.Contains(jungle, favTerrain.TerrainTypes);

        }
    }
}