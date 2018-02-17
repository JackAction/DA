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
using System.Windows.Ink;
using System.Windows.Media;

namespace MainApplication
{
    public class MainWindow_ViewModel : ObservableObject
    {
        private readonly Campaign_Repository campaignRepository = new Campaign_Repository();

        public MainWindow_ViewModel()
        {
            SelectedLayer_Workaround = new Layer_Model();
            DefaultDrawingAttributes = new DrawingAttributes();
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

        #region GUI Handling

        void ChangeBackground_Execute()
        {
            CampaignVM.BackgroundImagePath = "map_faerunLarge_2.jpg";
        }

        public RelayCommand ChangeBackground { get { return new RelayCommand(ChangeBackground_Execute); } } 

        #endregion

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

        void LoadCampaign_Execute()
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
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message);
                }
            }
        }

        public RelayCommand LoadCampaign { get { return new RelayCommand(LoadCampaign_Execute); } }

        void CreateCampaign_Execute()
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
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message);
                }
            }
        }

        public RelayCommand CreateCampaign { get { return new RelayCommand(CreateCampaign_Execute); } }

        #endregion

        #region Layer Handling

        void DeleteLayers_Execute()
        {
            CampaignVM.Layers.Remove(SelectedLayer_Workaround);
        }

        public RelayCommand DeleteLayers { get { return new RelayCommand(DeleteLayers_Execute); } }

        void AddLayer_Execute(string name)
        {
            CampaignVM.Layers.Add(new Layer_Model() { IsSelected = false, Name = name, Guid = Guid.NewGuid() });
        }

        public RelayCommand<string> AddLayer { get { return new RelayCommand<string>(AddLayer_Execute); } }

        public void StrokeAdded(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            // Layer hinzufügen
            foreach (var layer in LayersForNewStroke)
            {
                e.Stroke.AddPropertyData(layer.Guid, layer.Name);
            }
        }

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

        //void LayerChanged_Execute(Layer_Model layer)
        //{
        //    CampaignVM.Campaign.ChangeLayerVisibility(layer);
        //    SelectedLayer_Workaround = layer;
        //}

        //public RelayCommand<Layer_Model> LayerChanged { get { return new RelayCommand<Layer_Model>(LayerChanged_Execute); } }

        private DrawingAttributes _defaultDrawingAttributes;

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


        #endregion

    }
}
