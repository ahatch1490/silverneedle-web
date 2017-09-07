// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Characters
{
    using Xunit;
    using SilverNeedle.Characters;
    using SilverNeedle.Serialization;

    
    public class OccupationTests
    {
        [Fact]
        public void MatchesOnName()
        {
            var occ = new Occupation();
            occ.Name = "foo";
            Assert.True(occ.Matches("foo"));
        }

        [Fact]
        public void MatchesWithTags()
        {
            var occ = new Occupation("peasant", "commoner", new string[] { "working-class", "poor" });
            Assert.True(occ.MatchAnyTags(new string[] { "missing", "poor" }));
            Assert.False(occ.MatchAnyTags(new string[] { }));
        }
    }
}