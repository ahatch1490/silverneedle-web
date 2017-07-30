// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters.Domains
{
    using Xunit;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.Domains;
    using SilverNeedle.Characters.SpecialAbilities;
    using SilverNeedle.Serialization;

    
    public class AnimalDomainTests
    {
        private Animal animalDomain;
        private CharacterSheet character;

        public AnimalDomainTests()
        {
            var data = new MemoryStore();
            data.SetValue("spells", "");
            data.SetValue("name", "animal");
            animalDomain = new Animal(data);

            character = new CharacterSheet();
            character.InitializeComponents();
            var Class = new Class("cleric");
            character.SetClass(Class);
            character.AbilityScores.SetScore(AbilityScoreTypes.Wisdom, 16);
            character.Add(animalDomain);
        }

        [Fact]
        public void CanSpeakWithAnimals()
        {
            var speak = character.Get<SpeakWithAnimals>();
            Assert.NotNull(speak);
        }

        [Fact]
        public void GainsAnimalCompanionAtLevelFour()
        {
            character.SetLevel(4);
            animalDomain.LeveledUp(character.Components);
            var animalCompanion = character.Get<AnimalCompanion>();
            Assert.NotNull(animalCompanion);
        }
    }
}