using System.IO;
using System.Windows.Media.Imaging;

namespace MainApplication
{
    public class PictureDimension
    {
        /// <summary>
        /// Analysiert ein Bild und stellt über die Height und Width Properties dessen Dimensionen zur Verfügung.
        /// https://stackoverflow.com/questions/6455979/how-to-get-the-image-dimension-from-the-file-name
        /// </summary>
        /// <param name="picturePath">Pfad des Bildes, das analysiert werden soll</param>
        public PictureDimension(string picturePath)
        {
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
