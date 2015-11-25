using Orphee.Models.Interfaces;

namespace Orphee.Models
{
    public class UIControlsSizeManager : IUIControlSizeManager
    {
        public IFontSizeManager FontSizeManager { get; private set; }
        public IPictureSizeManager PictureSizeManager { get; private set; }

        public UIControlsSizeManager()
        {
            this.FontSizeManager = new FontSizeManager();
            this.PictureSizeManager = new PictureSizeManager();
        }
    }
}
