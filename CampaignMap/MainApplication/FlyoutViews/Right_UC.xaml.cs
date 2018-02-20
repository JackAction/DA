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

        public static readonly DependencyProperty EraseModeProperty =
            DependencyProperty.Register(
                "EraseMode",
                typeof(ICommand),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public ICommand EraseMode
        {
            get { return (ICommand)GetValue(EraseModeProperty); }
            set { SetValue(EraseModeProperty, value); }
        }

        public static readonly DependencyProperty InputModeProperty =
            DependencyProperty.Register(
                "InputMode",
                typeof(ICommand),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public ICommand InputMode
        {
            get { return (ICommand)GetValue(InputModeProperty); }
            set { SetValue(InputModeProperty, value); }
        }

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

        public double DefaultDrawingAttributesWidthHeight
        {
            get { return DefaultDrawingAttributes.Width; }
            set { DefaultDrawingAttributes.Width = value; DefaultDrawingAttributes.Height = value; }
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

        public static readonly DependencyProperty ElementNameForNewElementProperty =
            DependencyProperty.Register(
                "ElementNameForNewElement",
                typeof(string),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public string ElementNameForNewElement
        {
            get { return (string)GetValue(ElementNameForNewElementProperty); }
            set { SetValue(ElementNameForNewElementProperty, value); }
        }

        public static readonly DependencyProperty ElementDetailsForNewElementProperty =
            DependencyProperty.Register(
                "ElementDetailsForNewElement",
                typeof(string),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public string ElementDetailsForNewElement
        {
            get { return (string)GetValue(ElementDetailsForNewElementProperty); }
            set { SetValue(ElementDetailsForNewElementProperty, value); }
        }

        public static readonly DependencyProperty ElementNameOfSelectedElementProperty =
            DependencyProperty.Register(
                "ElementNameOfSelectedElement",
                typeof(string),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public string ElementNameOfSelectedElement
        {
            get { return (string)GetValue(ElementNameOfSelectedElementProperty); }
            set { SetValue(ElementNameOfSelectedElementProperty, value); }
        }

        public static readonly DependencyProperty ElementDetailsOfSelectedElementProperty =
            DependencyProperty.Register(
                "ElementDetailsOfSelectedElement",
                typeof(string),
                typeof(Right_UC),
                new UIPropertyMetadata(null));
        public string ElementDetailsOfSelectedElement
        {
            get { return (string)GetValue(ElementDetailsOfSelectedElementProperty); }
            set { SetValue(ElementDetailsOfSelectedElementProperty, value); }
        }

        #endregion

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
