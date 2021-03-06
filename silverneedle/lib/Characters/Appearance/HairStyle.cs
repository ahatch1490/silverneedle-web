// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters.Appearance
{
    using SilverNeedle.Lexicon;
    using SilverNeedle.Serialization;

    public class HairStyle : TemplateSentenceGenerator
    {

        public HairStyle(IObjectStore data) : base(data)
        {
        }

        public HairStyle(string name) : base(name)
        {
        }
    }
}