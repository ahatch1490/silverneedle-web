// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle.Utility {
    using System;
    public static class Reflector {
        public static T Instantiate<T>(this System.Type type, params IObjectStore[] constructor) 
        {
            return (T)Activator.CreateInstance(type, constructor);
        }

        public static T Instantiate<T>(this string typeName, params IObjectStore[] constructor)
        {
            var type = System.Type.GetType(typeName);
            ShortLog.DebugFormat("Type is: {0}", type.ToString());
            return type.Instantiate<T>(constructor);
        }
    }
}