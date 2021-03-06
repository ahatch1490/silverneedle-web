﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Actions.CharacterGeneration
{
    using SilverNeedle.Characters;
    using SilverNeedle.Serialization;
    public class AssignAge : ICharacterDesignStep
    {
        EntityGateway<Maturity> maturityGateway;

        public AssignAge()
        {
            maturityGateway = GatewayProvider.Get<Maturity>();            
        }

        public void ExecuteStep(CharacterSheet character)
        {
            character.Age = RandomAge(character.Class.ClassDevelopmentAge, maturityGateway.Find(character.Race.Name));
        }

        public int RandomAge(ClassDevelopmentAge classDevAge, Maturity maturity)
        {
            switch (classDevAge)
            {
                case ClassDevelopmentAge.Young:
                    return maturity.Adulthood + maturity.Young.Roll();
                case ClassDevelopmentAge.Trained:
                    return maturity.Adulthood + maturity.Trained.Roll();
                case ClassDevelopmentAge.Studied:
                    return maturity.Adulthood + maturity.Studied.Roll();
            }
            return 0;
        }
    }
}

