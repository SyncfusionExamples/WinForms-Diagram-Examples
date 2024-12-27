#region Copyright Syncfusion Inc. 2001-2022.
// Copyright Syncfusion Inc. 2001-2022. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Diagram;
using System.IO;
using System.Drawing.Drawing2D;
using Syncfusion.Windows.Forms.Diagram.Controls;
using Syncfusion.Windows.Forms.Tools;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Drawing.Imaging;
using Syncfusion.SVG.IO;
using Syncfusion.Windows.Forms;

namespace FlowDiagram_2005
{
    public partial class MainForm : Form
    {
        #region Members
        Syncfusion.Windows.Forms.Diagram.Label lbl;
        public string fileName;
        public bool isDragDrop = false;
        #endregion

        #region Form Initialize
        public MainForm()
        {
            InitializeComponent();
#if NET6_0_OR_GREATER
            paletteGroupBar1.LoadPalette(@"..\..\..\Flowchart Symbols.edp");
#else
            paletteGroupBar1.LoadPalette(@"..\..\Flowchart Symbols.edp");
#endif
            this.diagram1.BeginUpdate();
            this.diagram1.Model.RenderingStyle.SmoothingMode = SmoothingMode.HighQuality;
            this.diagram1.Model.BoundaryConstraintsEnabled = false;
            diagram1.Model.HistoryManager.Pause();
            InitailizeDiagram();
            diagram1.Model.HistoryManager.Resume();
            this.diagram1.View.SelectionList.Clear();
            GroupBarAppearance();
            this.diagram1.EndUpdate();
        }
        #endregion

        #region Initailize Diagram with events.
        /// <summary>
        /// Initialize the diagram
        /// </summary>
        protected void InitailizeDiagram()
        {
            diagram1.DragDrop += Diagram1_DragDrop;
            diagram1.EventSink.NodeCollectionChanged += EventSink_NodeCollectionChanged;
        }

        private void EventSink_NodeCollectionChanged(CollectionExEventArgs evtArgs)
        {
            if (isDragDrop)
            {
                isDragDrop = false;
                Node droppedNode = evtArgs.Element as Node;
                NodeCollection intersectingConnectors = this.GetNodesAtPoint(diagram1.Model.Nodes, droppedNode.BoundingRectangle);
                if (intersectingConnectors.First != null && intersectingConnectors.First is Line)
                {
                    Line hitConnectorLine = intersectingConnectors.First as Line;
                    Node hitConnectorTargetNode = hitConnectorLine.FromNode as Node;
                    droppedNode.CentralPort.TryConnect(hitConnectorLine.TailEndPoint);

                    LineConnector lineconnector = new LineConnector(new System.Drawing.PointF(10, 200), new System.Drawing.PointF(300, 250));
                    droppedNode.CentralPort.TryConnect(lineconnector.HeadEndPoint);
                    hitConnectorTargetNode.CentralPort.TryConnect(lineconnector.TailEndPoint);
                    this.diagram1.Model.AppendChild(lineconnector);
                }
            }
        }


        private void Diagram1_DragDrop(object sender, DragEventArgs e)
        {
            isDragDrop = true;
        }

            
        public NodeCollection GetNodesAtPoint(NodeCollection nodes, RectangleF recbBounding)
        {
            if (nodes == null)
                throw new ArgumentNullException("nodes");

            NodeCollection nodesToReturn = new NodeCollection();
            RectangleF rectNodeBounding;

            foreach (Node node in nodes)
            {
                rectNodeBounding = ((IUnitIndependent)node).GetBoundingRectangle(MeasureUnits.Pixel, false);
                if ((recbBounding.IntersectsWith(rectNodeBounding) || rectNodeBounding.IntersectsWith(recbBounding)))
                    nodesToReturn.Add(node);
            }

            return nodesToReturn;
        }

        /// <summary>
        /// Insert the node
        /// </summary>
        /// <param name="strText">TextNode's text</param>
        /// <param name="ptPinPoint">Node's Location </param>
        /// <param name="nodeSize">Node's size</param>
        /// <returns>returns the node</returns>
        private TextNode InsertTextNode(string strText, PointF ptPinPoint, SizeF nodeSize)
        {
            Syncfusion.Windows.Forms.Diagram.TextNode txtnode = new TextNode(strText);

            txtnode.FontStyle.Size = 10;
            txtnode.FontStyle.Family = "Arial";
            txtnode.FontColorStyle.Color = Color.Black;
            txtnode.LineStyle.LineColor = Color.Transparent;
            txtnode.Size = nodeSize;

            InsertNode(txtnode, ptPinPoint);
            return txtnode;
        }

