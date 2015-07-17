using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class NoteMapManager
    {
        private static NoteMapManager _instance;
        private int _columnNumberToAdd;
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

        private uint UpdateOrpheeNoteMesageList(ICollection<IOrpheeNoteMessage> orpheeNoteMessageList, IList<IToggleButtonNote> extractedToggleButtonNotes, int channel, int deltaTime)
        {
            var bytesToAdd = 0;
            bytesToAdd += (deltaTime > 127 ? (deltaTime > 2097151 ? 2 : 1) : 0);
            bytesToAdd += extractedToggleButtonNotes.Count * 8;
            for (var iterator = 0; iterator < extractedToggleButtonNotes.Count; iterator++)
                orpheeNoteMessageList.Add(new OrpheeNoteMessage() { Channel = channel, DeltaTime = (iterator == 0) ? deltaTime : 0, MessageCode = 0x90, Note = extractedToggleButtonNotes[iterator].Note, Velocity = 76} );
            for (var iterator = 0; iterator < extractedToggleButtonNotes.Count; iterator++)
                orpheeNoteMessageList.Add(new OrpheeNoteMessage() { Channel = channel, DeltaTime = (iterator == 0) ? 48 : 0, MessageCode = 0x80, Note = extractedToggleButtonNotes[iterator].Note, Velocity = 0 });
            
            return (uint)bytesToAdd;
        }

        private IList<IToggleButtonNote> ExtractToggleButtonNotesFromNoteMapColumn(int columnIndex, IList<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            var toggleButtonNoteList = new List<IToggleButtonNote>();

            for (var lineIndex = 0; lineIndex < this.NoteNameListManager.NoteNameList.Count; lineIndex++)
                if (noteMap[lineIndex][columnIndex].IsChecked)
                    toggleButtonNoteList.Add(noteMap[lineIndex][columnIndex]);
            return toggleButtonNoteList.Count == 0 ? null : toggleButtonNoteList;
        }

        private void MakeTheLastNoteLonger(IList<IOrpheeNoteMessage> orpheeNoteMessageList)
        {
            var orpheeNoteMessageListLastIndex = orpheeNoteMessageList.Count - 1;

            for (var iterator = orpheeNoteMessageListLastIndex - 1; iterator >= 0; iterator--)
            {
                if ((orpheeNoteMessageList[iterator].MessageCode & 0x90) == 0x90)
                {
                    orpheeNoteMessageList[iterator + 1].DeltaTime += 48;
                    return;
                }
            }
        }

        public IList<IOrpheeNoteMessage> ConvertNoteMapToOrpheeNoteMessageList(IList<ObservableCollection<IToggleButtonNote>> noteMap, int channel, ref uint trackLength)
        {
            var orpheeNoteMessageList = new List<IOrpheeNoteMessage>();
            var deltaTime = 0;

            for (var columnIndex = 0; columnIndex < noteMap[0].Count; columnIndex++)
            {
                var extractedToggleButtonNotes = ExtractToggleButtonNotesFromNoteMapColumn(columnIndex, noteMap);
                if (extractedToggleButtonNotes != null)
                   trackLength += UpdateOrpheeNoteMesageList(orpheeNoteMessageList, extractedToggleButtonNotes, channel, deltaTime);
                deltaTime = (extractedToggleButtonNotes == null) ? (deltaTime + 48) : 0;
            }
            MakeTheLastNoteLonger(orpheeNoteMessageList);
            return orpheeNoteMessageList;
        }

        public IList<ObservableCollection<IToggleButtonNote>> ConvertOrpheeMessageListToNoteMap(IList<IOrpheeNoteMessage> orpheeNoteMessageLists)
        {
            this._columnNumberToAdd = orpheeNoteMessageLists.Sum(message => message.DeltaTime / 48);
            var noteMap = GenerateNoteMap();
            var columnIndex = 0;

            foreach (var noteMessage in orpheeNoteMessageLists)
            {
                var lineIndex = NoteNameListManager.GetLineIndexFromNote(noteMessage.Note);
                columnIndex += GetColumnIndexFromDeltaTime(noteMessage.DeltaTime);
                if ((noteMessage.MessageCode & 0x90) == 0x90)
                {
                    noteMap[lineIndex][columnIndex].IsChecked = true;
                    noteMap[lineIndex][columnIndex].Note = noteMessage.Note;
                }
            }
            this._columnNumberToAdd = 10;
            return noteMap;
        }

        private int GetColumnIndexFromDeltaTime(int deltaTime)
        {
            return deltaTime / 48;
        }
    }
}
