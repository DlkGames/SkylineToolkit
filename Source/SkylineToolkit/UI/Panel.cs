using ColossalFramework.UI;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class Panel : ColossalControl<UIPanel>
    {
        public Panel(string name, Rect rect)
            : base(name)
        {
            this.SetDefaultStyle();
        }

        public Panel(UIPanel panel)
            : base(panel)
        {
        }

        private void SetDefaultStyle()
        {
            throw new System.NotImplementedException();
        }
    }
}
