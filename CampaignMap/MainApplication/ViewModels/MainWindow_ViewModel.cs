﻿using Microsoft.Win32;
using MVVM_Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;

namespace MainApplication
{
    public class MainWindow_ViewModel : ObservableObject
    {
        private readonly Campaign_Repository campaignRepository = new Campaign_Repository();

        public MainWindow_ViewModel()
        {
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
                RaisePropertyChanged("CampaignVM");
            }
        }

        #region GUI Handling

        void ExecuteChangeBackground()
        {
            CampaignVM.BackgroundImagePath = "map_faerunLarge_2.jpg";
        }

        public RelayCommand ChangeBackground { get { return new RelayCommand(ExecuteChangeBackground); } } 

        #endregion

        #region Campaign Handling

        void ExecuteSaveCampaign()
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

        public RelayCommand SaveCampaign { get { return new RelayCommand(ExecuteSaveCampaign); } }

        void ExecuteLoadCampaign()
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

        public RelayCommand LoadCampaign { get { return new RelayCommand(ExecuteLoadCampaign); } }

        void ExecuteCreateCampaign()
        {
            CampaignVM = new Campaign_ViewModel() { Campaign = campaignRepository.Create() };

            // https://stackoverflow.com/questions/728005/mvvm-binding-to-inkcanvas
            // Stokes müssen vorgegeben werden
            CampaignVM.Strokes = new StrokeCollection();
            (CampaignVM.Strokes as INotifyCollectionChanged).CollectionChanged += delegate { };
        }

        public RelayCommand CreateCampaign { get { return new RelayCommand(ExecuteCreateCampaign); } }

        #endregion

    }
}
