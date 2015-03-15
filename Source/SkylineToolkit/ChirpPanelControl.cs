using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ColossalChirpPanel = ChirpPanel;

namespace SkylineToolkit
{
    public static class ChirpPanelControl
    {
        private static ColossalChirpPanel panel;

        public static ColossalChirpPanel Panel
        {
            get
            {
                if (panel == null)
                {
                    ColossalChirpPanel foundPanel = ColossalChirpPanel.instance;

                    if (foundPanel != null)
                    {
                        panel = (ColossalChirpPanel)foundPanel;
                    }
                }

                return panel;
            }
        }

        public static int MessageBufferSize
        {
            get
            {
                return Panel.m_MessageBufferSize;
            }
            set
            {
                Panel.m_MessageBufferSize = value;
            }
        }

        public static float MessageTimeout
        {
            get
            {
                return Panel.m_MessageTimeout;
            }
            set
            {
                Panel.m_MessageTimeout = value;
            }
        }

        public static bool IsVisible
        {
            get
            {
                return Panel.isShowing;
            }
        }

        public static void AddMessage(string message, uint senderId = 0, string senderName = "", bool playSound = true, float timeout = 0.6f, bool silent = false)
        {
            if (Panel == null)
            {
                return;
            }

            ChirperMessage chirperMessage = new ChirperMessage() {
                senderID = senderId,
                senderName = senderName,
                text = message
            };

            Panel.AddEntry(chirperMessage, !playSound);

            if (silent)
            {
                return;
            }

            Panel.Show(timeout);
        }

        internal static void LogMessage(string module, string message, MessageType type)
        {
            AddMessage(message, 0, module, false, 0.0f, false);
        }

        public static void Expand(float timeout = 0.0f)
        {
            Panel.Expand(timeout);
        }

        public static void Collapse()
        {
            Panel.Collapse();
        }

        public static void Show(float timeout = 0.0f)
        {
            Panel.Show(timeout);
        }

        public static void Hide()
        {
            Panel.Hide();
        }

        public static void SynchronizeMessages()
        {
            Panel.SynchronizeMessages();
        }

        public static void Toggle()
        {
            Panel.Toggle();
        }
    }
}
