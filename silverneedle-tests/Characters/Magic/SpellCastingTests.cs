// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters.Magic
{
    using Xunit;
    using SilverNeedle;
    using SilverNeedle.Characters;
    using SilverNeedle.Characters.Magic;
    using SilverNeedle.Equipment;
    using SilverNeedle.Spells;
    using SilverNeedle.Utility;

    
    public class SpellCastingTests
    {
        [Fact]
        public void TracksAvailableSpellsForTheCharacter()
        {
            var cls = new ClassLevel(new Class());
            var spellcasting = new SpellCasting(new Inventory(), cls, "wizard");
            spellcasting.AddSpells(0, new Spell[] { new Spell("cantrip1", "evocation"), new Spell("cantrip2", "evocation") });
            Assert.NotStrictEqual(spellcasting.GetAvailableSpells(0), new string[] { "cantrip1", "cantrip2" });
        }

        [Fact]
        public void IfDependentOnSpellbookPullsAvailableFromSpellbook()
        {
            var inventory = new Inventory();
            var cls = new ClassLevel(new Class());
            var spellcasting = new SpellCasting(inventory, cls, "wizard");
            spellcasting.SpellsKnown = SpellsKnown.Spellbook;

            // Make a spellbook
            var spellbook = new Spellbook();
            spellbook.AddSpells(0, new string[] { "cantrip1", "cantrip2" });
            inventory.AddGear(spellbook);

            Assert.NotStrictEqual(spellcasting.GetAvailableSpells(0), new string[] { "cantrip1", "cantrip2" });
        }

        [Fact]
        public void MaxLevelIsSetForNowByTheMaxLevelKnown()
        {
            var cls = new ClassLevel(new Class());
            var spells = new SpellCasting(new Inventory(), cls, "wizard"); 
            spells.SpellsKnown = SpellsKnown.All;
            spells.AddSpells(0, new Spell[] { new Spell("foo", "bar") });
            spells.AddSpells(1, new Spell[] { new Spell("foo", "bar") });
            spells.AddSpells(2, new Spell[] { new Spell("foo", "bar") });

            Assert.Equal(spells.MaxLevel, 2);
        }

        [Fact]
        public void CanSpecifyTheNumberOfSpellsPerDay()
        {
            var cls = new ClassLevel(new Class());
            var spells = new SpellCasting(new Inventory(), cls, "wizard");
            spells.SetSpellsPerDay(0, 3);
            spells.SetSpellsPerDay(1, 1);

            Assert.Equal(spells.GetSpellsPerDay(0), 3);
            Assert.Equal(spells.GetSpellsPerDay(1), 1);
            Assert.Equal(spells.GetSpellsPerDay(2), 0);
        }

        [Fact]
        public void SpellsCanBePrepared()
        {
            var cls = new ClassLevel(new Class());
            var spells = new SpellCasting(new Inventory(), cls, "wizard");
            spells.AddSpells(0, new Spell[] { new Spell("cantrip1", "evocation"), new Spell("cantrip2", "evocation") });
            spells.PrepareSpells(0, new string[] { "cantrip1" });
            Assert.NotStrictEqual(spells.GetPreparedSpells(0), new string[] {"cantrip1"});
        }

        [Fact]
        public void CalculatesTheDCBasedOnSpellLevelAndAbility()
        {
            var cls = new ClassLevel(new Class());
            var spells = new SpellCasting(new Inventory(), cls, "wizard");
            var abilityScore = new AbilityScore(AbilityScoreTypes.Intelligence, 18);
            spells.SetCastingAbility(abilityScore);
            Assert.Equal(spells.GetDifficultyClass(0), 14);
            Assert.Equal(spells.GetDifficultyClass(3), 17);
            Assert.Equal(spells.GetDifficultyClass(9), 23);
        }

        [Fact]
        public void IfAskedForSpellsPastMaxLevelJustReturnZero()
        {
            var cls = new ClassLevel(new Class());
            var spells = new SpellCasting(new Inventory(), cls, "wizard");
            Assert.Equal(spells.GetSpellsPerDay(200), 0);

        }

        [Fact]
        public void RulesCanBeAppliedToSpellCastingThatLimitsAvailableSpells()
        {
            var cls = new ClassLevel(new Class());
            var spellcasting = new SpellCasting(new Inventory(), cls, "wizard");
            spellcasting.AddSpells(0, new Spell[] { new Spell("cantrip1", "conjuration"), new Spell("cantrip2", "evocation") });
            var cannotCastConjuration = new CannotCastConjuration();
            spellcasting.AddRule(cannotCastConjuration);
            Assert.NotStrictEqual(spellcasting.GetAvailableSpells(0), new string[] { "cantrip2" });
        }

        [Fact]
        public void IfTryingToPrepareSpellsAndThereAreNoKnownSpellsDoNothing()
        {
            var sc = new SpellCasting(new Inventory(), new ClassLevel(new Class()), "wizard");
            // DOES NOT THROW
            sc.PrepareSpells(0, new string[] { }); 
        }

        [Fact]
        public void AddingMoreSpellsToTheSpellCasterJustAppendsDoesNotReplace()
        {
            var sc = new SpellCasting(new Inventory(), new ClassLevel(new Class()), "wizard");
            sc.AddSpells(0, new Spell[] { new Spell("cantrip1", "evocation"), new Spell("cantrip2", "evocation") });
            sc.AddSpells(0, new Spell[] { new Spell("cantrip3", "evocation"), new Spell("cantrip4", "evocation") });
            Assert.NotStrictEqual(sc.GetAvailableSpells(0), new string[] { "cantrip1", "cantrip2", "cantrip3", "cantrip4" });
        }

        public class CannotCastConjuration : ISpellCastingRule
        {
            public bool CanCastSpell(Spell spell)
            {
                return !spell.School.EqualsIgnoreCase("conjuration");
            }
        }
    }
}