using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication { 
    public class Campaign_Repository
    {
        public Campaign_Model Create()
        {
            // Testcode
            return new Campaign_Model() { Name = "TestCampaign", BackgroundImagePath= "map_faerunLarge.jpg" };
        }

        // Überlaene Methode könnte string für definierte Kampagne mitgeben. Irgendwo gibt es dann eine Collection mit Kampagnen aus denen man auswählen kann
        public Campaign_Model Load(string path)
        {
            return XMLHelper<Campaign_Model>.Deserialize(path);
        }

        public void Save(Campaign_Model campaign, string path)
        {
            XMLHelper<Campaign_Model>.Serialize(path, campaign);
        }
    }
}
