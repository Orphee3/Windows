using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class ColorManager : IColorManager
    { 
        public ObservableCollection<SolidColorBrush> ColorList {get; private set;}

        public ColorManager()
        {
            this.ColorList = new ObservableCollection<SolidColorBrush>()
            {
                new SolidColorBrush(Color.FromArgb(0xFF, 0xF4, 0x43, 0x36)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0x1E, 0x63)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x9C, 0x27, 0xB0)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x67, 0x3A, 0xB7)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x3F, 0x51, 0xB5)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x03, 0xA9, 0xF4)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0xC7, 0xF9)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xBC, 0xD4)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x96, 0x88)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x4C, 0xAF, 0x50)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x8B, 0xC3, 0x4A)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0xCD, 0xDC, 0x39)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xEB, 0x3B)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC1, 0x07)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x98, 0x00)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x55, 0x48)),

            };
        }

        public int GetColorIndex(SolidColorBrush color)
        {
            for (var index = 0; index < this.ColorList.Count; index++)
            {
                if (this.ColorList[index].Color == color.Color)
                    return index;
            }
            return -1;
        }
    }
}
