using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Orphee.CreationShared.Interfaces
{
    public interface INoteMapGenerator
    {
        ObservableCollection<MyRectangle> GenerateColumnMap(ObservableCollection<OctaveManager> noteMap);
        OctaveManager GenerateNoteMap(int startingOctave);
        IList<IOrpheeNoteMessage> ConvertNoteMapToOrpheeNoteMessageList(IList<OctaveManager> noteMap, int channel, ref uint trackLength);
        ObservableCollection<OctaveManager> ConvertOrpheeMessageListToNoteMap(IList<IOrpheeNoteMessage> orpheeNoteMessageLists);
    }
}
