// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Spells
{
    using System.Collections.Generic;
    using System.Linq;
    using SilverNeedle.Characters.Magic;
    using SilverNeedle.Serialization;

    public class SpellList : IGatewayObject
    {
        private EntityGateway<Spell> spellInformation;
        public string Class { get; private set; }
        private Dictionary<int, IList<string>> Levels = new Dictionary<int, IList<string>>();

        private SpellList(EntityGateway<Spell> gateway)
        {
            spellInformation = gateway;
        }

        public SpellList()
        {
            spellInformation = GatewayProvider.Get<Spell>();
        }


        public SpellList(IObjectStore data) : this()
        {
            Class = data.GetString("class");

            var levelData = data.GetObject("levels");
            foreach(var key in levelData.Keys)
            {
                var level = int.Parse(key);
                var spells = levelData.GetList(key);
                Levels.Add(level, spells);
            }
        }

        public int GetLowestSpellLevel()
        {
            return Levels.Keys.Min();
        }
        public int GetHighestSpellLevel()
        {
            return Levels.Keys.Max();
        }

        public void Add(int level, string spellName)
        {
            AddLevelIfMissing(level);
            if(!Levels[level].Contains(spellName))
                Levels[level].Add(spellName);
        }

        private void AddLevelIfMissing(int level)
        {
            if(!Levels.ContainsKey(level))
            {
                Levels.Add(level, new List<string>());
            }
        }

        public IEnumerable<KeyValuePair<int, IList<string>>> FilterByMaxLevel(int maxLevel)
        {
            return this.Levels.Where(x => x.Key <= maxLevel);
        }

        public int GetSpellLevel(string spellName)
        {
            foreach(var spells in this.Levels)
            {
                if(spells.Value.Contains(spellName))
                    return spells.Key;
            }

            throw new SpellNotFoundException(spellName);
        }

        public IEnumerable<string> GetAllSpells(int level)
        {
            if(!Levels.ContainsKey(level))
                return new string[] { };
            return Levels[level];
        }

        public IEnumerable<string> GetSpells(int level, IEnumerable<ISpellCastingRule> rules)
        {
            return GetAllSpells(level)
                .Where(spellName =>  
                    rules.All(rule => rule.CanCastSpell(spellInformation.Find(spellName)))
                );
        }

        public bool Matches(string cls)
        {
            return Class.EqualsIgnoreCase(cls);
        }

        public static SpellList CreateForTesting(string className)
        {
            return CreateForTesting(className, GatewayProvider.Get<Spell>());
        }

        public static SpellList CreateForTesting(string className, EntityGateway<Spell> spellList)
        {
            var sl = new SpellList(spellList);
            sl.Class = className;
            return sl;
        }
    }
}