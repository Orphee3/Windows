﻿using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface IToggleButtonNote
    {
        // Methods

        // Properties
        int Width { get; }
        int Height { get; }
        int LineIndex { get; set; }
        int ColumnIndex { get; set; }
        Note Note { get; set; }
        bool IsChecked { get; set; }
    }
}
