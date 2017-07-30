// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGenerator.ClassFeatures
{
    using Xunit;
    using SilverNeedle.Actions.CharacterGenerator.ClassFeatures;
    using SilverNeedle.Bestiary;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.SpecialAbilities;
    using SilverNeedle.Serialization;

    
    public class ChooseFavoredEnemyTests
    {
        [Fact]
        public void ChooseFromFavoredEnemyList()
        {
            var aberration = new CreatureType("Aberration"); 
            var creatureTypes = new CreatureType[] { aberration }; 
            var ctGateway = new EntityGateway<CreatureType>(creatureTypes);
            var configureStep = new ChooseFavoredEnemy(ctGateway);
            var character = new CharacterSheet();
            configureStep.Process(character, new CharacterBuildStrategy());
            
            var favEnemy = character.Get<FavoredEnemy>();
            Assert.Contains(aberration, favEnemy.CreatureTypes);
        }

        [Fact]
        public void AddingASecondEnemyAppendsADifferentOneToTheList()
        {
            var aberration = new CreatureType("Aberration"); 
            var dragon = new CreatureType("Dragon"); 
            var creatureTypes = new CreatureType[] { aberration, dragon }; 
            var ctGateway = new EntityGateway<CreatureType>(creatureTypes);
            var configureStep = new ChooseFavoredEnemy(ctGateway);
            var character = new CharacterSheet();
            configureStep.Process(character, new CharacterBuildStrategy());
            configureStep.Process(character, new CharacterBuildStrategy());
            
            var favEnemy = character.Get<FavoredEnemy>();
            Assert.Contains(aberration, favEnemy.CreatureTypes);
            Assert.Contains(dragon, favEnemy.CreatureTypes);
        }
    }
}