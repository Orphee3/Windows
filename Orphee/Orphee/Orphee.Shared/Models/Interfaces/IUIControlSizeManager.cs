namespace Orphee.Models.Interfaces
{
    public interface IUIControlSizeManager
    {
        IFontSizeManager FontSizeManager { get; }
        IPictureSizeManager PictureSizeManager { get; }
    }
}
