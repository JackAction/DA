using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace MainApplication
{
    public class Campaign_Model
    {
        public string Name { get; set; }
        public string BackgroundImagePath { get; set; }
        public string StrokePath { get; set; }

        public StrokeCollection Strokes { get; set; }

    }
}
