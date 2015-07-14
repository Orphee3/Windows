using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Shapes;
using Midi;
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

        private static Dictionary<int, List<Note>> SwitchNoteListLinesAndColumns(IList<ObservableCollection<IToggleButtonNote>> noteList)
        {
            var noteListSwitched = new Dictionary<int, List<Note>>();

            for (var columnNumber = 0; columnNumber < noteList[0].Count; columnNumber++)
            {
                noteListSwitched.Add(columnNumber, new List<Note>());
                foreach (var line in noteList)
                {
                    if (line[columnNumber].IsChecked)
                        noteListSwitched[columnNumber].Add(line[columnNumber].Note);
                }
            }
            return noteListSwitched;
        }

        private static void UpdateOrpheeNoteMesageList(ICollection<IOrpheeNoteMessage> orpheeNoteMessageList, IList<IToggleButtonNote> extractedToggleButtonNotes, int channel, int deltaTime)
        {
            for (var iterator = 0; iterator < extractedToggleButtonNotes.Count; iterator++)
                orpheeNoteMessageList.Add(new OrpheeNoteMessage() { Channel = channel, DeltaTime = (iterator == 0) ? deltaTime : 0, MessageCode = 0x90, Note = extractedToggleButtonNotes[iterator].Note, Velocity = 76} );
            for (var iterator = 0; iterator < extractedToggleButtonNotes.Count; iterator++)
                orpheeNoteMessageList.Add(new OrpheeNoteMessage() { Channel = channel, DeltaTime = (iterator == 0) ? 48 : 0, MessageCode = 0x80, Note = extractedToggleButtonNotes[iterator].Note, Velocity = 0 });
        }

        private IList<IToggleButtonNote> ExtractToggleButtonNotesFromNoteMapColumn(int columnIndex, IList<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            var toggleButtonNoteList = new List<IToggleButtonNote>();

            for (var lineIndex = 0; lineIndex < this.NoteNameListManager.NoteNameList.Count; lineIndex++)
                if (noteMap[lineIndex][columnIndex].IsChecked)
                    toggleButtonNoteList.Add(noteMap[lineIndex][columnIndex]);
            return toggleButtonNoteList.Count == 0 ? null : toggleButtonNoteList;
        }

        public IList<IOrpheeNoteMessage> ConvertNoteMapToOrpheeMessageList(IList<ObservableCollection<IToggleButtonNote>> noteMap, int channel)
        {
            var orpheeNoteMessageList = new List<IOrpheeNoteMessage>();
            var deltaTime = 0;

            for (var columnIndex = 0; columnIndex < noteMap[0].Count; columnIndex++)
            {
                var extractedToggleButtonNotes = ExtractToggleButtonNotesFromNoteMapColumn(columnIndex, noteMap);
                if (extractedToggleButtonNotes != null)
                    UpdateOrpheeNoteMesageList(orpheeNoteMessageList, extractedToggleButtonNotes, channel, deltaTime);
                deltaTime = (extractedToggleButtonNotes == null) ? (deltaTime + 48) : 0;
            }
            return orpheeNoteMessageList;
        }
    }
}
