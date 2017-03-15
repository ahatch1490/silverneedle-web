// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT


namespace SilverNeedle.Actions.CharacterGenerator
{
    using SilverNeedle.Characters;
    using SilverNeedle.Utility;

    public class DesignerExecuterStep : ICharacterDesignStep
    {
        private string designerName;
        private EntityGateway<CharacterDesigner> gateway;

        public DesignerExecuterStep(string name)
        {
            designerName = name;
        }

        public DesignerExecuterStep(string name, EntityGateway<CharacterDesigner> gateway)
        {
            designerName = name;
            this.gateway = gateway;
        }

        public virtual void Process(CharacterSheet character, CharacterBuildStrategy strategy)
        {
            var designer = FindDesigner(designerName);
            designer.Process(character, strategy);
        }

        private CharacterDesigner FindDesigner(string name)
        { 
            if(gateway == null)
                gateway = GatewayProvider.Get<CharacterDesigner>();
            
            return gateway.Find(designerName);
        }
    }
}