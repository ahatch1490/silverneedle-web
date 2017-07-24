﻿//-----------------------------------------------------------------------
// <copyright file="YamlHelpers.cs" company="Short Leg Studio, LLC">
//     Copyright (c) Short Leg Studio, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SilverNeedle.Utility
{
    using System.IO;
    using YamlDotNet.RepresentationModel;
    using SilverNeedle.Serialization;

    /// <summary>
    /// Helper methods for dealing with Yaml
    /// </summary>
    public static class YamlHelpers
    {
        /// <summary>
        /// Parses a specific string into YamlNode
        /// </summary>
        /// <returns>The yaml parsed into an accessible class</returns>
        /// <param name="yamlString">Yaml string to parse out</param>
        public static YamlObjectStore ParseYaml(this string yamlString)
        {
            var input = new StringReader(yamlString);
            var yaml = new YamlStream();
            yaml.Load(input);
            return new YamlObjectStore(yaml.Documents[0].RootNode);
        }
    }
}