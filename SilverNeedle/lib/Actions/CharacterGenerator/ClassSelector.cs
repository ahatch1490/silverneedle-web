// Copyright (c) 2016 Trevor Redfern
// 
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System;
using System.Linq;
using SilverNeedle.Characters;
using SilverNeedle.Utility;

namespace SilverNeedle.Actions.CharacterGenerator
{
    public class ClassSelector : ICharacterDesignStep
    {
        private EntityGateway<Class> classes; 
        
        public ClassSelector(EntityGateway<Class> gateway)
        {
            classes = gateway;
        }
        
        public ClassSelector()
        {
            classes = GatewayProvider.Get<Class>();
        }
    
        public void ChooseAny(CharacterSheet character)
        {
            AssignClass(character, classes.All().ToList().ChooseOne());
        }

        public void ChooseClass(CharacterSheet character, WeightedOptionTable<string> classChoices)
        {
            if (classChoices.IsEmpty)
            {
                ChooseAny(character);
                return;
            }
            
            var choice = classChoices.ChooseRandomly();
            var cls = classes.Find(choice);
            AssignClass(character, cls);
        }   

        public void AssignClass(CharacterSheet character, Class cls) 
        {
            character.SetClass(cls);
            
            foreach(var skill in cls.ClassSkills)
            {
                character.SkillRanks.SetClassSkill(skill);
            }
            var firstClassLevel = cls.GetLevel(1);
            character.AddLevelAbilities(firstClassLevel);            
        }

        public void Process(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            ChooseClass(character, strategy.Classes);
        }
    }
}