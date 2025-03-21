using Syncfusion.UI.Xaml.Diagram;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using WPFDiagramLibrary;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            
            this.CreateDiagramNode();
            
        }

        //This method is used for creating a Node using the WPF
        private void CreateDiagramNode()
        {
            SfDiagram diagram = (this.elementHost2.Child as DiagramComponent).View;
            diagram.Connectors = new ConnectorCollection();
            (diagram.Info as IGraphInfo).ItemAdded += MainWindow_ItemAdded;
            NodeViewModel node = new NodeViewModel()
            {
                OffsetX = 200,
                OffsetY = 200,
                UnitHeight = 100,
                UnitWidth = 100,
                Shape = new RectangleGeometry() { Rect = new Rect(100, 100, 100, 100) }
            };
            (diagram.Nodes as NodeCollection).Add(node);
            NodeViewModel node1 = new NodeViewModel()
            {
                OffsetX = 300,
                OffsetY = 400,
                UnitHeight = 100,
                UnitWidth = 100,
                Shape = new RectangleGeometry() { Rect = new Rect(100, 100, 100, 100) }


            };

            (diagram.Nodes as NodeCollection).Add(node1);
            NodeViewModel node2 = new NodeViewModel()
            {
                OffsetX = 600,
                OffsetY = 400,
                UnitHeight = 100,
                UnitWidth = 100,
                Shape = new RectangleGeometry() { Rect = new Rect(100, 100, 100, 100) }


            };

            (diagram.Nodes as NodeCollection).Add(node2);
            NodeViewModel node3 = new NodeViewModel()
            {
                OffsetX = 600,
                OffsetY = 600,
                UnitHeight = 100,
                UnitWidth = 100,
                Shape = new RectangleGeometry() { Rect = new Rect(100, 100, 100, 100) }


            };

            (diagram.Nodes as NodeCollection).Add(node3);

            ConnectorViewModel nodeToNodeConnection = new ConnectorViewModel()
            {
                SourceNode = node,
                TargetNode = node1,


            };

            //Adding connector into Collection
            (diagram.Connectors as ConnectorCollection).Add(nodeToNodeConnection);
            ConnectorViewModel nodeToNodeConnection1 = new ConnectorViewModel()
            {
                SourceNode = node2,
                TargetPoint = new Point(400, 400)



            };

            //Adding connector into Collection
            (diagram.Connectors as ConnectorCollection).Add(nodeToNodeConnection1);
            ConnectorViewModel nodeToNodeConnection2 = new ConnectorViewModel()
            {
                SourcePoint = new Point(800, 800),
                TargetNode = node3,


            };

            //Adding connector into Collection
            (diagram.Connectors as ConnectorCollection).Add(nodeToNodeConnection2);
          
        }
        private void MainWindow_ItemAdded(object sender, ItemAddedEventArgs args)
        {
            if (args.Item is ConnectorViewModel)
            {
                if ((args.Item as ConnectorViewModel).SourceNode != null)
                {
                    (args.Item as ConnectorViewModel).Constraints = (args.Item as ConnectorViewModel).Constraints.Remove(ConnectorConstraints.SourceDraggable);
                }
                if ((args.Item as ConnectorViewModel).TargetNode != null)
                {
                    (args.Item as ConnectorViewModel).Constraints = (args.Item as ConnectorViewModel).Constraints.Remove(ConnectorConstraints.TargetDraggable);
                }
            }
        }

    }
}
