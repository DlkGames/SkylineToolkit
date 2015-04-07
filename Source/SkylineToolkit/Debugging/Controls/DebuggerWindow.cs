using SkylineToolkit.UI;
using SkylineToolkit.UI.CustomControls;
using SkylineToolkit.UI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.Debugging.Controls
{
    internal class DebuggerWindow : WindowControl
    {
        private static string windowTitleFormat = "SkylineToolkit Debugger\t\tFPS: {0:00.0}";

        private Debugger debugger;

        private UIDisposingManager disposingManager;

        #region Controls

        #region Tabs

        protected TabStrip tabStrip;
        protected TabContainer tabContainer;

        protected Panel tabPageConsole;
        protected Panel tabPageLog;
        protected Panel tabPageCode;
        protected Panel tabPageWatch;
        protected Panel tabPageInspector;

        #endregion

        #region Console tab

        protected AutocompleteTextField txtCommand;
        protected ScrollablePanel consoleLog;
        protected ScrollbarControl consoleLogScrollbar;

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

            tabContainer.SelectedIndex = -1;
            tabContainer.SelectedIndex = tabStrip.SelectedIndex;
        }

        protected virtual void SetupTabs()
        {
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

            tabStrip.AddTab("Console").Width = 120;
            tabStrip.AddTab("Log");
            tabStrip.AddTab("Code");
            tabStrip.AddTab("Watch");
            tabStrip.AddTab("Inspector");

            tabStrip.InitiallySelectedIndex = 0;
            tabStrip.SelectedIndex = 0;

            tabPageConsole = new Panel(tabContainer.Children[0]);
            tabPageLog = new Panel(tabContainer.Children[1]);
            tabPageCode = new Panel(tabContainer.Children[2]);
            tabPageWatch = new Panel(tabContainer.Children[3]);
            tabPageInspector = new Panel(tabContainer.Children[4]);
        }

        private void SetupTabPages()
        {
            SetupConsoleTab();

            Button testButton = new Button("test_button", "Testbutton Label", new Vector3(310, 100, 3));
            testButton.Width = 140;

            tabContainer.Children[1].AttachControl(testButton);
        }

        private void SetupConsoleTab()
        {
            txtCommand = new AutocompleteTextField("CommandField", String.Empty);
            tabPageConsole.AttachControl(disposingManager.R(txtCommand));
            txtCommand.Anchor = Anchor.Bottom | Anchor.Left | Anchor.Right;
            txtCommand.Size = new Vector2(tabContainer.Width - 50, txtCommand.Height);
            txtCommand.Pivot = PivotPoint.BottomLeft;
            txtCommand.RelativePosition = new Vector3(25, tabContainer.Height - (txtCommand.Height + 15));
            
            float consoleLogHeight = tabContainer.Height - (txtCommand.Height + 20);

            consoleLogScrollbar = new ScrollbarControl("ConsoleLogScrollbar");
            tabPageConsole.AttachControl(disposingManager.R(consoleLogScrollbar).Control);
            consoleLogScrollbar.Anchor = Anchor.Top | Anchor.Bottom | Anchor.Right;
            consoleLogScrollbar.Size = new Vector2(consoleLogScrollbar.Width, consoleLogHeight);
            consoleLogScrollbar.RelativePosition = new Vector3(tabContainer.Width - (consoleLogScrollbar.Width + 13), 0);

            consoleLog = new ScrollablePanel("ConsoleLog");
            tabPageConsole.AttachControl(disposingManager.R(consoleLog));
            consoleLog.Anchor = Anchor.All;
            consoleLog.Size = new Vector2(tabContainer.Width - (26 + consoleLogScrollbar.Width), consoleLogHeight);
            consoleLog.Pivot = PivotPoint.TopLeft;
            consoleLog.RelativePosition = new Vector3(13, 0);
            consoleLog.Color = new Color32(30, 30, 30, 150);
            consoleLog.VerticalScrollbar = consoleLogScrollbar.Control;
            
            Button test = new Button("test", "testbutton", Vector3.one, new Vector2(100, 800));
            consoleLog.AttachControl(test);
        }

        #region IDisposable

        public override void Dispose()
        {
            disposingManager.Dispose();

            base.Dispose();
        }

        #endregion
    }
}
