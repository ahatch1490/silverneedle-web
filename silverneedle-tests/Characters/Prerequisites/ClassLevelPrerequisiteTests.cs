// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters.Prerequisites
{
    using Xunit;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.Prerequisites;

    
    public class ClassLevelPrerequisiteTests
    {
        ClassLevelPrerequisite prereq;

        public ClassLevelPrerequisiteTests()
        {
            prereq = new ClassLevelPrerequisite("Fighter 4");
        }

        [Fact]
        public void CharacterIsQualifiedIfHasTheSameClassAtAppropriateLevel()
        {
            var character = new CharacterSheet();
            var fighter = new Class();
            fighter.Name = "Fighter";
            character.SetClass(fighter);
            character.SetLevel(4);
            Assert.True(prereq.IsQualified(character));
        }


        [Fact]
        public void CharacterIsNotQualifiedIfWrongClassButRightLevel()
        {
            var character = new CharacterSheet();
            var wizard = new Class();
            wizard.Name = "Wizard";
            character.SetClass(wizard);
            character.SetLevel(4);
            Assert.False(prereq.IsQualified(character));
        }

        [Fact]
        public void CharacterIsNotQualifiedIfRightClassButToLowLevel()
        {
            var character = new CharacterSheet();
            var fighter = new Class();
            fighter.Name = "Fighter";
            character.SetClass(fighter);
            character.SetLevel(3);
            Assert.False(prereq.IsQualified(character));
        }

        [Fact]
        public void IfCharacterIsNotSetToAClassYouDefinitelyDoNotQualify()
        {
            var character = new CharacterSheet();
            Assert.False(prereq.IsQualified(character));
        }
    }
}