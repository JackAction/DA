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
    /// Interaction logic for MapElementProperties_UC.xaml
    /// </summary>
    public partial class MapElementProperties_UC : UserControl
    {
        public MapElementProperties_UC()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public static readonly DependencyProperty LayerChangedProperty =
            DependencyProperty.Register(
                "LayerChanged",
                typeof(ICommand),
                typeof(MapElementProperties_UC),
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
                typeof(MapElementProperties_UC),
                new UIPropertyMetadata(null));
        public ObservableCollection<Layer_Model> Layers
        {
            get { return (ObservableCollection<Layer_Model>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public static readonly DependencyProperty SelectedLayersProperty =
            DependencyProperty.Register(
                "SelectedLayers",
                typeof(ObservableCollection<Layer_Model>),
                typeof(MapElementProperties_UC),
                new UIPropertyMetadata(null));
        public ObservableCollection<Layer_Model> SelectedLayers
        {
            get { return (ObservableCollection<Layer_Model>)GetValue(SelectedLayersProperty); }
            set { SetValue(SelectedLayersProperty, value); }
        }

        public static readonly DependencyProperty DrawingAttributesProperty =
            DependencyProperty.Register(
                "DrawingAttributes",
                typeof(DrawingAttributes),
                typeof(MapElementProperties_UC),
                new UIPropertyMetadata(null));
        public DrawingAttributes DrawingAttributes
        {
            get { return (DrawingAttributes)GetValue(DrawingAttributesProperty); }
            set { SetValue(DrawingAttributesProperty, value); }
        }

        public static readonly DependencyProperty SelectedMemberPathProperty =
            DependencyProperty.Register(
                "SelectedMemberPath",
                typeof(string),
                typeof(MapElementProperties_UC),
                new UIPropertyMetadata(null));
        public string SelectedMemberPath
        {
            get { return (string)GetValue(SelectedMemberPathProperty); }
            set { SetValue(SelectedMemberPathProperty, value); }
        }

        public static readonly DependencyProperty ElementNameProperty =
            DependencyProperty.Register(
                "ElementName",
                typeof(string),
                typeof(MapElementProperties_UC),
                new UIPropertyMetadata(null));
        public string ElementName
        {
            get { return (string)GetValue(ElementNameProperty); }
            set { SetValue(ElementNameProperty, value); }
        }

        public static readonly DependencyProperty ElementDetailsProperty =
            DependencyProperty.Register(
                "ElementDetails",
                typeof(string),
                typeof(MapElementProperties_UC),
                new UIPropertyMetadata(null));
        public string ElementDetails
        {
            get { return (string)GetValue(ElementDetailsProperty); }
            set { SetValue(ElementDetailsProperty, value); }
        }

        #endregion
    }
}
