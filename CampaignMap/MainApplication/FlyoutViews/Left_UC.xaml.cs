using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace MainApplication
{
    /// <summary>
    /// Interaction logic for Left_UC.xaml
    /// </summary>
    public partial class Left_UC : UserControl
    {
        public Left_UC()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DeleteLayersProperty =
            DependencyProperty.Register(
                "DeleteLayers",
                typeof(ICommand),
                typeof(Left_UC),
                new UIPropertyMetadata(null));
        public ICommand DeleteLayers
        {
            get { return (ICommand)GetValue(DeleteLayersProperty); }
            set { SetValue(DeleteLayersProperty, value); }
        }

        public static readonly DependencyProperty AddLayerProperty =
            DependencyProperty.Register(
                "AddLayer",
                typeof(ICommand),
                typeof(Left_UC),
                new UIPropertyMetadata(null));
        public ICommand AddLayer
        {
            get { return (ICommand)GetValue(AddLayerProperty); }
            set { SetValue(AddLayerProperty, value); }
        }
    }
}
