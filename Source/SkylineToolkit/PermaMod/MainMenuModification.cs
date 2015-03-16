using ColossalFramework.UI;
using SkylineToolkit.UI;
using SkylineToolkit.UI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using ColossalFramework;

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

        UIButton CreateMenuItem(string name, string text, string afterName)
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

        // TODO: Move this to a command?
        void ListTemplates()
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = typeof(UITemplateManager).GetField("m_Templates", flags);
            Dictionary<string, UIComponent> templates = (Dictionary<string, UIComponent>)field.GetValue(Singleton<UITemplateManager>.instance);
            foreach (var key in templates.Keys)
            {
                Log.Info(key);
            }
        }
    }
}
