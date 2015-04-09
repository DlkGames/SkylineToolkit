using ColossalFramework.UI;
using SkylineToolkit.UI;
using SkylineToolkit.UI.CustomControls;
using System;
using System.Linq;
using UnityEngine;

namespace SkylineToolkit.Debugging.Controls
{
    internal class DebuggerWindow : WindowControl
    {
        private static string windowTitleFormat = "SkylineToolkit Debugger\t\tFPS: {0:00.0}";

        private Debugger debugger;

        private UIDisposingManager disposingManager;

        #region Controls

        protected RectOffset tabContainerPadding = new RectOffset(13, 13, 0, 15);

        #region Tabs

        protected TabStrip tabStrip;
        protected TabContainer tabContainer;

        protected Panel tabPageConsole;
        protected Panel tabPageLog;
        protected Panel tabPageCode;
        protected Panel tabPageInspector;
        protected Panel tabPageWatch;

        #endregion

        #region Console tab

        protected AutocompleteTextField ct_txtCommand;
        protected ScrollablePanel ct_consoleLog;
        protected ScrollbarControl ct_consoleLogScrollbar;

        #endregion

        #region Log tab

        protected ScrollablePanel lt_panel;
        protected ScrollbarControl lt_panelScrollbar;

        #endregion

        #region Code tab

        protected ScrollableContainer ct_container;
        protected TextArea ct_text;
        protected Button ct_runBtn;
        protected Button ct_clearBtn;

        #endregion

        #endregion

        internal Debugger Debugger
        {
            get { return debugger; }
            set { debugger = value; }
        }

        protected UIDisposingManager DisposingManager
        {
            get { return disposingManager; }
            set { disposingManager = value; }
        }


        #region Unity Engine callbacks

        protected override void Awake()
        {
            DontDestroyOnLoad(this);

            base.Awake();

            //this.WindowPanel.UIComponent.hideFlags = HideFlags.DontSave;

            try
            {
                SetupGUI();
            }
            catch (Exception ex)
            {
                Log.Error("Unable to setup debugger window GUI. See the following error for more information:");
                Log.Exception(ex);
            }
        }

        protected virtual void OnGUI()
        {
            if (!this.IsVisible)
            {
                return;
            }

            this.Title = String.Format(windowTitleFormat, debugger.FpsCounter.Current);
        }

        #endregion

        protected virtual void SetupGUI()
        {
            if (disposingManager == null)
            {
                disposingManager = new UIDisposingManager();
            }

            this.Size = WindowPanel.MinSize = new Vector2(700, 400);
            this.Position = new Vector3((WindowPanel.Size.x / 2) * -1, (WindowPanel.Size.y / 2) * -1);
            this.IsResizable = true;
            this.IsVisible = false;

            SetupTabs();

            SetupTabPages();

            RefreshTabPage();
        }

        private void RefreshTabPage()
        {
            if (tabContainer == null || tabStrip == null)
            {
                return;
            }

            Log.Verbose("Debugger", "Refreshing debugger tab");

            tabContainer.SelectedIndex = -1;
            tabContainer.SelectedIndex = tabStrip.SelectedIndex;
        }

        protected virtual void SetupTabs()
        {
            Log.Verbose("Debugger", "Setup debug window tabs...");

            tabStrip = new TabStrip("DebuggerTabStrip");
            tabContainer = new TabContainer("DebuggerTabContainer");
            tabStrip.Container = tabContainer;

            InnerPanel.AttachControl(disposingManager.R(tabStrip));
            InnerPanel.AttachControl(disposingManager.R(tabContainer));

            tabStrip.Anchor = Anchor.Top | Anchor.CenterHorizontal;
            tabStrip.Size = new Vector2(5 * 120 + 4, 37);
            tabStrip.RelativePosition = Vector3.zero;

            tabContainer.Anchor = Anchor.All;
            tabContainer.RelativePosition = new Vector3(0, tabStrip.Height + 2);
            tabContainer.Size = new Vector2(InnerPanel.Width, InnerPanel.Height - (tabStrip.Height + 2));

            var tab1 = tabStrip.AddTab("Console");
            tab1.Width = 120; tab1.ZOrder = 10;
            tabStrip.AddTab("Log").ZOrder = 20;
            tabStrip.AddTab("Code").ZOrder = 30;
            tabStrip.AddTab("Inspector").ZOrder = 40;
            tabStrip.AddTab("Watch").ZOrder = 50;

            tabStrip.InitiallySelectedIndex = 0;
            tabStrip.SelectedIndex = 0;

            UIComponent[] tabs = tabStrip.UIComponent.tabs.OfType<UIButton>().ToArray();

            tabPageConsole = new Panel(tabContainer.Children[0]);
            tabPageLog = new Panel(tabContainer.Children[1]);
            tabPageCode = new Panel(tabContainer.Children[2]);
            tabPageInspector = new Panel(tabContainer.Children[3]);
            tabPageWatch = new Panel(tabContainer.Children[4]);
        }

