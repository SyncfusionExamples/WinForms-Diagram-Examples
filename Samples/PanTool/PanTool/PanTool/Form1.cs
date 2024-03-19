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
using Rectangle = Syncfusion.Windows.Forms.Diagram.Rectangle;

namespace PanTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

           
            this.diagram1.EventSink.NodeMouseEnter += EventSink_NodeMouseEnter;
            this.diagram1.EventSink.NodeMouseLeave += EventSink_NodeMouseLeave;
            this.CreateRectangle(200, 150);
        }

        private void EventSink_NodeMouseLeave(NodeMouseEventArgs evtArgs)
        {
            this.diagram1.Controller.ActivateTool("PanTool");
        }

        private void EventSink_NodeMouseEnter(NodeMouseEventArgs evtArgs)
        {
            this.diagram1.Controller.ActivateTool("SelectTool");
        }

        private void CreateRectangle(float x, float y)
        {
            var rectangle = new Rectangle(x, y, 100, 100);
            rectangle.FillStyle.Type = FillStyleType.Solid;
            rectangle.FillStyle.Color = Color.CornflowerBlue;
            rectangle.EnableCentralPort = true;

            var rectangle1 = new Rectangle(x + 50, y + 200, 100, 100);
            rectangle1.FillStyle.Type = FillStyleType.Solid;
            rectangle1.FillStyle.Color = Color.CornflowerBlue;
            rectangle1.EnableCentralPort = true;

            #region CentralPort
            var port1 = new ConnectionPoint()
            {
                //VisualType = PortVisualType.SquarePort,
                AllowConnectOnDrag = true,
                OffsetX = 0,
                OffsetY = 0,
            };
            port1.FillStyle.Type = FillStyleType.Solid;
            port1.FillStyle.Color = Color.Black;
            rectangle.Ports.Add(port1);

            var port = new ConnectionPoint()
            {
                //VisualType = PortVisualType.SquarePort,
                AllowConnectOnDrag = true,
                OffsetX = 0,
                OffsetY = 0,
            };
            port.FillStyle.Type = FillStyleType.Solid;
            port.FillStyle.Color = Color.Black;
            rectangle1.Ports.Add(port);
            #endregion
            OrgLineConnector connector = new OrgLineConnector(new PointF(0, 0), new PointF(0, 0));



            this.diagram1.Model.AppendChild(rectangle);
            this.diagram1.Model.AppendChild(rectangle1);
            rectangle1.CentralPort.TryConnect(connector.HeadEndPoint);
            rectangle.CentralPort.TryConnect(connector.TailEndPoint);
            rectangle1.CentralPort.ConnectionsLimit = 2;

            diagram1.Model.AppendChild(connector);

            var custRect = new Rectangle(x + 200, y, 100, 100);
            custRect.FillStyle.Type = FillStyleType.Solid;
            custRect.FillStyle.Color = Color.CornflowerBlue;
            custRect.EnableCentralPort = true;

           

            var custRect2 = new Rectangle(x + 300, y + 200, 100, 100);
            custRect2.FillStyle.Type = FillStyleType.Solid;
            custRect2.FillStyle.Color = Color.CornflowerBlue;
            custRect2.EnableCentralPort = true;

            
            diagram1.Model.AppendChild(custRect);
            diagram1.Model.AppendChild(custRect2);
        }
    }
}
