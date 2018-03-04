using System.IO;
using System.Linq;
using System.Windows.Ink;

namespace MainApplication
{
    public class Campaign_Repository
    {
        /// <summary>
        /// Liefert eine neue Campaign mit <paramref name="backgroundImagePath"/> als Hintergrundbild.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="backgroundImagePath">Hintergrundbildpfad</param>
        /// <returns>Neue Campaign</returns>
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

        /// <summary>
        /// Liefert eine existierende Campaign am Ablageort <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Existierende Campaign</returns>
        public Campaign_Model Load(string path)
        {
            try
            {
                Campaign_Model campaign = XMLHelper<Campaign_Model>.Deserialize(path);

                foreach (var poi in campaign.POIs)
                {
                    for (int i = 0; i < poi.Layers.Count; i++)
                    {
                        poi.Layers[i] = campaign.Layers.First(x => x.Id == poi.Layers[i].Id);
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
            catch (System.Exception)
            {

                throw new System.Exception("Ein File konnte nicht gefunden werden.");
            }
        }

        /// <summary>
        /// Speichert <paramref name="campaign"/> am Ablageort <paramref name="path"/>.
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="path"></param>
        public void Save(Campaign_Model campaign, string path)
        {
            try
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
            catch (System.Exception)
            {
                throw new System.Exception("Fehler beim Speichern.");
            }
        }

        /// <summary>
        /// Setzt den richtigen Pfad für die Strokecollections zusammen.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="strokeVariant"></param>
        /// <returns>StrokePath</returns>
        private static string CreateStrokePath(string path, string strokeVariant)
        {
            return $"{ Path.GetDirectoryName(path) }\\{Path.GetFileNameWithoutExtension(path)}_{strokeVariant}Strokes.bin";
        }
    }
}