using System;

namespace SkylineToolkit.UI
{
    [Flags]
    public enum MouseButtons
    {
        None = 0,
        Left = 1,
        Right = 2,
        Middle = 4,
        Special0 = 8,
        Special1 = 16,
        Special2 = 32,
        Special3 = 64
    }
}
