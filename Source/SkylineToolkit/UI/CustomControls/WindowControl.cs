using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public class WindowControl : UICustomControl
    {
        private string title = "Unnamed Window";

        private Vector2 position;

        private Vector2 size;

        protected WindowControl()
        {
            this.InitializeWindow();
        }

        public WindowControl(string title)
            : this(title, Vector2.zero, new Vector2(400, 320))
        {
        }

        public WindowControl(string title, Vector2 position, Vector2 size)
            : this()
        {
            this.title = title;
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }


        protected virtual void InitializeWindow()
        {
            Panel panelControl = new Panel();

            panelControl.BackgroundSprite = "MenuPanel";
        }
    }
}
