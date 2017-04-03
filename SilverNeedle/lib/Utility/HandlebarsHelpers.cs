// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Utility
{
    using HandlebarsDotNet;
    using SilverNeedle.General;
    using SilverNeedle.Treasure;

    public class HandlebarsHelpers
    {
        public static void ConfigureHelpers() {
            Handlebars.RegisterHelper("descriptor", (writer, context, parameters) => {
                var value = context.descriptors[parameters[0].ToString()] as string[];
                writer.Write(value.ChooseOne());
            });

            Handlebars.RegisterHelper("choose-color", (writer, context, parameters) => {
                var color = GatewayProvider.Get<Color>().ChooseOne();
                writer.Write(color.Name);
            });

            Handlebars.RegisterHelper("choose-gem", (writer, context, parameters) => {
                var gem = GatewayProvider.Get<Gem>().ChooseOne();
                writer.Write(gem.Name);
            });
        }
    }
}