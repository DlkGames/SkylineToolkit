using ColossalFramework;
using ColossalFramework.Globalization;
using SkylineToolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ColossalFramework.UI
{
    /// <summary>
    /// TODO Textarea
    /// NOTE This is not usable at the moment, it will only have the functionality of the normal UITextField.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This custom UI component was created using the decompiled sources from ColossalFramework.UI.UITextField as
    /// a template.
    /// 
    /// Type: ColossalFramework.UI.UITextField
    /// Assembly: ColossalManaged, Version=0.3.0.0, Culture=neutral, PublicKeyToken=null
    /// MVID: 559D896B-933B-4BA1-BFDE-2F1AB218052C
    /// Assembly location: {SteamApps}\common\Cities_Skylines\Cities_Data\Managed\ColossalManaged.dll
    /// </para>
    /// </remarks>
    [UIComponentMenu("Text Area")]
    [ExecuteInEditMode]
    public class UITextArea : UIInteractiveComponent
    {
        #region Fields

        public static int kUndoLimit = 20;

        #region Serialization

        [SerializeField]
        protected Color32 m_SelectionBackground = new Color32(0, 105, 210, 255);

        [SerializeField]
        protected string m_SelectionSprite = "EmptySprite";

        [SerializeField]
        protected float m_CursorBlinkTime = 0.45f;

        [SerializeField]
        protected int m_CursorWidth = 1;

        [SerializeField]
        protected int m_MaxLength = 1024;

        [SerializeField]
        protected bool m_SubmitOnFocusLost = true;

        [SerializeField]
        protected bool m_AcceptsTab;

        [SerializeField]
        protected bool m_IsPasswordField;

        [SerializeField]
        protected bool m_ReadOnly;

        [SerializeField]
        protected RectOffset m_Padding;

        [SerializeField]
        protected bool m_SelectOnFocus;

        [SerializeField]
        protected bool m_NumericalOnly;

        [SerializeField]
        protected bool m_AllowFloats;

        #endregion

        #region Selection

        private int m_SelectionStart;

        private int m_SelectionEnd;

        private int m_MouseSelectionAnchor;

        #endregion

        private int m_ScrollIndex;

        private int m_CursorIndex;

        private float m_LeftOffset;

        private bool m_CursorShown;

        private float[] m_CharWidths;

        private float m_TimeSinceFocus;

        private bool m_FocusForced;

        #region Undo

        private int m_UndoCount;

        private bool m_Undoing;

        private string m_UndoText = "";

        private List<UndoData> m_UndoData = new List<UndoData>(UITextArea.kUndoLimit);

        #endregion

        #endregion

        #region Events

        public event PropertyChangedEventHandler<bool> eventReadOnlyChanged;

        public event PropertyChangedEventHandler<string> eventTextChanged;

        public event PropertyChangedEventHandler<string> eventTextSubmitted;

        public event PropertyChangedEventHandler<string> eventTextCancelled;

        #endregion

        #region Properties

        #region Selection

        public int selectionStart
        {
            get
            {
                return this.m_SelectionStart;
            }
            set
            {
                if (value == this.m_SelectionStart)
                    return;
                this.m_SelectionStart = Mathf.Max(0, Mathf.Min(value, this.m_Text.Length));
                this.m_SelectionEnd = Mathf.Max(this.m_SelectionEnd, this.m_SelectionStart);
                this.Invalidate();
            }
        }

        public int selectionEnd
        {
            get
            {
                return this.m_SelectionEnd;
            }
            set
            {
                if (value == this.m_SelectionEnd)
                    return;
                this.m_SelectionEnd = Mathf.Max(0, Mathf.Min(value, this.m_Text.Length));
                this.m_SelectionStart = Mathf.Max(this.m_SelectionStart, this.m_SelectionEnd);
                this.Invalidate();
            }
        }

        public int selectionLength
        {
            get
            {
                return this.m_SelectionEnd - this.m_SelectionStart;
            }
        }

        public string selectedText
        {
            get
            {
                if (this.m_SelectionEnd == this.m_SelectionStart)
                    return "";
                else
                    return this.m_Text.Substring(this.m_SelectionStart, this.selectionLength);
            }
        }

        public bool selectOnFocus
        {
            get
            {
                return this.m_SelectOnFocus;
            }
            set
            {
                this.m_SelectOnFocus = value;
            }
        }

        #endregion

        #region State

        public bool submitOnFocusLost
        {
            get
            {
                return this.m_SubmitOnFocusLost;
            }
            set
            {
                this.m_SubmitOnFocusLost = value;
            }
        }

        public bool readOnly
        {
            get
            {
                return this.m_ReadOnly;
            }
            set
            {
                if (value == this.m_ReadOnly)
                    return;
                this.m_ReadOnly = value;
                this.OnReadOnlyChanged();
                this.Invalidate();
            }
        }

        #endregion

        #region Content

        public bool numericalOnly
        {
            get
            {
                return this.m_NumericalOnly;
            }
            set
            {
                this.m_NumericalOnly = value;
            }
        }

        public bool allowFloats
        {
            get
            {
                return this.m_AllowFloats;
            }
            set
            {
                this.m_AllowFloats = value;
            }
        }

        public override string text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                if (value.Length > this.maxLength)
                    value = value.Substring(0, this.maxLength);
                value = value.Replace("\t", " ");
                if (!(value != this.m_Text))
                    return;
                this.m_Text = value;
                this.m_ScrollIndex = this.m_CursorIndex = 0;
                this.OnTextChanged();
                this.Invalidate();
            }
        }

        public int maxLength
        {
            get
            {
                return this.m_MaxLength;
            }
            set
            {
                if (value == this.m_MaxLength)
                    return;
                this.m_MaxLength = Mathf.Max(0, value);
                if (this.maxLength < this.m_Text.Length)
                    this.text = this.m_Text.Substring(0, this.maxLength);
                this.Invalidate();
            }
        }

        #endregion

        #region Appearance

        public RectOffset padding
        {
            get
            {
                if (this.m_Padding == null)
                    this.m_Padding = new RectOffset();
                return this.m_Padding;
            }
            set
            {
                value = RectExtensions.ConstrainPadding(value);
                if (object.Equals((object)value, (object)this.m_Padding))
                    return;
                this.m_Padding = value;
                this.Invalidate();
            }
        }

        public float cursorBlinkTime
        {
            get
            {
                return this.m_CursorBlinkTime;
            }
            set
            {
                this.m_CursorBlinkTime = value;
            }
        }

        public int cursorWidth
        {
            get
            {
                return this.m_CursorWidth;
            }
            set
            {
                this.m_CursorWidth = value;
            }
        }

        public string selectionSprite
        {
            get
            {
                return this.m_SelectionSprite;
            }
            set
            {
                if (!(value != this.m_SelectionSprite))
                    return;
                this.m_SelectionSprite = value;
                this.Invalidate();
            }
        }

        public Color32 selectionBackgroundColor
        {
            get
            {
                return this.m_SelectionBackground;
            }
            set
            {
                this.m_SelectionBackground = value;
                this.Invalidate();
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Selection

        public void MoveSelectionPointRightWord()
        {
            if (this.m_CursorIndex == this.m_Text.Length)
            {
                return;
            }

            int nextWord = this.FindNextWord(this.m_CursorIndex);

            if (this.m_SelectionEnd == this.m_SelectionStart)
            {
                this.m_SelectionStart = this.m_CursorIndex;
                this.m_SelectionEnd = nextWord;
            }
            else if (this.m_SelectionEnd == this.m_CursorIndex)
            {
                this.m_SelectionEnd = nextWord;
            }
            else if (this.m_SelectionStart == this.m_CursorIndex)
            {
                this.m_SelectionStart = nextWord;
            }

            this.SetCursorPos(nextWord);
        }

        public void MoveSelectionPointLeftWord()
        {
            if (this.m_CursorIndex == 0)
            {
                return;
            }

            int previousWord = this.FindPreviousWord(this.m_CursorIndex);

            if (this.m_SelectionEnd == this.m_SelectionStart)
            {
                this.m_SelectionEnd = this.m_CursorIndex;
                this.m_SelectionStart = previousWord;
            }
            else if (this.m_SelectionEnd == this.m_CursorIndex)
            {
                this.m_SelectionEnd = previousWord;
            }
            else if (this.m_SelectionStart == this.m_CursorIndex)
            {
                this.m_SelectionStart = previousWord;
            }

            this.SetCursorPos(previousWord);
        }

        public void MoveSelectionPointRight()
        {
            if (this.m_CursorIndex == this.m_Text.Length)
            {
                return;
            }

            if (this.m_SelectionEnd == this.m_SelectionStart)
            {
                this.m_SelectionEnd = this.m_CursorIndex + 1;
                this.m_SelectionStart = this.m_CursorIndex;
            }
            else if (this.m_SelectionEnd == this.m_CursorIndex)
            {
                ++this.m_SelectionEnd;
            }
            else if (this.m_SelectionStart == this.m_CursorIndex)
            {
                ++this.m_SelectionStart;
            }

            this.SetCursorPos(this.m_CursorIndex + 1);
        }

        public void MoveSelectionPointLeft()
        {
            if (this.m_CursorIndex == 0)
            {
                return;
            }

            if (this.m_SelectionEnd == this.m_SelectionStart)
            {
                this.m_SelectionEnd = this.m_CursorIndex;
                this.m_SelectionStart = this.m_CursorIndex - 1;
            }
            else if (this.m_SelectionEnd == this.m_CursorIndex)
            {
                --this.m_SelectionEnd;
            }
            else if (this.m_SelectionStart == this.m_CursorIndex)
            {
                --this.m_SelectionStart;
            }

            this.SetCursorPos(this.m_CursorIndex - 1);
        }

        public void MoveToSelectionEnd()
        {
            int index = this.m_SelectionEnd;
            this.ClearSelection();
            this.SetCursorPos(index);
        }

        public void MoveToSelectionStart()
        {
            int index = this.m_SelectionStart;
            this.ClearSelection();
            this.SetCursorPos(index);
        }

        public void SelectAll()
        {
            this.m_SelectionStart = 0;
            this.m_SelectionEnd = this.m_Text.Length;
            this.m_ScrollIndex = 0;
            this.SetCursorPos(0);
        }

        public void ClearSelection()
        {
            this.m_SelectionStart = 0;
            this.m_SelectionEnd = 0;
            this.m_MouseSelectionAnchor = 0;
        }

        public void SelectToStart()
        {
            if (this.m_CursorIndex == 0)
            {
                return;
            }

            if (this.m_SelectionEnd == this.m_SelectionStart)
            {
                this.m_SelectionEnd = this.m_CursorIndex;
            }
            else if (this.m_SelectionEnd == this.m_CursorIndex)
            {
                this.m_SelectionEnd = this.m_SelectionStart;
            }

            this.m_SelectionStart = 0;
            this.SetCursorPos(0);
        }

        public void SelectToEnd()
        {
            if (this.m_CursorIndex == this.m_Text.Length)
            {
                return;
            }

            if (this.m_SelectionEnd == this.m_SelectionStart)
            {
                this.m_SelectionStart = this.m_CursorIndex;
            }
            else if (this.m_SelectionStart == this.m_CursorIndex)
            {
                this.m_SelectionStart = this.m_SelectionEnd;
            }

            this.m_SelectionEnd = this.m_Text.Length;
            this.SetCursorPos(this.m_Text.Length);
        }

        public void SelectWordAtIndex(int index)
        {
            index = Mathf.Max(Mathf.Min(this.m_Text.Length - 1, index), 0);

            if (!char.IsLetterOrDigit(this.m_Text[index]))
            {
                this.m_SelectionStart = index;
                this.m_SelectionEnd = index + 1;
                this.m_MouseSelectionAnchor = 0;
            }
            else
            {
                this.m_SelectionStart = index;
                for (int index1 = index; index1 > 0 && char.IsLetterOrDigit(this.m_Text[index1 - 1]); --index1)
                {
                    --this.m_SelectionStart;
                }

                this.m_SelectionEnd = index;

                for (int index1 = index; index1 < this.m_Text.Length && char.IsLetterOrDigit(this.m_Text[index1]); ++index1)
                {
                    this.m_SelectionEnd = index1 + 1;
                }
            }

            this.m_CursorIndex = this.m_SelectionStart;
            this.Invalidate();
        }

        private void CutSelectionToClipboard()
        {
            this.CopySelectionToClipboard();
            this.DeleteSelection();
        }

        private void CopySelectionToClipboard()
        {
            if (this.m_SelectionStart == this.m_SelectionEnd)
            {
                return;
            }

            Clipboard.text = this.m_Text.Substring(this.m_SelectionStart, this.selectionLength);
        }

        #endregion

        #region Content

        public int FindPreviousWord(int startIndex)
        {
            int num;

            for (num = startIndex; num > 0; --num)
            {
                char c = this.m_Text[num - 1];
                if (!char.IsWhiteSpace(c) && !char.IsSeparator(c) && !char.IsPunctuation(c))
                    break;
            }

            for (int index = num; index >= 0; --index)
            {
                if (index == 0)
                {
                    num = 0;
                    break;
                }
                else
                {
                    char c = this.m_Text[index - 1];

                    if (char.IsWhiteSpace(c) || char.IsSeparator(c) || char.IsPunctuation(c))
                    {
                        num = index;
                        break;
                    }
                }
            }
            return num;
        }

        public int FindNextWord(int startIndex)
        {
            int length = this.m_Text.Length;
            int index1 = startIndex;

            for (int index2 = index1; index2 < length; ++index2)
            {
                char c = this.m_Text[index2];

                if (char.IsWhiteSpace(c) || char.IsSeparator(c) || char.IsPunctuation(c))
                {
                    index1 = index2;
                    break;
                }
            }

            for (; index1 < length; ++index1)
            {
                char c = this.m_Text[index1];

                if (!char.IsWhiteSpace(c) && !char.IsSeparator(c) && !char.IsPunctuation(c))
                    break;
            }

            return index1;
        }

        private bool IsDigit(char character)
        {
            string decimalSeparator = LocaleManager.cultureInfo.NumberFormat.NumberDecimalSeparator;

            if (char.IsDigit(character))
            {
                return true;
            }

            if (this.allowFloats && character.ToString() == decimalSeparator)
            {
                return !this.m_Text.Contains(decimalSeparator);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Cursor movement

        private int GetCharIndexAt(UIMouseEventParameter p)
        {
            Vector2 hitPosition = this.GetHitPosition(p);

            float num1 = this.PixelsToUnits();
            int num2 = this.m_ScrollIndex;
            float num3 = this.m_LeftOffset / num1;

            for (int index = this.m_ScrollIndex; index < this.m_CharWidths.Length; ++index)
            {
                num3 += this.m_CharWidths[index] / num1;

                if (num3 < hitPosition.x)
                    ++num2;
            }

            return num2;
        }

        public void MoveToStart()
        {
            this.ClearSelection();
            this.SetCursorPos(0);
        }

        public void MoveToEnd()
        {
            this.ClearSelection();
            this.SetCursorPos(this.m_Text.Length);
        }

        public void MoveToNextChar()
        {
            this.ClearSelection();
            this.SetCursorPos(this.m_CursorIndex + 1);
        }

        public void MoveToPreviousChar()
        {
            this.ClearSelection();
            this.SetCursorPos(this.m_CursorIndex - 1);
        }

        public void MoveToNextWord()
        {
            this.ClearSelection();

            if (this.m_CursorIndex == this.m_Text.Length)
                return;

            this.SetCursorPos(this.FindNextWord(this.m_CursorIndex));
        }

        public void MoveToPreviousWord()
        {
            this.ClearSelection();

            if (this.m_CursorIndex == 0)
                return;

            this.SetCursorPos(this.FindPreviousWord(this.m_CursorIndex));
        }

        #endregion

        #region Appearance

        private IEnumerator MakeCursorBlink()
        {
            if (Application.isPlaying)
            {
                this.m_CursorShown = true;

                while (this.hasFocus)
                {
                    yield return new WaitForSeconds(this.cursorBlinkTime);

                    this.m_CursorShown = !this.m_CursorShown;
                    this.Invalidate();
                }

                this.m_CursorShown = false;
            }
        }

        #endregion

        private void PasteAtCursor(string clipData)
        {
            this.DeleteSelection();

            StringBuilder stringBuilder = new StringBuilder(this.m_Text.Length + clipData.Length);
            stringBuilder.Append(this.m_Text);

            for (int index = 0; index < clipData.Length; ++index)
            {
                char ch = clipData[index];

                if ((int)ch >= 32)
                {
                    stringBuilder.Insert(this.m_CursorIndex++, ch);
                }
            }

            stringBuilder.Length = Mathf.Min(stringBuilder.Length, this.maxLength);

            this.m_Text = stringBuilder.ToString();
            this.SetCursorPos(this.m_CursorIndex);
        }

        private void SetCursorPos(int index)
        {
            index = Mathf.Max(0, Mathf.Min(this.m_Text.Length, index));

            if (index == this.m_CursorIndex)
                return;

            this.m_CursorIndex = index;
            this.m_CursorShown = this.hasFocus;
            this.m_ScrollIndex = Mathf.Min(this.m_ScrollIndex, this.m_CursorIndex);

            this.Invalidate();
        }

        private void DeleteSelection()
        {
            if (this.m_SelectionStart == this.m_SelectionEnd)
                return;

            this.m_Text = this.m_Text.Remove(this.m_SelectionStart, this.selectionLength);
            this.SetCursorPos(this.m_SelectionStart);
            this.ClearSelection();

            this.Invalidate();
        }

        private void DeleteNextChar()
        {
            this.ClearSelection();

            if (this.m_CursorIndex >= this.m_Text.Length)
                return;

            this.m_Text = this.m_Text.Remove(this.m_CursorIndex, 1);
            this.m_CursorShown = true;

            this.Invalidate();
        }

        private void DeletePreviousChar()
        {
            if (this.m_SelectionStart != this.m_SelectionEnd)
            {
                int index = this.m_SelectionStart;
                this.DeleteSelection();
                this.SetCursorPos(index);
            }
            else
            {
                this.ClearSelection();

                if (this.m_CursorIndex == 0)
                    return;

                this.m_Text = this.m_Text.Remove(this.m_CursorIndex - 1, 1);

                --this.m_CursorIndex;
                this.m_CursorShown = true;

                this.OnTextChanged();

                this.Invalidate();
            }
        }

        private void DeleteNextWord()
        {
            this.ClearSelection();

            if (this.m_CursorIndex == this.m_Text.Length)
                return;

            int num = this.FindNextWord(this.m_CursorIndex);

            if (num == this.m_CursorIndex)
            {
                num = this.m_Text.Length;
            }

            this.m_Text = this.m_Text.Remove(this.m_CursorIndex, num - this.m_CursorIndex);

            this.Invalidate();
        }

        private void DeletePreviousWord()
        {
            this.ClearSelection();

            if (this.m_CursorIndex == 0)
                return;

            int num = this.FindPreviousWord(this.m_CursorIndex);

            if (num == this.m_CursorIndex)
            {
                num = 0;
            }

            this.m_Text = this.m_Text.Remove(num, this.m_CursorIndex - num);
            this.SetCursorPos(num);
        }

        public override void OnEnable()
        {
            if (this.padding == null)
                this.padding = new RectOffset();

            base.OnEnable();

            if ((double)this.size.magnitude == 0.0)
            {
                this.size = new Vector2(100f, 20f);
            }

            this.m_CursorShown = false;
            this.m_CursorIndex = this.m_ScrollIndex = 0;

            if (!Application.isPlaying || this.font != null && this.font.isValid)
                return;

            this.font = this.GetUIView().defaultFont;
        }

        protected override void OnRebuildRenderData()
        {
            if (this.atlas == null || this.font == null || !this.font.isValid)
                return;

            if (this.textRenderData != null)
            {
                this.textRenderData.Clear();
            }
            else
            {
                this.m_RenderData.Add(UIRenderData.Obtain());
            }

            this.renderData.material = this.atlas.material;
            this.textRenderData.material = this.atlas.material;

            this.RenderBackground();
            this.RenderText();
        }

        private RectOffset GetSelectionPadding()
        {
            if ((Object)this.atlas == (Object)null)
                return this.padding;

            UITextureAtlas.SpriteInfo backgroundSprite = this.GetBackgroundSprite();

            if (backgroundSprite == (UITextureAtlas.SpriteInfo)null)
            {
                return this.padding;
            }

            return backgroundSprite.border;

        }

        private void RenderText()
        {
            float pixelRatio = this.PixelsToUnits();

            Vector2 innerSize = new Vector2(this.size.x - this.padding.horizontal, this.size.y - this.padding.vertical);
            Vector3 point1 = UIPivotExtensions.TransformToUpperLeft(this.pivot, this.size, this.arbitraryPivotOffset);
            Vector3 innerPoint1 = new Vector3(point1.x + this.padding.left, point1.y - this.padding.top, 0.0f) * pixelRatio;

            string text = this.m_Text;

            Color32 fontColor = this.isEnabled ? this.textColor : this.disabledTextColor;

            float textScaleMultiplier = this.GetTextScaleMultiplier();

            using (UIFontRenderer uiFontRenderer = this.font.ObtainRenderer())
            {
                uiFontRenderer.wordWrap = false;
                uiFontRenderer.maxSize = innerSize;
                uiFontRenderer.pixelRatio = pixelRatio;
                uiFontRenderer.textScale = this.textScale * textScaleMultiplier;
                uiFontRenderer.characterSpacing = this.characterSpacing;
                uiFontRenderer.vectorOffset = innerPoint1;
                uiFontRenderer.multiLine = false;
                uiFontRenderer.textAlign = UIHorizontalAlignment.Left;
                uiFontRenderer.processMarkup = this.processMarkup;
                uiFontRenderer.colorizeSprites = this.colorizeSprites;
                uiFontRenderer.defaultColor = fontColor;
                uiFontRenderer.bottomColor = this.useGradient ? new Color32?(this.bottomColor) : new Color32?();
                uiFontRenderer.overrideMarkupColors = false;
                uiFontRenderer.opacity = this.CalculateOpacity();
                uiFontRenderer.outline = this.useOutline;
                uiFontRenderer.outlineSize = this.outlineSize;
                uiFontRenderer.outlineColor = this.outlineColor;
                uiFontRenderer.shadow = this.useDropShadow;
                uiFontRenderer.shadowColor = this.dropShadowColor;
                uiFontRenderer.shadowOffset = this.dropShadowOffset;

                this.m_CursorIndex = Mathf.Min(this.m_CursorIndex, text.Length);
                this.m_ScrollIndex = Mathf.Min(Mathf.Min(this.m_ScrollIndex, this.m_CursorIndex), text.Length);
                this.m_CharWidths = uiFontRenderer.GetCharacterWidths(text);

                Vector2 innerSizeS = innerSize * pixelRatio;
                this.m_LeftOffset = 0.0f;

                if (this.horizontalAlignment == UIHorizontalAlignment.Left)
                {
                    float totalTextWidth = 0.0f;

                    for (int index = this.m_ScrollIndex; index < this.m_CursorIndex; ++index)
                    {
                        totalTextWidth += this.m_CharWidths[index];
                    }

                    while (totalTextWidth >= innerSizeS.x && this.m_ScrollIndex < this.m_CursorIndex)
                    {
                        totalTextWidth -= this.m_CharWidths[this.m_ScrollIndex++];
                    }
                }
                else
                {
                    this.m_ScrollIndex = Mathf.Max(0, Mathf.Min(this.m_CursorIndex, text.Length - 1));

                    float totalTextWidth = 0.0f;
                    float fontSizeS = this.font.size * 1.25f * pixelRatio;

                    while (this.m_ScrollIndex > 0 && totalTextWidth < innerSizeS.x - fontSizeS)
                    {
                        totalTextWidth += this.m_CharWidths[this.m_ScrollIndex--];
                    }

                    float num4 = text.Length > 0 ? uiFontRenderer.GetCharacterWidths(text.Substring(this.m_ScrollIndex)).Sum() : 0.0f;

                    switch (this.horizontalAlignment)
                    {
                        case UIHorizontalAlignment.Center:
                            this.m_LeftOffset = Mathf.Max(0.0f, (float)((innerSizeS.x - num4) * 0.5));
                            break;
                        case UIHorizontalAlignment.Right:
                            this.m_LeftOffset = Mathf.Max(0.0f, innerSizeS.x - num4);
                            break;
                    }

                    innerPoint1.x += this.m_LeftOffset;
                    uiFontRenderer.vectorOffset = innerPoint1;
                }

                if (this.m_SelectionEnd != this.m_SelectionStart)
                {
                    this.RenderSelection(this.m_ScrollIndex, this.m_CharWidths, this.m_LeftOffset);
                }

                uiFontRenderer.Render(text.Substring(this.m_ScrollIndex), this.textRenderData);

                if (!this.m_CursorShown || this.m_SelectionEnd != this.m_SelectionStart)
                {
                    return;
                }

                this.RenderCursor(this.m_ScrollIndex, this.m_CursorIndex, this.m_CharWidths, this.m_LeftOffset);
            }
        }

        private void RenderSelection(int scrollIndex, float[] charWidths, float leftOffset)
        {
            if (string.IsNullOrEmpty(this.selectionSprite) || this.atlas == null)
            {
                return;
            }

            float pixelRatio = this.PixelsToUnits();
            float b1 = (this.size.x - this.padding.horizontal) * pixelRatio;
            int b2 = scrollIndex;
            float totalTextWidth = 0.0f;

            for (int index = scrollIndex; index < this.m_Text.Length; ++index)
            {
                ++b2;
                totalTextWidth += charWidths[index];

                if (totalTextWidth > b1)
                    break;
            }

            if (this.m_SelectionStart > b2 || this.m_SelectionEnd < scrollIndex)
                return;

            int num3 = Mathf.Max(scrollIndex, this.m_SelectionStart);

            if (num3 > b2)
                return;

            int num4 = Mathf.Min(this.m_SelectionEnd, b2);

            if (num4 <= scrollIndex)
                return;

            float num5 = 0.0f;
            float num6 = 0.0f;
            float num7 = 0.0f;

            for (int index = scrollIndex; index <= b2; ++index)
            {
                if (index == num3)
                {
                    num5 = num7;
                }

                if (index == num4)
                {
                    num6 = num7;
                    break;
                }
                else
                {
                    num7 += charWidths[index];
                }
            }

            this.AddTriangles(this.renderData.triangles, this.renderData.vertices.Count);

            float num8 = (this.size.y - this.padding.vertical) * pixelRatio;
            float x1 = (num5 + leftOffset + this.padding.left * pixelRatio);
            float x2 = x1 + Mathf.Min(num6 - num5, b1);
            float y1 = -this.padding.top * pixelRatio;
            float y2 = y1 - num8;

            Vector3 vector3_1 = UIPivotExtensions.TransformToUpperLeft(this.pivot, this.size, this.arbitraryPivotOffset) * pixelRatio;
            Vector3 vector3_2 = new Vector3(x1, y1) + vector3_1;
            Vector3 vector3_3 = new Vector3(x2, y1) + vector3_1;
            Vector3 vector3_4 = new Vector3(x1, y2) + vector3_1;
            Vector3 vector3_5 = new Vector3(x2, y2) + vector3_1;

            this.renderData.vertices.Add(vector3_2);
            this.renderData.vertices.Add(vector3_3);
            this.renderData.vertices.Add(vector3_5);
            this.renderData.vertices.Add(vector3_4);

            Color32 color32 = this.ApplyOpacity(this.selectionBackgroundColor);

            this.renderData.colors.Add(color32);
            this.renderData.colors.Add(color32);
            this.renderData.colors.Add(color32);
            this.renderData.colors.Add(color32);

            UITextureAtlas.SpriteInfo spriteInfo = this.atlas[this.selectionSprite];
            Rect region = spriteInfo.region;

            float num9 = region.width / spriteInfo.pixelSize.x;
            float num10 = region.height / spriteInfo.pixelSize.y;

            this.renderData.uvs.Add(new Vector2(region.x + num9, region.yMax - num10));
            this.renderData.uvs.Add(new Vector2(region.xMax - num9, region.yMax - num10));
            this.renderData.uvs.Add(new Vector2(region.xMax - num9, region.y + num10));
            this.renderData.uvs.Add(new Vector2(region.x + num9, region.y + num10));
        }

        private void RenderCursor(int startIndex, int cursorIndex, float[] charWidths, float leftOffset)
        {
            if (string.IsNullOrEmpty(this.selectionSprite) || this.atlas == null)
                return;

            float totalTextWidth = 0.0f;

            for (int index = startIndex; index < cursorIndex; ++index)
            {
                totalTextWidth += charWidths[index];
            }

            float stepSize = this.PixelsToUnits();
            float x = FloatExtensions.Quantize((totalTextWidth + leftOffset + this.padding.left * stepSize), stepSize);
            float y = -this.padding.top * stepSize;

            float cursorWidthS = stepSize * this.GetUIView().ratio * this.cursorWidth;
            float cursorHeightS = (this.size.y - this.padding.vertical) * stepSize;

            Vector3 pointTopLeft = new Vector3(x, y);
            Vector3 pointTopRight = new Vector3(x + cursorWidthS, y);
            Vector3 pointBottomLeft = new Vector3(x, y - cursorHeightS);
            Vector3 pointBottomRight = new Vector3(x + cursorWidthS, y - cursorHeightS);

            PoolList<Vector3> vertices = this.renderData.vertices;
            PoolList<int> triangles = this.renderData.triangles;
            PoolList<Vector2> uvs = this.renderData.uvs;
            PoolList<Color32> colors = this.renderData.colors;

            Vector3 cursorPosition = UIPivotExtensions.TransformToUpperLeft(this.pivot, this.size, this.arbitraryPivotOffset) * stepSize;

            this.AddTriangles(triangles, vertices.Count);

            vertices.Add(pointTopLeft + cursorPosition);
            vertices.Add(pointTopRight + cursorPosition);
            vertices.Add(pointBottomRight + cursorPosition);
            vertices.Add(pointBottomLeft + cursorPosition);

            Color32 color32 = this.ApplyOpacity(this.textColor);

            colors.Add(color32);
            colors.Add(color32);
            colors.Add(color32);
            colors.Add(color32);

            Rect region = this.atlas[this.selectionSprite].region;

            uvs.Add(new Vector2(region.x, region.yMax));
            uvs.Add(new Vector2(region.xMax, region.yMax));
            uvs.Add(new Vector2(region.xMax, region.y));
            uvs.Add(new Vector2(region.x, region.y));
        }

        private void AddTriangles(PoolList<int> triangles, int baseIndex)
        {
            for (int index = 0; index < ColossalControl.TriangleIndices.Length; ++index)
            {
                triangles.Add(ColossalControl.TriangleIndices[index] + baseIndex);
            }
        }

        #endregion

        #region On events

        protected virtual void OnTextChanged()
        {
            if (!this.m_Undoing)
            {
                this.m_UndoData.RemoveRange(this.m_UndoData.Count - this.m_UndoCount, this.m_UndoCount);
                this.m_UndoData.Add(new UndoData(this.text, this.m_CursorIndex));
                this.m_UndoCount = 0;

                if (UITextArea.kUndoLimit != 0 && UITextArea.kUndoLimit <= this.m_UndoData.Count)
                {
                    this.m_UndoData.RemoveAt(0);
                }
            }

            if (this.eventTextChanged != null)
            {
                this.eventTextChanged(this, this.text);
            }

            this.Invoke("OnTextChanged", this.text);
        }

        protected virtual void OnReadOnlyChanged()
        {
            if (this.eventReadOnlyChanged != null)
            {
                this.eventReadOnlyChanged(this, this.readOnly);
            }

            this.Invoke("OnReadOnlyChanged", this.readOnly);
        }

        protected virtual void OnSubmit()
        {
            this.m_FocusForced = true;

            this.Unfocus();

            if (this.eventTextSubmitted != null)
            {
                this.eventTextSubmitted(this, this.text);
            }

            this.InvokeUpward("OnTextSubmitted", this.text);
        }

        protected virtual void OnCancel()
        {
            this.m_FocusForced = true;
            this.m_Text = this.m_UndoText;

            this.Unfocus();

            if (this.eventTextCancelled != null)
            {
                this.eventTextCancelled(this, this.text);
            }

            this.InvokeUpward("OnTextCancelled", this, this.text);
        }

        protected override void OnKeyPress(UIKeyEventParameter p)
        {
            if (this.builtinKeyNavigation)
            {
                if (this.readOnly || char.IsControl(p.character))
                {
                    base.OnKeyPress(p);
                }
                else if (this.numericalOnly && !this.IsDigit(p.character))
                {
                    base.OnKeyPress(p);
                }
                else
                {
                    base.OnKeyPress(p);

                    if (p.used)
                    {
                        return;
                    }

                    this.DeleteSelection();

                    if (this.m_Text.Length < this.maxLength)
                    {
                        if (this.m_CursorIndex == this.m_Text.Length)
                        {
                            UITextArea uiTextField = this;

                            string str = uiTextField.m_Text + (object)p.character;
                            uiTextField.m_Text = str;
                        }
                        else
                        {
                            this.m_Text = this.m_Text.Insert(this.m_CursorIndex, p.character.ToString());
                        }

                        ++this.m_CursorIndex;

                        this.OnTextChanged();
                        this.Invalidate();
                    }

                    p.Use();
                }
            }
            else
            {
                base.OnKeyPress(p);
            }
        }

        protected override void OnKeyDown(UIKeyEventParameter p)
        {
            if (this.builtinKeyNavigation)
            {
                if (this.readOnly)
                    return;

                base.OnKeyDown(p);

                if (p.used)
                    return;

                switch (p.keycode)
                {
                    case KeyCode.Delete:
                        if (this.m_SelectionStart != this.m_SelectionEnd)
                        {
                            this.DeleteSelection();
                            break;
                        }
                        else if (p.control)
                        {
                            this.DeleteNextWord();
                            break;
                        }
                        else
                        {
                            this.DeleteNextChar();
                            break;
                        }
                    case KeyCode.RightArrow:
                        if (p.control)
                        {
                            if (p.shift)
                            {
                                this.MoveSelectionPointRightWord();
                                break;
                            }
                            else
                            {
                                this.MoveToNextWord();
                                break;
                            }
                        }
                        else if (p.shift)
                        {
                            this.MoveSelectionPointRight();
                            break;
                        }
                        else if (this.selectionLength > 0)
                        {
                            this.MoveToSelectionEnd();
                            break;
                        }
                        else
                        {
                            this.MoveToNextChar();
                            break;
                        }
                    case KeyCode.LeftArrow:
                        if (p.control)
                        {
                            if (p.shift)
                            {
                                this.MoveSelectionPointLeftWord();
                                break;
                            }
                            else
                            {
                                this.MoveToPreviousWord();
                                break;
                            }
                        }
                        else if (p.shift)
                        {
                            this.MoveSelectionPointLeft();
                            break;
                        }
                        else if (this.selectionLength > 0)
                        {
                            this.MoveToSelectionStart();
                            break;
                        }
                        else
                        {
                            this.MoveToPreviousChar();
                            break;
                        }
                    case KeyCode.Insert:
                        if (p.shift)
                        {
                            string text = Clipboard.text;
                            if (!string.IsNullOrEmpty(text))
                            {
                                this.PasteAtCursor(text);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    case KeyCode.Home:
                        if (p.shift)
                        {
                            this.SelectToStart();
                            break;
                        }
                        else
                        {
                            this.MoveToStart();
                            break;
                        }
                    case KeyCode.End:
                        if (p.shift)
                        {
                            this.SelectToEnd();
                            break;
                        }
                        else
                        {
                            this.MoveToEnd();
                            break;
                        }
                    case KeyCode.A:
                        if (p.control)
                        {
                            this.SelectAll();
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case KeyCode.C:
                        if (p.control)
                        {
                            this.CopySelectionToClipboard();
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case KeyCode.V:
                        if (p.control)
                        {
                            string text = Clipboard.text;
                            if (!string.IsNullOrEmpty(text))
                            {
                                this.PasteAtCursor(text);
                                break;
                            }
                            else
                                break;
                        }
                        else
                        {
                            break;
                        }
                    case KeyCode.X:
                        if (p.control)
                        {
                            this.CutSelectionToClipboard();
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case KeyCode.Y:
                        if (p.control)
                        {
                            this.m_Undoing = true;

                            try
                            {
                                --this.m_UndoCount;

                                this.ClearSelection();

                                this.text = this.m_UndoData[this.m_UndoData.Count - this.m_UndoCount - 1].Text;
                                this.m_CursorIndex = this.m_UndoData[this.m_UndoData.Count - this.m_UndoCount - 1].Index;
                            }
                            catch
                            {
                                ++this.m_UndoCount;
                            }

                            this.m_Undoing = false;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case KeyCode.Z:
                        if (p.control)
                        {
                            this.m_Undoing = true;

                            try
                            {
                                ++this.m_UndoCount;

                                this.ClearSelection();

                                this.text = this.m_UndoData[this.m_UndoData.Count - this.m_UndoCount - 1].Text;
                                this.m_CursorIndex = this.m_UndoData[this.m_UndoData.Count - this.m_UndoCount - 1].Index;
                            }
                            catch
                            {
                                --this.m_UndoCount;
                            }

                            this.m_Undoing = false;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case KeyCode.Backspace:
                        if (p.control)
                        {
                            this.DeletePreviousWord();
                            break;
                        }
                        else
                        {
                            this.DeletePreviousChar();
                            break;
                        }
                    case KeyCode.Return:
                        this.OnSubmit();
                        break;
                    case KeyCode.Escape:
                        this.ClearSelection();

                        this.m_CursorIndex = this.m_ScrollIndex = 0;

                        this.Invalidate();

                        this.OnCancel();
                        break;
                    default:
                        base.OnKeyDown(p);
                        return;
                }
                p.Use();
            }
            else
            {
                base.OnKeyDown(p);
            }
        }

        protected override void OnGotFocus(UIFocusEventParameter p)
        {
            base.OnGotFocus(p);

            this.m_UndoText = this.text;

            if (!this.readOnly)
            {
                this.m_TimeSinceFocus = Time.realtimeSinceStartup;
                this.StartCoroutine(this.MakeCursorBlink());

                if (this.selectOnFocus)
                {
                    this.m_SelectionStart = 0;
                    this.m_SelectionEnd = this.m_Text.Length;
                }
            }

            this.Invalidate();
        }

        protected override void OnLostFocus(UIFocusEventParameter p)
        {
            base.OnLostFocus(p);

            if (!this.m_FocusForced)
            {
                if (this.submitOnFocusLost)
                {
                    this.OnSubmit();
                }
                else
                {
                    this.OnCancel();
                }
            }

            this.m_FocusForced = false;
            this.m_CursorShown = false;

            this.ClearSelection();
            this.Invalidate();

            this.m_TimeSinceFocus = 0.0f;
        }

        protected override void OnDoubleClick(UIMouseEventParameter p)
        {
            if (!this.readOnly && UIMouseButtonExtensions.IsFlagSet(p.buttons, UIMouseButton.Left))
            {
                this.SelectWordAtIndex(this.GetCharIndexAt(p));
            }

            base.OnDoubleClick(p);
        }

        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (!this.readOnly && UIMouseButtonExtensions.IsFlagSet(p.buttons, UIMouseButton.Left))
            {
                int charIndexAt = this.GetCharIndexAt(p);

                if (charIndexAt != this.m_CursorIndex)
                {
                    this.m_CursorIndex = charIndexAt;
                    this.m_CursorShown = true;

                    this.Invalidate();

                    p.Use();
                }

                this.m_MouseSelectionAnchor = this.m_CursorIndex;
                this.m_SelectionStart = this.m_SelectionEnd = this.m_CursorIndex;
            }

            base.OnMouseDown(p);
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            if (!this.readOnly && this.hasFocus && UIMouseButtonExtensions.IsFlagSet(p.buttons, UIMouseButton.Left))
            {
                int charIndexAt = this.GetCharIndexAt(p);

                if (charIndexAt != this.m_CursorIndex)
                {
                    this.m_CursorIndex = charIndexAt;
                    this.m_CursorShown = true;

                    this.Invalidate();

                    p.Use();

                    this.m_SelectionStart = Mathf.Min(this.m_MouseSelectionAnchor, charIndexAt);
                    this.m_SelectionEnd = Mathf.Max(this.m_MouseSelectionAnchor, charIndexAt);

                    return;
                }
            }

            base.OnMouseMove(p);
        }

        #endregion
    }
}