        /// <summary>
        /// Inserts the node
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="ptPinPoint">Node's Location</param>
        /// <returns>returns the node</returns>
        private Node InsertNode(Node node, PointF ptPinPoint)
        {
            MeasureUnits units = MeasureUnits.Pixel;
            SizeF szPinOffset = ((IUnitIndependent)node).GetPinPointOffset(units);
            ptPinPoint.X += szPinOffset.Width;
            ptPinPoint.Y += szPinOffset.Height;
            ((IUnitIndependent)node).SetPinPoint(ptPinPoint, units);
            node.EnableCentralPort = true;
            this.diagram1.Model.AppendChild(node);
            return node;
        }

        /// <summary>
        /// Insert the node from palette
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="ptPinPoint">Node location</param>
        /// <param name="label">Label's text</param>
        /// <param name="nodeSize">Node size</param>
        /// <returns>returns the node</returns>
        private Node InsertNodeFromPallete(string name, PointF ptPinPoint, string label, SizeF nodeSize)
        {
            Node node = null;
            NodeCollection nodes = paletteGroupBar1.CurrentSymbolPalette.Nodes;

            if (name != null)
            {
                node = (Node)nodes[name].Clone();
                InsertNode(node, ptPinPoint);
            }

            //Set Node's size
            node.Size = nodeSize;
            //Add labels to node
            lbl = new Syncfusion.Windows.Forms.Diagram.Label(node, label);
            lbl.FontStyle.Family = "Arial";
            ((PathNode)node).Labels.Add(lbl);

            return node;
        }

        /// <summary>
        /// Connects the node
        /// </summary>
        /// <param name="parentNode">parent node</param>
        /// <param name="subNode">child node</param>
        private void ConnectNodes(Node parentNode, Node subNode)
        {
            if (parentNode.CentralPort == null || subNode.CentralPort == null)
                return;

            this.diagram1.Model.BeginUpdate();
            // Create directed link
            PointF[] pts = new PointF[] { PointF.Empty, new PointF(0, 1) };
            OrthogonalConnector line = new OrthogonalConnector(pts[0], pts[1]);
            line.HeadDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
            line.HeadDecorator.FillStyle.Color = Color.Black;
            line.LineStyle.LineColor = Color.Black;
            this.diagram1.Model.AppendChild(line);
            //Connect connectors's end points to the node ports. 
            parentNode.CentralPort.TryConnect(line.TailEndPoint);
            subNode.CentralPort.TryConnect(line.HeadEndPoint);
            this.diagram1.Model.SendToBack(line);
            this.diagram1.Model.EndUpdate();
        }

        /// <summary>
        /// Connect the nodes
        /// </summary>
        /// <param name="parentNode">parent node</param>
        /// <param name="position">node's position</param>
        private void ConnectNodes(Node parentNode, string position)
        {
            if (parentNode.CentralPort == null)
                return;
            PointF[] pts;
            OrthogonalConnector line;
            this.diagram1.Model.BeginUpdate();

            // Create directed link
            if (position == "right")
            {
                pts = new PointF[] { parentNode.PinPoint, new PointF(parentNode.PinPoint.X + 200, parentNode.PinPoint.Y) };
                line = new OrthogonalConnector(pts[0], pts[1]);
            }
            else
            {
                pts = new PointF[] { parentNode.PinPoint, new PointF(parentNode.PinPoint.X - 150, parentNode.PinPoint.Y + 220) };
                line = new OrthogonalConnector(pts[0], pts[1], true);
            }
            line.HeadDecorator.DecoratorShape = DecoratorShape.Filled45Arrow;
            line.HeadDecorator.FillStyle.Color = Color.Black;
            line.LineStyle.LineColor = Color.Black;

            this.diagram1.Model.AppendChild(line);

            parentNode.CentralPort.TryConnect(line.TailEndPoint);
            this.diagram1.Model.SendToBack(line);

            this.diagram1.Model.EndUpdate();
        }
        #endregion

        #region ChangeApperance of controls
        /// <summary>
        /// Change's the appearance of GroupBar
        /// </summary>
        private void GroupBarAppearance()
        {
            this.paletteGroupBar1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#119EDA");
            foreach (GroupBarItem item in paletteGroupBar1.GroupBarItems)
            {
                //palette view settings
                if (item.Client is PaletteGroupView)
                {
                    PaletteGroupView view = item.Client as PaletteGroupView;
                    view.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                    view.ForeColor = Color.Black;
                    view.FlowView = true;
                    view.ShowToolTips = true;
                    view.ShowFlowViewItemText = true;
                    view.SelectedItemColor = Color.FromArgb(255, 219, 118);
                    view.HighlightItemColor = Color.FromArgb(255, 227, 149);
                    view.SelectingItemColor = Color.FromArgb(255, 238, 184);
                    view.SelectedHighlightItemColor = Color.FromArgb(255, 218, 115);
                    view.FlowViewItemTextLength = (int)DpiAware.LogicalToDeviceUnits(80);
                    view.BackColor = Color.White;
                    view.TextWrap = true;
                    view.FlatLook = true;
                    view.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        #endregion
        //To draw a connector.
        private void button1_Click(object sender, EventArgs e)
        {
            this.diagram1.Controller.ActivateTool("LineTool");
        }
    }
}