        private void SetupTabPages()
        {
            Log.Verbose("Debugger", "Setup debugger window tab pages...");

            SetupConsoleTab();

            SetupLogTab();

            SetupCodeTab();
        }

        private void SetupConsoleTab()
        {
            Log.Verbose("Debugger", "Setup debugger console tab...");

            float totalHPadding = tabContainerPadding.left + tabContainerPadding.right;

            ct_txtCommand = new AutocompleteTextField("CommandField", String.Empty);
            tabPageConsole.AttachControl(disposingManager.R(ct_txtCommand));
            ct_txtCommand.Anchor = Anchor.Bottom | Anchor.Left | Anchor.Right;
            ct_txtCommand.Size = new Vector2(tabContainer.Width - (totalHPadding + 24), ct_txtCommand.Height);
            ct_txtCommand.Pivot = PivotPoint.BottomLeft;
            ct_txtCommand.RelativePosition = new Vector3(tabContainerPadding.left, tabContainer.Height - (ct_txtCommand.Height + tabContainerPadding.bottom));

            float consoleLogHeight = tabContainer.Height - (ct_txtCommand.Height + tabContainerPadding.bottom + 5);

            ct_consoleLogScrollbar = new ScrollbarControl("ConsoleLogScrollbar");
            tabPageConsole.AttachControl(disposingManager.R(ct_consoleLogScrollbar).Control);
            ct_consoleLogScrollbar.Anchor = Anchor.Top | Anchor.Bottom | Anchor.Right;
            ct_consoleLogScrollbar.Size = new Vector2(ct_consoleLogScrollbar.Width, consoleLogHeight);
            ct_consoleLogScrollbar.RelativePosition = new Vector3(tabContainer.Width - (ct_consoleLogScrollbar.Width + tabContainerPadding.left), tabContainerPadding.top);

            ct_consoleLog = new ScrollablePanel("ConsoleLog");
            tabPageConsole.AttachControl(disposingManager.R(ct_consoleLog));
            ct_consoleLog.Anchor = Anchor.All;
            ct_consoleLog.Size = new Vector2(tabContainer.Width - (totalHPadding + ct_consoleLogScrollbar.Width), consoleLogHeight);
            ct_consoleLog.Pivot = PivotPoint.TopLeft;
            ct_consoleLog.RelativePosition = new Vector3(tabContainerPadding.left, tabContainerPadding.top);
            ct_consoleLog.Color = new Color32(30, 30, 30, 150);
            ct_consoleLog.VerticalScrollbar = ct_consoleLogScrollbar.Control;
            
            Button test = new Button("test", "testbutton", Vector3.one, new Vector2(100, 800));
            ct_consoleLog.AttachControl(test);
        }

