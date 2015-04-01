using UnityEngine;

namespace SkylineToolkit.UI
{
    public class CursorInfo : ScriptableObject
    {
        public Texture2D Texture { get; set; }

        public Vector2 HotSpot { get; set; }

        public ColossalFramework.CursorInfo ToColossalCursorInfo()
        {
            ColossalFramework.CursorInfo info = ScriptableObject.CreateInstance<ColossalFramework.CursorInfo>();
            
            info.m_texture = this.Texture;
            info.m_hotspot = this.HotSpot;

            return info;
        }

        public static CursorInfo FromColossalCursorInfo(ColossalFramework.CursorInfo info)
        {
            CursorInfo cInfo = ScriptableObject.CreateInstance<CursorInfo>();

            cInfo.Texture = info.m_texture;
            cInfo.HotSpot = info.m_hotspot;

            return cInfo;
        }
    }
}
