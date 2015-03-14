using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit
{
    public sealed class StLoadingExtension : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            ReactivateAchivements();
        }

        private static void ReactivateAchivements()
        {
            Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;

            Log.Info("Re-enabled achivements.");

            var uiView = GameObject.FindObjectOfType<UIView>();
            if (uiView == null) return;

            // Create a GameObject with a ColossalFramework.UI.UIButton component.
            var buttonObject = new GameObject("MyButton", typeof(UIButton));

            // Make the buttonObject a child of the uiView.
            buttonObject.transform.parent = uiView.transform;

            // Get the button component.
            var button = buttonObject.GetComponent<UIButton>();

            // Set the text to show on the button.
            button.text = "Click Me!";

            // Set the button dimensions.
            button.width = 100;
            button.height = 30;

            // Style the button to look like a menu button.
            button.normalBgSprite = "ButtonMenu";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.focusedBgSprite = "ButtonMenuFocused";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.textColor = new Color32(255, 255, 255, 255);
            button.disabledTextColor = new Color32(7, 7, 7, 255);
            button.hoveredTextColor = new Color32(7, 132, 255, 255);
            button.focusedTextColor = new Color32(255, 255, 255, 255);
            button.pressedTextColor = new Color32(30, 30, 44, 255);

            // Place the button.
            button.transformPosition = new Vector3(-1.65f, 0.97f); 
        }
    }
}
