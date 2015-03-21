using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class KeyboardEventArgs : ControlEventArgs
    {
        public KeyboardEventArgs(KeyCode keyCode, char keyChar, bool controlModifier, bool shiftModifier, bool altModifier)
        {
            this.KeyCode = keyCode;
            this.KeyCharacter = keyChar;
            this.ControlModifier = controlModifier;
            this.ShiftModifier = shiftModifier;
            this.AltModifier = altModifier;
        }

        public KeyboardEventArgs(UIKeyEventParameter originalParams)
            : base(originalParams)
        {
            this.KeyCode = originalParams.keycode;
            this.KeyCharacter = originalParams.character;
            this.ControlModifier = originalParams.control;
            this.ShiftModifier = originalParams.shift;
            this.AltModifier = originalParams.alt;
        }

        public KeyCode KeyCode { get; protected set; }

        public char KeyCharacter { get; protected set; }

        public bool ControlModifier { get; protected set; }

        public bool ShiftModifier { get; protected set; }

        public bool AltModifier { get; protected set; }

        public override string ToString()
        {
            return string.Format("keycode: {0}, character: {1}, control: {2}, shift: {3}, alt: {4}",
                this.KeyCode, this.KeyCharacter, this.ControlModifier, this.ShiftModifier, this.AltModifier);
        }
    }
}
