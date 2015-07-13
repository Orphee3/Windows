using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class NoteMapManager
    {
        private static NoteMapManager _instance;
        private readonly int _columnNumberToAdd;
        private readonly int _lineNumber;
        public static NoteMapManager Instance 
        {
            get
            {
                if (_instance == null)
                    _instance = new NoteMapManager();
                return _instance;
            }
        }
        public NoteNameListManager NoteNameListManager { get; private set; }

        private NoteMapManager()
        {
            this.NoteNameListManager = new NoteNameListManager();
            this._columnNumberToAdd = 10;
            this._lineNumber = this.NoteNameListManager.NoteNameList.Count;
        }

        public List<ObservableCollection<IToggleButtonNote>> GenerateNoteMap()
        {
            var noteMap = new List<ObservableCollection<IToggleButtonNote>>();
            for (var lineIndex = 0; lineIndex < this._lineNumber; lineIndex++)
                noteMap.Add(NoteMapLineGenerator(lineIndex));
            return noteMap;
        }

        private ObservableCollection<IToggleButtonNote> NoteMapLineGenerator(int lineIndex)
        {
            var newLine = new ObservableCollection<IToggleButtonNote>();
            var note = this.NoteNameListManager.NoteNameList[this.NoteNameListManager.NoteNameList.Keys.ElementAt(lineIndex)];

            for (var columnIndex = 0; columnIndex < this._columnNumberToAdd; columnIndex++)
                newLine.Add(new ToggleButtonNote() { LineIndex = lineIndex, ColumnIndex = columnIndex, Note = note });
            return newLine;
        }

        public void AddColumnsToThisNoteMap(IList<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            if (noteMap == null || noteMap[0].Count >= 200)
                return;
            for (var lineIndex = 0; lineIndex < this._lineNumber; lineIndex++)
                for (var columnIndex = 0; columnIndex < this._columnNumberToAdd; columnIndex++)
                    noteMap[lineIndex].Add(new ToggleButtonNote() { LineIndex = lineIndex, ColumnIndex = columnIndex, Note = this.NoteNameListManager.NoteNameList[this.NoteNameListManager.NoteNameList.Keys.ElementAt(lineIndex)]});
        }

        public void RemoveAColumnFromThisNoteMap(IList<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            if (noteMap == null || noteMap[0].Count <= 1)
                return;
            for (var lineIndex = 0; lineIndex < this._lineNumber; lineIndex++)
                noteMap[lineIndex].RemoveAt(noteMap[lineIndex].Count - 1);
        }
    }
}
