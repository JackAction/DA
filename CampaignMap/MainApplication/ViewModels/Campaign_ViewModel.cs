using MVVM_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    public class Campaign_ViewModel : ObservableObject
    {
        private Campaign_Model _campaign;

        public Campaign_ViewModel()
        {
            _campaign = new Campaign_Model();
        }

        public Campaign_Model Campaign
        {
            get { return _campaign; }
            set
            {
                _campaign = value;
                RaisePropertyChanged("Campaign");
            }
        }

        public String BackgroundImagePath
        {
            get { return _campaign.BackgroundImagePath; }
            set
            {
                _campaign.BackgroundImagePath = value;
                RaisePropertyChanged("BackgroundImagePath");
            }
        }
    }
}
