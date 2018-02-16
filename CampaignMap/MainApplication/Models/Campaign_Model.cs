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
                for (int i = 0; i < InvisibleStrokes.Count; i++) // for anstatt foreach, damit RemoveAt verwendet werden kann
                {
                    var currentStroke = InvisibleStrokes[i];
                    if (currentStroke.ContainsPropertyData(layer.Guid))
                    {
                        Strokes.Add(currentStroke);
                        InvisibleStrokes.RemoveAt(i); // Schneller als Remove()
                    }
                }
            }
            else
            {
                List<Layer_Model> inactiveLayers = Layers.Where(l => l.IsSelected == false).ToList();

                for (int i = 0; i < Strokes.Count; i++)
                {
                    var currentStroke = Strokes[i];
                    if (currentStroke.ContainsPropertyData(layer.Guid))
                    {
                        // Prüfe ob deselektierter Layer der letzte sichtbare war
                        List<Guid> remainingLayers = currentStroke.GetPropertyDataIds().ToList();
                        remainingLayers.Remove(layer.Guid);

                        foreach (var guidLayer in remainingLayers.ToList())
                        {
                            if (inactiveLayers.Any(x => x.Guid == guidLayer))
                            {
                                remainingLayers.Remove(guidLayer);
                            }
                        }
                        for (int k = 0; k < remainingLayers.Count; k++)
                        {
                            if (inactiveLayers.Any(x => x.Guid == remainingLayers[k]))
                            {
                                remainingLayers.RemoveAt(k);
                            }
                        }

                        // Lösche nur wenn Stroke keine anderen sichtbaren Layer mehr hat
                        if (remainingLayers.Count == 0)
                        {
                            InvisibleStrokes.Add(currentStroke);
                            Strokes.RemoveAt(i);
                        }
                    }
                }
            }
        }


    }
}
