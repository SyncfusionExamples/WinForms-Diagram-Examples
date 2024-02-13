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
          
            Syncfusion.Windows.Forms.Diagram.Rectangle rectangle1 = new Syncfusion.Windows.Forms.Diagram.Rectangle(250, 50, 100, 100);
            this.diagram1.Model.AppendChild(rectangle1);

            Syncfusion.Windows.Forms.Diagram.Rectangle rectangle2 = new Syncfusion.Windows.Forms.Diagram.Rectangle(350, 300, 100, 100);
            this.diagram1.Model.AppendChild(rectangle2);

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

            Syncfusion.Windows.Forms.Diagram.ConnectionPoint cp11 = new Syncfusion.Windows.Forms.Diagram.ConnectionPoint();
            cp11.VisualType = PortVisualType.CirclePort;
            cp11.FillStyle.Color = Color.Green;
            cp11.Position = Position.MiddleLeft;
            cp11.Visible = false;

            Syncfusion.Windows.Forms.Diagram.ConnectionPoint cp21 = new Syncfusion.Windows.Forms.Diagram.ConnectionPoint();
            cp21.VisualType = PortVisualType.CirclePort;
            cp21.FillStyle.Color = Color.Green;
            cp21.Position = Position.MiddleRight;
            cp21.Visible = false;

            Syncfusion.Windows.Forms.Diagram.ConnectionPoint cp31 = new Syncfusion.Windows.Forms.Diagram.ConnectionPoint();
            cp31.VisualType = PortVisualType.CirclePort;
            cp31.FillStyle.Color = Color.Green;
            cp31.Position = Position.TopCenter;
            cp31.Visible = false;

            Syncfusion.Windows.Forms.Diagram.ConnectionPoint cp41 = new Syncfusion.Windows.Forms.Diagram.ConnectionPoint();
            cp41.VisualType = PortVisualType.CirclePort;
            cp41.FillStyle.Color = Color.Green;
            cp41.Position = Position.BottomCenter;
            cp41.Visible = false;

            rectangle1.Ports.Add(cp1);
            rectangle1.Ports.Add(cp2);
            rectangle1.Ports.Add(cp3);
            rectangle1.Ports.Add(cp4);
            rectangle2.Ports.Add(cp11);
            rectangle2.Ports.Add(cp21);
            rectangle2.Ports.Add(cp31);
            rectangle2.Ports.Add(cp41);
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

                    x = x + (int)diagram1.Origin.X;
                    y = y + (int)diagram1.Origin.Y;

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


