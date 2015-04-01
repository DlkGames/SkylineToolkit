using ColossalFramework;
using ColossalFramework.UI;
using SkylineToolkit.Debugging;
using SkylineToolkit.Debugging.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.PermaMod
{
    public sealed class SkylineToolkitPermaMod : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);

            if (DebugConsole.Instance != null)
            {
                RegisterDebugCommands();

                Log.Info("PermaMod", "Debug console created. Open with {0}.", Debugging.DebugConsole.Instance.toggleKey);
            }
        }

        void OnLevelWasLoaded(int level)
        {
            Log.Info("Loaded new level: {0} {1}", level, Application.loadedLevelName);
        }

        private void RegisterDebugCommands()
        {
            DelegateCommand command = new DelegateCommand(ListUITemplates, true);

            command.HelpText = "Lists all currently loaded UI templates from the Colossal UITemplateManager.";
            command.UsageText = "list-ui-templates [filter]";

            DebugConsole.RegisterCommand("list-ui-templates", command);
        }
        
        // TODO: Move to other class / own class?
        private int ListUITemplates(ICommandContext context, string[] args, char[] flags)
        {
            Dictionary<string, UIComponent> templates;
            
            try
            {
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
                FieldInfo field = typeof(UITemplateManager).GetField("m_Templates", bindingFlags);

                templates = (Dictionary<string, UIComponent>)field.GetValue(Singleton<UITemplateManager>.instance);
            }
            catch (Exception ex)
            {
                Log.Exception("Command", ex);

                return 1;
            }

            StringBuilder result = new StringBuilder();

            foreach (var key in templates.Keys)
            {
                if (args.Length > 0)
                {
                    if (key.ToLower().Contains(args[0].ToLower()))
                    {
                        result.AppendLine(key);
                    }
                }
                else
                {
                    result.AppendLine(key);
                }
            }

            context.SendOutput(result.ToString());

            return 0;
        }
    }
}
