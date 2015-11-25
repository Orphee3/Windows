using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Orphee.Models.Interfaces;

namespace Orphee.Models
{
    public class FontSizeManager : IFontSizeManager
    {
        public int PageTitleSize { get; private set; }

        public FontSizeManager()
        {
            if (DisplayProperties.CurrentOrientation == DisplayOrientations.Landscape || DisplayProperties.CurrentOrientation == DisplayOrientations.LandscapeFlipped)
                SetFontSizeFromWidth();
            else
                SetFontSizeFromHeight();
        }

        private void SetFontSizeFromHeight()
        {
            this.PageTitleSize = (int)(Window.Current.Bounds.Height / 54.64);
        }

        private void SetFontSizeFromWidth()
        {
            this.PageTitleSize = (int)(Window.Current.Bounds.Width / 54.64);
        }
    }
}
