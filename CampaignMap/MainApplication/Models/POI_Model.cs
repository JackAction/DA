using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    public class POI_Model
    {
        public POI_Model()
        {
            Layers = new ObservableCollection<Layer_Model>();
        }

        public string Name { get; set; }
        public string Details { get; set; }
        public Symbol_Model Symbol { get; set; }
        public ObservableCollection<Layer_Model> Layers { get; set; }
    }
}
