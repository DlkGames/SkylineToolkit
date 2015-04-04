using ColossalFramework.UI;
using SkylineToolkit.UI;
using SkylineToolkit.UI.CustomControls;
using SkylineToolkit.UI.Styles;
using System.Linq;
using UnityEngine;

namespace SkylineToolkit.PermaMod
{
    public sealed class MainMenuModification : MonoBehaviour
    {
        MainMenu mainMenu;
        Button modOptionsButton;

        void OnEnable()
        {
            Log.Debug("MainMenu", "Enabled main menu modification");

            mainMenu = UIView.FindObjectOfType<MainMenu>();

            if (GameObject.Find("btn_ModOptions") == null)
            {
                modOptionsButton = CreateMenuItem("btn_ModOptions", "MOD OPTIONS", "Options");
            }
            else
            {
                modOptionsButton = Button.FromUIComponent<Button>(GameObject.Find("btn_ModOptions").GetComponent<UIButton>());
            }

            modOptionsButton.Click += btn_ModOptions_Click;
        }

        void OnDisable()
        {
            Log.Debug("MainMenu", "Disabled main menu modification");

            if (modOptionsButton == null)
            {
                return;
            }

            if (modOptionsButton.Parent != null)
            {
                modOptionsButton.Parent.RemoveControl<Button>(modOptionsButton);
            }
        }

        private void btn_ModOptions_Click(object sender, MouseEventArgs e)
        {
            Log.Debug("MainMenu", "Showing mod options window.");

            ModOptionsController.ShowOptionsWindow();
            
            //GameObject go = UITemplateManager.GetAsGameObject(ColossalTemplate.ScrollablePanelTemplate);
            //go.name = "test_scrollable_panel";

            //ColossalControl<UIComponent>.ColossalUIView.AttachUIComponent(go);
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

            Log.Debug("MainMenu", "Created main menu button {0} with label {1} after button {2}", name, label, insertAfterName);

            return new Button(button, true);
        }
    }
}
