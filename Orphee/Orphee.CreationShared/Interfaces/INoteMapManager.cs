﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Orphee.CreationShared.Interfaces
{
    public interface INoteMapManager
    {
        void AddOneHigherOctaveToThisNoteMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap);
        void AddOneLowerOctaveToThisNoteMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap);
        void AddColumnsToThisColumnMap(ObservableCollection<MyRectangle> columnMap);
        void AddColumnsToThisNoteMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap);
        void RemoveAColumnFromThisNoteMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap);
        bool IsColumnEmpty(int columnIndex, ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap);
    }
}
