using SkylineToolkit.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyT.TestMod01
{
    public class TestCustomOptionSerializer : ISettingSerializer<TestCustomOption, string>
    {
        public string Serialize(IOptionsProvider provider, TestCustomOption setting)
        {
            return setting.Test + "##" + setting.Test2;
        }

        public TestCustomOption Deserialize(IOptionsProvider provider, string value)
        {
            string[] parts = value.Split(new string[]{ "##" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                throw new Exception("Invalid saved value.");
            }

            return new TestCustomOption() { Test = parts[0], Test2 = Int32.Parse(parts[1]) };
        }
    }
}
