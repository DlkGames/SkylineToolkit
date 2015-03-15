using ColossalFramework.UI;
using SkylineToolkit.UI;
using SkylineToolkit.UI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.PermaMod
{
    public class MainMenuModification : MonoBehaviour
    {
        void OnEnable()
        {
            Button btn = new Button("btn_modOptions", "Mod Options", new Vector3(-1.65f, 0.97f), 180, 60);

            btn.IsActive = true;
            btn.AbsolutePosition = new Vector3(10f, 10f);

            btn.Click += (sender, e) =>
            {
                Log.Info("Open mod options..." + e.Clicks);
            };
        }
    }
}
