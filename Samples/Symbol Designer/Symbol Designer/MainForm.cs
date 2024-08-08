

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.IO;
using Syncfusion.Windows.Forms.Diagram.Controls;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Tools.XPMenus;
using System.Diagnostics;
using Syncfusion.Licensing;

namespace Syncfusion.Windows.Forms.Diagram.Samples.DiagramTool
{
    /// <summary>
    /// MainForm for the Essential Diagram Builder.
    /// </summary>
    public class MainForm
        : Office2007Form
    {
        #region Constants
        private const string c_strMSG_PROMPT_SAVE =
            "You have made changes to the symbol palette. Would you like to save your changes?";
        private const string c_strCAPTION_PROMPT_SAVE = "Save Changes?";
        private const string c_strFORM_TITLE = "Symbol Designer";
        private const string c_strSYMBOL_DEFAULT_NAME = "New Symbol";
        private const string c_strMSG_REMOVE_SYMBOL = "Are you sure you want to remove '{0}' symbol from current palette?";
        private const string c_strUNDO = "&Undo";
        private const string c_strREDO = "&Redo";
        private const int WM_CLOSE = 16;
        #endregion

        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private bool m_bNeedSave;
        private bool m_bSaving;
        // Palette node to Diagram mapping
        private Hashtable m_hashPalNodeToModelMapping;
        #endregion

        #region Form Controls
        private Tools.DockingManager dockingManager;
        private Tools.TabbedMDIManager tabbedMDIManager;
        private OpenFileDialog openPaletteDialog;
        private OpenFileDialog openDiagramDialog;
        private SaveFileDialog saveDiagramDialog;
        private SaveFileDialog savePaletteDialog;
        private OpenFileDialog openImageDialog;
        private PaletteGroupBar symbolPaletteGroupBar;
        private Tools.XPMenus.MainFrameBarManager mainFrameBarManager;
        private Tools.XPMenus.Bar mainMenuBar;
        private Tools.XPMenus.BarItem biFileNew;
        private Tools.XPMenus.BarItem biFileOpen;
        private Tools.XPMenus.BarItem biFileAppend;
        private Tools.XPMenus.BarItem biFileOpenDiagram;
        private Tools.XPMenus.BarItem biFileSave;
        private Tools.XPMenus.BarItem biFileSaveAs;
        private Tools.XPMenus.BarItem biFileExit;
        private Tools.XPMenus.BarItem barShowRulers;
        private Tools.XPMenus.BarItem biTabbedMDI;
        private Tools.XPMenus.BarItem barItemEditCut;
        private Tools.XPMenus.BarItem barItemEditCopy;
        private Tools.XPMenus.BarItem barItemEditPaste;
        private Tools.XPMenus.BarItem barItemEditUndo;
        private Tools.XPMenus.BarItem barItemEditRedo;
        private Tools.XPMenus.BarItem barItemEditDelete;
        private Tools.XPMenus.BarItem barItemAbout;
        private PropertyEditor propertyEditor;
        private Tools.XPMenus.Bar standardToolbar;
        private Tools.XPMenus.BarItem biFilePrint;
        private Tools.XPMenus.BarItem biPageSetup;
        private Tools.XPMenus.BarItem barItemViewSymbolPalette;
        private Tools.XPMenus.BarItem barItemViewProperties;
        private Tools.XPMenus.BarItem barItemEditSelectAll;
        private Tools.XPMenus.ParentBarItem parentBarItemEdit;
        private Tools.XPMenus.ParentBarItem parentBarItemFile;
        private Tools.XPMenus.ParentBarItem parentBarItemView;
        private Tools.XPMenus.ParentBarItem parentBarItemWindow;
        private Tools.XPMenus.ParentBarItem parentBarItemHelp;
        private ImageList smallImageList;
        private Tools.XPMenus.BarItem biPrintPreview;
        private Tools.XPMenus.BarItem barItemPanZoom;
        private OverviewControl overviewControl1;
        private Tools.XPMenus.BarItem barItemPageBorders;
        private Tools.XPMenus.ParentBarItem parentBarItemToolsGrouping;
        private Tools.XPMenus.ParentBarItem parentBarItemToolsOrder;
        private Tools.XPMenus.BarItem barItemAlignLeft;
        private Tools.XPMenus.BarItem barItemAlignCenter;
        private Tools.XPMenus.BarItem barItemAlignRight;
        private Tools.XPMenus.BarItem barItemAlignTop;
        private Tools.XPMenus.BarItem barItemAlignMiddle;
        private Tools.XPMenus.BarItem barItemAlignBottom;
        private Tools.XPMenus.BarItem barItemFlipHorizontally;
        private Tools.XPMenus.BarItem barItemFlipVertically;
        private Tools.XPMenus.BarItem barItemFlipBoth;
        private Tools.XPMenus.BarItem barItemGroupingGroup;
        private Tools.XPMenus.BarItem barItemGroupingUnGroup;
        private Tools.XPMenus.BarItem barItemOrderFront;
        private Tools.XPMenus.BarItem barItemOrderForward;
        private Tools.XPMenus.BarItem barItemOrderBackward;
        private Tools.XPMenus.BarItem barItemOrderBack;
        private Tools.XPMenus.BarItem barItemRotateClock;
        private Tools.XPMenus.BarItem barItemRotateConter;
        private Tools.XPMenus.BarItem barItemResizeWidth;
        private Tools.XPMenus.BarItem barItemResizeHeight;
        private Tools.XPMenus.BarItem barItemResizeSize;
        private Tools.XPMenus.BarItem barItemResizeAcross;
        private Tools.XPMenus.BarItem barItemResizeDown;
        private Tools.XPMenus.ParentBarItem parentBarItemActions;
        private Tools.XPMenus.ParentBarItem parentBarItemAlign;
        private Tools.XPMenus.ParentBarItem parentBarItemFlip;
        private Tools.XPMenus.ParentBarItem parentBarItemRotate;
        private Tools.XPMenus.ParentBarItem parentBarItemResize;
        private Tools.XPMenus.ParentBarItem parentBarItemNode;
        private Tools.XPMenus.ParentBarItem parentBarItemPopUp;
        private Tools.XPMenus.BarItem barItemLayout;
        private Tools.XPMenus.ParentBarItem parentBarItemFormat;
        private Tools.XPMenus.BarItem barItemFillStyle;
        private Tools.XPMenus.BarItem barItemShadowStyle;
        private Tools.XPMenus.ParentBarItem parentBarItem1;
        private Tools.XPMenus.BarItem barAddSymbol;
        private Tools.XPMenus.BarItem barRemoveSymbol;
        private DocumentExplorer documentExplorer1;
        private BarItem biFileClosePalette;
        private Bar symbolToolBar;
        private BarItem barItemObjectModel;
        private BarItem barItemHelp;
        private SplashPanel splashPanel1;
        private SplashControl splashControl1;
        private IContainer components;
        #endregion

        #region Initialize/finalize methods
        public MainForm(string fileLoc)
            : this()
        {
            openDiagramDialog.FileName = String.Empty;
            saveDiagramDialog.FileName = String.Empty;

            // load palette
            if (File.Exists(fileLoc))
            {
                SymbolPalette newPalette = OpenPaletteFile(fileLoc);

                if (newPalette != null)
                {
                    symbolPaletteGroupBar.AddPalette(newPalette);
                    foreach (Node node in newPalette.Nodes)
                    {
                        // create mappings
                        this.NodeToDiagramMap.Add(node, null);
                    }
                }
            }

            UpdateMainFormTitle();
        }

        public MainForm()
        {
            InitializeComponent();
            splashControl1.ShowDialogSplash(this);
            this.ColorScheme = Office2007Theme.Silver;

            //
            // Initialize tabbed MDI manager
            //
            tabbedMDIManager = new Tools.TabbedMDIManager();
            tabbedMDIManager.AttachToMdiContainer(this);
            tabbedMDIManager.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererDockingWhidbey);

            this.dockingManager.DockContextMenu += new DockContextMenuEventHandler(dockingManager_DockContextMenu);
            this.dockingManager.DragAllow += new DragAllowEventHandler(dockingManager_DragAllow);

            // Wire up OnIdle processing
            Application.Idle += new EventHandler(this.OnIdle);

            #region       // PropertyEditor Color Settings
            this.propertyEditor.PropertyGrid.BackColor = System.Drawing.Color.Silver;
            this.propertyEditor.PropertyGrid.CommandsBackColor = System.Drawing.Color.Silver;
            this.propertyEditor.PropertyGrid.CommandsForeColor = System.Drawing.Color.Silver;
            this.propertyEditor.PropertyGrid.LineColor = System.Drawing.Color.Silver;
            this.propertyEditor.PropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
            #endregion

        }

        void dockingManager_DragAllow(object sender, DragAllowEventArgs arg)
        {
            if (arg.Control.Name.Equals("symbolPaletteGroupBar") || arg.Control.Name.Equals("documentExplorer1"))
                arg.Cancel = true;
        }

        Control dockControl;

        void dockingManager_DockContextMenu(object sender, DockContextMenuEventArgs arg)
        {
            arg.ContextMenu.BeforePopup += new CancelMouseEventHandler(ContextMenu_BeforePopup);
            dockControl = arg.Owner;
        }

        void ContextMenu_BeforePopup(object sender, CancelMouseEventArgs e)
        {
            if (dockControl.Name.Equals("symbolPaletteGroupBar") || dockControl.Name.Equals("documentExplorer1"))
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            UpdateMDIChildText();
        }

        private void UpdateMDIChildText()
        {
            if (this.ActiveMdiChild != null)
            {
                if (this.ActiveMdiChild.GetType().Name.Equals("DiagramForm"))
                {
                    if (!this.ActiveMdiChild.Text.EndsWith("*"))
                        this.ActiveMdiChild.Text += "*";
                    if (!this.symbolPaletteGroupBar.GroupBarItems[0].Text.EndsWith("*"))
                        this.symbolPaletteGroupBar.GroupBarItems[0].Text += "*";

                    this.symbolPaletteGroupBar.Refresh();
                }
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    this.dockingManager.Dispose();
                    this.propertyEditor.Dispose();
                    this.symbolPaletteGroupBar.Dispose();
                    this.overviewControl1.Dispose();
                    this.documentExplorer1.Dispose();
                    this.openPaletteDialog.Dispose();
                    this.savePaletteDialog.Dispose();
                    this.openDiagramDialog.Dispose();
                    this.saveDiagramDialog.Dispose();
                    this.smallImageList.Dispose();
                    this.openImageDialog.Dispose();
                    this.mainMenuBar.Dispose();
                    this.parentBarItemFile.Dispose();
                    this.biFileNew.Dispose();
                    this.biFileOpen.Dispose();
                    this.biFileAppend.Dispose();
                    this.biFileClosePalette.Dispose();
                    this.biFileSave.Dispose();
                    this.biFileSaveAs.Dispose();
                    this.biPageSetup.Dispose();
                    this.biPrintPreview.Dispose();
                    this.biFilePrint.Dispose();
                    this.biFileExit.Dispose();
                    this.parentBarItemEdit.Dispose();
                    this.barItemEditUndo.Dispose();
                    this.barItemEditRedo.Dispose();
                    this.barItemEditSelectAll.Dispose();
                    this.barItemEditDelete.Dispose();
                    this.barItemEditCut.Dispose();
                    this.barItemEditCopy.Dispose();
                    this.barItemEditPaste.Dispose();
                    this.parentBarItemFormat.Dispose();
                    this.barItemFillStyle.Dispose();
                    this.barItemShadowStyle.Dispose();
                    this.parentBarItem1.Dispose();
                    this.barAddSymbol.Dispose();
                    this.barRemoveSymbol.Dispose();
                    this.parentBarItemView.Dispose();
                    this.barShowRulers.Dispose();
                    this.barItemViewSymbolPalette.Dispose();
                    this.barItemViewProperties.Dispose();
                    this.barItemObjectModel.Dispose();
                    this.barItemPanZoom.Dispose();
                    this.barItemPageBorders.Dispose();
                    this.parentBarItemActions.Dispose();
                    this.parentBarItemAlign.Dispose();
                    this.barItemAlignLeft.Dispose();
                    this.barItemAlignCenter.Dispose();
                    this.barItemAlignRight.Dispose();
                    this.barItemAlignTop.Dispose();
                    this.barItemAlignMiddle.Dispose();
                    this.barItemAlignBottom.Dispose();
                    this.parentBarItemFlip.Dispose();
                    this.barItemFlipHorizontally.Dispose();
                    this.barItemFlipVertically.Dispose();
                    this.barItemFlipBoth.Dispose();
                    this.parentBarItemToolsGrouping.Dispose();
                    this.barItemGroupingGroup.Dispose();
                    this.barItemGroupingUnGroup.Dispose();
                    this.parentBarItemToolsOrder.Dispose();
                    this.barItemOrderFront.Dispose();
                    this.barItemOrderForward.Dispose();
                    this.barItemOrderBackward.Dispose();
                    this.barItemOrderBack.Dispose();
                    this.parentBarItemRotate.Dispose();
                    this.barItemRotateClock.Dispose();
                    this.barItemRotateConter.Dispose();
                    this.parentBarItemResize.Dispose();
                    this.barItemResizeWidth.Dispose();
                    this.barItemResizeHeight.Dispose();
                    this.barItemResizeSize.Dispose();
                    this.barItemResizeAcross.Dispose();
                    this.barItemResizeDown.Dispose();
                    this.barItemLayout.Dispose();
                    this.parentBarItemWindow.Dispose();
                    this.biTabbedMDI.Dispose();
                    this.parentBarItemHelp.Dispose();
                    this.barItemAbout.Dispose();
                    this.barItemHelp.Dispose();
                    this.standardToolbar.Dispose();
                    this.symbolToolBar.Dispose();
                    this.biFileOpenDiagram.Dispose();
                    this.parentBarItemPopUp.Dispose();
                    this.parentBarItemNode.Dispose();
                    this.splashPanel1.Dispose();
                    this.splashControl1.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbpropertyEditor = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbsymbolPaletteGroupBar = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccboverviewControl1 = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection ccbdocumentExplorer1 = new Syncfusion.Windows.Forms.Tools.CaptionButtonsCollection();
            this.dockingManager = new Syncfusion.Windows.Forms.Tools.DockingManager(this.components);
            this.propertyEditor = new Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor(this.components);
            this.symbolPaletteGroupBar = new Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar(this.components);
            this.overviewControl1 = new Syncfusion.Windows.Forms.Diagram.Controls.OverviewControl(this.components);
            this.documentExplorer1 = new Syncfusion.Windows.Forms.Diagram.Controls.DocumentExplorer();
            this.openPaletteDialog = new System.Windows.Forms.OpenFileDialog();
            this.savePaletteDialog = new System.Windows.Forms.SaveFileDialog();
            this.openDiagramDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveDiagramDialog = new System.Windows.Forms.SaveFileDialog();
            this.smallImageList = new System.Windows.Forms.ImageList(this.components);
            this.openImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.mainFrameBarManager = new Syncfusion.Windows.Forms.Tools.XPMenus.MainFrameBarManager(this);
            this.mainMenuBar = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "MainMenu");
            this.parentBarItemFile = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.biFileNew = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.biFileOpen = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.biFileAppend = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.biFileClosePalette = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.biFileSave = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.biFileSaveAs = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.biPageSetup = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.biPrintPreview = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.biFilePrint = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.biFileExit = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemEdit = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barItemEditUndo = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemEditRedo = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemEditSelectAll = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemEditDelete = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemEditCut = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemEditCopy = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemEditPaste = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemFormat = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barItemFillStyle = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemShadowStyle = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barAddSymbol = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barRemoveSymbol = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemView = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barShowRulers = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemViewSymbolPalette = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemViewProperties = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemObjectModel = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemPanZoom = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemPageBorders = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemActions = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.parentBarItemAlign = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barItemAlignLeft = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemAlignCenter = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemAlignRight = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemAlignTop = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemAlignMiddle = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemAlignBottom = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemFlip = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barItemFlipHorizontally = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemFlipVertically = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemFlipBoth = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemToolsGrouping = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barItemGroupingGroup = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemGroupingUnGroup = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemToolsOrder = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barItemOrderFront = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemOrderForward = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemOrderBackward = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemOrderBack = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemRotate = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barItemRotateClock = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemRotateConter = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemResize = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barItemResizeWidth = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemResizeHeight = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemResizeSize = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemResizeAcross = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemResizeDown = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemLayout = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemWindow = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.biTabbedMDI = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemHelp = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.barItemAbout = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItemHelp = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.standardToolbar = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "Standard");
            this.symbolToolBar = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "Symbol");
            this.biFileOpenDiagram = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.parentBarItemPopUp = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.parentBarItemNode = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
            this.splashPanel1 = new Syncfusion.Windows.Forms.Tools.SplashPanel();
            this.splashControl1 = new Syncfusion.Windows.Forms.Tools.SplashControl();
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.symbolPaletteGroupBar)).BeginInit();
            this.overviewControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainFrameBarManager)).BeginInit();
            this.SuspendLayout();
            // 
            // dockingManager
            // 
            this.dockingManager.ActiveCaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
