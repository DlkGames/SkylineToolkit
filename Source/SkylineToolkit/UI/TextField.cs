using ColossalFramework.UI;
using SkylineToolkit.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class TextField : ColossalUserControl
    {
        #region Events

        public event PropChangedEventHandler<bool> IsReadOnlyChanged;

        public event PropChangedEventHandler<string> PasswordHintChanged;

        public event PropChangedEventHandler<string> TextChanged;

        public event PropChangedEventHandler<string> TextSubmitted;

        public event PropChangedEventHandler<string> TextCancelled;

        #endregion

        #region Constructors

        public TextField()
            : base("TextField", typeof(UITextField))
        {
        }

        public TextField(string name)
            : this(name, name, new Vector3(0f,0f))
        {
        }

        public TextField(string name, string label, Vector3 position)
            : this(name, label, position, new Vector2(200, 40))
        {
        }

        public TextField(string name, string label, Vector3 position, Vector2 size)
            : base(name, typeof(UITextField))
        {
            this.Position = position;
            this.Text = label;

            this.UIComponent.size = size;

            this.SetDefaultStyle();
        }

        public TextField(UITextField textField)
            : base(textField)
        {
        }

        public TextField(IColossalControl control) 
            : base(control)
        {
        }

        #endregion

        #region Properties

        #region Component

        public new UITextField UIComponent
        {
            get
            {
                return (UITextField)base.UIComponent;
            }
            set
            {
                base.UIComponent = value;
            }
        }

        #endregion

        #region Selection

        public string Selection
        {
            get
            {
                return this.UIComponent.selectedText;
            }
        }

        public int SelectionStart
        {
            get
            {
                return this.UIComponent.selectionStart;
            }

            set
            {
                this.UIComponent.selectionStart = value;
            }
        }

        public int SelectionEnd
        {
            get
            {
                return this.UIComponent.selectionEnd;
            }
            set
            {
                this.UIComponent.selectionEnd = value;
            }
        }

        public int SelectionLength
        {
            get
            {
                return this.UIComponent.selectionLength;
            }
        }

        #endregion

        #region State

        public float CursorBlinkTime
        {
            get
            {
                return this.UIComponent.cursorBlinkTime;
            }
            set
            {
                this.UIComponent.cursorBlinkTime = value;
            }
        }

        public int CursorWidth
        {
            get
            {
                return this.UIComponent.cursorWidth;
            }
            set
            {
                this.UIComponent.cursorWidth = value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.UIComponent.readOnly;
            }
            set
            {
                this.UIComponent.readOnly = value;
            }
        }

        public bool SelectOnFocus
        {
            get
            {
                return this.UIComponent.selectOnFocus;
            }
            set
            {
                this.UIComponent.selectOnFocus = value;
            }
        }

        public bool SubmitOnFocusLost
        {
            get
            {
                return this.UIComponent.submitOnFocusLost;
            }
            set
            {
                this.UIComponent.submitOnFocusLost = value;
            }
        }

        public static int UndoLimit
        {
            get
            {
                return UITextField.kUndoLimit;
            }
            set
            {
                UITextField.kUndoLimit = value;
            }
        }

        #endregion

        #region Content

        public bool IsPassword
        {
            get
            {
                return this.UIComponent.isPasswordField;
            }
            set
            {
                this.UIComponent.isPasswordField = value;
            }
        }

        public bool IsNumeric
        {
            get
            {
                return this.UIComponent.numericalOnly;
            }
            set
            {
                this.UIComponent.numericalOnly = value;
            }
        }

        public bool AllowFloats
        {
            get
            {
                return this.UIComponent.allowFloats;
            }
            set
            {
                this.UIComponent.allowFloats = value;
            }
        }

        /// <summary>
        /// TODO Find a way to hook this into the control
        /// </summary>
        public int MinLength
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int MaxLength
        {
            get
            {
                return this.UIComponent.maxLength;
            }
            set
            {
                this.UIComponent.maxLength = value;
            }
        }

        public string Text
        {
            get
            {
                return this.UIComponent.text;
            }
            set
            {
                this.UIComponent.text = value;
            }
        }

        #endregion

        #region Appearance

        public RectOffset Padding
        {
            get
            {
                return this.UIComponent.padding;
            }
            set
            {
                this.UIComponent.padding = value;
            }
        }

        public string PasswordHint
        {
            get
            {
                return this.UIComponent.passwordCharacter;
            }
            set
            {
                this.UIComponent.passwordCharacter = value;
            }
        }

        public Color32 SelectionBackgroundColor
        {
            get
            {
                return this.UIComponent.selectionBackgroundColor;
            }
            set
            {
                this.UIComponent.selectionBackgroundColor = value;
            }
        }

        public string SelectionSprite
        {
            get
            {
                return this.UIComponent.selectionSprite;
            }
            set
            {
                this.UIComponent.selectionSprite = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Selection

        public virtual void ClearSelection()
        {
            this.UIComponent.ClearSelection();
        }

        protected void CopySelectionToClipboard()
        {
            CallUIComponentMethod<object>("CopySelectionToClipboard");
        }

        protected void CutSelectionToClipboard()
        {
            CallUIComponentMethod<object>("CutSelectionToClipboard");
        }

        public virtual void MoveSelectionPointLeft()
        {
            this.UIComponent.MoveSelectionPointLeft();
        }

        public virtual void MoveSelectionPointLeftWord()
        {
            this.UIComponent.MoveSelectionPointLeftWord();
        }

        public virtual void MoveSelectionPointRight()
        {
            this.UIComponent.MoveSelectionPointRight();
        }

        public virtual void MoveSelectionPointRightWord()
        {
            this.UIComponent.MoveSelectionPointRightWord();
        }

        public virtual void SelectAll()
        {
            this.UIComponent.SelectAll();
        }

        public virtual void SelectToStart()
        {
            this.UIComponent.SelectToStart();
        }

        public virtual void SelectToEnd()
        {
            this.UIComponent.SelectToEnd();
        }

        public virtual void SelectWordAt(int index)
        {
            this.UIComponent.SelectWordAtIndex(index);
        }

        #endregion

        #region Content

        protected void DeleteNextChar()
        {
            CallUIComponentMethod<object>("DeleteNextChar");
        }

        protected void DeleteNextWord()
        {
            CallUIComponentMethod<object>("DeleteNextWord");
        }

        protected void DeletePreviousChar()
        {
            CallUIComponentMethod<object>("DeletePreviousChar");
        }

        protected void DeletePreviousWord()
        {
            CallUIComponentMethod<object>("DeletePreviousWord");
        }

        protected void DeleteSelection()
        {
            CallUIComponentMethod<object>("DeleteSelection");
        }

        public virtual int FindNextWord(int start)
        {
            return this.UIComponent.FindNextWord(start);
        }

        public virtual int FindPreviousWord(int start)
        {
            return this.UIComponent.FindPreviousWord(start);
        }

        public bool IsDigit(char character)
        {
            return CallUIComponentMethod<bool>("IsDigit", character);
        }

        #endregion

        #region Cursor Movement

        public virtual void MoveToStart()
        {
            this.UIComponent.MoveToStart();
        }

        public virtual void MoveToEnd()
        {
            this.UIComponent.MoveToEnd();
        }

        public virtual void MoveToNextChar()
        {
            this.UIComponent.MoveToNextChar();
        }

        public virtual void MoveToNextWord()
        {
            this.UIComponent.MoveToNextWord();
        }

        public virtual void MoveToPreviousChar()
        {
            this.UIComponent.MoveToPreviousChar();
        }

        public virtual void MoveToPreviousWord()
        {
            this.UIComponent.MoveToPreviousWord();
        }

        public virtual void MoveToSelectionStart()
        {
            this.UIComponent.MoveToSelectionStart();
        }

        public virtual void MoveToSelectionEnd()
        {
            this.UIComponent.MoveToSelectionEnd();
        }

        #endregion

        private void SetDefaultStyle()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Event wrappers

        protected void OnReadOnlyChanged(UIComponent component, bool e)
        {
            if (this.IsReadOnlyChanged != null)
            {
                PropChangedEventArgs<bool> args = new PropChangedEventArgs<bool>("IsReadOnly", e);

                this.IsReadOnlyChanged(this, args);
            }
        }

        protected void OnPasswordHintChanged(UIComponent component, string e)
        {
            if (this.PasswordHintChanged != null)
            {
                PropChangedEventArgs<string> args = new PropChangedEventArgs<string>("PasswordHint", e);

                this.PasswordHintChanged(this, args);
            }
        }

        protected void OnTextChanged(UIComponent component, string e)
        {
            if (this.TextChanged != null)
            {
                PropChangedEventArgs<string> args = new PropChangedEventArgs<string>("Text", e);

                this.TextChanged(this, args);
            }
        }

        protected void OnTextSubmitted(UIComponent component, string e)
        {
            if (this.TextSubmitted != null)
            {
                PropChangedEventArgs<string> args = new PropChangedEventArgs<string>("Text", e);

                this.TextSubmitted(this, args);
            }
        }

        protected void OnTextCancelled(UIComponent component, string e)
        {
            if (this.TextCancelled != null)
            {
                PropChangedEventArgs<string> args = new PropChangedEventArgs<string>("Text", e);

                this.TextCancelled(this, args);
            }
        }

        #endregion
    }
}
