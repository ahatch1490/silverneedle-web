// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Lexicon
{
    using SilverNeedle.Serialization;

    [ObjectStoreSerializable]
    public class Color : ILexiconGatewayObject
    {
        [ObjectStore("id")]
        public string ID { get; set; }

        [ObjectStore("name")]
        public string Name { get; set; }

        [ObjectStore("hex")]
        public string Hex { get; set; }

        [ObjectStore("red")]
        public int Red { get; set; }

        [ObjectStore("green")]
        public int Green { get; set; }
        
        [ObjectStore("blue")]
        public int Blue { get; set; }

        public bool Matches(string name)
        {
            return this.Name.EqualsIgnoreCase(name);
        }

        public Color() { }
        public Color(string name, int r, int g, int b)
        {
            this.Name = name;
            this.Red = r;
            this.Green = g;
            this.Blue = b;
        }
    }
}