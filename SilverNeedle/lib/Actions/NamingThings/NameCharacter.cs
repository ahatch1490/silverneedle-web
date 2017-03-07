﻿// //-----------------------------------------------------------------------
// // <copyright file="NameCharacter.cs" company="Short Leg Studio, LLC">
// //     Copyright (c) Short Leg Studio, LLC. All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------
using SilverNeedle.Names;
using SilverNeedle.Characters;

namespace SilverNeedle.Actions.NamingThings
{
    using System;
    using SilverNeedle;

    public class NameCharacter : ICharacterBuildStep
    {
        private ICharacterNamesGateway namesGateway;

        public NameCharacter(ICharacterNamesGateway namesGateway)
        {
            this.namesGateway = namesGateway;    
        }

        public NameCharacter()
        {
            this.namesGateway = GatewayProvider.Instance().Names;
        }

        public string CreateFullName(Gender gender, string race)
        {
            var firstName = namesGateway.GetFirstNames(gender, race).ChooseOne();
            var lastName = namesGateway.GetLastNames(race).ChooseOne();

            return string.Format("{0} {1}", firstName, lastName);
        }

        public void ProcessFirstLevel(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            character.Name = CreateFullName(character.Gender, character.Race.Name);
        }

        public void ProcessLevelUp(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            throw new NotImplementedException();
        }
    }
}

