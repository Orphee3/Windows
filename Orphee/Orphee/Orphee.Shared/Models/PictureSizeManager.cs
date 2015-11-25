using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Orphee.Models.Interfaces;

namespace Orphee.Models
{
    public class PictureSizeManager : IPictureSizeManager
    {
        public int PiecePictureSize { get; private set; }

        public PictureSizeManager()
        {
            if (DisplayProperties.CurrentOrientation == DisplayOrientations.Landscape || DisplayProperties.CurrentOrientation == DisplayOrientations.LandscapeFlipped)
                SetPictureSizeFromWidth();
            else
                SetPictureSizeFromHeight();
        }

        private void SetPictureSizeFromHeight()
        {
            this.PiecePictureSize = (int)(Window.Current.Bounds.Height / 11.38);
        }

        private void SetPictureSizeFromWidth()
        {
            this.PiecePictureSize = (int)(Window.Current.Bounds.Width / 11.38);
        }
    }
}
