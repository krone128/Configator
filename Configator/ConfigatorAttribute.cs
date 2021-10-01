using System;
using System.Runtime.CompilerServices;

namespace Configator
{
    public class ConfigatorAttribute : Attribute
    {
        public readonly string ConfigPath;
        
        public ConfigatorAttribute(string configPath)
        {
            ConfigPath = configPath;
        }
    }
}