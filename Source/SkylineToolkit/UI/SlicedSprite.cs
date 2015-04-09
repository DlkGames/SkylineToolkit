using ColossalFramework.UI;
using SkylineToolkit.UI.Styles;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class SlicedSprite : Sprite
    {
        public static readonly int[] TriangleIndices = new int[54] { 0, 1, 2, 2, 3, 0, 4, 5, 6, 6, 7, 4, 8, 9, 10, 10, 11, 8, 12, 13, 14, 14, 15, 12, 1, 4, 7, 7, 2, 1, 9, 12, 15, 15, 10, 9, 3, 2, 9, 9, 8, 3, 7, 6, 13, 13, 12, 7, 2, 7, 12, 12, 9, 2 };

        #region Constructors

        public SlicedSprite()
            : base("SlicedSprite", typeof(UISlicedSprite))
        {
        }

        public SlicedSprite(string name)
            : this(name, new Vector3(0f, 0f), new Vector2(400, 200))
        {
        }

        public SlicedSprite(string name, Vector3 position, Vector2 size)
            : base(name, typeof(UISlicedSprite))
        {
            this.Position = position;
            this.Size = size;
        }

        public SlicedSprite(UISlicedSprite sprite, bool subschribeEvents = false)
            : base(sprite, subschribeEvents)
        {
        }

        public SlicedSprite(IColossalControl control, bool subschribeEvents = false)
            : base(control, subschribeEvents)
        {
        }

        #endregion

        #region Properties

        #region Component

        public new UISlicedSprite UIComponent
        {
            get
            {
                return (UISlicedSprite)base.UIComponent;
            }
            set
            {
                base.UIComponent = value;
            }
        }

        #endregion

        #endregion

        protected override void SetDefaultStyle()
        {
            base.SetDefaultStyle();

            this.EnableAutoSize = false;
            this.Color = new Color32(11, 13, 16, 255);
            this.SpriteName = ColossalSprite.ButtonMenu;
        }
    }
}
