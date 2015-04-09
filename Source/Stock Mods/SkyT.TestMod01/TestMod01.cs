using ICities;
using SkylineToolkit;
using SkylineToolkit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyT.TestMod01
{
    public class TestMod01 : Mod
    {
        public override string ModName
        {
            get { return "Test Mod 01 - Don't enable!"; }
        }

        public override string ModDescription
        {
            get { return "Test Mod 01 - Don't enable!"; }
        }

        protected override void OnApplicationStarted()
        {
            Log.Info("Game started from test mod.");
        }

        protected override void OnMainMenuLoaded()
        {
            Log.Info("Main menu loaded from test mod.");

            MyOptions myModOptions = new MyOptions(this);

            Log.Info("Test 1");

            myModOptions.TestBool = true;
            myModOptions.TestBool2 = false;
            myModOptions.testInt = 123;
            myModOptions.TestInt = 435;
            myModOptions.NameInClass = new TestCustomOption() { Test = "Teststring", Test2 = 542 };

            Log.Info("Test 2");

            myModOptions.Save();

            Log.Info("Test 3");
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        public override string Author
        {
            get { return "SkylineToolkit"; }
        }
    }
}
