using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MainApplication
{
    public class PictureDimension
    {
        public PictureDimension(string picturePath)
        {
            // https://stackoverflow.com/questions/6455979/how-to-get-the-image-dimension-from-the-file-name

            using (var imageStream = File.OpenRead(picturePath))
            {
                var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.IgnoreColorProfile,
                    BitmapCacheOption.Default);
                Height = decoder.Frames[0].PixelHeight;
                Width = decoder.Frames[0].PixelWidth;
            }

        }

        public int Height { get; }

        public int Width { get; }


    }
}
