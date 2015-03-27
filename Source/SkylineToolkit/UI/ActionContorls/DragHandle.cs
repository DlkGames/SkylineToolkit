using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI.ActionContorls
{
    public class DragHandle : ColossalControl<UIDragHandle>
    {
        public DragHandle()
            : base("Button")
        {
        }

        public DragHandle(string name)
            : this(name, name, new Vector3(0f,0f), new Vector2(200, 10))
        {
        }

        public DragHandle(string name, string label, Vector3 position, Vector2 size)
            : base(name)
        {
            this.Position = position;
            this.Size = size;
        }

        public DragHandle(UIDragHandle button)
            : base(button)
        {
        }

        public DragHandle(IColossalControl control) 
            : base(control)
        {
        }

        public bool ConstrainToScreen
        {
            get
            {
                return this.UIComponent.constrainToScreen;
            }
            set
            {
                this.UIComponent.constrainToScreen = value;
            }
        }

        public IColossalControl Target
        {
            get
            {
                return this.UIComponent.target.ToSkylineToolkitControl();
            }
            set
            {
                this.UIComponent.target = value.UIComponent;
            }
        }
    }
}
