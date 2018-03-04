using MVVM_Framework;
using System.Collections.ObjectModel;

namespace MainApplication
{
    public class POI_Model : ObservableObject
    {
        public POI_Model()
        {
            Layers = new ObservableCollection<Layer_Model>();
        }

        public string Name { get; set; }
        public string Details { get; set; }
        public double PositionTop { get; set; }
        public double PositionLeft { get; set; }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value)
                {
                    return;
                }
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public Symbol_Model Symbol { get; set; }
        public ObservableCollection<Layer_Model> Layers { get; set; }
    }
}