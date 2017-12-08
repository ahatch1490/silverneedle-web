// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT


namespace SilverNeedle.Characters.Magic
{
    using System.Collections.Generic;
    using SilverNeedle.Serialization;
    using SilverNeedle.Utility;
    public class ArcaneSchool : IArcaneSchool
    {
        private IObjectStore abilities;
        public string Name { get; private set; }
        public bool NoOppositionSchools { get; private set; }
        private ArcaneSchool() { }
        public ArcaneSchool(IObjectStore configuration)
        {
            this.abilities = configuration.GetObject("abilities");
            this.Name = configuration.GetString("name");
            this.NoOppositionSchools = configuration.GetBoolOptional("no-opposition-schools");
        }
        public IEnumerable<IObjectStore> GetAbilities()
        {
            return abilities.Children;
        }
        public bool Matches(string name)
        {
            return this.Name.EqualsIgnoreCase(name);
        }

        public void AddPower(int level, string fullTypeName)
        {
            var obj = new MemoryStore();
            obj.SetValue("level", level);
            obj.SetValue("ability", fullTypeName);

            var memstore = (MemoryStore)abilities;
            memstore.AddListItem(obj);
        }

        public static ArcaneSchool CreateForTesting(string name, bool ignoreOpposition)
        {
            var arcane = new ArcaneSchool();
            arcane.Name = name;
            arcane.NoOppositionSchools = ignoreOpposition;
            arcane.abilities = new MemoryStore();
            return arcane;
        }
    }
}