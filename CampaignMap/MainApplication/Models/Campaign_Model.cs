using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace MainApplication
{
    public class Campaign_Model
    {
        public Campaign_Model()
        {
            Layers = new ObservableCollection<Layer_Model>();
        }

        public string Name { get; set; }
        public string BackgroundImagePath { get; set; }
        public string StrokePath { get; set; }

        [XmlIgnore]
        public StrokeCollection Strokes { get; set; }

        public ObservableCollection<Layer_Model> Layers { get; set; }

    }
}
