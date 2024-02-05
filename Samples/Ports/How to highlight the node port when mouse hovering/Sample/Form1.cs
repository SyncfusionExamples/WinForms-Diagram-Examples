using Syncfusion.Windows.Forms.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HighLightPortSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();

            diagram1.MouseMove += diagram_MouseMove;

            //Events for node/connectors selection, deselection, addition, removing
            diagram1.EventSink.NodeCollectionChanged += EventSink_NodeCollectionChanged;
            diagram1.EventSink.NodeSelected += EventSink_NodeSelected;
            diagram1.EventSink.SelectionListChanged += EventSink_SelectionListChanged;
            diagram1.EventSink.NodeDeselected += EventSink_NodeDeselected;


            Syncfusion.Windows.Forms.Diagram.Rectangle rectangle = new Syncfusion.Windows.Forms.Diagram.Rectangle(250, 50, 100, 100);
            this.diagram1.Model.AppendChild(rectangle);

            Syncfusion.Windows.Forms.Diagram.ConnectionPoint cp1 = new Syncfusion.Windows.Forms.Diagram.ConnectionPoint();
            cp1.VisualType = PortVisualType.CirclePort;
            cp1.FillStyle.Color = Color.Green;
            cp1.Position = Position.MiddleLeft;
            cp1.Visible = false;
            Syncfusion.Windows.Forms.Diagram.ConnectionPoint cp2 = new Syncfusion.Windows.Forms.Diagram.ConnectionPoint();
            cp2.VisualType = PortVisualType.CirclePort;
            cp2.FillStyle.Color = Color.Green;
            cp2.Position = Position.MiddleRight;
            cp2.Visible = false;

            Syncfusion.Windows.Forms.Diagram.ConnectionPoint cp3 = new Syncfusion.Windows.Forms.Diagram.ConnectionPoint();
            cp3.VisualType = PortVisualType.CirclePort;
            cp3.FillStyle.Color = Color.Green;
            cp3.Position = Position.TopCenter;
            cp3.Visible = false;

            Syncfusion.Windows.Forms.Diagram.ConnectionPoint cp4 = new Syncfusion.Windows.Forms.Diagram.ConnectionPoint();
            cp4.VisualType = PortVisualType.CirclePort;
            cp4.FillStyle.Color = Color.Green;
            cp4.Position = Position.BottomCenter;
            cp4.Visible = false;

            rectangle.Ports.Add(cp1);
            rectangle.Ports.Add(cp2);
            rectangle.Ports.Add(cp3);
            rectangle.Ports.Add(cp4);
        }

        Node globalNode;
        public bool MouseLeft;

        //Method to highlight the port when mouse hover on it.
        private void diagram_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Node node1 = (Node)this.diagram1.Controller.GetNodeUnderMouse(new Point(e.X, e.Y));
                if (node1 != null)
                {
                    globalNode = node1;
                    for (int i = 0; i < globalNode.Ports.Count; i++)
                        globalNode.Ports[i].Visible = true;

                    int x = e.X - (int)node1.BoundingRectangle.X;
                    int y = e.Y - (int)node1.BoundingRectangle.Y;
                    if (diagram1.ShowRulers) { x -= 20; y -= 20; }
                    if (e.Button == MouseButtons.None)
                    {
                        for (int i = 0; i < node1.Ports.Count; i++)
                        {
                            if (Math.Abs(x - node1.Ports[i].GetPosition().X) < 15 && Math.Abs(y - node1.Ports[i].GetPosition().Y) < 15)
                            {
                                node1.Ports[i].Size = 14;
                                if ("OrgLineConnectorTool" != diagram1.Controller.ActiveTool.Name)
                                {
                                    diagram1.Controller.ActivateTool("OrgLineConnectorTool"); //"SelectTool" "OrgLineConnectorTool"
                                    break;
                                }
                            }
                            else
                            {
                                node1.Ports[i].Size = 10;
                                if ("OrgLineConnectorTool" == diagram1.Controller.ActiveTool.Name)
                                    diagram1.Controller.ActivateTool("SelectTool");
                            }
                        }
                    }
                    diagram1.Refresh();
                }
                else
                {
                    if (globalNode != null)
                    {
                        for (int i = 0; i < globalNode.Ports.Count; i++)
                            globalNode.Ports[i].Visible = false;
                        globalNode = null;
                        if (e.Button == MouseButtons.None) diagram1.Controller.ActivateTool("SelectTool");
                        diagram1.Refresh();
                    }
                }
            }
            catch
            {
            }
        }

        //Event to notify when node and connector is deselected
        private void EventSink_NodeDeselected(Syncfusion.Windows.Forms.Diagram.NodeSelectedEventArgs evtArgs)
        {
        }
        //Event to notify the selected items list when node/connector is selected or deselected
        private void EventSink_SelectionListChanged(Syncfusion.Windows.Forms.Diagram.CollectionExEventArgs evtArgs)
        {
        }
        //Event to notify when node and connector is selected
        private void EventSink_NodeSelected(Syncfusion.Windows.Forms.Diagram.NodeSelectedEventArgs evtArgs)
        {
        }
        //Event to notify when node and connector is added or removed
        private void EventSink_NodeCollectionChanged(Syncfusion.Windows.Forms.Diagram.CollectionExEventArgs evtArgs)
        {
        }
    }
}


