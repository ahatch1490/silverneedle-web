﻿// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Characters.Appearance
{
    public class CharacterAppearance
    {
        public CharacterAppearance()
        {
            HairColor = new HairColor("none");
            HairStyle = new HairStyle("none");
            FacialHair = new FacialHair("none");
            EyeColor = new EyeColor("none");
        }

        public EyeColor EyeColor { get; set; }
        public FacialHair FacialHair { get; set; }
        public HairColor HairColor { get; set; }
        public HairStyle HairStyle { get; set; }

        public string PhysicalAppearance { get; set; }

        public string Description { get; set; }
    }
}