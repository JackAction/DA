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

        public static readonly DependencyProperty DeleteLayerProperty =
            DependencyProperty.Register(
                "DeleteLayer",
                typeof(ICommand),
                typeof(Left_UC),
                new UIPropertyMetadata(null));
        public ICommand DeleteLayer
        {
            get { return (ICommand)GetValue(DeleteLayerProperty); }
            set { SetValue(DeleteLayerProperty, value); }
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

        private void txtNameOfSelectedLayer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtNameOfSelectedLayer.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
        }

        private void btnAddLayer_Click(object sender, RoutedEventArgs e)
        {
            txtNameOfNewLayer.Text = "";
            txtNameOfNewLayer.Visibility = Visibility.Visible;
            //btnAddLayer.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            txtNameOfNewLayer.Focus();
        }

        private void txtNameOfNewLayer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtNameOfNewLayer.Visibility = Visibility.Hidden; 
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            txtNameOfSelectedLayer.Text = "";
        }

        private void checkListBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            txtNameOfSelectedLayer.Focus();
        }
    }
}
