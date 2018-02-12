using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication { 
    public class Campaign_Repository
    {
        public Campaign_Model Load()
        {
            // Lade XML von Pfad auf Computer
            // Überlaene Methode könnte string für definierte Kampagne mitgeben. Irgendwo gibt es dann eine Collection mit Kampagnen aus denen man auswählen kann
            // Testcode
            return new Campaign_Model() { Name = "TestCampaign", BackgroundImagePath= "map_faerunLarge.jpg" };
        }
        public Campaign_Model Save()
        {
            throw new NotImplementedException();
        }
    }
}
