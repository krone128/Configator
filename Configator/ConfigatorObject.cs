using System;

namespace Configator
{
    [Serializable]
    [Configator("testConfig.json")]
    public class ConfigatorObject
    {
        public int Int = 213;
        string Str = "sfsef";
        float FloatProp { get; set; } = 23.1111f;
        int[] Arr = {2, 2, 2, 2, 5};

        public ConfigatorObject()
        {
            
        }
    }
}