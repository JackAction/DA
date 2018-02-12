using MVVM_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    public class MainWindow_ViewModel : ObservableObject
    {
        private readonly Campaign_Repository campaignRepository = new Campaign_Repository();

        public MainWindow_ViewModel()
        {
            // Load soll nacher erst apssieren wenn LoadButton geklickt wird
            _campaign = new Campaign_ViewModel() { Campaign = campaignRepository.Load()};
        }


        private Campaign_ViewModel _campaign;

        public Campaign_ViewModel Campaign
        {
            get { return _campaign; }
            set
            {
                if (_campaign == value)
                {
                    return;
                }
                _campaign = value;
                RaisePropertyChanged("Campaign");
            }
        }


        void ExecuteChangeBackground()
        {
            Campaign.BackgroundImagePath = "map_faerunLarge_2.jpg";
        }

        public RelayCommand ChangeBackground { get { return new RelayCommand(ExecuteChangeBackground); } }

    }
}
