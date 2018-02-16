using MVVM_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                //RaisePropertyChanged("BackgroundImagePath");
                //RaisePropertyChanged("Strokes");
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

        //public StrokeCollection InvisibleStrokes
        //{
        //    get { return _campaign.InvisibleStrokes; }
        //    set
        //    {
        //        _campaign.InvisibleStrokes = value;
        //        RaisePropertyChanged();
        //    }
        //}

        private ObservableCollection<Layer_Model> _layers = new ObservableCollection<Layer_Model>();
        public ObservableCollection<Layer_Model> Layers
        {
            get { return _campaign.Layers; }
            set { _campaign.Layers = value; }
        }

    }
}
