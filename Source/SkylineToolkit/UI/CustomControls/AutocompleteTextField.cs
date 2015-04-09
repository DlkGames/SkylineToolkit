using ColossalFramework.UI;
using UnityEngine;

namespace SkylineToolkit.UI.CustomControls
{
    public class AutocompleteTextField : TextField
    {
        #region Constructors

        public AutocompleteTextField()
            : base("AutocompleteTextField")
        {
        }

        public AutocompleteTextField(string name)
            : base(name)
        {
        }

        public AutocompleteTextField(string name, string text)
            : base(name, text)
        {
        }

        public AutocompleteTextField(string name, string text, Vector3 position)
            : base(name, text, position)
        {
        }

        public AutocompleteTextField(string name, string text, Vector3 position, Vector2 size)
            : base(name, text, position, size)
        {
        }

        public AutocompleteTextField(UITextField textField, bool subschribeEvents = false)
            : base(textField, subschribeEvents)
        {
        }

        public AutocompleteTextField(IColossalControl control, bool subschribeEvents = false) 
            : base(control, subschribeEvents)
        {
        }

        #endregion
    }
}
