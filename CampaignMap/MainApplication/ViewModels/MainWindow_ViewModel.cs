using Microsoft.Win32;
using MVVM_Framework;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MainApplication
{
    public class MainWindow_ViewModel : ObservableObject
    {
        public MainWindow_ViewModel()
        {
            campaignRepository = new Campaign_Repository();
            CampaignVM = new Campaign_ViewModel();
            SelectedLayer_Workaround = new Layer_Model();
            DrawingAttributesForNewStroke = new DrawingAttributes();
            SelectedStroke = new Stroke(new StylusPointCollection(new StylusPoint[] { new StylusPoint(100, 100) }));
        }

        #region ApplicationState

        private bool _isACampaignOpen;

        public bool IsACampaignOpen
        {
            get { return _isACampaignOpen; }
            set
            {
                if (_isACampaignOpen == value)
                {
                    return;
                }
                _isACampaignOpen = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Campaign Handling

        private readonly Campaign_Repository campaignRepository;
        private string currentCampaignPath;

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

        public RelayCommand SaveAsCampaign { get { return new RelayCommand(SaveAsCampaign_Execute, SaveAsCampaign_CanExecute); } }
        public RelayCommand SaveCampaign { get { return new RelayCommand(SaveCampaign_Execute, SaveCampaign_CanExecute); } }
        public RelayCommand<object> LoadCampaign { get { return new RelayCommand<object>(LoadCampaign_Execute); } }
        public RelayCommand<object> CreateCampaign { get { return new RelayCommand<object>(CreateCampaign_Execute); } }

        bool SaveAsCampaign_CanExecute()
        {
            if (CampaignVM.Campaign.BackgroundImagePath == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Speichert die Kampagne ab. Pfad muss definiert werden.
        /// (Eigentlich SaveAs)
        /// </summary>
        void SaveAsCampaign_Execute()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save Campaign";
            save.Filter = "xml files (*.xml)|*.xml";
            // Zum Testen
            //save.InitialDirectory = $"{Directory.GetCurrentDirectory()}";
            save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (save.ShowDialog() == true)
            {
                try
                {
                    campaignRepository.Save(CampaignVM.Campaign, save.FileName);
                    currentCampaignPath = save.FileName;
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        bool SaveCampaign_CanExecute()
        {
            if (currentCampaignPath == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Speichert die Kampagne ohne nach dem Pfad zu fragen.
        /// </summary>
        void SaveCampaign_Execute()
        {
            if (currentCampaignPath != null)
            {
                try
                {
                    campaignRepository.Save(CampaignVM.Campaign, currentCampaignPath);
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                SaveAsCampaign_Execute();
            }
        }

        /// <summary>
        /// Lädt eine existierende Kampagne. Der Benutzer kann den Pfad angeben.
        /// </summary>
        /// <param name="window"></param>
        void LoadCampaign_Execute(object window)
        {
            if (IsACampaignOpen)
            {
                if (UserWantsToCancel())
                {
                    return;
                }
            }
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Open Campaign";
            open.Filter = "xml files (*.xml)|*.xml";
            // Zum Testen
            //open.InitialDirectory = $"{Directory.GetCurrentDirectory()}";
            open.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

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
                    IsACampaignOpen = true;
                    currentCampaignPath = open.FileName;
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Erstellt eine neuen Kampagne. Der Benutzer muss den Pfad eines Hintergrundbildes angeben.
        /// </summary>
        /// <param name="window"></param>
        void CreateCampaign_Execute(object window)
        {
            if (IsACampaignOpen)
            {
                if (UserWantsToCancel())
                {
                    return;
                }
            }
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Choose Background Picture";
            open.Filter = ImageFilterString.GetImageFilter();
            // Zum Testen
            //open.InitialDirectory = $"{Directory.GetCurrentDirectory()}";
            open.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (open.ShowDialog() == true)
            {
                try
                {
                    CampaignVM = new Campaign_ViewModel() { Campaign = campaignRepository.Create(Path.GetFileNameWithoutExtension(open.FileName), open.FileName) };
                    
                    // Stokes müssen vorgegeben werden
                    // https://stackoverflow.com/questions/728005/mvvm-binding-to-inkcanvas
                    CampaignVM.Strokes = new StrokeCollection();
                    (CampaignVM.Strokes as INotifyCollectionChanged).CollectionChanged += delegate { };

                    InkCanvas canvas = (InkCanvas)((MainWindow)window).FindName("content");
                    canvas.Children.Clear();
                    IsACampaignOpen = true;
                    currentCampaignPath = null;
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Fragt den Benutzer ob die aktuell geöffnete Kampagne gespeichert werden soll, bevor sie geschlossen wird.
        /// </summary>
        /// <returns>Vorgang abbrechen</returns>
        private bool UserWantsToCancel()
        {
            MessageBoxResult result = MessageBox.Show("Aktuelle Kampagne speichern, bevor sie geschlossen wird?", "Speichern", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (currentCampaignPath == null)
                    {
                        SaveAsCampaign_Execute();
                    }
                    else
                    {
                        campaignRepository.Save(CampaignVM.Campaign, currentCampaignPath);
                    }
                    return false;
                case MessageBoxResult.No:
                    return false;
                case MessageBoxResult.Cancel:
                    return true;
                default:
                    return true;
            }
        }

        #endregion

        #region Layer Handling

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

        public RelayCommand DeleteLayer { get { return new RelayCommand(DeleteLayer_Execute); } }
        public RelayCommand<string> AddLayer { get { return new RelayCommand<string>(AddLayer_Execute); } }
        public RelayCommand<Layer_Model> VisibilityOfLayerChanged { get { return new RelayCommand<Layer_Model>(VisibilityOfLayerChanged_Execute); } }

        /// <summary>
        /// Löscht den aktuell selektieren Layer.
        /// </summary>
        void DeleteLayer_Execute()
        {
            CampaignVM.Campaign.RemoveLayer(SelectedLayer_Workaround);
        }

        /// <summary>
        /// Fügt einen neuen Layer mit dem Namen <paramref name="name"/> hinzu.
        /// </summary>
        /// <param name="name">Layername</param>
        void AddLayer_Execute(string name)
        {
            CampaignVM.Campaign.Layers.Add(new Layer_Model() { Name = name, Id = Guid.NewGuid() });
        }

        /// <summary>
        /// Aktualisiert die Sichtbarkeit aller Kartenelemente die mit <paramref name="layer"/> verbunden sind und den aktuell selektierten Layer.
        /// </summary>
        /// <param name="layer">Layer der angepasst wurde</param>
        void VisibilityOfLayerChanged_Execute(Layer_Model layer)
        {
            CampaignVM.Campaign.UpdateVisibilityOfMapElements(layer);
            SelectedLayer_Workaround = layer;
        }

        #endregion

        #region Edit&DeleteSelectedElement

        #region EditSelectedStroke

        private Stroke _selectedStroke;
        private StrokeData_Model _strokeDataOfSelectedElement = new StrokeData_Model();
        private bool benutzerinteraktion;

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

        public RelayCommand<Layer_Model> LayersOfSelectedStrokeChanged { get { return new RelayCommand<Layer_Model>(LayersOfSelectedStrokeChanged_Execute); } }
        // CanExecute FUnktioniert hier nicht wegen Framework

        /// <summary>
        /// Entfernt oder fügt den <paramref name="layer"/> dem Selektierten Stroke hinzu.
        /// </summary>
        /// <param name="layer"></param>
        void LayersOfSelectedStrokeChanged_Execute(Layer_Model layer)
        {
            // ----> CanExecute Workaround
            if (benutzerinteraktion)
            {
                CampaignVM.Campaign.SetLayersOfStroke(SelectedStroke, layer);
            }
        }

        #endregion

        #region Edit&DeleteSelectedPOI

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

        /// <summary>
        /// Falls ein POI von dem InkCanvas gelöscht wird, muss er auch im CamapignModel gelöscht werden.
        /// Das macht diese Methode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeletePOI(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                CampaignVM.Campaign.POIs.Remove(SelectedPOI);
                SelectedPOI = new POI_Model();
            }
        }

        #endregion

        private ObservableCollection<Layer_Model> _layersOfSelectedElement = new ObservableCollection<Layer_Model>();
        private string _elementNameForSelectedElement;
        private string _elementDetailsForSelectedElement;

        public ObservableCollection<Layer_Model> LayersOfSelectedElement
        {
            get { return _layersOfSelectedElement; }
            set { _layersOfSelectedElement = value; }
        }

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

        /// <summary>
        /// Setzt die Meta-Daten für das aktuell selektierte Kartenelement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ElementSelected(object sender, InkCanvasSelectionChangingEventArgs e)
        {
            benutzerinteraktion = false;
            StrokeCollection selectedStrokes = e.GetSelectedStrokes();
            var selectedPOIs = e.GetSelectedElements();

            if (selectedStrokes.Count > 0)
            {
                SelectedPOI = new POI_Model();

                SelectedStroke = selectedStrokes.First();
                // Set Layers
                LayersOfSelectedElement = CampaignVM.Campaign.GetLayersOfStroke(SelectedStroke);
                RaisePropertyChanged("LayersOfSelectedElement");
                benutzerinteraktion = true;

                // Set StrokeData
                StrokeDataOfSelectedElement = CampaignVM.Campaign.GetStrokeDataOfStroke(SelectedStroke);
                ElementNameForSelectedElement = StrokeDataOfSelectedElement.Name;
                ElementDetailsForSelectedElement = StrokeDataOfSelectedElement.Details;
            }

            if (selectedPOIs.Count > 0)
            {
                StrokeDataOfSelectedElement = new StrokeData_Model();

                SelectedPOI = (POI_Model)((Image)selectedPOIs.First()).Tag;
                // Set Layers
                LayersOfSelectedElement = SelectedPOI.Layers;
                RaisePropertyChanged("LayersOfSelectedElement");
                ElementNameForSelectedElement = SelectedPOI.Name;
                ElementDetailsForSelectedElement = SelectedPOI.Details;
            }
        }

        /// <summary>
        /// Aktualisiert die Sichtbarkeit aller Kartenelemente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectedElementChanged(object sender, EventArgs e)
        {
            foreach (var layer in CampaignVM.Campaign.Layers)
            {
                CampaignVM.Campaign.UpdateVisibilityOfMapElements(layer);
            }
        }

        #endregion

        #region AddNewElement

        #region AddNewStroke

        private DrawingAttributes _drawingAttributesForNewStroke = new DrawingAttributes();

        public DrawingAttributes DrawingAttributesForNewStroke
        {
            get { return _drawingAttributesForNewStroke; }
            set
            {
                if (_drawingAttributesForNewStroke == value)
                {
                    return;
                }
                _drawingAttributesForNewStroke = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Fügt die Meta-Daten zu einem neu gezeichneten Stroke hinzu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddStroke(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            // Layer hinzufügen
            foreach (var layer in LayersForNewElement)
            {
                e.Stroke.AddPropertyData(layer.Id, layer.Name);
            }

            // StrokeData hinzufügen
            StrokeData_Model newStrokeData = new StrokeData_Model() { Id = Guid.NewGuid(), Name = ElementNameForNewElement, Details = ElementDetailsForNewElement };
            CampaignVM.Campaign.StrokeDataList.Add(newStrokeData);
            e.Stroke.AddPropertyData(newStrokeData.Id, "Id");
        }

        #endregion

        #region AddNewPOI

        /// <summary>
        /// Fügt die Meta-Daten zu einem neuen POI hinzu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddPOI(object sender, MouseButtonEventArgs e)
        {
            var canvas = (InkCanvas)sender;
            var mouseDownPoint = e.GetPosition(canvas);
            PictureDimension pictureDimension = new PictureDimension(AppDomain.CurrentDomain.BaseDirectory + @"Images/Pin.png");
            var x = mouseDownPoint.Y - pictureDimension.Height;
            var y = mouseDownPoint.X;

            POI_Model poi = new POI_Model() { Name = ElementNameForNewElement, Details = ElementDetailsForNewElement, PositionTop = x, PositionLeft = y, IsEnabled = true };
            // Layer hinzufügen
            foreach (var layer in LayersForNewElement)
            {
                poi.Layers.Add(layer);
            }
            CampaignVM.Campaign.POIs.Add(poi);
            AddPOIToCanvas(canvas, poi);
        }

        /// <summary>
        /// Fügt einen <paramref name="poi"/> dem <paramref name="canvas"/> hinzu.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="poi"></param>
        private void AddPOIToCanvas(InkCanvas canvas, POI_Model poi)
        {
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(@"../Images/Pin.png", UriKind.Relative)),
                Tag = poi
            };
            InkCanvas.SetTop(image, poi.PositionTop);
            InkCanvas.SetLeft(image, poi.PositionLeft);

            // Binding für die Sichtbarkeitssteuerung hinzufügen
            Binding myBinding = new Binding("IsEnabled");
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.Converter = new BooleanToVisibilityConverter();
            myBinding.Source = poi;
            image.SetBinding(UIElement.VisibilityProperty, myBinding);

            canvas.Children.Add(image);
        }

        #endregion

        private ObservableCollection<Layer_Model> _layersForNewElement = new ObservableCollection<Layer_Model>();
        private string _elementNameForNewElement;
        private string _elementDetailsForNewElement;

        public ObservableCollection<Layer_Model> LayersForNewElement
        {
            get { return _layersForNewElement; }
            set { _layersForNewElement = value; }
        }

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

        #region Input Mode

        private InkCanvasEditingMode _inkCanvasEditingMode = InkCanvasEditingMode.Select;
        private InkCanvasEditingMode previousMode = InkCanvasEditingMode.Select;

        public InkCanvasEditingMode InkCanvasEditingMode
        {
            get { return _inkCanvasEditingMode; }
            set
            {
                if (_inkCanvasEditingMode == value)
                {
                    return;
                }
                _inkCanvasEditingMode = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<bool> EraseMode { get { return new RelayCommand<bool>(EraseMode_Execute); } }
        public RelayCommand<string> MapElementInputMode { get { return new RelayCommand<string>(MapElementInputMode_Execute); } }


        /// <summary>
        /// Aktiviert oder deaktiviert den Radiergummimodus aufgrund von <paramref name="isChecked"/>.
        /// </summary>
        /// <param name="isChecked"></param>
        void EraseMode_Execute(bool isChecked)
        {
            if (isChecked)
            {
                SetInputMode("Erase");
            }
            else
            {
                SetInputMode("NonErase");
            }
        }

        /// <summary>
        /// Setzt den Inputmode aufgrund von <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        void MapElementInputMode_Execute(string name)
        {
            SetInputMode(name);
        }

        /// <summary>
        /// Setzt den InkCanvasEditingMode aufgrund von <paramref name="mode"/>.
        /// </summary>
        /// <param name="mode">Zu setzender Modus</param>
        private void SetInputMode(string mode)
        {
            switch (mode)
            {
                case "Draw":
                    InkCanvasEditingMode = previousMode;
                    break;

                case "Select":
                    InkCanvasEditingMode = InkCanvasEditingMode.Select;
                    break;

                case "Erase":
                    InkCanvasEditingMode = InkCanvasEditingMode.EraseByPoint;
                    break;

                case "NonErase":
                    InkCanvasEditingMode = previousMode;
                    break;

                case "POI":
                    InkCanvasEditingMode = InkCanvasEditingMode.None;
                    previousMode = InkCanvasEditingMode.None;
                    break;

                case "Route":
                    InkCanvasEditingMode = InkCanvasEditingMode.Ink;
                    previousMode = InkCanvasEditingMode.Ink;
                    DrawingAttributesForNewStroke.IsHighlighter = false;
                    break;

                case "Region":
                    InkCanvasEditingMode = InkCanvasEditingMode.Ink;
                    previousMode = InkCanvasEditingMode.Ink;
                    DrawingAttributesForNewStroke.IsHighlighter = true;
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Symbol Handling

        private ObservableCollection<Symbol_Model> _symbols = new ObservableCollection<Symbol_Model>();

        public ObservableCollection<Symbol_Model> Symbols
        {
            get { return _symbols; }
            set { _symbols = value; }
        }

        #endregion

    }
}