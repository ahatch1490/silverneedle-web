﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters {
    using NUnit.Framework;
    using SilverNeedle.Characters;
    using System.Linq;
    using System.Collections.Generic;
    using SilverNeedle.Serialization;
	using SilverNeedle.Utility;

	[TestFixture]
	public class PrerequisiteTests {
		[Test]
		public void ParseSomeYaml() {
			var yamlNode = PrerequisitesYaml.ParseYaml();
			var prereq = yamlNode.GetObject ("prerequisites");

			var prereqs = new Prerequisites (prereq);

			Assert.AreEqual (4, prereqs.Count);
            Assert.IsInstanceOf<Prerequisites.AbilityPrerequisite> (prereqs.First ());
		}

		[Test]
		public void AlwaysQualifiedIfNoQualificationsNeeded() {
			var pre = new Prerequisites ();
			Assert.IsTrue(pre.IsQualified(new CharacterSheet(new List<Skill>())));
		}

		[Test]
		public void AbilityIsQualifiedIfExceedingScore() {
            var pre = new Prerequisites.AbilityPrerequisite ("Intelligence 13");
			var c = new CharacterSheet (new List<Skill>());
			c.AbilityScores.SetScore (AbilityScoreTypes.Intelligence, 15);
			Assert.IsTrue (pre.IsQualified (c));
		}

		[Test]
		public void AbilityIsNotQualifiedIfNotExceedingScore() {
            var pre = new Prerequisites.AbilityPrerequisite ("Intelligence 13");
			var c = new CharacterSheet (new List<Skill>());
			c.AbilityScores.SetScore (AbilityScoreTypes.Intelligence, 11);
			Assert.IsFalse (pre.IsQualified (c));
		}




		private const string PrerequisitesYaml = @"--- 
prerequisites:
  - ability: Intelligence 13
  - race: Elf
  - feat: Weapon Finesse
  - skillranks: Acrobatics 4
";
	}
}