#if NETCORE
            this.dockingManager.DockLayoutStream = ((System.IO.MemoryStream)(resources.GetObject("dockingManager.DockLayoutStream")));
#endif
            this.dockingManager.HostControl = this;
            this.dockingManager.InActiveCaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockingManager.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.dockingManager.PersistState = true;
            this.dockingManager.ThemesEnabled = true;
            this.dockingManager.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007Outlook;
            this.dockingManager.DockVisibilityChanged += new Syncfusion.Windows.Forms.Tools.DockVisibilityChangedEventHandler(this.dockingManager_DockVisibilityChanged);
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Close, "CloseButton", -1, System.Drawing.Color.Transparent, ""));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Pin, "PinButton", -1, System.Drawing.Color.Transparent, ""));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Menu, "MenuButton", -1, System.Drawing.Color.Transparent, ""));
            this.dockingManager.CaptionButtons.Add(new Syncfusion.Windows.Forms.Tools.CaptionButton(Syncfusion.Windows.Forms.Tools.CaptionButtonType.Maximize, "MaximizeButton", -1, System.Drawing.Color.Transparent, ""));
            this.dockingManager.SetDockLabel(this.propertyEditor, "Properties");
            ccbpropertyEditor.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.propertyEditor, ccbpropertyEditor);
            this.dockingManager.SetDockLabel(this.symbolPaletteGroupBar, "Symbol Palettes");
            this.dockingManager.SetAllowFloating(this.symbolPaletteGroupBar, false);
            this.dockingManager.SetFreezeResize(this.symbolPaletteGroupBar, true);
            this.dockingManager.SetDockAbility(this.symbolPaletteGroupBar, "None");
            this.dockingManager.SetOuterDockAbility(this.symbolPaletteGroupBar, "None");
            ccbsymbolPaletteGroupBar.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.symbolPaletteGroupBar, ccbsymbolPaletteGroupBar);
            this.dockingManager.SetDockLabel(this.overviewControl1, "Pan & Zoom");
            this.dockingManager.SetHiddenOnLoad(this.overviewControl1, true);
            ccboverviewControl1.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.overviewControl1, ccboverviewControl1);
            this.dockingManager.SetDockLabel(this.documentExplorer1, "Object Model");
            this.dockingManager.SetFreezeResize(this.documentExplorer1, true);
            ccbdocumentExplorer1.MergeWith(this.dockingManager.CaptionButtons, false);
            this.dockingManager.SetCustomCaptionButtons(this.documentExplorer1, ccbdocumentExplorer1);
            // 
            // propertyEditor
            // 
            this.propertyEditor.Diagram = null;
            this.dockingManager.SetEnableDocking(this.propertyEditor, true);
            this.propertyEditor.Location = new System.Drawing.Point(3, 23);
            this.propertyEditor.Name = "propertyEditor";
            this.propertyEditor.ShowCombo = true;
            this.propertyEditor.Size = new System.Drawing.Size(197, 320);
            this.propertyEditor.TabIndex = 11;
            // 
            // symbolPaletteGroupBar
            // 
            this.symbolPaletteGroupBar.AllowDrop = true;
            this.symbolPaletteGroupBar.AnimatedSelection = false;
            this.symbolPaletteGroupBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.symbolPaletteGroupBar.EditMode = false;
            this.dockingManager.SetEnableDocking(this.symbolPaletteGroupBar, true);
            this.symbolPaletteGroupBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.symbolPaletteGroupBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(81)))), ((int)(((byte)(90)))));
            this.symbolPaletteGroupBar.Location = new System.Drawing.Point(3, 23);
            this.symbolPaletteGroupBar.Name = "symbolPaletteGroupBar";
            this.symbolPaletteGroupBar.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.symbolPaletteGroupBar.PopupClientSize = new System.Drawing.Size(0, 0);
            this.symbolPaletteGroupBar.Size = new System.Drawing.Size(174, 228);
            this.symbolPaletteGroupBar.TabIndex = 9;
            this.symbolPaletteGroupBar.Text = "paletteGroupBar1";
            this.symbolPaletteGroupBar.TextAlign = Syncfusion.Windows.Forms.Tools.TextAlignment.Left;
            this.symbolPaletteGroupBar.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2007;
            this.symbolPaletteGroupBar.GroupBarItemAdded += new Syncfusion.Windows.Forms.Tools.GroupBarItemEventHandler(this.symbolPaletteGroupBar_GroupBarItemAdded);
            this.symbolPaletteGroupBar.NodeSelected += new Syncfusion.Windows.Forms.Diagram.Controls.NodeEventHandler(this.symbolPaletteGroupBar_NodeSelected);
            this.symbolPaletteGroupBar.GroupBarItemRemoved += new Syncfusion.Windows.Forms.Tools.GroupBarItemEventHandler(this.symbolPaletteGroupBar_GroupBarItemRemoved);
            // 
            // overviewControl1
            // 
            this.overviewControl1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.overviewControl1.Controls.Add(this.splashPanel1);
            this.overviewControl1.Diagram = null;
            this.dockingManager.SetEnableDocking(this.overviewControl1, true);
            this.overviewControl1.ForeColor = System.Drawing.Color.Red;
            this.overviewControl1.Location = new System.Drawing.Point(3, 23);
            this.overviewControl1.Name = "overviewControl1";
            this.overviewControl1.Size = new System.Drawing.Size(197, 140);
            this.overviewControl1.TabIndex = 11;
            // 
            // documentExplorer1
            // 
            this.dockingManager.SetEnableDocking(this.documentExplorer1, true);
            this.documentExplorer1.ImageIndex = 0;
            this.documentExplorer1.Location = new System.Drawing.Point(3, 23);
            this.documentExplorer1.Name = "documentExplorer1";
            this.documentExplorer1.SelectedImageIndex = 0;
            this.documentExplorer1.Size = new System.Drawing.Size(174, 232);
            this.documentExplorer1.TabIndex = 13;
            // 
            // openPaletteDialog
            // 
            this.openPaletteDialog.DefaultExt = "edp";
            this.openPaletteDialog.Filter = "Essential Diagram Palettes|*.edp|Visio Stencils|*.vss; *.vsx|Visio Drawings(Shape" +
                "s only)|*.vsd; *.vdx|All files|*.*";
            this.openPaletteDialog.Title = "Add SymbolPalette";
            // 
            // savePaletteDialog
            // 
            this.savePaletteDialog.DefaultExt = "edp";
            this.savePaletteDialog.Filter = "Essential Diagram Palettes|*.edp|All files|*.*";
            this.savePaletteDialog.Title = "Save SymbolPalette";
            // 
            // openDiagramDialog
            // 
            this.openDiagramDialog.Filter = "Diagram Files|*.edd|All files|*.*";
            this.openDiagramDialog.Title = "Open Diagram";
            // 
            // saveDiagramDialog
            // 
            this.saveDiagramDialog.FileName = "doc1";
            this.saveDiagramDialog.Filter = "Diagram files|*.edd|EMF file|*.emf|GIF file|*.gif|PNG file|*.png|BMP file|*.bmp|J" +
                "PEG file|*.jpeg,*.jpg|TIFF file|*.tiff|SVG file|*.svg|All files|*.*";
            // 
            // smallImageList
            // 
            this.smallImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImageList.ImageStream")));
            this.smallImageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.smallImageList.Images.SetKeyName(0, "");
            this.smallImageList.Images.SetKeyName(1, "");
            this.smallImageList.Images.SetKeyName(2, "");
            this.smallImageList.Images.SetKeyName(3, "");
            this.smallImageList.Images.SetKeyName(4, "");
            this.smallImageList.Images.SetKeyName(5, "");
            this.smallImageList.Images.SetKeyName(6, "");
            this.smallImageList.Images.SetKeyName(7, "");
            this.smallImageList.Images.SetKeyName(8, "");
            this.smallImageList.Images.SetKeyName(9, "");
            this.smallImageList.Images.SetKeyName(10, "");
            this.smallImageList.Images.SetKeyName(11, "");
            this.smallImageList.Images.SetKeyName(12, "");
            this.smallImageList.Images.SetKeyName(13, "");
            this.smallImageList.Images.SetKeyName(14, "");
            this.smallImageList.Images.SetKeyName(15, "");
            this.smallImageList.Images.SetKeyName(16, "");
            this.smallImageList.Images.SetKeyName(17, "");
            this.smallImageList.Images.SetKeyName(18, "");
            this.smallImageList.Images.SetKeyName(19, "");
            this.smallImageList.Images.SetKeyName(20, "");
            this.smallImageList.Images.SetKeyName(21, "");
            this.smallImageList.Images.SetKeyName(22, "");
            this.smallImageList.Images.SetKeyName(23, "");
            this.smallImageList.Images.SetKeyName(24, "");
            this.smallImageList.Images.SetKeyName(25, "");
            this.smallImageList.Images.SetKeyName(26, "");
            this.smallImageList.Images.SetKeyName(27, "");
            this.smallImageList.Images.SetKeyName(28, "");
            this.smallImageList.Images.SetKeyName(29, "");
            this.smallImageList.Images.SetKeyName(30, "");
            this.smallImageList.Images.SetKeyName(31, "");
            this.smallImageList.Images.SetKeyName(32, "");
            this.smallImageList.Images.SetKeyName(33, "");
            this.smallImageList.Images.SetKeyName(34, "close_palette(2).png");
            this.smallImageList.Images.SetKeyName(35, "add_symbol(6).png");
            this.smallImageList.Images.SetKeyName(36, "add_symbol(7).png");
            // 
            // openImageDialog
            // 
            this.openImageDialog.Filter = "Windows Bitmaps|*.bmp|Enhanced Metafiles|*.emf|All files|*.*";
            this.openImageDialog.Title = "Insert Image";
            // 
            // mainFrameBarManager
            // 
#if NETCORE
            this.mainFrameBarManager.BarPositionInfo = ((System.IO.MemoryStream)(resources.GetObject("mainFrameBarManager.BarPositionInfo")));
