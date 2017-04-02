// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT


namespace SilverNeedle.Actions.CharacterGenerator
{
    using SilverNeedle.Characters;
    using SilverNeedle.Serialization;

    public class BuildCharacterSkillList : ICharacterDesignStep
    {
        IEntityGateway<Skill> skills;

        public BuildCharacterSkillList()
        {
            skills = GatewayProvider.Get<Skill>();
        }

        public void Process(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            character.SkillRanks.FillSkills(skills.All(), character.AbilityScores);
        }
    }
}