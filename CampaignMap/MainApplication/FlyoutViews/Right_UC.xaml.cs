using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainApplication
{
    /// <summary>
    /// Interaction logic for Right_UC.xaml
    /// </summary>
    public partial class Right_UC : UserControl
    {
        public Right_UC()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public static readonly DependencyProperty LayerChangedProperty =
            DependencyProperty.Register(
                "LayerChanged",
                typeof(ICommand),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public ICommand LayerChanged
        {
            get { return (ICommand)GetValue(LayerChangedProperty); }
            set { SetValue(LayerChangedProperty, value); }
        }

        public static readonly DependencyProperty LayersProperty =
            DependencyProperty.Register(
                "Layers",
                typeof(ObservableCollection<Layer_Model>),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public ObservableCollection<Layer_Model> Layers
        {
            get { return (ObservableCollection<Layer_Model>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public static readonly DependencyProperty LayersForNewStrokeProperty =
            DependencyProperty.Register(
                "LayersForNewStroke",
                typeof(ObservableCollection<Layer_Model>),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public ObservableCollection<Layer_Model> LayersForNewStroke
        {
            get { return (ObservableCollection<Layer_Model>)GetValue(LayersForNewStrokeProperty); }
            set { SetValue(LayersForNewStrokeProperty, value); }
        }

        public static readonly DependencyProperty DefaultDrawingAttributesProperty =
            DependencyProperty.Register(
                "DefaultDrawingAttributes",
                typeof(DrawingAttributes),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public DrawingAttributes DefaultDrawingAttributes
        {
            get { return (DrawingAttributes)GetValue(DefaultDrawingAttributesProperty); }
            set { SetValue(DefaultDrawingAttributesProperty, value); }
        }

        public static readonly DependencyProperty LayersOfSelectedStrokeProperty =
            DependencyProperty.Register(
                "LayersOfSelectedStroke",
                typeof(ObservableCollection<Layer_Model>),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public ObservableCollection<Layer_Model> LayersOfSelectedStroke
        {
            get { return (ObservableCollection<Layer_Model>)GetValue(LayersOfSelectedStrokeProperty); }
            set { SetValue(LayersOfSelectedStrokeProperty, value); }
        }

        public static readonly DependencyProperty DrawingAttributesOfSelectedStrokeProperty =
            DependencyProperty.Register(
                "DrawingAttributesOfSelectedStroke",
                typeof(DrawingAttributes),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public DrawingAttributes DrawingAttributesOfSelectedStroke
        {
            get { return (DrawingAttributes)GetValue(DrawingAttributesOfSelectedStrokeProperty); }
            set { SetValue(DrawingAttributesOfSelectedStrokeProperty, value); }
        }

        #endregion
    }
}
