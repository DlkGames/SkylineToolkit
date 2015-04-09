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
