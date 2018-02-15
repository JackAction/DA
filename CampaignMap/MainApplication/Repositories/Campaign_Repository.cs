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
            return new Campaign_Model() { Name = name, BackgroundImagePath = backgroundImagePath };
        }

        // Überladene Methode könnte string für definierte Kampagne mitgeben. Irgendwo gibt es dann eine Collection mit Kampagnen aus denen man auswählen kann
        public Campaign_Model Load(string path)
        {
            Campaign_Model campaign = XMLHelper<Campaign_Model>.Deserialize(path);

            using (FileStream fs = new FileStream(CreateStrokePath(path), FileMode.Open, FileAccess.Read))
            {
                StrokeCollection sc = new StrokeCollection(fs);
                campaign.Strokes = sc;
            }

            return campaign;
        }

        public void Save(Campaign_Model campaign, string path)
        {
            XMLHelper<Campaign_Model>.Serialize(path, campaign);

            using (FileStream fs = new FileStream(CreateStrokePath(path), FileMode.Create))
            {
                campaign.Strokes.Save(fs);
            }
        }
        private static string CreateStrokePath(string path)
        {
            return $"{ Path.GetDirectoryName(path) }\\{Path.GetFileNameWithoutExtension(path)}_Strokes.bin";
        }
    }
}
