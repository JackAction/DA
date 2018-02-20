using Microsoft.Win32;
using MVVM_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MainApplication
{
    public class MainWindow_ViewModel : ObservableObject
    {
        private readonly Campaign_Repository campaignRepository = new Campaign_Repository();

        public MainWindow_ViewModel()
        {
            CampaignVM = new Campaign_ViewModel();
            SelectedLayer_Workaround = new Layer_Model();
            DefaultDrawingAttributes = new DrawingAttributes();
            SelectedStroke = new Stroke(new StylusPointCollection(new StylusPoint[] { new StylusPoint(100, 100) }));
        }

        private Campaign_ViewModel _campaignVM;

        public Campaign_ViewModel CampaignVM
        {
            get { return _campaignVM; }
            set
            {
                if (_campaignVM == value)
                {
                    return;
                }
                _campaignVM = value;
                RaisePropertyChanged();
            }
        }

        #region Campaign Handling

        void SaveCampaign_Execute()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save Campaign";
            save.Filter = "xml files (*.xml)|*.xml";
            save.InitialDirectory = $"{Directory.GetCurrentDirectory()}";
            // Current directory nur zum Testen. SpecialFolder kann auf alles lustige zugreifen für release version
            //save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (save.ShowDialog() == true)
            {
                try
                {
                    campaignRepository.Save(CampaignVM.Campaign, save.FileName);
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message);
                }
            }
        }

        public RelayCommand SaveCampaign { get { return new RelayCommand(SaveCampaign_Execute); } }

        void LoadCampaign_Execute(object window)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Open Campaign";
            open.Filter = "xml files (*.xml)|*.xml";
            open.InitialDirectory = $"{Directory.GetCurrentDirectory()}";
            // Current directory nur zum Testen. SpecialFolder kann auf alles lustige zugreifen für release version
            //save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (open.ShowDialog() == true)
            {
                try
                {
                    CampaignVM = new Campaign_ViewModel() { Campaign = campaignRepository.Load(open.FileName) };
                    InkCanvas canvas = (InkCanvas)((MainWindow)window).FindName("content");
                    canvas.Children.Clear();
                    foreach (var poi in CampaignVM.Campaign.POIs)
                    {
                        AddPOIToCanvas(canvas, poi);
                    }
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message);
                }
            }
        }

        public RelayCommand<object> LoadCampaign { get { return new RelayCommand<object>(LoadCampaign_Execute); } }

        void CreateCampaign_Execute(object window)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Choose Background Picture";
            open.Filter = ImageFilterString.GetImageFilter();
            open.InitialDirectory = $"{Directory.GetCurrentDirectory()}";
            // Current directory nur zum Testen. SpecialFolder kann auf alles lustige zugreifen für release version
            //save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (open.ShowDialog() == true)
            {
                try
                {
                    // Name ist im moment noch fix. Sollte von Benutzer eingegeben werden können
                    CampaignVM = new Campaign_ViewModel() { Campaign = campaignRepository.Create("NewCampaign", open.FileName) };


                    // https://stackoverflow.com/questions/728005/mvvm-binding-to-inkcanvas
                    // Stokes müssen vorgegeben werden
                    CampaignVM.Strokes = new StrokeCollection();
                    (CampaignVM.Strokes as INotifyCollectionChanged).CollectionChanged += delegate { };

                    InkCanvas canvas = (InkCanvas)((MainWindow)window).FindName("content");
                    canvas.Children.Clear();
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message);
                }
            }
        }

        public RelayCommand<object> CreateCampaign { get { return new RelayCommand<object>(CreateCampaign_Execute); } }

        #endregion

        #region Layer Handling

        void DeleteLayers_Execute()
        {
            CampaignVM.Layers.Remove(SelectedLayer_Workaround);
        }

        public RelayCommand DeleteLayers { get { return new RelayCommand(DeleteLayers_Execute); } }

        void AddLayer_Execute(string name)
        {
            CampaignVM.Layers.Add(new Layer_Model() { Name = name, Guid = Guid.NewGuid() });
        }

        public RelayCommand<string> AddLayer { get { return new RelayCommand<string>(AddLayer_Execute); } }

        private ObservableCollection<Layer_Model> _layersForNewStroke = new ObservableCollection<Layer_Model>();
        public ObservableCollection<Layer_Model> LayersForNewStroke
        {
            get { return _layersForNewStroke; }
            set { _layersForNewStroke = value; }
        }

        void LayerChanged_Execute(Layer_Model layer)
        {
            CampaignVM.Campaign.ChangeLayerVisibility(layer);
            SelectedLayer_Workaround = layer;
        }

        public RelayCommand<Layer_Model> LayerChanged { get { return new RelayCommand<Layer_Model>(LayerChanged_Execute); } }

        private Layer_Model _selectedLayer_Workaround;

        public Layer_Model SelectedLayer_Workaround
        {
            get { return _selectedLayer_Workaround; }
            set
            {
                if (_selectedLayer_Workaround == value)
                {
                    return;
                }
                _selectedLayer_Workaround = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region StrokePropertyHandling

        private Stroke _selectedStroke;

        public Stroke SelectedStroke
        {
            get { return _selectedStroke; }
            set
            {
                if (_selectedStroke == value)
                {
                    return;
                }
                _selectedStroke = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Layer_Model> LayersOfSelectedStroke
        {
            get { return _layersOfSelectedStroke; }
            set { _layersOfSelectedStroke = value; }
        }

        private ObservableCollection<Layer_Model> _layersOfSelectedStroke = new ObservableCollection<Layer_Model>();

        //private void LayersOfSelectedStrokeChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    CampaignVM.Campaign.SetLayersOfStroke(SelectedStroke, e.);
        //}

        void LayersOfSelectedStrokeChanged_Execute(Layer_Model layer)
        {

            // ----> CanExecute nur wenn 
            if (pfuuuuiFlag)
            {
                CampaignVM.Campaign.SetLayersOfStroke(SelectedStroke, layer); 
            }
        }
        // -----> pfuuuuuuuiiiiiiii
        bool pfuuuuiFlag;


        public RelayCommand<Layer_Model> LayersOfSelectedStrokeChanged { get { return new RelayCommand<Layer_Model>(LayersOfSelectedStrokeChanged_Execute); } }
        // CanExecute FUnktioniert hier nicht wegen Framework

        public void StrokeSelected(object sender, InkCanvasSelectionChangingEventArgs e)
        {
            pfuuuuiFlag = false;
            StrokeCollection selectedStrokes = e.GetSelectedStrokes();
            var selectedPOIs = e.GetSelectedElements();

            if (selectedStrokes.Count != 0)
            {
                SelectedPOI = new POI_Model();
                // ----> first ist bullshit, sollte nur geschehen wenn nur 1 stroke selektiert ist. ansonsten fehler?
                SelectedStroke = selectedStrokes.First();
                // Set Layers
                LayersOfSelectedStroke = CampaignVM.Campaign.GetLayersOfStroke(SelectedStroke);
                //LayersOfSelectedStroke.CollectionChanged += new NotifyCollectionChangedEventHandler(LayersOfSelectedStrokeChanged);
                RaisePropertyChanged("LayersOfSelectedStroke");
                pfuuuuiFlag = true;

                // Set StrokeData
                StrokeDataOfSelectedElement = CampaignVM.Campaign.GetStrokeDataOfStroke(SelectedStroke);
                ElementNameForSelectedElement = StrokeDataOfSelectedElement.Name;
                ElementDetailsForSelectedElement = StrokeDataOfSelectedElement.Details;
            }

            if (selectedPOIs.Count != 0)
            {
                StrokeDataOfSelectedElement = new StrokeData_Model();

                SelectedPOI = (POI_Model)((Image)selectedPOIs.First()).Tag;

                LayersOfSelectedStroke = SelectedPOI.Layers;
                RaisePropertyChanged("LayersOfSelectedStroke");
                var x = CampaignVM.Campaign.Layers;
                ElementNameForSelectedElement = SelectedPOI.Name;
                ElementDetailsForSelectedElement = SelectedPOI.Details;
            }


        }


        private POI_Model _selectedPOI = new POI_Model();

        public POI_Model SelectedPOI
        {
            get { return _selectedPOI; }
            set
            {
                if (_selectedPOI == value)
                {
                    return;
                }
                _selectedPOI = value;
                RaisePropertyChanged();
            }
        }


        public void SelectionChanged(object sender, EventArgs e)
        {
            foreach (var layer in CampaignVM.Layers)
            {
                CampaignVM.Campaign.ChangeLayerVisibility(layer); 
            }
        }

        private StrokeData_Model _strokeDataOfSelectedElement = new StrokeData_Model();

        public StrokeData_Model StrokeDataOfSelectedElement
        {
            get { return _strokeDataOfSelectedElement; }
            set
            {
                if (_strokeDataOfSelectedElement == value)
                {
                    return;
                }
                _strokeDataOfSelectedElement = value;
                RaisePropertyChanged();
            }
        }

        private string _elementNameForSelectedElement;

        public string ElementNameForSelectedElement
        {
            get { return _elementNameForSelectedElement; }
            set
            {
                if (_elementNameForSelectedElement == value)
                {
                    return;
                }
                _elementNameForSelectedElement = value;
                StrokeDataOfSelectedElement.Name = value;
                SelectedPOI.Name = value;
                RaisePropertyChanged();
            }
        }

        private string _elementDetailsForSelectedElement;

        public string ElementDetailsForSelectedElement
        {
            get { return _elementDetailsForSelectedElement; }
            set
            {
                if (_elementDetailsForSelectedElement == value)
                {
                    return;
                }
                _elementDetailsForSelectedElement = value;
                StrokeDataOfSelectedElement.Details = value;
                SelectedPOI.Details = value;
                RaisePropertyChanged();
            }
        }

        private DrawingAttributes _defaultDrawingAttributes = new DrawingAttributes();

        public DrawingAttributes DefaultDrawingAttributes
        {
            get { return _defaultDrawingAttributes; }
            set
            {
                if (_defaultDrawingAttributes == value)
                {
                    return;
                }
                _defaultDrawingAttributes = value;
                RaisePropertyChanged();
            }
        }

        public void StrokeAdded(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            // Layer hinzufügen
            foreach (var layer in LayersForNewStroke)
            {
                e.Stroke.AddPropertyData(layer.Guid, layer.Name);
            }

            // StrokeData hinzufügen
            StrokeData_Model newStrokeData = new StrokeData_Model() { Id = Guid.NewGuid(), Name = ElementNameForNewElement, Details = ElementDetailsForNewElement };
            CampaignVM.Campaign.StrokeDataList.Add(newStrokeData);
            e.Stroke.AddPropertyData(newStrokeData.Id, "Id");
        }

        private string _elementNameForNewElement;

        public string ElementNameForNewElement
        {
            get { return _elementNameForNewElement; }
            set
            {
                if (_elementNameForNewElement == value)
                {
                    return;
                }
                _elementNameForNewElement = value;
                RaisePropertyChanged();
            }
        }

        private string _elementDetailsForNewElement;

        public string ElementDetailsForNewElement
        {
            get { return _elementDetailsForNewElement; }
            set
            {
                if (_elementDetailsForNewElement == value)
                {
                    return;
                }
                _elementDetailsForNewElement = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region POI

        public void AddPOI(object sender, MouseButtonEventArgs e)
        {
            var canvas = (InkCanvas)sender;

            var mouseDownPoint = e.GetPosition(canvas);


            PictureDimension pictureDimension = new PictureDimension(@"C:\Users\JackAction\Documents\DA\CampaignMap\MainApplication\Pin.png");
            var x = mouseDownPoint.Y - pictureDimension.Height;
            var y = mouseDownPoint.X;


            POI_Model poi = new POI_Model() { Name = ElementNameForNewElement, Details = ElementDetailsForNewElement, PositionTop = x, PositionLeft = y };
            // Layer hinzufügen
            foreach (var layer in LayersForNewStroke)
            {
                poi.Layers.Add(layer);
            }

            CampaignVM.Campaign.POIs.Add(poi);


            AddPOIToCanvas(canvas, poi);


        }

        private void AddPOIToCanvas(InkCanvas canvas, POI_Model poi)
        {
            Image image = new Image
            {
                //Width = 100, // Um Pin kleiner zu machen
                Source = new BitmapImage(new Uri(@"Pin.png", UriKind.Relative)),
                Tag = poi

            };
            InkCanvas.SetTop(image, poi.PositionTop);
            InkCanvas.SetLeft(image, poi.PositionLeft);

            Binding myBinding = new Binding("IsEnabled");
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.Converter = new BooleanToVisibilityConverter();
            myBinding.Source = poi;
            image.SetBinding(UIElement.VisibilityProperty, myBinding);

            canvas.Children.Add(image);
        }

        #endregion

    }
}
