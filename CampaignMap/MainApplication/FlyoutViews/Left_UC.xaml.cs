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

        #region Dependency Properties

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

        public static readonly DependencyProperty LayerChangedProperty =
            DependencyProperty.Register(
                "LayerChanged",
                typeof(ICommand),
                typeof(Left_UC),
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
                typeof(Left_UC),
                new UIPropertyMetadata(null));
        public ObservableCollection<Layer_Model> Layers
        {
            get { return (ObservableCollection<Layer_Model>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public static readonly DependencyProperty ActiveLayersProperty =
            DependencyProperty.Register(
                "ActiveLayers",
                typeof(ObservableCollection<Layer_Model>),
                typeof(Left_UC),
                new UIPropertyMetadata(null));
        public ObservableCollection<Layer_Model> ActiveLayers
        {
            get { return (ObservableCollection<Layer_Model>)GetValue(ActiveLayersProperty); }
            set { SetValue(ActiveLayersProperty, value); }
        }

        public static readonly DependencyProperty SelectedLayer_WorkaroundProperty =
            DependencyProperty.Register(
                "SelectedLayer_Workaround",
                typeof(Layer_Model),
                typeof(Left_UC),
                new UIPropertyMetadata(null));
        public Layer_Model SelectedLayer_Workaround
        {
            get { return (Layer_Model)GetValue(SelectedLayer_WorkaroundProperty); }
            set { SetValue(SelectedLayer_WorkaroundProperty, value); }
        }

        #endregion

        private void editName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                editName.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            addName.Text = "";
            addName.Visibility = Visibility.Visible;
        }

        private void addName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                addName.Visibility = Visibility.Hidden; 
            }
        }
    }
}