        private void SetupLogTab()
        {
            Log.Verbose("Debugger", "Setup debugger log tab...");

            float totalHPadding = tabContainerPadding.left + tabContainerPadding.right;
            float logPanelHeight = tabContainer.Height - tabContainerPadding.bottom;

            lt_panelScrollbar = new ScrollbarControl("LogPanelScrollbar");
            tabPageLog.AttachControl(disposingManager.R(lt_panelScrollbar).Control);
            lt_panelScrollbar.Anchor = Anchor.Top | Anchor.Bottom | Anchor.Right;
            lt_panelScrollbar.Size = new Vector2(lt_panelScrollbar.Width, logPanelHeight);
            lt_panelScrollbar.RelativePosition = new Vector3(tabContainer.Width - (lt_panelScrollbar.Width + tabContainerPadding.left), tabContainerPadding.top);

            lt_panel = new ScrollablePanel("LogPanel");
            tabPageLog.AttachControl(disposingManager.R(lt_panel));
            lt_panel.Anchor = Anchor.All;
            lt_panel.Size = new Vector2(tabContainer.Width - (totalHPadding + ct_consoleLogScrollbar.Width), logPanelHeight);
            lt_panel.Pivot = PivotPoint.TopLeft;
            lt_panel.RelativePosition = new Vector3(tabContainerPadding.left, tabContainerPadding.top);
            lt_panel.Color = new Color32(30, 30, 30, 150);
            lt_panel.VerticalScrollbar = ct_consoleLogScrollbar.Control;
        }

        private void SetupCodeTab()
        {
            Log.Verbose("Debugger", "Setup debugger code tab...");

            float totalHPadding = tabContainerPadding.left + tabContainerPadding.right;
            float buttonWidth = 120;
            float buttonPadding = 10;
            float buttonExtraMargin = 40;
            float buttonWidthPlace = buttonWidth + buttonPadding;

            ct_runBtn = new Button("RunBtn", "Run");
            tabPageCode.AttachControl(disposingManager.R(ct_runBtn));
            ct_runBtn.Anchor = UI.Anchor.Bottom | UI.Anchor.Right;
            ct_runBtn.Width = buttonWidth;
            ct_runBtn.RelativePosition = new Vector2(tabContainer.Width - (2 * buttonWidthPlace + buttonExtraMargin), tabContainer.Height - (ct_runBtn.Height + tabContainerPadding.bottom));
            ct_runBtn.Click += ct_runBtn_Click;

            ct_clearBtn = new Button("ClearBtn", "Clear");
            tabPageCode.AttachControl(disposingManager.R(ct_clearBtn));
            ct_clearBtn.Anchor = UI.Anchor.Bottom | UI.Anchor.Right;
            ct_clearBtn.Width = buttonWidth;
            ct_clearBtn.RelativePosition = new Vector2(tabContainer.Width - (buttonWidthPlace + buttonExtraMargin), tabContainer.Height - (ct_clearBtn.Height + tabContainerPadding.bottom));
            ct_clearBtn.Click += ct_clearBtn_Click;

            ct_container = new ScrollableContainer("CodeContainer");
            tabPageCode.AttachContainer(disposingManager.R(ct_container));
            ct_container.Position = Vector3.zero;
            ct_container.Size = tabContainer.Size - new Vector2(totalHPadding, ct_runBtn.Height + tabContainerPadding.bottom + 5);
            ct_container.RelativePosition = new Vector3(tabContainerPadding.left, tabContainerPadding.top);

            ct_text = new TextArea("Code");
            ct_container.Panel.AttachControl(disposingManager.R(ct_text));
            ct_text.Anchor = UI.Anchor.All;
            ct_text.RelativePosition = new Vector3(1, 1); // Small offset to prevent scrollbar flickering
            ct_text.Size = ct_container.Panel.Size - new Vector2(2, 2); // Small offset to prevent scrollbar flickering
        }

        #region Event handlers

        private void ct_runBtn_Click(object sender, MouseEventArgs e)
        {
            if(ct_text == null || String.IsNullOrEmpty(ct_text.Text))
            {
                return;
            }

            Log.Verbose("Debugger", "Clicked run code button, starting code execution...");

            tabStrip.SelectedIndex = 0;

            debugger.ExecuteCode(ct_text.Text);
        }

        private void ct_clearBtn_Click(object sender, MouseEventArgs e)
        {
            if (ct_text != null)
            {
                Log.Verbose("Debugger", "Clicked clear code button");

                ct_text.Text = String.Empty;
            }
        }

        #endregion

        #region IDisposable

        public override void Dispose()
        {
            disposingManager.Dispose();

            base.Dispose();
        }

        #endregion
    }
}
