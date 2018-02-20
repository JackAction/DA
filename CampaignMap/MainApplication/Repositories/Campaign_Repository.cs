using System.IO;
using System.Linq;
using System.Windows.Ink;
using System.Windows.Media;

namespace MainApplication
{
    public class Campaign_Repository
    {
        public Campaign_Model Create(string name, string backgroundImagePath)
        {
            // Bildgrösse auslesen
            PictureDimension pictureDimension = new PictureDimension(backgroundImagePath);

            return new Campaign_Model() {
                Name = name,
                BackgroundImagePath = backgroundImagePath,
                BackgroundImageHeight = pictureDimension.Height,
                BackGroundImageWidth = pictureDimension.Width
            };
        }

        // ---> Überall ErrorHandling, falls Files nicht gefunden werden können!!

        // Überladene Methode könnte string für definierte Kampagne mitgeben. Irgendwo gibt es dann eine Collection mit Kampagnen aus denen man auswählen kann
        public Campaign_Model Load(string path)
        {
            Campaign_Model campaign = XMLHelper<Campaign_Model>.Deserialize(path);

            foreach (var poi in campaign.POIs)
            {
                for (int i = 0; i < poi.Layers.Count; i++)
                {
                    poi.Layers[i] = campaign.Layers.First(x => x.Guid == poi.Layers[i].Guid);
                }
            }

            using (FileStream fs = new FileStream(CreateStrokePath(path, ""), FileMode.Open, FileAccess.Read))
            {
                StrokeCollection sc = new StrokeCollection(fs);
                campaign.Strokes = sc;
            }
            using (FileStream fs = new FileStream(CreateStrokePath(path, "Invisible"), FileMode.Open, FileAccess.Read))
            {
                StrokeCollection sc = new StrokeCollection(fs);
                campaign.InvisibleStrokes = sc;
            }

            return campaign;
        }

        public void Save(Campaign_Model campaign, string path)
        {
            XMLHelper<Campaign_Model>.Serialize(path, campaign);

            using (FileStream fs = new FileStream(CreateStrokePath(path, ""), FileMode.Create))
            {
                campaign.Strokes.Save(fs);
            }

            using (FileStream fs = new FileStream(CreateStrokePath(path, "Invisible"), FileMode.Create))
            {
                campaign.InvisibleStrokes.Save(fs);
            }
        }
        private static string CreateStrokePath(string path, string strokeVariant)
        {
            return $"{ Path.GetDirectoryName(path) }\\{Path.GetFileNameWithoutExtension(path)}_{strokeVariant}Strokes.bin";
        }
    }
}
