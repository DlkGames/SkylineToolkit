using ICities;
using SkylineToolkit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyT.TestMod01
{
    public class TestMod01 : IUserMod
    {
        
        public string Description
        {
            get
            {
                return "TEST MOD, DO NOT ACTIVATE!";
            }
        }

        public string Name
        {
            get
            {


                Panel panel = new Panel("test_panel");

                panel.IsActive = true;

                return "TEST MOD 01";
            }
        }
    }
}
