// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Serialization
{
    using Xunit;
    using SilverNeedle;

    public class GatewayProviderTests : RequiresDataFiles
    {
        [Fact]
        public void LoadsGatewayForCharacterCreator()
        {
            var creatorGateway = GatewayProvider.Get<SilverNeedle.Actions.CharacterGenerator.CharacterDesigner>();
            Assert.NotNull(creatorGateway);
            Assert.True(creatorGateway.Count() > 0);
        }
    }
}