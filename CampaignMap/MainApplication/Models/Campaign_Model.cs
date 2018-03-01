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
            StrokeDataList = new ObservableCollection<StrokeData_Model>();
            POIs = new ObservableCollection<POI_Model>();
        }

        public string Name { get; set; }
        public string BackgroundImagePath { get; set; }
        public int BackgroundImageHeight { get; set; }
        public int BackGroundImageWidth { get; set; }
        //public string StrokePath { get; set; }

        [XmlIgnore]
        public StrokeCollection Strokes { get; set; }

        [XmlIgnore]
        public StrokeCollection InvisibleStrokes { get; set; }

        public ObservableCollection<StrokeData_Model> StrokeDataList { get; set; }

        public ObservableCollection<POI_Model> POIs { get; set; }

        public ObservableCollection<Layer_Model> Layers { get; set; }

        public StrokeData_Model GetStrokeDataOfStroke(Stroke stroke)
        {
            List<Guid> guidLayers = stroke.GetPropertyDataIds().ToList();
            return StrokeDataList.SingleOrDefault(x => x.Id == guidLayers.SingleOrDefault(r => r.Equals(x.Id)));
        }

        public ObservableCollection<Layer_Model> GetLayersOfStroke(Stroke stroke)
        {
            List<Guid> guidLayers = stroke.GetPropertyDataIds().ToList();
            ObservableCollection<Layer_Model> layers = new ObservableCollection<Layer_Model>();

            foreach (var guidLayer in guidLayers)
            {
                Layer_Model layer = Layers.SingleOrDefault(x => x.Id == guidLayer);
                if (layer != null)
                {
                    layers.Add(layer); 
                }
            }
            return layers;
        }

        public void SetLayersOfStroke(Stroke stroke, Layer_Model layer)
        {
            if (layer.IsSelectedInExistingElement)
            {
                stroke.AddPropertyData(layer.Id, layer.Name);
            }
            else
            {
                stroke.RemovePropertyData(layer.Id);
            }
        }


        public void UpdateVisibilityOfMapElements(Layer_Model layer)
        {
            if (layer.IsSelectedForVisibilityHandling)
            {
                // Setze POI Visibility
                foreach (var poi in POIs)
                {
                    if (poi.Layers.Any(x => x.Id == layer.Id))
                    {
                        poi.IsEnabled = true;
                    }
                }
                // Setze Stroke Visibility
                foreach (var stroke in InvisibleStrokes.ToList())
                {
                    if (stroke.ContainsPropertyData(layer.Id))
                    {
                        Strokes.Add(stroke);
                        InvisibleStrokes.Remove(stroke);
                    }
                }
            }
            else
            {
                List<Layer_Model> inactiveLayers = Layers.Where(l => l.IsSelectedForVisibilityHandling == false).ToList();
                // Setze POI Visibility
                foreach (var poi in POIs)
                {
                    if (poi.Layers.Any(x => x.Id == layer.Id))
                    {
                        // Prüfe ob deselektierter Layer der letzte sichtbare war
                        List<Layer_Model> remainingLayers = poi.Layers.ToList();
                        remainingLayers.Remove(layer);

                        foreach (var layerInPOI in poi.Layers.ToList())
                        {
                            if (inactiveLayers.Contains(layerInPOI))
                            {
                                remainingLayers.Remove(layerInPOI);
                            }
                        }

                        // Lösche nur wenn Stroke keine anderen sichtbaren Layer mehr hat
                        if (remainingLayers.Count == 0) 
                        {
                            poi.IsEnabled = false;
                        }
                    }
                }
                // Setze Stroke Visibility
                foreach (var stroke in Strokes.ToList())
                {
                    if (stroke.ContainsPropertyData(layer.Id))
                    {
                        // Prüfe ob deselektierter Layer der letzte sichtbare war
                        List<Guid> remainingLayers = stroke.GetPropertyDataIds().ToList();
                        remainingLayers.Remove(layer.Id);

                        foreach (var guidLayer in remainingLayers.ToList())
                        {
                            if (inactiveLayers.Any(x => x.Id == guidLayer))
                            {
                                remainingLayers.Remove(guidLayer);
                            }
                        }

                        // Lösche nur wenn Stroke keine anderen sichtbaren Layer mehr hat
                        if (remainingLayers.Count == 1) // 1 Guid bleibt immer übrig, das ist StrokeData
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
