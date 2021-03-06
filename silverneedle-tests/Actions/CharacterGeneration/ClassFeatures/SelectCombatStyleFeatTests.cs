// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Actions.CharacterGeneration.ClassFeatures
{
    using System.Linq;
    using Xunit;
    using SilverNeedle.Actions.CharacterGeneration.ClassFeatures;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.SpecialAbilities;
    using SilverNeedle.Serialization;
    using SilverNeedle.Utility;

    
    public class SelectCombatStyleFeatTests
    {
        [Fact]
        public void ChoosesAFeatFromTheAvailableFeats()
        {
            var yaml = @"
- name: Archery
  bonus-feats:
    - level: 1
      feats: [feat one]".ParseYaml().Children.First();
            var combatStyle = new CombatStyle(yaml);
            var character = CharacterTestTemplates.Ranger();
            character.SetLevel(2);
            character.Add(combatStyle);
            var step = new SelectCombatStyleFeat();
            step.ExecuteStep(character);
            AssertCharacter.HasFeatToken("feat one", character);
        }
    }

}