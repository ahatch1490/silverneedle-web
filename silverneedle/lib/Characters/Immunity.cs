// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters
{
    using SilverNeedle.Serialization;

    public class Immunity  : IResistance
    {
        public string DamageType { get; private set; }
        public bool IsImmune { get { return true; } }
        public Immunity(string immunity)
        {
            DamageType = immunity;
        }

        public Immunity(IObjectStore configuration)
        {
            this.DamageType = configuration.GetString("damage-type");
        }

        public string DisplayString()
        {
            return "{0} Immunity".Formatted(DamageType);
        }
    }
}