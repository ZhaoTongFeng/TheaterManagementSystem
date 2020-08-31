using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FinalProject.Tools
{
    public class ImageTools
    {
        public static BitmapImage UrlToImage(string path)
        {
            BitmapImage img = new BitmapImage();
            try
            {
                img.BeginInit();
                img.UriSource = new Uri(path);
                img.EndInit();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("UrlToImage:" + ex.Message);
                return null;
            }

            return img;
        }
    }
}
