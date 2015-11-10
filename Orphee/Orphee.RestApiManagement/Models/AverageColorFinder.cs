using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Orphee.RestApiManagement.Models;

namespace Orphee.Models
{
    public class AverageColorFinder
    {
        public async Task<Color> GetDominantImageColor()
        {
            var stream = await GetImageStream();
            if (stream == null)
                return (Color.FromArgb(0xFF, 0x78, 0xC7, 0xF9));
            var decoder = await BitmapDecoder.CreateAsync(stream);
            var myTransform = new BitmapTransform { ScaledHeight = 10, ScaledWidth = 10 };
            var pixels = await decoder.GetPixelDataAsync(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Straight, myTransform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
            var bytes = pixels.DetachPixelData();

            return GetImageAverageColor(bytes);
        }

        private Color GetImageAverageColor(byte[] bytes)
        {
            int alpha = 0;
            int red = 0;
            int blue = 0;
            int green = 0;
            for (var i = 0; i < bytes.Length; i += 4)
            {
                red += bytes[i];
                green += bytes[i + 1];
                blue += bytes[i + 2];
                alpha += bytes[i + 3];
            }
            var alphaByte = (byte)(alpha / (bytes.Length / 4));
            var bytered = (byte)(red / (bytes.Length / 4));
            var bytegreen = (byte)(green / (bytes.Length / 4));
            var byteblue = (byte)(blue / (bytes.Length / 4));
            var color = Color.FromArgb(alphaByte, bytered, bytegreen, byteblue);
            return (color);
        }

        private async Task<IRandomAccessStream> GetImageStream()
        {
            try
            {
                IRandomAccessStream stream;
                if (RestApiManagerBase.Instance.UserData.User.Picture != "ms-appx:///Assets/defaultUser.png")
                {
                    var streamReference = RandomAccessStreamReference.CreateFromUri(new Uri(RestApiManagerBase.Instance.UserData.User.Picture)).OpenReadAsync();
                    stream = await streamReference;
                }
                else
                {
                    var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(RestApiManagerBase.Instance.UserData.User.Picture));
                    stream = await storageFile.OpenAsync(FileAccessMode.Read);
                }
                return stream;
            }
            catch
            {
                return null;
            }
        }
    }
}
