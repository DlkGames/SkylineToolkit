using UnityEngine;

namespace SkylineToolkit.UI
{
    public class CursorInfo : ScriptableObject
    {
        public Texture2D Texture { get; set; }

        public Vector2 HotSpot { get; set; }

        public ColossalFramework.CursorInfo ToColossalCursorInfo()
        {
            return new ColossalFramework.CursorInfo()
            {
                m_texture = this.Texture,
                m_hotspot = this.HotSpot
            };
        }

        public static CursorInfo FromColossalCursorInfo(ColossalFramework.CursorInfo info)
        {
            return new CursorInfo()
            {
                Texture = info.m_texture,
                HotSpot = info.m_hotspot
            };
        }
    }
}
