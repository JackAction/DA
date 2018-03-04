using MVVM_Framework;
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