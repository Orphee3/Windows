using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Orphee.CreationShared.Interfaces
{
    public interface INoteMapGenerator
    {
        ObservableCollection<MyRectangle> GenerateColumnMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap);
        ObservableCollection<ObservableCollection<IToggleButtonNote>> GenerateNoteMap(int startingOctave);
        IList<IOrpheeNoteMessage> ConvertNoteMapToOrpheeNoteMessageList(IList<ObservableCollection<IToggleButtonNote>> noteMap, int channel, ref uint trackLength);
        ObservableCollection<ObservableCollection<IToggleButtonNote>> ConvertOrpheeMessageListToNoteMap(IList<IOrpheeNoteMessage> orpheeNoteMessageLists);
    }
}
