using Syncfusion.UI.Xaml.Diagram;
using System.Windows;
using System.Windows.Controls;

namespace WPFDiagramLibrary
{
    /// <summary>
    /// Interaction logic for DiagramComponent.xaml
    /// </summary>
    public partial class DiagramComponent : UserControl
    {
        public DiagramComponent()
        {
            InitializeComponent();
          
        }
       
        public SfDiagram View
        {
            get
            {
                return this.diagram;
            }
        }
    }
}