#endif
            this.mainFrameBarManager.Bars.Add(this.mainMenuBar);
            this.mainFrameBarManager.Bars.Add(this.standardToolbar);
            this.mainFrameBarManager.Bars.Add(this.symbolToolBar);
            this.mainFrameBarManager.Categories.Add("File");
            this.mainFrameBarManager.Categories.Add("Popups");
            this.mainFrameBarManager.Categories.Add("Window");
            this.mainFrameBarManager.Categories.Add("Edit");
            this.mainFrameBarManager.Categories.Add("Help");
            this.mainFrameBarManager.Categories.Add("Symbol");
            this.mainFrameBarManager.Categories.Add("View");
            this.mainFrameBarManager.Categories.Add("Dialogs");
            this.mainFrameBarManager.Categories.Add("Actions");
            this.mainFrameBarManager.Categories.Add("Align");
            this.mainFrameBarManager.Categories.Add("Flip");
            this.mainFrameBarManager.Categories.Add("Grouping");
            this.mainFrameBarManager.Categories.Add("Order");
            this.mainFrameBarManager.Categories.Add("Rotate");
            this.mainFrameBarManager.Categories.Add("Resize");
            this.mainFrameBarManager.Categories.Add("Format");
            this.mainFrameBarManager.CurrentBaseFormType = "Syncfusion.Windows.Forms.Office2007Form";
            this.mainFrameBarManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mainFrameBarManager.Form = this;
            this.mainFrameBarManager.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemViewSymbolPalette,
            this.biFileNew,
            this.barItemViewProperties,
            this.biFileOpen,
            this.barItemObjectModel,
            this.parentBarItemFile,
            this.barItemPanZoom,
            this.biFileAppend,
            this.barShowRulers,
            this.biFileOpenDiagram,
            this.barItemPageBorders,
            this.parentBarItemAlign,
            this.biFileClosePalette,
            this.biFileSave,
            this.parentBarItemRotate,
            this.biFileSaveAs,
            this.biPageSetup,
            this.parentBarItemFlip,
            this.biPrintPreview,
            this.biFilePrint,
            this.parentBarItemResize,
            this.biFileExit,
            this.parentBarItemEdit,
            this.parentBarItemPopUp,
            this.parentBarItemView,
            this.parentBarItemWindow,
            this.parentBarItemHelp,
            this.parentBarItemActions,
            this.parentBarItemToolsGrouping,
            this.parentBarItemToolsOrder,
            this.biTabbedMDI,
            this.barItemEditCut,
            this.barItemEditCopy,
            this.barItemEditPaste,
            this.barItemEditUndo,
            this.barItemEditRedo,
            this.barItemAbout,
            this.barItemEditSelectAll,
            this.barItemAlignBottom,
            this.barItemAlignCenter,
            this.barItemAlignLeft,
            this.barItemAlignMiddle,
            this.barItemAlignRight,
            this.barItemAlignTop,
            this.barItemEditDelete,
            this.barItemFlipBoth,
            this.barItemFlipHorizontally,
            this.barItemFlipVertically,
            this.barItemGroupingGroup,
            this.barItemGroupingUnGroup,
            this.barItemOrderBack,
            this.barItemOrderBackward,
            this.barItemOrderForward,
            this.barItemOrderFront,
            this.barItemResizeAcross,
            this.barItemResizeDown,
            this.barItemResizeHeight,
            this.barItemResizeSize,
            this.barItemResizeWidth,
            this.barItemRotateClock,
            this.barItemRotateConter,
            this.barItemLayout,
            this.parentBarItemFormat,
            this.barItemFillStyle,
            this.barItemShadowStyle,
            this.parentBarItem1,
            this.barAddSymbol,
            this.barRemoveSymbol,
            this.barItemHelp});
            this.mainFrameBarManager.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.mainFrameBarManager.ResetCustomization = false;
            this.mainFrameBarManager.ShowDropShadow = false;
            this.mainFrameBarManager.ShowShadow = false;
            this.mainFrameBarManager.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007Outlook;
            // 
            // mainMenuBar
            // 
            this.mainMenuBar.BarName = "MainMenu";
            this.mainMenuBar.BarStyle = ((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle)(((((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.IsMainMenu | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.MultiLine)
                        | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.RotateWhenVertical)
                        | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.Visible)
                        | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.UseWholeRow)));
            this.mainMenuBar.Caption = "MainMenu";
            this.mainMenuBar.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.parentBarItemFile,
            this.parentBarItemEdit,
            this.parentBarItem1,
            this.parentBarItemView,
            this.parentBarItemActions,
            this.parentBarItemWindow,
            this.parentBarItemHelp});
            this.mainMenuBar.Manager = this.mainFrameBarManager;
            // 
            // parentBarItemFile
            // 
            this.parentBarItemFile.CategoryIndex = 1;
            this.parentBarItemFile.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemFile.ID = "File";
            this.parentBarItemFile.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.biFileNew,
            this.biFileOpen,
            this.biFileAppend,
            this.biFileClosePalette,
            this.biFileSave,
            this.biFileSaveAs,
            this.biPageSetup,
            this.biPrintPreview,
            this.biFilePrint,
            this.biFileExit});
            this.parentBarItemFile.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemFile.SeparatorIndices.AddRange(new int[] {
            4,
            6,
            9});
            this.parentBarItemFile.Text = "&File";
            // 
            // biFileNew
            // 
            this.biFileNew.CategoryIndex = 0;
            this.biFileNew.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biFileNew.ID = "New";
            this.biFileNew.ImageIndex = 0;
            this.biFileNew.ImageList = this.smallImageList;
            this.biFileNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.biFileNew.Text = "&New";
            this.biFileNew.Click += new System.EventHandler(this.biFileNew_Click);
            // 
            // biFileOpen
            // 
            this.biFileOpen.CategoryIndex = 0;
            this.biFileOpen.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biFileOpen.ID = "OpenPal";
            this.biFileOpen.ImageIndex = 1;
            this.biFileOpen.ImageList = this.smallImageList;
            this.biFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.biFileOpen.Text = "&Open Palette";
            this.biFileOpen.Click += new System.EventHandler(this.biFileOpenPalette_Click);
            // 
            // biFileAppend
            // 
            this.biFileAppend.CategoryIndex = 0;
            this.biFileAppend.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biFileAppend.ID = "Append";
            this.biFileAppend.ImageIndex = 1;
            this.biFileAppend.ImageList = this.smallImageList;
            this.biFileAppend.Text = "&Append";
            this.biFileAppend.Click += new System.EventHandler(this.biFileAppendPaletteNodes_Click);
            // 
            // biFileClosePalette
            // 
            this.biFileClosePalette.CategoryIndex = 0;
            this.biFileClosePalette.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biFileClosePalette.ID = "ClosePalette";
            this.biFileClosePalette.ImageIndex = 34;
            this.biFileClosePalette.ImageList = this.smallImageList;
            this.biFileClosePalette.Text = "&Close Palette";
            this.biFileClosePalette.Click += new System.EventHandler(this.biFileClosePalette_Click);
            // 
            // biFileSave
            // 
            this.biFileSave.CategoryIndex = 0;
            this.biFileSave.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biFileSave.ID = "Save";
            this.biFileSave.ImageIndex = 2;
            this.biFileSave.ImageList = this.smallImageList;
            this.biFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.biFileSave.Text = "&Save";
            this.biFileSave.Click += new System.EventHandler(this.biFileSave_Click);
            // 
            // biFileSaveAs
            // 
            this.biFileSaveAs.CategoryIndex = 0;
            this.biFileSaveAs.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biFileSaveAs.ID = "Save As...";
            this.biFileSaveAs.Text = "Save As...";
            this.biFileSaveAs.Click += new System.EventHandler(this.biFileSaveAs_Click);
            // 
            // biPageSetup
            // 
            this.biPageSetup.CategoryIndex = 0;
            this.biPageSetup.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biPageSetup.ID = "Page Setup";
            this.biPageSetup.ImageIndex = 5;
            this.biPageSetup.ImageList = this.smallImageList;
            this.biPageSetup.Text = "Page Setup";
            this.biPageSetup.Click += new System.EventHandler(this.biPageSetup_Click);
            // 
            // biPrintPreview
            // 
            this.biPrintPreview.CategoryIndex = 0;
            this.biPrintPreview.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biPrintPreview.ID = "Print Preview";
            this.biPrintPreview.ImageIndex = 3;
            this.biPrintPreview.ImageList = this.smallImageList;
            this.biPrintPreview.Text = "Print Preview";
            this.biPrintPreview.Tooltip = "Print Preview";
            this.biPrintPreview.Click += new System.EventHandler(this.biPrintPreview_Click);
            // 
            // biFilePrint
            // 
            this.biFilePrint.CategoryIndex = 0;
            this.biFilePrint.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biFilePrint.ID = "Print";
            this.biFilePrint.ImageIndex = 6;
            this.biFilePrint.ImageList = this.smallImageList;
            this.biFilePrint.Text = "Print";
            this.biFilePrint.Click += new System.EventHandler(this.biFilePrint_Click);
            // 
            // biFileExit
            // 
            this.biFileExit.CategoryIndex = 0;
            this.biFileExit.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biFileExit.ID = "Exit";
            this.biFileExit.Text = "Exit";
            this.biFileExit.Click += new System.EventHandler(this.biFileExit_Click);
            // 
            // parentBarItemEdit
            // 
            this.parentBarItemEdit.CategoryIndex = 1;
            this.parentBarItemEdit.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemEdit.ID = "Edit";
            this.parentBarItemEdit.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemEditUndo,
            this.barItemEditRedo,
            this.barItemEditSelectAll,
            this.barItemEditDelete,
            this.barItemEditCut,
            this.barItemEditCopy,
            this.barItemEditPaste,
            this.parentBarItemFormat});
            this.parentBarItemEdit.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemEdit.SeparatorIndices.AddRange(new int[] {
            2,
            4});
            this.parentBarItemEdit.Text = "&Edit";
            // 
            // barItemEditUndo
            // 
            this.barItemEditUndo.CategoryIndex = 3;
            this.barItemEditUndo.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemEditUndo.ID = "Undo";
            this.barItemEditUndo.ImageIndex = 10;
            this.barItemEditUndo.ImageList = this.smallImageList;
            this.barItemEditUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.barItemEditUndo.Text = "&Undo";
            this.barItemEditUndo.Click += new System.EventHandler(this.barItemEditUndo_Click);
            // 
            // barItemEditRedo
            // 
            this.barItemEditRedo.CategoryIndex = 3;
            this.barItemEditRedo.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemEditRedo.ID = "EditRedo";
            this.barItemEditRedo.ImageIndex = 11;
            this.barItemEditRedo.ImageList = this.smallImageList;
            this.barItemEditRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.barItemEditRedo.Text = "&Redo";
            this.barItemEditRedo.Click += new System.EventHandler(this.barItemEditRedo_Click);
            // 
            // barItemEditSelectAll
            // 
            this.barItemEditSelectAll.CategoryIndex = 3;
            this.barItemEditSelectAll.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemEditSelectAll.ID = "&Select All";
            this.barItemEditSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.barItemEditSelectAll.Text = "Select &All";
            this.barItemEditSelectAll.Click += new System.EventHandler(this.barItemEditSelectAll_Click);
            // 
            // barItemEditDelete
            // 
            this.barItemEditDelete.CategoryIndex = 3;
            this.barItemEditDelete.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemEditDelete.ID = "Delete";
            this.barItemEditDelete.ImageIndex = 12;
            this.barItemEditDelete.ImageList = this.smallImageList;
            this.barItemEditDelete.Text = "&Delete";
            this.barItemEditDelete.Click += new System.EventHandler(this.barItemEditDelete_Click);
            // 
            // barItemEditCut
            // 
            this.barItemEditCut.CategoryIndex = 3;
            this.barItemEditCut.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemEditCut.ID = "Cut";
            this.barItemEditCut.ImageIndex = 8;
            this.barItemEditCut.ImageList = this.smallImageList;
            this.barItemEditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.barItemEditCut.Text = "C&ut";
            this.barItemEditCut.Click += new System.EventHandler(this.barItemEditCut_Click);
            // 
            // barItemEditCopy
            // 
            this.barItemEditCopy.CategoryIndex = 3;
            this.barItemEditCopy.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemEditCopy.ID = "Copy";
            this.barItemEditCopy.ImageIndex = 7;
            this.barItemEditCopy.ImageList = this.smallImageList;
            this.barItemEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.barItemEditCopy.Text = "&Copy";
            this.barItemEditCopy.Click += new System.EventHandler(this.barItemEditCopy_Click);
            // 
            // barItemEditPaste
            // 
            this.barItemEditPaste.CategoryIndex = 3;
            this.barItemEditPaste.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemEditPaste.ID = "Paste";
            this.barItemEditPaste.ImageIndex = 9;
            this.barItemEditPaste.ImageList = this.smallImageList;
            this.barItemEditPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.barItemEditPaste.Text = "&Paste";
            this.barItemEditPaste.Click += new System.EventHandler(this.barItemEditPaste_Click);
            // 
            // parentBarItemFormat
            // 
            this.parentBarItemFormat.CategoryIndex = 1;
            this.parentBarItemFormat.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemFormat.ID = "Format";
            this.parentBarItemFormat.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemFillStyle,
            this.barItemShadowStyle});
            this.parentBarItemFormat.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemFormat.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007Outlook;
            this.parentBarItemFormat.Text = "&Format";
            // 
            // barItemFillStyle
            // 
            this.barItemFillStyle.CategoryIndex = 15;
            this.barItemFillStyle.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemFillStyle.ID = "Fill";
            this.barItemFillStyle.Text = "&Fill...";
            this.barItemFillStyle.Click += new System.EventHandler(this.fillBarItem_Click);
            // 
            // barItemShadowStyle
            // 
            this.barItemShadowStyle.CategoryIndex = 15;
            this.barItemShadowStyle.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemShadowStyle.ID = "Shadow";
            this.barItemShadowStyle.Text = "&Shadow...";
            this.barItemShadowStyle.Click += new System.EventHandler(this.shadowBarItem_Click);
            // 
            // parentBarItem1
            // 
            this.parentBarItem1.CategoryIndex = 1;
            this.parentBarItem1.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItem1.ID = "Symbol";
            this.parentBarItem1.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barAddSymbol,
            this.barRemoveSymbol});
            this.parentBarItem1.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItem1.Text = "Symbol";
            // 
            // barAddSymbol
            // 
            this.barAddSymbol.CategoryIndex = 5;
            this.barAddSymbol.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barAddSymbol.Enabled = false;
            this.barAddSymbol.ID = "Add";
            this.barAddSymbol.ImageIndex = 35;
            this.barAddSymbol.ImageList = this.smallImageList;
            this.barAddSymbol.PaintStyle = Syncfusion.Windows.Forms.Tools.XPMenus.PaintStyle.ImageAndText;
            this.barAddSymbol.Text = "Add Symbol";
            this.barAddSymbol.Click += new System.EventHandler(this.barAddSymbol_Click);
            // 
            // barRemoveSymbol
            // 
            this.barRemoveSymbol.CategoryIndex = 5;
            this.barRemoveSymbol.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barRemoveSymbol.Enabled = false;
            this.barRemoveSymbol.ID = "Remove";
            this.barRemoveSymbol.ImageIndex = 36;
            this.barRemoveSymbol.ImageList = this.smallImageList;
            this.barRemoveSymbol.PaintStyle = Syncfusion.Windows.Forms.Tools.XPMenus.PaintStyle.ImageAndText;
            this.barRemoveSymbol.Text = "Remove Symbol";
            this.barRemoveSymbol.Click += new System.EventHandler(this.barRemoveSymbol_Click);
            // 
            // parentBarItemView
            // 
            this.parentBarItemView.CategoryIndex = 1;
            this.parentBarItemView.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemView.ID = "View";
            this.parentBarItemView.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barShowRulers,
            this.barItemViewSymbolPalette,
            this.barItemViewProperties,
            this.barItemObjectModel,
            this.barItemPanZoom,
            this.barItemPageBorders});
            this.parentBarItemView.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemView.SeparatorIndices.AddRange(new int[] {
            4});
            this.parentBarItemView.Text = "&View";
            // 
            // barShowRulers
            // 
            this.barShowRulers.CategoryIndex = 6;
            this.barShowRulers.Checked = true;
            this.barShowRulers.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barShowRulers.ID = "Rulers";
            this.barShowRulers.Text = "Rulers";
            this.barShowRulers.Click += new System.EventHandler(this.barShowRulers_Click);
            // 
            // barItemViewSymbolPalette
            // 
            this.barItemViewSymbolPalette.CategoryIndex = 6;
            this.barItemViewSymbolPalette.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemViewSymbolPalette.ID = "Symbol SymbolPalette";
            this.barItemViewSymbolPalette.Text = "SymbolPalette";
            this.barItemViewSymbolPalette.Click += new System.EventHandler(this.barItemViewSymbolPalette_Click);
            // 
            // barItemViewProperties
            // 
            this.barItemViewProperties.CategoryIndex = 6;
            this.barItemViewProperties.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemViewProperties.ID = "Properties";
            this.barItemViewProperties.Text = "Properties";
            this.barItemViewProperties.Click += new System.EventHandler(this.barItemViewProperties_Click);
            // 
            // barItemObjectModel
            // 
            this.barItemObjectModel.CategoryIndex = 6;
            this.barItemObjectModel.Checked = true;
            this.barItemObjectModel.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemObjectModel.ID = "Object Model";
            this.barItemObjectModel.Text = "Object Model";
            this.barItemObjectModel.Click += new System.EventHandler(this.barItemObjectModel_Click);
            // 
            // barItemPanZoom
            // 
            this.barItemPanZoom.CategoryIndex = 6;
            this.barItemPanZoom.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemPanZoom.ID = "panZoomWindow";
            this.barItemPanZoom.Text = "Pan && &Zoom Window";
            this.barItemPanZoom.Tooltip = "Pan & Zoom Window";
            this.barItemPanZoom.Click += new System.EventHandler(this.barItemPanZoom_Click);
            // 
            // barItemPageBorders
            // 
            this.barItemPageBorders.CategoryIndex = 6;
            this.barItemPageBorders.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemPageBorders.ID = "Page Borders";
            this.barItemPageBorders.Text = "Page Borders";
            this.barItemPageBorders.Click += new System.EventHandler(this.barItemPageBorders_Click);
            // 
            // parentBarItemActions
            // 
            this.parentBarItemActions.CategoryIndex = 1;
            this.parentBarItemActions.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemActions.ID = "Actions";
            this.parentBarItemActions.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.parentBarItemAlign,
            this.parentBarItemFlip,
            this.parentBarItemToolsGrouping,
            this.parentBarItemToolsOrder,
            this.parentBarItemRotate,
            this.parentBarItemResize,
            this.barItemLayout});
            this.parentBarItemActions.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemActions.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007Outlook;
            this.parentBarItemActions.Text = "&Actions";
            // 
            // parentBarItemAlign
            // 
            this.parentBarItemAlign.CategoryIndex = 1;
            this.parentBarItemAlign.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemAlign.ID = "Align";
            this.parentBarItemAlign.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemAlignLeft,
            this.barItemAlignCenter,
            this.barItemAlignRight,
            this.barItemAlignTop,
            this.barItemAlignMiddle,
            this.barItemAlignBottom});
            this.parentBarItemAlign.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemAlign.Style = Syncfusion.Windows.Forms.VisualStyle.Office2007Outlook;
            this.parentBarItemAlign.Text = "&Align";
            // 
            // barItemAlignLeft
            // 
            this.barItemAlignLeft.CategoryIndex = 9;
            this.barItemAlignLeft.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemAlignLeft.ID = "AlignLeft";
            this.barItemAlignLeft.ImageIndex = 15;
            this.barItemAlignLeft.ImageList = this.smallImageList;
            this.barItemAlignLeft.Text = "Align Left";
            this.barItemAlignLeft.Click += new System.EventHandler(this.barItemAlign_Click);
            // 
            // barItemAlignCenter
            // 
            this.barItemAlignCenter.CategoryIndex = 9;
            this.barItemAlignCenter.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemAlignCenter.ID = "AlignCenter";
            this.barItemAlignCenter.ImageIndex = 14;
            this.barItemAlignCenter.ImageList = this.smallImageList;
            this.barItemAlignCenter.Text = "Align Center";
            this.barItemAlignCenter.Click += new System.EventHandler(this.barItemAlign_Click);
            // 
            // barItemAlignRight
            // 
            this.barItemAlignRight.CategoryIndex = 9;
            this.barItemAlignRight.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemAlignRight.ID = "AlignRight";
            this.barItemAlignRight.ImageIndex = 17;
            this.barItemAlignRight.ImageList = this.smallImageList;
            this.barItemAlignRight.Text = "Align Right";
            this.barItemAlignRight.Click += new System.EventHandler(this.barItemAlign_Click);
            // 
            // barItemAlignTop
            // 
            this.barItemAlignTop.CategoryIndex = 9;
            this.barItemAlignTop.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemAlignTop.ID = "AlignTop";
            this.barItemAlignTop.ImageIndex = 18;
            this.barItemAlignTop.ImageList = this.smallImageList;
            this.barItemAlignTop.Text = "Align Top";
            this.barItemAlignTop.Click += new System.EventHandler(this.barItemAlign_Click);
            // 
            // barItemAlignMiddle
            // 
            this.barItemAlignMiddle.CategoryIndex = 9;
            this.barItemAlignMiddle.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemAlignMiddle.ID = "AlignMiddle";
            this.barItemAlignMiddle.ImageIndex = 16;
            this.barItemAlignMiddle.ImageList = this.smallImageList;
            this.barItemAlignMiddle.Text = "Align Middle";
            this.barItemAlignMiddle.Click += new System.EventHandler(this.barItemAlign_Click);
            // 
            // barItemAlignBottom
            // 
            this.barItemAlignBottom.CategoryIndex = 9;
            this.barItemAlignBottom.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemAlignBottom.ID = "AlignBottom";
            this.barItemAlignBottom.ImageIndex = 13;
            this.barItemAlignBottom.ImageList = this.smallImageList;
            this.barItemAlignBottom.Text = "Align Bottom";
            this.barItemAlignBottom.Click += new System.EventHandler(this.barItemAlign_Click);
            // 
            // parentBarItemFlip
            // 
            this.parentBarItemFlip.CategoryIndex = 1;
            this.parentBarItemFlip.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemFlip.ID = "Flip";
            this.parentBarItemFlip.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemFlipHorizontally,
            this.barItemFlipVertically,
            this.barItemFlipBoth});
            this.parentBarItemFlip.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemFlip.Style = Syncfusion.Windows.Forms.VisualStyle.OfficeXP;
            this.parentBarItemFlip.Text = "&Flip";
            // 
            // barItemFlipHorizontally
            // 
            this.barItemFlipHorizontally.CategoryIndex = 10;
            this.barItemFlipHorizontally.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemFlipHorizontally.ID = "FlipHorizontally";
            this.barItemFlipHorizontally.ImageIndex = 20;
            this.barItemFlipHorizontally.ImageList = this.smallImageList;
            this.barItemFlipHorizontally.Text = "Flip Horizontally";
            this.barItemFlipHorizontally.Click += new System.EventHandler(this.barItemFlip_Click);
            // 
            // barItemFlipVertically
            // 
            this.barItemFlipVertically.CategoryIndex = 10;
            this.barItemFlipVertically.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemFlipVertically.ID = "FlipVertically";
            this.barItemFlipVertically.ImageIndex = 19;
            this.barItemFlipVertically.ImageList = this.smallImageList;
            this.barItemFlipVertically.Text = "Flip Vertically";
            this.barItemFlipVertically.Click += new System.EventHandler(this.barItemFlip_Click);
            // 
            // barItemFlipBoth
            // 
            this.barItemFlipBoth.CategoryIndex = 10;
            this.barItemFlipBoth.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemFlipBoth.ID = "FlipBoth";
            this.barItemFlipBoth.Text = "Flip Both";
            this.barItemFlipBoth.Click += new System.EventHandler(this.barItemFlip_Click);
            // 
            // parentBarItemToolsGrouping
            // 
            this.parentBarItemToolsGrouping.CategoryIndex = 1;
            this.parentBarItemToolsGrouping.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemToolsGrouping.ID = "Grouping";
            this.parentBarItemToolsGrouping.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemGroupingGroup,
            this.barItemGroupingUnGroup});
            this.parentBarItemToolsGrouping.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemToolsGrouping.Style = Syncfusion.Windows.Forms.VisualStyle.OfficeXP;
            this.parentBarItemToolsGrouping.Text = "&Grouping";
            // 
            // barItemGroupingGroup
            // 
            this.barItemGroupingGroup.CategoryIndex = 11;
            this.barItemGroupingGroup.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemGroupingGroup.ID = "Group";
            this.barItemGroupingGroup.ImageIndex = 21;
            this.barItemGroupingGroup.ImageList = this.smallImageList;
            this.barItemGroupingGroup.Text = "Group";
            this.barItemGroupingGroup.Click += new System.EventHandler(this.barItemGroupingGroup_Click);
            // 
            // barItemGroupingUnGroup
            // 
            this.barItemGroupingUnGroup.CategoryIndex = 11;
            this.barItemGroupingUnGroup.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemGroupingUnGroup.ID = "UnGroup";
            this.barItemGroupingUnGroup.ImageIndex = 22;
            this.barItemGroupingUnGroup.ImageList = this.smallImageList;
            this.barItemGroupingUnGroup.Text = "UnGroup";
            this.barItemGroupingUnGroup.Click += new System.EventHandler(this.barItemGroupingGroup_Click);
            // 
            // parentBarItemToolsOrder
            // 
            this.parentBarItemToolsOrder.CategoryIndex = 1;
            this.parentBarItemToolsOrder.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemToolsOrder.ID = "Order";
            this.parentBarItemToolsOrder.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemOrderFront,
            this.barItemOrderForward,
            this.barItemOrderBackward,
            this.barItemOrderBack});
            this.parentBarItemToolsOrder.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemToolsOrder.Style = Syncfusion.Windows.Forms.VisualStyle.OfficeXP;
            this.parentBarItemToolsOrder.Text = "&Order";
            // 
            // barItemOrderFront
            // 
            this.barItemOrderFront.CategoryIndex = 12;
            this.barItemOrderFront.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemOrderFront.ID = "Bring to Front";
            this.barItemOrderFront.ImageIndex = 26;
            this.barItemOrderFront.ImageList = this.smallImageList;
            this.barItemOrderFront.Text = "Bring to Front";
            this.barItemOrderFront.Click += new System.EventHandler(this.barItemOrder_Click);
            // 
            // barItemOrderForward
            // 
            this.barItemOrderForward.CategoryIndex = 12;
            this.barItemOrderForward.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemOrderForward.ID = "Bring Forward";
            this.barItemOrderForward.ImageIndex = 25;
            this.barItemOrderForward.ImageList = this.smallImageList;
            this.barItemOrderForward.Text = "Bring Forward";
            this.barItemOrderForward.Click += new System.EventHandler(this.barItemOrder_Click);
            // 
            // barItemOrderBackward
            // 
            this.barItemOrderBackward.CategoryIndex = 12;
            this.barItemOrderBackward.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemOrderBackward.ID = "Send Backward";
            this.barItemOrderBackward.ImageIndex = 27;
            this.barItemOrderBackward.ImageList = this.smallImageList;
            this.barItemOrderBackward.Text = "Send Backward";
            this.barItemOrderBackward.Click += new System.EventHandler(this.barItemOrder_Click);
            // 
            // barItemOrderBack
            // 
            this.barItemOrderBack.CategoryIndex = 12;
            this.barItemOrderBack.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemOrderBack.ID = "Send to Back";
            this.barItemOrderBack.ImageIndex = 28;
            this.barItemOrderBack.ImageList = this.smallImageList;
            this.barItemOrderBack.Text = "Send to Back";
            this.barItemOrderBack.Click += new System.EventHandler(this.barItemOrder_Click);
            // 
            // parentBarItemRotate
            // 
            this.parentBarItemRotate.CategoryIndex = 1;
            this.parentBarItemRotate.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemRotate.ID = "Rotate";
            this.parentBarItemRotate.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemRotateClock,
            this.barItemRotateConter});
            this.parentBarItemRotate.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemRotate.Style = Syncfusion.Windows.Forms.VisualStyle.OfficeXP;
            this.parentBarItemRotate.Text = "&Rotate";
            // 
            // barItemRotateClock
            // 
            this.barItemRotateClock.CategoryIndex = 13;
            this.barItemRotateClock.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemRotateClock.ID = "RotateClock";
            this.barItemRotateClock.ImageIndex = 24;
            this.barItemRotateClock.ImageList = this.smallImageList;
            this.barItemRotateClock.Text = "Rotate 90 clockwise";
            this.barItemRotateClock.Click += new System.EventHandler(this.barItemRotate_Click);
            // 
            // barItemRotateConter
            // 
            this.barItemRotateConter.CategoryIndex = 13;
            this.barItemRotateConter.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemRotateConter.ID = "RotateConter";
            this.barItemRotateConter.ImageIndex = 23;
            this.barItemRotateConter.ImageList = this.smallImageList;
            this.barItemRotateConter.Text = "Rotate 90 conter-clockwise";
            this.barItemRotateConter.Click += new System.EventHandler(this.barItemRotate_Click);
            // 
            // parentBarItemResize
            // 
            this.parentBarItemResize.CategoryIndex = 1;
            this.parentBarItemResize.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemResize.ID = "Resize";
            this.parentBarItemResize.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemResizeWidth,
            this.barItemResizeHeight,
            this.barItemResizeSize,
            this.barItemResizeAcross,
            this.barItemResizeDown});
            this.parentBarItemResize.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemResize.SeparatorIndices.AddRange(new int[] {
            3});
            this.parentBarItemResize.Style = Syncfusion.Windows.Forms.VisualStyle.OfficeXP;
            this.parentBarItemResize.Text = "R&esize";
            // 
            // barItemResizeWidth
            // 
            this.barItemResizeWidth.CategoryIndex = 14;
            this.barItemResizeWidth.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemResizeWidth.ID = "SameWidth";
            this.barItemResizeWidth.ImageIndex = 31;
            this.barItemResizeWidth.ImageList = this.smallImageList;
            this.barItemResizeWidth.Text = "Same Width";
            this.barItemResizeWidth.Click += new System.EventHandler(this.barItemResize_Click);
            // 
            // barItemResizeHeight
            // 
            this.barItemResizeHeight.CategoryIndex = 14;
            this.barItemResizeHeight.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemResizeHeight.ID = "SameHeight";
            this.barItemResizeHeight.ImageIndex = 29;
            this.barItemResizeHeight.ImageList = this.smallImageList;
            this.barItemResizeHeight.Text = "Same Height";
            this.barItemResizeHeight.Click += new System.EventHandler(this.barItemResize_Click);
            // 
            // barItemResizeSize
            // 
            this.barItemResizeSize.CategoryIndex = 14;
            this.barItemResizeSize.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemResizeSize.ID = "SameSize";
            this.barItemResizeSize.ImageIndex = 30;
            this.barItemResizeSize.ImageList = this.smallImageList;
            this.barItemResizeSize.Text = "Same Size";
            this.barItemResizeSize.Click += new System.EventHandler(this.barItemResize_Click);
            // 
            // barItemResizeAcross
            // 
            this.barItemResizeAcross.CategoryIndex = 14;
            this.barItemResizeAcross.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemResizeAcross.ID = "SpaceAcross";
            this.barItemResizeAcross.ImageIndex = 32;
            this.barItemResizeAcross.ImageList = this.smallImageList;
            this.barItemResizeAcross.Text = "Space Across";
            this.barItemResizeAcross.Click += new System.EventHandler(this.barItemResize_Click);
            // 
            // barItemResizeDown
            // 
            this.barItemResizeDown.CategoryIndex = 14;
            this.barItemResizeDown.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemResizeDown.ID = "SpaceDown";
            this.barItemResizeDown.ImageIndex = 33;
            this.barItemResizeDown.ImageList = this.smallImageList;
            this.barItemResizeDown.Text = "Space Down";
            this.barItemResizeDown.Click += new System.EventHandler(this.barItemResize_Click);
            // 
            // barItemLayout
            // 
            this.barItemLayout.CategoryIndex = 1;
            this.barItemLayout.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemLayout.ID = "Layout nodes";
            this.barItemLayout.Text = "Layout nodes";
            this.barItemLayout.Click += new System.EventHandler(this.barItemLayout_Click);
            // 
            // parentBarItemWindow
            // 
            this.parentBarItemWindow.CategoryIndex = 1;
            this.parentBarItemWindow.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemWindow.ID = "Window";
            this.parentBarItemWindow.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.biTabbedMDI});
            this.parentBarItemWindow.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemWindow.Text = "&Window";
            // 
            // biTabbedMDI
            // 
            this.biTabbedMDI.CategoryIndex = 2;
            this.biTabbedMDI.Checked = true;
            this.biTabbedMDI.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biTabbedMDI.ID = "Tabbed MDI";
            this.biTabbedMDI.Text = "Tabbed MDI";
            this.biTabbedMDI.Click += new System.EventHandler(this.biTabbedMDI_Click);
            // 
            // parentBarItemHelp
            // 
            this.parentBarItemHelp.CategoryIndex = 1;
            this.parentBarItemHelp.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemHelp.ID = "Help";
            this.parentBarItemHelp.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemAbout,
            this.barItemHelp});
            this.parentBarItemHelp.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemHelp.PaintStyle = Syncfusion.Windows.Forms.Tools.XPMenus.PaintStyle.TextOnly;
            this.parentBarItemHelp.Text = "&Help";
            // 
            // barItemAbout
            // 
            this.barItemAbout.CategoryIndex = 4;
            this.barItemAbout.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.barItemAbout.ID = "About";
            this.barItemAbout.Text = "About...";
            this.barItemAbout.Click += new System.EventHandler(this.barItemAbout_Click);
            // 
            // barItemHelp
            // 
            this.barItemHelp.CategoryIndex = 4;
            this.barItemHelp.ID = "Contents Help";
            this.barItemHelp.Text = "Contents Help";
            this.barItemHelp.Click += new System.EventHandler(this.barItem1_Click);
            // 
            // standardToolbar
            // 
            this.standardToolbar.BarName = "Standard";
            this.standardToolbar.BarStyle = ((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle)(((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.RotateWhenVertical | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.Visible)
                        | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.DrawDragBorder)));
            this.standardToolbar.Caption = "Standard";
            this.standardToolbar.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.biFileNew,
            this.biFileOpen,
            this.biFileClosePalette,
            this.biFileSave,
            this.biPrintPreview,
            this.biFilePrint});
            this.standardToolbar.Manager = this.mainFrameBarManager;
            // 
            // symbolToolBar
            // 
            this.symbolToolBar.BarName = "Symbol";
            this.symbolToolBar.BarStyle = ((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle)((((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.AllowQuickCustomizing | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.RotateWhenVertical)
                        | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.Visible)
                        | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.DrawDragBorder)));
            this.symbolToolBar.Caption = "Symbol";
            this.symbolToolBar.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barAddSymbol,
            this.barRemoveSymbol});
            this.symbolToolBar.Manager = this.mainFrameBarManager;
            // 
            // biFileOpenDiagram
            // 
            this.biFileOpenDiagram.CategoryIndex = 0;
            this.biFileOpenDiagram.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.biFileOpenDiagram.ID = "Open";
            this.biFileOpenDiagram.ImageIndex = 1;
            this.biFileOpenDiagram.ImageList = this.smallImageList;
            this.biFileOpenDiagram.Text = "&Open Diagram";
            // 
            // parentBarItemPopUp
            // 
            this.parentBarItemPopUp.CategoryIndex = 1;
            this.parentBarItemPopUp.Customizable = false;
            this.parentBarItemPopUp.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemPopUp.ID = "PopUpMenu";
            this.parentBarItemPopUp.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemEditCut,
            this.barItemEditCopy,
            this.barItemEditPaste,
            this.barItemEditDelete,
            this.parentBarItemActions,
            this.parentBarItemFormat});
            this.parentBarItemPopUp.Office2007Theme = Syncfusion.Windows.Forms.Office2007Theme.Silver;
            this.parentBarItemPopUp.SeparatorIndices.AddRange(new int[] {
            3,
            4});
            this.parentBarItemPopUp.ShowTooltip = false;
            this.parentBarItemPopUp.Text = "PopUpMenu";
            this.parentBarItemPopUp.UsePartialMenus = false;
            this.parentBarItemPopUp.Visible = false;
            // 
            // parentBarItemNode
            // 
            this.parentBarItemNode.CategoryIndex = 0;
            this.parentBarItemNode.CustomTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parentBarItemNode.ID = "NodeMenu";
            this.parentBarItemNode.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
            this.barItemEditCut,
            this.barItemEditCopy,
            this.barItemEditPaste,
            this.parentBarItemActions});
            this.parentBarItemNode.SeparatorIndices.AddRange(new int[] {
            3});
            this.parentBarItemNode.Text = "NodeMenu";
            this.parentBarItemNode.Visible = false;
            // 
            // splashPanel1
            // 
            this.splashPanel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Vertical, System.Drawing.SystemColors.Highlight, System.Drawing.SystemColors.HighlightText);
            this.splashPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splashPanel1.BackgroundImage")));
            this.splashPanel1.BorderStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.splashPanel1.DiscreetLocation = new System.Drawing.Point(0, 0);
            this.splashPanel1.Location = new System.Drawing.Point(169, 76);
            this.splashPanel1.Name = "splashPanel1";
            this.splashPanel1.ShowAnimation = false;
            this.splashPanel1.Size = new System.Drawing.Size(430, 230);
            this.splashPanel1.TabIndex = 17;
            this.splashPanel1.TimerInterval = 3000;
            // 
            // splashControl1
            // 
            this.splashControl1.AutoMode = false;
            this.splashControl1.CustomSplashPanel = this.splashPanel1;
            this.splashControl1.HideHostForm = true;
            this.splashControl1.HostForm = this;
            this.splashControl1.UseCustomSplashPanel = true;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Symbol Designer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dockingManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.symbolPaletteGroupBar)).EndInit();
            this.overviewControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainFrameBarManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        #endregion

        #region Properties
        private Hashtable NodeToDiagramMap
        {
            get
            {
                if (m_hashPalNodeToModelMapping == null)
                {
                    m_hashPalNodeToModelMapping = new Hashtable();
                }

                return m_hashPalNodeToModelMapping;
            }
        }
        private Controls.Diagram ActiveDiagram
        {
            get
            {
                Controls.Diagram diagram = null;

                if (this.ActiveMdiChild != null)
                {
                    DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
                    if (diagramForm != null)
                    {
                        diagram = diagramForm.Diagram;
                    }
                }

                return diagram;
            }
        }

        public PropertyEditor PropertyEditor
        {
            get
            {
                return this.propertyEditor;
            }
        }

        #endregion

        #region File Menu Event Handlers
        private void biFileNew_Click(object sender, EventArgs e)
        {
            if (NeedSavePalette() || m_bNeedSave)
                ClosePalette();
            NewPalette();
            UpdateMainFormTitle();
            openDiagramDialog.FileName = String.Empty;
            saveDiagramDialog.FileName = String.Empty;
            m_bSaving = false;
        }
        private void biFileAppendPaletteNodes_Click(object sender, EventArgs e)
        {
            // append palette node's to current
            if (openPaletteDialog.ShowDialog(this) == DialogResult.OK)
            {
                string strFileName = openPaletteDialog.FileName;
                SymbolPalette newPalette = OpenPaletteFile(strFileName);

                if (newPalette != null)
                {
                    SymbolPalette palette = symbolPaletteGroupBar.CurrentSymbolPalette;

                    if (palette != null)
                    {
                        int nNodesCount = palette.Nodes.Count;

                        foreach (Node node in newPalette.Nodes)
                        {
                            // create mappings
                            this.NodeToDiagramMap.Add(node, null);

                            // add to current palette
                            palette.AppendChild(node);

                            int nIdx = newPalette.Nodes.IndexOf(node);
                            palette.SetUserLargeImage(nIdx + nNodesCount, newPalette.LargeImageList.Images[nIdx]);
                            palette.SetUserSmallImage(nIdx + nNodesCount, newPalette.SmallImageList.Images[nIdx]);
                        }

                        symbolPaletteGroupBar.Refresh();
                    }
                    else
                    {
                        symbolPaletteGroupBar.AddPalette(newPalette);

                        foreach (Node node in newPalette.Nodes)
                        {
                            // create mappings
                            this.NodeToDiagramMap.Add(node, null);
                        }
                    }
                }

                UpdateMainFormTitle();
            }
        }
        private void biFileOpenPalette_Click(object sender, EventArgs e)
        {
            if (NeedSavePalette() || m_bNeedSave)
                ClosePalette();
            openDiagramDialog.FileName = String.Empty;
            saveDiagramDialog.FileName = String.Empty;


            // load palette
            if (openPaletteDialog.ShowDialog(this) == DialogResult.OK)
            {
                string strFileName = openPaletteDialog.FileName;

                SymbolPalette newPalette = OpenPaletteFile(strFileName);

                if (newPalette != null)
                {
                    symbolPaletteGroupBar.AddPalette(newPalette);
                    UpdateSymbolMenu(true);
                    foreach (Node node in newPalette.Nodes)
                    {
                        // create mappings
                        this.NodeToDiagramMap.Add(node, null);
                    }
                }

                UpdateMainFormTitle();
            }
        }

        private void biFileClosePalette_Click(object sender, EventArgs e)
        {
            ClosePalette();
        }

        private void biFileSave_Click(object sender, EventArgs e)
        {
            if (savePaletteDialog.FileName.Length > 0)
            {
                SymbolPalette palette = PreparePaletteToSave();

                if (palette.Nodes.Count > 0)
                {
                    SavePalette(palette, savePaletteDialog.FileName);
                    symbolPaletteGroupBar.Refresh();
                }

            }
            else
            {
                SaveCurrentPaletteAs();
            }

            DiagramForm dfrm;

            foreach (Form frm in this.MdiChildren)
            {
                dfrm = frm as DiagramForm;

                if (dfrm != null)
                {
                    if (dfrm.Diagram.Model != null && dfrm.Diagram.Model.Modified)
                    {
                        dfrm.Diagram.Model.EndInit();
                    }
                }
            }

            m_bNeedSave = false;
        }
        private void biFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveCurrentPaletteAs();

            DiagramForm dfrm;

            foreach (Form frm in this.MdiChildren)
            {
                dfrm = frm as DiagramForm;

                if (dfrm != null)
                {
                    if (dfrm.Diagram.Model != null && dfrm.Diagram.Model.Modified)
                    {
                        dfrm.Diagram.Model.EndInit();
                    }
                }
            }

            m_bNeedSave = false;
        }

        private void biPrintPreview_Click(object sender, EventArgs e)
        {
            this.PrintPreview();
        }

        private void biFilePrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void biPageSetup_Click(object sender, EventArgs e)
        {
            this.PageSetup();
        }

        private void biFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barItemPageBorders_Click(object sender, EventArgs e)
        {
            PageBordersSetup();
        }
        #endregion

        #region Edit Menu Event Handlers

        private void barItemEditCut_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
                if (diagramForm != null)
                {
                    diagramForm.Diagram.Controller.Cut();
                }
            }
        }
        private void barItemEditCopy_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
                if (diagramForm != null)
                {
                    diagramForm.Diagram.Controller.Copy();
                }
            }
        }
        private void barItemEditPaste_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
                if (diagramForm != null)
                {
                    diagramForm.Diagram.Controller.Paste();
                }
            }
        }
        private void barItemEditUndo_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
                if (diagramForm != null)
                {
                    diagramForm.Diagram.Model.HistoryManager.Undo();
                    propertyEditor.PropertyGrid.Refresh();
                }
            }
        }
        private void barItemEditRedo_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
                if (diagramForm != null)
                {
                    diagramForm.Diagram.Model.HistoryManager.Redo();
                    propertyEditor.PropertyGrid.Refresh();
                }
            }
        }
        private void barItemEditSelectAll_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
                if (diagramForm != null)
                {
                    diagramForm.Diagram.Controller.SelectAll();
                }
            }
        }
        #endregion

        #region View Menu Event Handlers
        public void barShowRulers_Click(object sender, EventArgs e)
        {
            if (ActiveDiagram != null)
            {
                barShowRulers.Checked = !barShowRulers.Checked;
                this.ActiveDiagram.ShowRulers = barShowRulers.Checked;
            }
        }
        private void barItemViewSymbolPalette_Click(object sender, EventArgs e)
        {
            if (this.barItemViewSymbolPalette.Checked)
            {
                this.dockingManager.SetDockVisibility(this.symbolPaletteGroupBar, false);
                this.barItemViewSymbolPalette.Checked = false;
            }
            else
            {
                this.dockingManager.SetDockVisibility(this.symbolPaletteGroupBar, true);
                this.barItemViewSymbolPalette.Checked = true;
            }
        }

        private void barItemViewProperties_Click(object sender, EventArgs e)
        {
            if (this.barItemViewProperties.Checked)
            {
                this.dockingManager.SetDockVisibility(this.propertyEditor, false);
                this.barItemViewProperties.Checked = false;
            }
            else
            {
                this.dockingManager.SetDockVisibility(this.propertyEditor, true);
                this.barItemViewProperties.Checked = true;
            }
        }


        void barItemObjectModel_Click(object sender, EventArgs e)
        {
            if (barItemObjectModel.Checked)
            {
                this.dockingManager.SetDockVisibility(this.documentExplorer1, false);
                this.barItemObjectModel.Checked = false;
            }
            else
            {
                this.dockingManager.SetDockVisibility(this.documentExplorer1, true);
                this.barItemObjectModel.Checked = true;
            }
        }

        private void barItemPanZoom_Click(object sender, EventArgs e)
        {
            if (barItemPanZoom.Checked)
            {
                // Hide the overview control
                dockingManager.SetDockVisibility(overviewControl1, false);
                barItemPanZoom.Checked = false;
            }
            else
            {
                // Initialize the OverviewControl with the active diagram's Model & View and display the control
                if ((this.ActiveDiagram != null) && !(dockingManager.GetDockVisibility(overviewControl1)))
                {
                    dockingManager.SetDockVisibility(overviewControl1, true);
                    overviewControl1.Diagram = this.ActiveDiagram;
                }

                barItemPanZoom.Checked = true;
            }
        }
        private void PageBordersSetup()
        {
            Controls.Diagram activeDiagram = this.ActiveDiagram;
            if (activeDiagram != null && activeDiagram.Model != null)
            {
                PageBorderDialog borderDialog = new PageBorderDialog();
                borderDialog.PageBorderStyle = activeDiagram.View.PageBorderStyle;
                if (borderDialog.ShowDialog() == DialogResult.OK)
                {
                    activeDiagram.View.PageBorderStyle = borderDialog.PageBorderStyle;
                    activeDiagram.View.RefreshPageSettings();
                    activeDiagram.UpdateView();
                }
            }
        }


        #endregion

        #region Window Menu Event Handlers

        private void biTabbedMDI_Click(object sender, EventArgs e)
        {
            // Toggle tabbed MDI mode
            Tools.XPMenus.BarItem barItem = sender as Tools.XPMenus.BarItem;
            if (barItem != null)
            {
                if (barItem.Checked)
                {
                    tabbedMDIManager.DetachFromMdiContainer(this, true);
                    barItem.Checked = false;
                }
                else
                {
                    tabbedMDIManager.AttachToMdiContainer(this);
                    barItem.Checked = true;
                }
            }
        }

        #endregion

        #region Help Menu Event Handlers

        private void barItemAbout_Click(object sender, EventArgs e)
        {
            About aboutDlg = new About();
            aboutDlg.ShowDialog(this);
        }

        #endregion

        #region Helper methods
        private SymbolPalette OpenPaletteFile(string strFileName)
        {
            SymbolPalette paletteToReturn = null;

            // Open symbol palette and add it to the symbol palette group bar
            //if( openPaletteDialog.ShowDialog( this ) == DialogResult.OK )
            //{
            //string strFileName = openPaletteDialog.FileName;

            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.RightToLeft;
            Match match = Regex.Match(strFileName, ".vss|.vsx|.vsd|.vdx", options);

            if (match.Success)
            {
                VisioStencilConverter converter = new VisioStencilConverter(strFileName, this);
                converter.ShowProgressDialog = true;

                paletteToReturn = converter.Convert();
                savePaletteDialog.FileName = String.Empty;
                //openPaletteDialog.FileName.Replace(openPaletteDialog.FileName.Substring(openPaletteDialog.FileName.LastIndexOf(".")+1, 3), savePaletteDialog.DefaultExt);
            }
            else
            {
                //using (FileStream fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                //{
                //    IFormatter formatter = new BinaryFormatter();
                //    formatter.Binder = Syncfusion.Runtime.Serialization.AppStateSerializer.CustomBinder;

                //    try
                //    {
                //        AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DiagramBaseAssembly.AssemblyResolver);
                //        paletteToReturn = (SymbolPalette)formatter.Deserialize(fs);
                //        paletteToReturn.UpdateIcons();
                //    }
                //    catch (SerializationException)
                //    {
                //        try
                //        {
                //            formatter = new SoapFormatter();
                //            fs.Position = 0;

                //            paletteToReturn = (SymbolPalette)formatter.Deserialize(fs);
                //            paletteToReturn.UpdateIcons();
                //        }
                //        catch (Exception se)
                //        {
                //            MessageBox.Show(this, se.Message);
                //        }
                //    }
                //}
                paletteToReturn = new SymbolPalette();
                paletteToReturn = paletteToReturn.FromFile(strFileName);
                savePaletteDialog.FileName = openPaletteDialog.FileName;
            }
            //}
            return paletteToReturn;
        }
        private void SavePalette(SymbolPalette palette, string strSavePath)
        {
            if (palette == null) throw new ArgumentNullException("palette");

            using (FileStream fStream = new FileStream(strSavePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fStream, palette);
            }

            if (this.ActiveMdiChild != null)
            {
                if (this.ActiveMdiChild.GetType().Name.Equals("DiagramForm"))
                {
                    if (this.ActiveMdiChild.Text.EndsWith("*"))
                        this.ActiveMdiChild.Text = this.ActiveMdiChild.Text.Remove(this.ActiveMdiChild.Text.IndexOf("*"));
                    if (symbolPaletteGroupBar.GroupBarItems[0].Text.EndsWith("*"))
                        symbolPaletteGroupBar.GroupBarItems[0].Text = symbolPaletteGroupBar.GroupBarItems[0].Text.Remove(symbolPaletteGroupBar.GroupBarItems[0].Text.IndexOf("*")); ;
                }
            }
        }
        private void SaveCurrentPaletteAs()
        {
            if (savePaletteDialog.ShowDialog() == DialogResult.OK)
            {
                SymbolPalette palette = PreparePaletteToSave();

                if (palette.Nodes.Count > 0)
                {
                    SavePalette(palette, savePaletteDialog.FileName);
                    symbolPaletteGroupBar.Refresh();
                }
            }
        }
        private SymbolPalette PreparePaletteToSave()
        {
            SymbolPalette palette = new SymbolPalette();
            palette.Name = symbolPaletteGroupBar.CurrentSymbolPalette.Name;

            Node nodeTmp = null;
            Model modelTmp;
            // iterate through palette nodes
            NodeCollection nodesPalette = symbolPaletteGroupBar.CurrentSymbolPalette.Nodes;
            nodesPalette.QuietMode = true;

            for (int i = 0; i < nodesPalette.Count; i++)
            {
                Node node = nodesPalette[i];

                if (!this.NodeToDiagramMap.ContainsKey(node)) continue;

                DiagramForm frmDgm = this.NodeToDiagramMap[node] as DiagramForm;

                if (frmDgm != null)
                {
                    modelTmp = frmDgm.Diagram.Model;

                    if (modelTmp != null)
                    {
                        // get model nodes
                        NodeCollection nodes = (NodeCollection)modelTmp.Nodes.Clone();
                        // add nodes to group
                        if (nodes.Count > 1)
                        {
                            nodeTmp = new Group(nodes);
                        }
                        else if (nodes.Count > 0)
                        {
                            nodeTmp = nodes.First;
                        }
                        else
                        {
                            nodeTmp = null;
                        }

                        // add group to palette
                    }
                }
                else
                {
                    nodeTmp = node.Clone() as Node;
                }

                if (nodeTmp != null)
                {
                    nodeTmp.Name = nodesPalette[i].Name;
                    symbolPaletteGroupBar.CurrentSymbolPalette.UpdateNode(nodeTmp, i);
                    symbolPaletteGroupBar.CurrentSymbolPalette.UpdateImage(i);
                    this.NodeToDiagramMap[nodeTmp] = frmDgm;
                    UpdateMDIChildText();
                }
            }

            return symbolPaletteGroupBar.CurrentSymbolPalette;
        }
        private bool NeedSavePalette()
        {
            // iterate through open DiagramForms and check Model.Modified flag
            bool bSuccess = false;
            DiagramForm dfrm;

            foreach (Form frm in this.MdiChildren)
            {
                dfrm = frm as DiagramForm;

                if (dfrm != null)
                {
                    if (dfrm.Diagram.Model != null && dfrm.Diagram.Model.Modified)
                    {
                        bSuccess = true;
                        break;
                    }
                }
            }

            return bSuccess;
        }

        bool cancelClosing = false;

        private void ClosePalette()
        {
            if (SaveOnDemand())
            {
                cancelClosing = true;
                return;
            }
            cancelClosing = false;
            foreach (Form frm in this.MdiChildren)
            {
                DiagramForm dfrm = frm as DiagramForm;
                if (dfrm != null && !dfrm.IsDisposed)
                {
                    dfrm.Dispose();
                }
            }

            this.NodeToDiagramMap.Clear();
            if (symbolPaletteGroupBar.GroupBarItems.Count > 0)
                symbolPaletteGroupBar.GroupBarItems.RemoveAt(symbolPaletteGroupBar.SelectedItem);
            symbolPaletteGroupBar.Refresh();
            UpdateSymbolMenu(false);
        }

        private void UpdateSymbolMenu(bool status)
        {
            this.barAddSymbol.Enabled = status;
            this.barRemoveSymbol.Enabled = status;
        }

        private void NewPalette()
        {
            symbolPaletteGroupBar.AddPalette();
            UpdateSymbolMenu(true);
            this.savePaletteDialog.FileName = "";
            this.saveDiagramDialog.FileName = "";
        }
        private void UpdateMainFormTitle()
        {
            SymbolPalette curSymbolPalette = symbolPaletteGroupBar.CurrentSymbolPalette;
            string textVal = c_strFORM_TITLE;

            if (curSymbolPalette != null)
            {
                textVal += " - " + curSymbolPalette.Name;
            }

            this.Text = textVal;
        }
        /// <summary>
        /// Save currecnt palette on demand.
        /// </summary>
        /// <returns>TRUE if DialogResult equals cancel - don't close window, otherwise FALSE.</returns>
        private bool SaveOnDemand()
        {
            bool cancel = false;

            if (NeedSavePalette() || m_bNeedSave)
            {
                System.Windows.Forms.DialogResult dlgResult = MessageBox.Show(this, c_strMSG_PROMPT_SAVE, c_strCAPTION_PROMPT_SAVE
                    , MessageBoxButtons.YesNoCancel);

                if (dlgResult == DialogResult.Cancel)
                {
                    cancel = true;
                }

                if (dlgResult == DialogResult.Yes)
                {
                    if (savePaletteDialog.FileName.Length > 0)
                    {
                        SymbolPalette palette = PreparePaletteToSave();

                        if (palette.Nodes.Count > 0)
                        {
                            SavePalette(palette, savePaletteDialog.FileName);
                            symbolPaletteGroupBar.Refresh();
                        }

                    }
                    else
                    {
                        SaveCurrentPaletteAs();
                    }

                    DiagramForm dfrm;

                    foreach (Form frm in this.MdiChildren)
                    {
                        dfrm = frm as DiagramForm;

                        if (dfrm != null)
                        {
                            if (dfrm.Diagram.Model != null && dfrm.Diagram.Model.Modified)
                            {
                                dfrm.Diagram.Model.EndInit();
                            }
                        }
                    }

                    m_bNeedSave = false;
                }
                if (dlgResult == DialogResult.No)
                    m_bNeedSave = false;
            }

            return cancel;
        }
        #endregion

        #region Docking

        private void dockingManager_DockVisibilityChanged(object sender, Tools.DockVisibilityChangedEventArgs e)
        {
            if (e.Control == this.propertyEditor)
            {
                this.barItemViewProperties.Checked = this.dockingManager.GetDockVisibility(propertyEditor);
            }
            else if (e.Control == this.symbolPaletteGroupBar)
            {
                this.barItemViewSymbolPalette.Checked = this.dockingManager.GetDockVisibility(symbolPaletteGroupBar);
            }
            else if (e.Control == this.overviewControl1)
            {
                this.barItemPanZoom.Checked = this.dockingManager.GetDockVisibility(overviewControl1);
            }
            if (e.Control == this.documentExplorer1)
            {
                this.barItemObjectModel.Checked = this.dockingManager.GetDockVisibility(documentExplorer1);
            }
        }

        #endregion

        #region UI Updating

        private void OnIdle(object sender, EventArgs evtArgs)
        {
            Controls.Diagram activeDiagram = this.ActiveDiagram;

            if (symbolPaletteGroupBar.SelectedItem != -1)
                biFileOpenDiagram.Enabled = symbolPaletteGroupBar.SelectedNode != null;
            else
                biFileOpenDiagram.Enabled = false;

            string editUndoText = c_strUNDO;
            string editRedoText = c_strREDO;
            this.barItemPanZoom.Checked = dockingManager.GetDockVisibility(overviewControl1);
            this.barItemViewProperties.Checked = dockingManager.GetDockVisibility(propertyEditor);
            this.barItemViewSymbolPalette.Checked = dockingManager.GetDockVisibility(symbolPaletteGroupBar);

            if (activeDiagram != null && activeDiagram.Model != null)
            {
                this.biFileSave.Enabled = true;
                this.biFileSaveAs.Enabled = true;
                this.biFileClosePalette.Enabled = true;
                this.barItemEditCopy.Enabled = activeDiagram.CanCopy;
                this.barItemEditCut.Enabled = activeDiagram.CanCut;
                if (this.WindowState != FormWindowState.Minimized)
                    this.barItemEditPaste.Enabled = activeDiagram.CanPaste;
                this.barItemEditUndo.Enabled = activeDiagram.Model.HistoryManager.CanUndo;
                this.barItemEditRedo.Enabled = activeDiagram.Model.HistoryManager.CanRedo;
                this.barItemEditDelete.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                this.barShowRulers.Enabled = true;
                this.barShowRulers.Checked = this.ActiveDiagram.ShowRulers;
                this.barItemEditSelectAll.Enabled = true;
                this.biFilePrint.Enabled = true;
                this.biPageSetup.Enabled = true;
                barItemAlignLeft.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count >= 2);
                barItemAlignCenter.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count >= 2);
                barItemAlignRight.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count >= 2);
                barItemAlignTop.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count >= 2);
                barItemAlignMiddle.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count >= 2);
                barItemAlignBottom.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count >= 2);
                barItemFlipHorizontally.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                barItemFlipVertically.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                barItemFlipBoth.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                barItemGroupingGroup.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 1);
                barItemGroupingUnGroup.Enabled = true;
                barItemOrderFront.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                barItemOrderForward.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                barItemOrderBackward.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                barItemOrderBack.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                barItemRotateClock.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                barItemRotateConter.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 0);
                barItemResizeWidth.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 1);
                barItemResizeHeight.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 1);
                barItemResizeSize.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 1);
                barItemResizeAcross.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 1);
                barItemResizeDown.Enabled = (this.ActiveDiagram.Controller.SelectionList.Count > 1);
                if (this.ActiveDiagram.Controller.SelectionList.Count > 0)
                {
                    Node node = this.ActiveDiagram.Controller.SelectionList[0];
                    barItemFillStyle.Enabled = (TypeDescriptor.GetProperties(node, false)["FillStyle"] != null);
                    barItemShadowStyle.Enabled = (TypeDescriptor.GetProperties(node, false)["ShadowStyle"] != null);
                }

                string[] strDescriptions;
                int nDescWanted = 1;
                int nDescReturned = activeDiagram.Model.HistoryManager.GetUndoDescriptions(nDescWanted, out strDescriptions);

                if (nDescReturned == nDescWanted)
                    editUndoText = editUndoText + " " + strDescriptions[0];

                // clear strDecsriptions.
                nDescReturned = activeDiagram.Model.HistoryManager.GetRedoDescriptions(nDescWanted, out strDescriptions);
                if (nDescReturned == nDescWanted)
                    editRedoText = editRedoText + " " + strDescriptions[0];
            }
            else
            {
                this.biFileSave.Enabled = false;
                this.biFileSaveAs.Enabled = false;
                this.biFileClosePalette.Enabled = false;
                this.barItemEditCopy.Enabled = false;
                this.barItemEditCut.Enabled = false;
                this.barItemEditPaste.Enabled = false;
                this.barItemEditUndo.Enabled = false;
                this.barItemEditRedo.Enabled = false;
                this.barItemEditSelectAll.Enabled = false;
                this.biFilePrint.Enabled = false;
                this.biPageSetup.Enabled = false;
                this.barItemEditDelete.Enabled = false;
                this.barShowRulers.Enabled = false;

                barItemAlignLeft.Enabled = false;
                barItemAlignCenter.Enabled = false;
                barItemAlignRight.Enabled = false;
                barItemAlignTop.Enabled = false;
                barItemAlignMiddle.Enabled = false;
                barItemAlignBottom.Enabled = false;
                barItemFlipHorizontally.Enabled = false;
                barItemFlipVertically.Enabled = false;
                barItemFlipBoth.Enabled = false;
                barItemGroupingGroup.Enabled = false;
                barItemGroupingUnGroup.Enabled = false;
                barItemOrderFront.Enabled = false;
                barItemOrderForward.Enabled = false;
                barItemOrderBackward.Enabled = false;
                barItemOrderBack.Enabled = false;
                barItemRotateClock.Enabled = false;
                barItemRotateConter.Enabled = false;
                barItemResizeWidth.Enabled = false;
                barItemResizeHeight.Enabled = false;
                barItemResizeSize.Enabled = false;
                barItemResizeAcross.Enabled = false;
                barItemResizeDown.Enabled = false;
                barItemFillStyle.Enabled = false;
                barItemShadowStyle.Enabled = false;
            }

            this.barItemEditUndo.Text = editUndoText;
            this.barItemEditRedo.Text = editRedoText;
            this.dockingManager.SetCloseButtonVisibility(symbolPaletteGroupBar, false);
            this.dockingManager.SetMenuButtonVisibility(symbolPaletteGroupBar, false);
            this.dockingManager.SetMenuButtonVisibility(documentExplorer1, false);
            biFileClosePalette.Enabled = biFileSave.Enabled = biFileSaveAs.Enabled = this.symbolPaletteGroupBar.CurrentSymbolPalette != null;
        }

        #endregion

        #region Printing

        private void PageSetup()
        {
            Controls.Diagram activeDiagram = this.ActiveDiagram;

            if (activeDiagram != null && activeDiagram.Model != null)
            {
                PrintSetupDialog dlgPrintSetup = new PrintSetupDialog();

                // Made to make values more user friendly 
                dlgPrintSetup.PageSettings = activeDiagram.View.PageSettings;
                dlgPrintSetup.PrintZoom = activeDiagram.View.PrintZoom;

                if (dlgPrintSetup.ShowDialog() == DialogResult.OK)
                {
                    activeDiagram.View.PageSettings = dlgPrintSetup.PageSettings;
                    activeDiagram.View.PrintZoom = dlgPrintSetup.PrintZoom;
                    activeDiagram.View.RefreshPageSettings();
                    activeDiagram.UpdateView();
                }
            }
        }

        private void Print()
        {
            Controls.Diagram activeDiagram = this.ActiveDiagram;
            if (activeDiagram != null)
            {
                PrintDocument printDoc = activeDiagram.CreatePrintDocument();
                PrintDialog printDlg = new PrintDialog();
                printDlg.Document = printDoc;

                printDlg.AllowSomePages = true;

                if (printDlg.ShowDialog(this) == DialogResult.OK)
                {
                    printDoc.PrinterSettings = printDlg.PrinterSettings;
                    printDoc.Print();
                }
            }
        }

        private void PrintPreview()
        {
            Controls.Diagram activeDiagram = this.ActiveDiagram;

            if (activeDiagram != null)
            {
                PrintDocument printDoc = activeDiagram.CreatePrintDocument();
                PrintPreviewDialog printPreviewDlg = new PrintPreviewDialog();
                printPreviewDlg.StartPosition = FormStartPosition.CenterScreen;

                printDoc.PrinterSettings.FromPage = 0;
                printDoc.PrinterSettings.ToPage = 0;
                printDoc.PrinterSettings.PrintRange = PrintRange.AllPages;

                printPreviewDlg.Document = printDoc;
                printPreviewDlg.ShowDialog(this);
            }
        }

        #endregion

        #region Event handlers
        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            DiagramForm frmDgm = this.ActiveMdiChild as DiagramForm;

            if (frmDgm != null)
            {
                this.propertyEditor.Diagram = frmDgm.Diagram;

                Bar mergeBar = frmDgm.childFrameBarManager.MainFrameBarManager.GetMergedEquivalent(frmDgm.bar1, frmDgm.bar1);
                this.mainFrameBarManager.GetBarControl(mergeBar).RowIndex = 2;
                this.mainFrameBarManager.GetBarControl(mergeBar).RowOffset = 0;

                mergeBar = frmDgm.childFrameBarManager.MainFrameBarManager.GetMergedEquivalent(frmDgm.barDrawing, frmDgm.barDrawing);
                this.mainFrameBarManager.GetBarControl(mergeBar).RowIndex = 2;
                this.mainFrameBarManager.GetBarControl(mergeBar).RowOffset = 0;

                mergeBar = frmDgm.childFrameBarManager.MainFrameBarManager.GetMergedEquivalent(frmDgm.barNode, frmDgm.barNode);
                this.mainFrameBarManager.GetBarControl(mergeBar).RowIndex = 2;
                this.mainFrameBarManager.GetBarControl(mergeBar).RowOffset = 0;

                mergeBar = frmDgm.childFrameBarManager.MainFrameBarManager.GetMergedEquivalent(frmDgm.barNudge, frmDgm.barNudge);
                this.mainFrameBarManager.GetBarControl(mergeBar).RowIndex = 2;
                this.mainFrameBarManager.GetBarControl(mergeBar).RowOffset = 0;

                // If the OverviewControl is visible then change the OverviewControl.Model and OverviewControl.View 
                // properties to reference the new diagram's Model and View
                if (this.dockingManager.GetDockVisibility(overviewControl1))
                {
                    this.overviewControl1.Diagram = frmDgm.Diagram;
                }

                // update palette selection
                Node nodeMapped = GetMappedNode(frmDgm);
                int index = symbolPaletteGroupBar.CurrentSymbolPalette.Nodes.IndexOf(nodeMapped);

                if (nodeMapped != null)
                    symbolPaletteGroupBar.SelectNode(nodeMapped);

                GroupView gvPalette = symbolPaletteGroupBar.GroupBarItems[0].Client as GroupView;
                gvPalette.BringItemIntoView(index);

                if (dockingManager.GetDockVisibility(this.documentExplorer1))
                {
                    this.documentExplorer1.Nodes.Clear();
                    this.documentExplorer1.AttachModel(frmDgm.Diagram.Model);
                }

                this.documentExplorer1.AfterSelect += new TreeViewEventHandler(documentExplorer1_AfterSelect);
            }
        }

        void documentExplorer1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Update diagram's selection list depending on TreeNode Tag
            if (e.Node.Tag is Model)
            {
                foreach (Form frm in this.MdiChildren)
                {
                    DiagramForm frmDgm = frm as DiagramForm;

                    if (frmDgm != null)
                    {
                        if (frmDgm.Diagram != null && frmDgm.Diagram.Model != null
                            && frmDgm.Diagram.Model.Equals(e.Node.Tag))
                        {
                            frmDgm.BringToFront();
                            break;
                        }
                    }
                }
            }
            else if (e.Node.Tag is Node)
            {
                Node nodeTemp = e.Node.Tag as Node;

                if (nodeTemp != null)
                {
                    if (nodeTemp.Visible && nodeTemp.Root.Equals(this.ActiveDiagram.Model))
                    {
                        ActiveDiagram.View.SelectionList.Clear();
                        ActiveDiagram.View.SelectionList.Add(e.Node.Tag as Node);
                    }
                    else
                    {
                        propertyEditor.PropertyGrid.SelectedObject = nodeTemp;
                    }
                }
            }
        }

        private void symbolPaletteGroupBar_GroupBarItemRemoved(object sender, GroupBarItemEventArgs args)
        {
            if (args.Item != null && args.Item.GroupBar != null)
            {
                PaletteGroupView paletteView = args.Item.Client as PaletteGroupView;

                if (paletteView != null)
                {
                    paletteView.Click -= new EventHandler(groupBar_NodeClick);
                    paletteView.GroupViewItems.CollectionChanged -= new CollectionChangeEventHandler(GroupViewItems_CollectionChanged);
                }
            }
        }

        private void symbolPaletteGroupBar_GroupBarItemAdded(object sender, GroupBarItemEventArgs args)
        {
            if (args.Item != null && args.Item.GroupBar != null)
            {
                PaletteGroupView paletteView = args.Item.Client as PaletteGroupView;

                if (paletteView != null)
                {
                    paletteView.ButtonView = false;
                    paletteView.BackColor = SystemColors.Control;
                    paletteView.Click += new EventHandler(groupBar_NodeClick);
                    paletteView.GroupViewItems.CollectionChanged += new CollectionChangeEventHandler(GroupViewItems_CollectionChanged);
                }
            }
        }

        void GroupViewItems_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if (e.Action == CollectionChangeAction.Add || e.Action == CollectionChangeAction.Remove)
            {
                m_bNeedSave = true;
                if (symbolPaletteGroupBar.SelectedItem != -1)
                {

                    if (symbolPaletteGroupBar.GroupBarItems[symbolPaletteGroupBar.SelectedItem] != null)
                        if (!symbolPaletteGroupBar.GroupBarItems[symbolPaletteGroupBar.SelectedItem].Text.EndsWith("*"))
                            symbolPaletteGroupBar.GroupBarItems[symbolPaletteGroupBar.SelectedItem].Text += "*";
                }
            }
        }

        private void symbolPaletteGroupBar_NodeSelected(object sender, NodeEventArgs evtArgs)
        {
            if (evtArgs.Node != null)
            {
                // activate mapped DiagramForm
                if (this.NodeToDiagramMap.ContainsKey(evtArgs.Node) && this.NodeToDiagramMap[evtArgs.Node] != null)
                {
                    ((Form)this.NodeToDiagramMap[evtArgs.Node]).BringToFront();
                }

                PaletteGroupView view = symbolPaletteGroupBar.GroupBarItems[symbolPaletteGroupBar.SelectedItem].Client as PaletteGroupView;
                if (view != null && view.SelectedItem >= 0)
                {
                    propertyEditor.PropertyGrid.SelectedObject = view.GroupViewItems[view.SelectedItem] as PaletteGroupView.GroupViewPaletteItem;
                }
            }
        }
        private void groupBar_NodeClick(object sender, EventArgs evtArgs)
        {
            //if (this.symbolPaletteGroupBar.SelectedNode != null)
            //{
            //    DiagramForm frmDgm = this.ActiveMdiChild as DiagramForm;

            //    if (frmDgm != null)
            //    {
            //        this.propertyEditor.Diagram = frmDgm.Diagram;

            //        // update palette selection
            //        Node nodeMapped = GetMappedNode(frmDgm);

            //        if (nodeMapped != null)
            //            symbolPaletteGroupBar.SelectNode(nodeMapped);

            //        PaletteGroupView view = symbolPaletteGroupBar.GroupBarItems[symbolPaletteGroupBar.SelectedItem].Client as PaletteGroupView;

            //        if (view != null && view.SelectedItem >= 0)
            //        {
            //            propertyEditor.PropertyGrid.SelectedObject = view.GroupViewItems[view.SelectedItem] as PaletteGroupView.GroupViewPaletteItem;
            //        }
            //    }
            //}


            if (symbolPaletteGroupBar.SelectedNode == null) return;

            Node node = symbolPaletteGroupBar.SelectedNode;

            // open symbol editor if it is closed
            if ((this.NodeToDiagramMap.ContainsKey(node)) && (this.NodeToDiagramMap[node] == null ||
                (this.NodeToDiagramMap[node] != null && (this.NodeToDiagramMap[node] as Form).Parent == null)))
            {
                DiagramForm frmDgm = CreateDiagramForm();
                frmDgm.Text = node.Name;

                Model mdlTmp = frmDgm.Diagram.Model;
                mdlTmp.BeginInit();

                if (mdlTmp != null)
                {
                    Group group = node as Group;
                    mdlTmp.AppendChild(node);

                    mdlTmp.SizeToContent = true;
                    RectangleF bounds = RenderingHelper.GetBoundingRectangle(mdlTmp.Nodes, MeasureUnits.Pixel);

                    mdlTmp.SizeToContent = true;
                    mdlTmp.SizeToContent = false;
                    mdlTmp.LogicalSize = new SizeF(
                        mdlTmp.LogicalSize.Width + bounds.Size.Width / 2,
                        mdlTmp.LogicalSize.Height + bounds.Size.Height / 2
                        );

                    foreach (Node n in mdlTmp.Nodes)
                    {
                        // handling AllowMove styles
                        bool allowmovex = n.EditStyle.AllowMoveX;
                        bool allowmovey = n.EditStyle.AllowMoveY;
                        n.EditStyle.AllowMoveX = false;
                        n.EditStyle.AllowMoveY = false;

                        float fx = MeasureUnitsConverter.Convert((mdlTmp.LogicalSize.Width - bounds.Size.Width) / 2, MeasureUnits.Pixel, n.MeasurementUnit);
                        float fy = MeasureUnitsConverter.Convert((mdlTmp.LogicalSize.Height - bounds.Size.Height) / 2, MeasureUnits.Pixel, n.MeasurementUnit);

                        n.Translate(fx, fy);

                        n.EditStyle.AllowMoveX = allowmovex;
                        n.EditStyle.AllowMoveY = allowmovey;
                    }

                    // set model name
                    mdlTmp.Name = node.Name;
                }

                // update mapping
                this.NodeToDiagramMap[node] = frmDgm;
                frmDgm.Closing += new CancelEventHandler(DiagramForm_Closing);
                frmDgm.Show();
                mdlTmp.EndInit();
                mdlTmp.HistoryManager.ClearHistory();
            }
        }

        private void barItem1_Click(object sender, EventArgs e)
        {
            Process.Start("http://help.syncfusion.com/ug_93/User%20Interface/Windows%20Forms/Diagram/Documents/3212symboldesigner.htm");
        }

        private void shadowBarItem_Click(object sender, EventArgs e)
        {
            ShadowStyleDialog ssd = new ShadowStyleDialog();
            ShadowStyle fs = null;
            foreach (Node n in this.ActiveDiagram.Controller.SelectionList)
            {
                fs = (ShadowStyle)TypeDescriptor.GetProperties(n, false)["ShadowStyle"].GetValue(n);
                if (fs != null)
                {
                    break;
                }
            }
            if (fs != null)
            {
                ssd.ShadowStyle.Color = fs.Color;
                ssd.ShadowStyle.ForeColor = fs.ForeColor;
                ssd.ShadowStyle.ColorAlphaFactor = fs.ColorAlphaFactor;
                ssd.ShadowStyle.ForeColorAlphaFactor = fs.ForeColorAlphaFactor;
                ssd.ShadowStyle.PathBrushStyle = fs.PathBrushStyle;
                ssd.ShadowStyle.OffsetX = fs.OffsetX;
                ssd.ShadowStyle.OffsetY = fs.OffsetY;
                ssd.ShadowStyle.Visible = fs.Visible;
                if (DialogResult.OK == ssd.ShowDialog())
                {
                    foreach (Node n in this.ActiveDiagram.Controller.SelectionList)
                    {
                        fs = (ShadowStyle)TypeDescriptor.GetProperties(n, false)["ShadowStyle"].GetValue(n);
                        if (fs != null)
                        {
                            fs.Color = ssd.ShadowStyle.Color;
                            fs.ForeColor = ssd.ShadowStyle.ForeColor;
                            fs.ColorAlphaFactor = ssd.ShadowStyle.ColorAlphaFactor;
                            fs.ForeColorAlphaFactor = ssd.ShadowStyle.ForeColorAlphaFactor;
                            fs.PathBrushStyle = ssd.ShadowStyle.PathBrushStyle;
                            fs.OffsetX = ssd.ShadowStyle.OffsetX;
                            fs.OffsetY = ssd.ShadowStyle.OffsetY;
                            fs.Visible = ssd.ShadowStyle.Visible;
                        }
                    }
                }
            }
        }
        private void fillBarItem_Click(object sender, EventArgs e)
        {
            FillStyleDialog fsd = new FillStyleDialog();
            FillStyle fs = null;
            foreach (Node n in this.ActiveDiagram.Controller.SelectionList)
            {
                fs = (FillStyle)TypeDescriptor.GetProperties(n, false)["FillStyle"].GetValue(n);
                if (fs != null)
                {
                    break;
                }
            }

            if (fs != null)
            {
                fsd.FillStyle.Color = fs.Color;
                fsd.FillStyle.ForeColor = fs.ForeColor;
                fsd.FillStyle.ColorAlphaFactor = fs.ColorAlphaFactor;
                fsd.FillStyle.ForeColorAlphaFactor = fs.ForeColorAlphaFactor;
                fsd.FillStyle.Type = fs.Type;
                fsd.FillStyle.GradientAngle = fs.GradientAngle;
                fsd.FillStyle.GradientCenter = fs.GradientCenter;
                fsd.FillStyle.PathBrushStyle = fs.PathBrushStyle;
                fsd.FillStyle.HatchBrushStyle = fs.HatchBrushStyle;
                fsd.FillStyle.Texture = fs.Texture;
                fsd.FillStyle.TextureWrapMode = fs.TextureWrapMode;
                if (DialogResult.OK == fsd.ShowDialog())
                {
                    foreach (Node n in this.ActiveDiagram.Controller.SelectionList)
                    {
                        fs = (FillStyle)TypeDescriptor.GetProperties(n, false)["FillStyle"].GetValue(n);
                        if (fs != null)
                        {
                            fs.Color = fsd.FillStyle.Color;
                            fs.ForeColor = fsd.FillStyle.ForeColor;
                            fs.ColorAlphaFactor = fsd.FillStyle.ColorAlphaFactor;
                            fs.ForeColorAlphaFactor = fsd.FillStyle.ForeColorAlphaFactor;
                            fs.Type = fsd.FillStyle.Type;
                            fs.GradientAngle = fsd.FillStyle.GradientAngle;
                            fs.GradientCenter = fsd.FillStyle.GradientCenter;
                            fs.PathBrushStyle = fsd.FillStyle.PathBrushStyle;
                            fs.HatchBrushStyle = fsd.FillStyle.HatchBrushStyle;
                            fs.Texture = fsd.FillStyle.Texture;
                            fs.TextureWrapMode = fsd.FillStyle.TextureWrapMode;
                        }
                    }
                }
            }
        }

        static int nodeIndex = 0;
        private void barAddSymbol_Click(object sender, EventArgs e)
        {
            Cursor prevCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            SymbolPalette palette = symbolPaletteGroupBar.CurrentSymbolPalette;

            if (palette != null)
            {
                // add new node with default name
                Node node = new Group();
                if (nodeIndex > 0)
                    nodeIndex = GetChildFormIndex();
                node.Name = c_strSYMBOL_DEFAULT_NAME + ++nodeIndex;
                palette.AppendChild(node);

                GroupView grpView = this.symbolPaletteGroupBar.GroupBarItems[symbolPaletteGroupBar.GroupBarItems.Count - 1].Client as GroupView;
                grpView.BringItemIntoView(grpView.GroupViewItems.Count - 1);

                // create document for new symbol customization
                DiagramForm frmDgrm = CreateDiagramForm();

                // create mapping -- Palette node to Diagram
                if (!this.NodeToDiagramMap.ContainsKey(node))
                    this.NodeToDiagramMap.Add(node, frmDgrm);

                frmDgrm.Show();
            }

            Cursor.Current = prevCursor;
        }

        int childFormIndex = 0;
        private DiagramForm CreateDiagramForm()
        {
            DiagramForm frmDgrm = new DiagramForm();
            frmDgrm.MdiParent = this;
            propertyEditor.Diagram = frmDgrm.Diagram;
            if (childFormIndex > 0)
                childFormIndex = GetChildFormIndex();
            frmDgrm.Diagram.Model.Name = c_strSYMBOL_DEFAULT_NAME + ++childFormIndex;
            frmDgrm.Text = c_strSYMBOL_DEFAULT_NAME + childFormIndex;

            //Hook up the diagram events
            frmDgrm.Diagram.Model.EventSink.PropertyChanged += new PropertyChangedEventHandler(OnDiagramPropertyChanged);
            frmDgrm.Diagram.Model.EventSink.LabelsChanged += new CollectionExEventHandler(OnDiagramLabelsChanged);
            frmDgrm.Diagram.Model.EventSink.NodeCollectionChanged += new CollectionExEventHandler(OnDiagramNodeCollectionChanged);
            frmDgrm.Diagram.Model.EventSink.LayersChanged += new CollectionExEventHandler(OnDiagramLayersChanged);
            frmDgrm.Diagram.Model.EventSink.PinOffsetChanged += new PinOffsetChangedEventHandler(OnDiagramPinOffsetChanged);
            frmDgrm.Diagram.Model.EventSink.PinPointChanged += new PinPointChangedEventHandler(OnDiagramPinPointChanged);
            frmDgrm.Diagram.Model.EventSink.PortsChanged += new CollectionExEventHandler(OnDiagramPortsChanged);
            frmDgrm.Diagram.Model.EventSink.RotationChanged += new RotationChangedEventHandler(OnDiagramRotationChanged);
            frmDgrm.Diagram.Model.EventSink.SizeChanged += new SizeChangedEventHandler(OnDiagramSizeChanged);
            frmDgrm.Diagram.Model.EventSink.VertexChanged += new VertexChangedEventHandler(OnDiagramVertexChanged);
            frmDgrm.Diagram.Model.EventSink.ZOrderChanged += new ZOrderChangedEventHandler(OnDiagramZOrderChanged);
            frmDgrm.Closed += new EventHandler(frmDgrm_Closed);
            frmDgrm.Diagram.Click += new EventHandler(Diagram_Click);

            return frmDgrm;
        }

        private int GetChildFormIndex()
        {
            int index = 0;
            int prevIndex = 0;
            ArrayList mdiChildList = new ArrayList();
            foreach (Form f in this.MdiChildren)
                if (f is DiagramForm)
                    mdiChildList.Add(f.Text);
            mdiChildList.Sort();
            foreach (object name in mdiChildList)
            {
                if (name.ToString().StartsWith("New Symbol"))
                {
                    string strIndex = name.ToString().Substring(10).TrimEnd(new char[] { '*' });
                    if (!strIndex.Equals(""))
                    {
                        int temp = int.Parse(strIndex);
                        if (temp == prevIndex) continue;

                        if (temp == (prevIndex + 1))
                        {
                            if (index < temp)
                                index = temp;
                            prevIndex = temp;
                        }
                        else
                        {
                            index = prevIndex;
                            break;
                        }
                    }
                }
            }
            return index;
        }

        void Diagram_Click(object sender, EventArgs e)
        {
            if (this.ActiveDiagram.View.Model.Modified)
                UpdateMDIChildText();
        }

        private void barRemoveSymbol_Click(object sender, EventArgs e)
        {
            SymbolPalette paletteCur = symbolPaletteGroupBar.CurrentSymbolPalette;
            if (symbolPaletteGroupBar.SelectedNode == null)
                if (this.ActiveMdiChild != null)
                    symbolPaletteGroupBar.SelectNode(GetMappedNode(this.ActiveMdiChild as DiagramForm));

            // get selected palette node
            Node nodeSel = symbolPaletteGroupBar.SelectedNode;

            if (nodeSel == null)
            {
                MessageBox.Show("Please select a node to delete");
                return;
            }

            if (MessageBox.Show(string.Format(c_strMSG_REMOVE_SYMBOL, nodeSel.Name), Application.ProductName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (paletteCur != null && nodeSel != null)
                {
                    // Close MDI child window if open
                    if (this.NodeToDiagramMap.ContainsKey(nodeSel))
                    {
                        DiagramForm frm = this.NodeToDiagramMap[nodeSel] as DiagramForm;

                        if (frm != null)
                        {
                            frm.Diagram.Model.EndInit();
                            this.documentExplorer1.DetachModel(frm.Diagram.Model);
                            // unsubscribe from closed event first
                            frm.Closed -= new EventHandler(frmDgrm_Closed);
                            // close palette node editing diagram form
                            frm.Close();
                        }

                        this.NodeToDiagramMap.Remove(nodeSel);
                    }

                    // Remove symbol from the palette
                    paletteCur.RemoveChild(nodeSel);
                }
            }
        }

        private void frmDgrm_Closed(object sender, EventArgs e)
        {
            if (m_bSaving) return;

            DiagramForm frmDgm = sender as DiagramForm;

            if (frmDgm != null)
            {
                Group nodeToUpdate;
                // unsubscribe from Form.Closed event
                frmDgm.Closed -= new EventHandler(frmDgrm_Closed);

                //Unhook diagram events
                if (frmDgm.Diagram.Model != null)
                {
                    frmDgm.Diagram.Model.EventSink.PropertyChanged -=
                        new PropertyChangedEventHandler(OnDiagramPropertyChanged);
                    frmDgm.Diagram.Model.EventSink.LabelsChanged -= new CollectionExEventHandler(OnDiagramLabelsChanged);
                    frmDgm.Diagram.Model.EventSink.NodeCollectionChanged -= new CollectionExEventHandler(OnDiagramNodeCollectionChanged);
                    frmDgm.Diagram.Model.EventSink.LayersChanged -= new CollectionExEventHandler(OnDiagramLayersChanged);
                    frmDgm.Diagram.Model.EventSink.PinOffsetChanged -= new PinOffsetChangedEventHandler(OnDiagramPinOffsetChanged);
                    frmDgm.Diagram.Model.EventSink.PinPointChanged -= new PinPointChangedEventHandler(OnDiagramPinPointChanged);
                    frmDgm.Diagram.Model.EventSink.PortsChanged -= new CollectionExEventHandler(OnDiagramPortsChanged);
                    frmDgm.Diagram.Model.EventSink.RotationChanged -= new RotationChangedEventHandler(OnDiagramRotationChanged);
                    frmDgm.Diagram.Model.EventSink.SizeChanged -= new SizeChangedEventHandler(OnDiagramSizeChanged);
                    frmDgm.Diagram.Model.EventSink.VertexChanged -= new VertexChangedEventHandler(OnDiagramVertexChanged);
                    frmDgm.Diagram.Model.EventSink.ZOrderChanged -= new ZOrderChangedEventHandler(OnDiagramZOrderChanged);
                }

                // update node mapped to this diagram form
                // ---------------------------------------
                // get DiagramForm mapping
                nodeToUpdate = GetMappedNode(frmDgm) as Group;

                // update node with 
                if (nodeToUpdate != null)
                {
                    // get diagram nodes
                    NodeCollection nodes = (NodeCollection)frmDgm.Diagram.Model.Nodes.Clone();
                    // append nodes to palette node
                    nodeToUpdate = new Group(nodes);
                    // update node mapping
                    this.NodeToDiagramMap[nodeToUpdate] = null;
                }

                this.overviewControl1.DetachDiagram();
                this.documentExplorer1.DetachModel(frmDgm.Diagram.Model);
            }
        }
        private Node GetMappedNode(DiagramForm frmDgm)
        {
            Node nodeToUpdate = null;

            if (frmDgm != null)
            {
                foreach (DictionaryEntry entry in this.NodeToDiagramMap)
                {
                    if (frmDgm.Equals(entry.Value))
                    {
                        nodeToUpdate = entry.Key as Node;
                        break;
                    }
                }
            }

            return nodeToUpdate;
        }

        #region Track DiagramChanges

        void OnDiagramZOrderChanged(ZOrderChangedEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        void OnDiagramVertexChanged(VertexChangedEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        void OnDiagramSizeChanged(SizeChangedEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        void OnDiagramRotationChanged(RotationChangedEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        void OnDiagramPortsChanged(CollectionExEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        void OnDiagramPinPointChanged(PinPointChangedEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        void OnDiagramPinOffsetChanged(PinOffsetChangedEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        void OnDiagramLayersChanged(CollectionExEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        void OnDiagramNodeCollectionChanged(CollectionExEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        void OnDiagramLabelsChanged(CollectionExEventArgs evtArgs)
        {
            UpdateMDIChildText();
        }

        private void OnDiagramPropertyChanged(PropertyChangedEventArgs evtArgs)
        {
            Console.WriteLine("OnDiagramPropertyChanged");
            if (evtArgs.PropertyName == "Name" && evtArgs.NodeAffected is Model)
            {
                DiagramForm frmDgm;
                Model mdlTmp;

                // get mapped node
                foreach (DictionaryEntry entry in this.NodeToDiagramMap)
                {
                    // search for mapped node
                    frmDgm = entry.Value as DiagramForm;

                    if (frmDgm != null)
                    {
                        mdlTmp = frmDgm.Diagram.Model;

                        if (mdlTmp.Equals(evtArgs.NodeAffected))
                        {
                            // update mapped node name
                            ((Node)entry.Key).Name = mdlTmp.Name;
                            // update group view item name
                            PaletteGroupView pgv = symbolPaletteGroupBar.GroupBarItems[symbolPaletteGroupBar.SelectedItem].Client as PaletteGroupView;
                            foreach (GroupViewItem item in pgv.GroupViewItems)
                            {
                                Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView.GroupViewPaletteItem groupItem = item as Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView.GroupViewPaletteItem;
                                if (groupItem.Name == mdlTmp.Name)
                                    item.Text = mdlTmp.Name;
                            }
                            //pgv.GroupViewItems[ pgv.SelectedItem ].Text = mdlTmp.Name;

                            // upate diagram form text
                            frmDgm.Text = mdlTmp.Name;
                        }
                    }
                }
            }

            UpdateMDIChildText();
        }
        #endregion

        #endregion

        #region Actions
        private void barItemEditDelete_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
                if (diagramForm != null)
                {
                    diagramForm.Diagram.Controller.Delete();
                }
            }
        }

        private void barItemAlign_Click(object sender, EventArgs e)
        {
            switch (((Tools.XPMenus.BarItem)sender).ID)
            {
                case "AlignLeft":
                    this.ActiveDiagram.AlignLeft();
                    break;
                case "AlignCenter":
                    this.ActiveDiagram.AlignCenter();
                    break;
                case "AlignBottom":
                    this.ActiveDiagram.AlignBottom();
                    break;
                case "AlignMiddle":
                    this.ActiveDiagram.AlignMiddle();
                    break;
                case "AlignTop":
                    this.ActiveDiagram.AlignTop();
                    break;
                case "AlignRight":
                    this.ActiveDiagram.AlignRight();
                    break;
                default:
                    break;
            }

        }

        private void barItemFlip_Click(object sender, EventArgs e)
        {
            switch (((Tools.XPMenus.BarItem)sender).ID.ToLower())
            {
                case "fliphorizontally":
                    this.ActiveDiagram.FlipHorizontal();
                    break;
                case "flipvertically":
                    this.ActiveDiagram.FlipVertical();
                    break;
                case "flipboth":
                    this.ActiveDiagram.FlipHorizontal();
                    this.ActiveDiagram.FlipVertical();
                    break;
                default:
                    break;
            }

        }

        private void barItemGroupingGroup_Click(object sender, EventArgs e)
        {
            switch (((Tools.XPMenus.BarItem)sender).ID.ToLower())
            {
                case "group":
                    this.ActiveDiagram.Controller.Group();
                    break;
                case "ungroup":
                    this.ActiveDiagram.Controller.UnGroup();
                    break;
                default:
                    break;
            }
        }

        private void barItemOrder_Click(object sender, EventArgs e)
        {
            switch (((Tools.XPMenus.BarItem)sender).ID.ToLower())
            {
                case "bringtofront":
                    this.ActiveDiagram.Controller.BringToFront();
                    break;
                case "bringforward":
                    this.ActiveDiagram.Controller.BringForward();
                    break;
                case "sendbackward":
                    this.ActiveDiagram.Controller.SendBackward();
                    break;
                case "sendtoback":
                    this.ActiveDiagram.Controller.SendToBack();
                    break;
                default:
                    break;
            }

        }

        private void barItemRotate_Click(object sender, EventArgs e)
        {
            switch (((Tools.XPMenus.BarItem)sender).ID.ToLower())
            {
                case "rotateclock":
                    this.ActiveDiagram.Rotate(90);
                    break;
                case "rotateconter":
                    this.ActiveDiagram.Rotate(-90);
                    break;
                default:
                    break;
            }

        }

        private void barItemResize_Click(object sender, EventArgs e)
        {
            switch (((Tools.XPMenus.BarItem)sender).ID.ToLower())
            {
                case "spaceacross":
                    this.ActiveDiagram.SpaceAcross();
                    break;
                case "spacedown":
                    this.ActiveDiagram.SpaceDown();
                    break;
                case "samesize":
                    this.ActiveDiagram.SameSize();
                    break;
                case "sameheight":
                    this.ActiveDiagram.SameHeight();
                    break;
                case "samewidth":
                    this.ActiveDiagram.SameWidth();
                    break;
                default:
                    break;
            }
        }

        private void barItemLayout_Click(object sender, EventArgs e)
        {
            LayoutDialog dialog = new LayoutDialog(this.ActiveDiagram);
            dialog.Show();
        }
        #endregion

        private void DiagramForm_Closing(object sender, CancelEventArgs e)
        {
            DiagramForm docForm = sender as DiagramForm;
            if (docForm != null && docForm.Diagram.Model != null)
            {
                if (!e.Cancel)
                {
                    if (docForm.SaveChanges)
                    {
                        if (savePaletteDialog.FileName.Length > 0)
                        {
                            SymbolPalette palette = PreparePaletteToSave();

                            if (palette.Nodes.Count > 0)
                            {
                                SavePalette(palette, savePaletteDialog.FileName);
                                symbolPaletteGroupBar.Refresh();
                            }
                        }
                        else
                        {
                            SaveCurrentPaletteAs();
                        }
                    }
                    this.overviewControl1.DetachDiagram();
                    this.documentExplorer1.DetachModel(docForm.Diagram.Model);
                }
            }
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosePalette();
            if (cancelClosing)
                e.Cancel = true;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            SyncfusionLicenseProvider.ValidateLicense(Platform.Utility);
            //			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler( DefaultExceptionHandler.Singleton.OnThreadException );
            if (args.Length > 0)
            {
                Application.Run(new MainForm(args[0]));
            }
            else
                Application.Run(new MainForm());
            //			Application.ThreadException -= new System.Threading.ThreadExceptionEventHandler( DefaultExceptionHandler.Singleton.OnThreadException );

        }

    }
}
