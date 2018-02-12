using MVVM_Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

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
                RaisePropertyChanged();
            }
        }

        public string BackgroundImagePath
        {
            get { return _campaign.BackgroundImagePath; }
            set
            {
                _campaign.BackgroundImagePath = value;
                RaisePropertyChanged();
            }
        }

        public StrokeCollection Strokes
        {
            get { return _campaign.Strokes; }
            set
            {
                _campaign.Strokes = value;
                RaisePropertyChanged();
            }
        }

    }
}
