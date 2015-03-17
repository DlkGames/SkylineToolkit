using ColossalFramework;
using ColossalFramework.UI;
using SkylineToolkit.Debugging;
using SkylineToolkit.Debugging.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SkylineToolkit.PermaMod
{
    public class MainMenuModification : MonoBehaviour
    {
        MainMenu mainMenu;

        void OnEnable()
        {
            mainMenu = UIView.FindObjectOfType<MainMenu>();

            UIButton modOptionsButton = CreateMenuItem("ModOptions", "MOD OPTIONS", "Options");

            modOptionsButton.eventClick += ModOptions;
        }

        private void ModOptions(UIComponent component, UIMouseEventParameter eventParam)
        {
            Log.Info("Mod options clicked");
        }

        private UIButton CreateMenuItem(string name, string text, string afterName)
        {
            GameObject gameObject = UITemplateManager.GetAsGameObject("MainMenuButtonTemplate");
            gameObject.name = name;
            UIButton button = (UIButton)mainMenu.component.AttachUIComponent(gameObject);
            button.text = text;

            UIButton[] buttons = mainMenu.GetComponentsInChildren<UIButton>();

            for (int i = buttons.Count() - 2; i >= 0; i--)
            {
                if (buttons[i].name == afterName)
                    break;

                button.MoveBackward();
            }

            return button;
        }
    }
}
