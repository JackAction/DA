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
            InvisibleStrokes = new StrokeCollection();
        }

        public string Name { get; set; }
        public string BackgroundImagePath { get; set; }
        public string StrokePath { get; set; }

        [XmlIgnore]
        public StrokeCollection Strokes { get; set; }

        [XmlIgnore]
        public StrokeCollection InvisibleStrokes { get; set; }

        public ObservableCollection<Layer_Model> Layers { get; set; }

        public void ChangeLayerVisibility(Layer_Model layer)
        {
            if (layer.IsSelected)
            {
                foreach (var stroke in InvisibleStrokes.ToList())
                {
                    if (stroke.ContainsPropertyData(layer.Guid))
                    {
                        Strokes.Add(stroke);
                        InvisibleStrokes.Remove(stroke);
                    }
                }
            }
            else
            {
                List<Layer_Model> inactiveLayers = Layers.Where(l => l.IsSelected == false).ToList();

                foreach (var stroke in Strokes.ToList())
                {
                    if (stroke.ContainsPropertyData(layer.Guid))
                    {
                        // Prüfe ob deselektierter Layer der letzte sichtbare war
                        List<Guid> remainingLayers = stroke.GetPropertyDataIds().ToList();
                        remainingLayers.Remove(layer.Guid);

                        foreach (var guidLayer in remainingLayers.ToList())
                        {
                            if (inactiveLayers.Any(x => x.Guid == guidLayer))
                            {
                                remainingLayers.Remove(guidLayer);
                            }
                        }

                        // Lösche nur wenn Stroke keine anderen sichtbaren Layer mehr hat
                        if (remainingLayers.Count == 0)
                        {
                            InvisibleStrokes.Add(stroke);
                            Strokes.Remove(stroke);
                        }
                    }
                }
            }
        }


    }
}
