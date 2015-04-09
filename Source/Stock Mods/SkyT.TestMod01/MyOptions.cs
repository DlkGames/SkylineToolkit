using SkylineToolkit.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyT.TestMod01
{
    public class MyOptions : ModOptions
    {
        public MyOptions(TestMod01 mod)
            : base(mod)
        {
        }

        [Setting]
        public bool TestBool { get; set; }

        [Setting("NameInSavedFile", typeof(TestCustomOptionSerializer))]
        public TestCustomOption NameInClass { get; set; }

        // Won't be saved, no attribute
        public bool TestBool2 { get; set; }

        [Setting]
        public int testInt;

        // Won't be saved, no attribute
        public int TestInt { get; set; }
    }
}
