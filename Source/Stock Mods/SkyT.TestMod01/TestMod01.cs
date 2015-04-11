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

            myModOptions.TestBool = true;
            myModOptions.TestBool2 = false;
            myModOptions.testInt = 123;
            myModOptions.TestInt = 435;
            myModOptions.NameInClass = new TestCustomOption() { Test = "Teststring", Test2 = 542 };

            myModOptions.Save();

            Log.Info(myModOptions.TestBool3);
            Log.Info(myModOptions.NameInClass.Test2);
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
