using ColossalFramework.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit
{
    public static class SkylineGame
    {
        public static string CompanyName
        {
            get
            {
                return DataLocation.companyName;
            }
        }

        public static string ProductName
        {
            get
            {
                return DataLocation.productName;
            }
        }

        public static uint ProductVersion
        {
            get
            {
                return DataLocation.productVersion;
            }
        }

        public static string DetailedProductVersion
        {
            get
            {
                return DataLocation.productVersionString;
            }
        }
    }
}
