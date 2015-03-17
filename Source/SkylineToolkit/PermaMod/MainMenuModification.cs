using ColossalFramework;
using ColossalFramework.UI;
using SkylineToolkit.Debugging;
using SkylineToolkit.Debugging.Commands;
using SkylineToolkit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SkylineToolkit.PermaMod
{
    public class MainMenuModification : MonoBehaviour
    {
        MainMenu mainMenu;
        Button modOptionsButton;

        void OnEnable()
        {
            mainMenu = UIView.FindObjectOfType<MainMenu>();

            modOptionsButton = CreateMenuItem("btn_ModOptions", "MOD OPTIONS", "Options");

            modOptionsButton.Click += btn_ModOptions_Click;
        }

        private void btn_ModOptions_Click(object sender, MouseEventArgs e)
        {
            Log.Info("Mod options clicked");
        }

        private Button CreateMenuItem(string name, string label, string insertAfterName)
        {
            // TODO: Create a proper Wrapper for UITemplateManager
            // TODO: AttachUIComponent in ColossalControl
            GameObject gameObject = UITemplateManager.GetAsGameObject("MainMenuButtonTemplate");
            gameObject.name = name;

            UIButton button = (UIButton)mainMenu.component.AttachUIComponent(gameObject);
            button.text = label;

            UIButton[] buttons = mainMenu.GetComponentsInChildren<UIButton>();

            for (int i = buttons.Count() - 2; i >= 0; i--)
            {
                if (buttons[i].name == insertAfterName)
                    break;

                button.MoveBackward();
            }

            return new Button(button);
        }
    }
}
