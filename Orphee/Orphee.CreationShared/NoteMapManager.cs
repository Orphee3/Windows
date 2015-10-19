﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Singleton class managing the OrpheeTrack NoteMap
    /// </summary>
    public class NoteMapManager
    {
        private static NoteMapManager _instance;
        private int _columnNumberToAdd;
        private int _lineNumberToAdd;
        /// <summary>Instance of the actual NoteMapManager </summary>
        public static NoteMapManager Instance 
        {
            get
            {
                if (_instance == null)
                    _instance = new NoteMapManager();
                return _instance;
            }
        }
        /// <summary>Instance of the NoteNameListManager class</summary>
        public NoteNameListManager NoteNameListManager { get; private set; }

        private NoteMapManager()
        {
            this.NoteNameListManager = new NoteNameListManager();
            this._columnNumberToAdd = 500;
            this._lineNumberToAdd = 60;
        }

        public ObservableCollection<MyRectangle> GenerateColumnMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            var columnMap = new ObservableCollection<MyRectangle>();
            for (var columnIndex = 0; columnIndex < noteMap[0].Count; columnIndex++)
            {
                for (var lineIndex = 0; lineIndex < noteMap.Count; lineIndex++)
                {
                    if (noteMap[lineIndex][columnIndex].IsChecked)
                    {
                        columnMap.Add(new MyRectangle() {RectangleBackgroundColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0xC7, 0xF9)), IsSelectionRectangleVisible = Visibility.Collapsed});
                        break;
                    }
                    else if (lineIndex == noteMap.Count -1)
                        columnMap.Add(new MyRectangle() { RectangleBackgroundColor = new SolidColorBrush(Colors.Gray), IsSelectionRectangleVisible = Visibility.Collapsed });
                }
            }
            return columnMap;
        }

        public bool IsColumnEmpty(int columnIndex, ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            return noteMap.All(line => !line[columnIndex].IsChecked);
        }

        /// <summary>
        /// Generates a new note map for the calling orpheeTrack
        /// </summary>
        /// <param name="startingOctave">Starting octave of the generated note map</param>
        /// <returns></returns>
        public ObservableCollection<ObservableCollection<IToggleButtonNote>> GenerateNoteMap(int startingOctave)
        {
            var noteMap = new ObservableCollection<ObservableCollection<IToggleButtonNote>>();
            for (var lineIndex = 0; lineIndex < this._lineNumberToAdd; lineIndex++)
                noteMap.Add(NoteMapLineGenerator(lineIndex, startingOctave));
            this._lineNumberToAdd = 60;
            return noteMap;
        }

        private ObservableCollection<IToggleButtonNote> NoteMapLineGenerator(int lineIndex, int octaveNumber)
        {
            var newLine = new ObservableCollection<IToggleButtonNote>();
            var note = this.NoteNameListManager.NoteNameList[this.NoteNameListManager.NoteNameList.Keys.ElementAt(lineIndex + (12 * octaveNumber))];

            for (var columnIndex = 0; columnIndex < this._columnNumberToAdd; columnIndex++)
                newLine.Add(new ToggleButtonNote() { LineIndex = lineIndex, ColumnIndex = columnIndex, Note = note });
            return newLine;
        }

        /// <summary>
        /// Adds a higher octave to the given note map
        /// </summary>
        /// <param name="noteMap">note map in which the octave is to be added</param>
        public void AddOneHigherOctaveToThisNoteMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            int octaveNumber;
            if (noteMap == null || (octaveNumber = this.NoteNameListManager.CanAddHigherOctave(noteMap.Last()[0].Note)) == -1)
                return;
            this._columnNumberToAdd = noteMap[0].Count;
            for (var lineIndex = 0; lineIndex < 12; lineIndex++)
                noteMap.Add(NoteMapLineGenerator(lineIndex, octaveNumber));
        }

        /// <summary>
        /// Adds a lower octave to the given note map
        /// </summary>
        /// <param name="noteMap">note map in which the octave is to be added</param>
        public void AddOneLowerOctaveToThisNoteMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            int octaveNumber;
            if (noteMap == null || (octaveNumber = this.NoteNameListManager.CanAddLowerOctave(noteMap.First()[0].Note)) == -1)
                return;
            this._columnNumberToAdd = noteMap[0].Count;
            for (var lineIndex = 0; lineIndex < 12; lineIndex++)
                noteMap.Insert(lineIndex, NoteMapLineGenerator(lineIndex, octaveNumber));
        }

        /// <summary>
        /// Add columns to the given note map
        /// </summary>
        /// <param name="noteMap">note map in which the columns are to be added</param>
        public void AddColumnsToThisNoteMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            if (noteMap == null || noteMap[0].Count >= 200)
                return;
            for (var lineIndex = 0; lineIndex < noteMap.Count; lineIndex++)
                for (var columnIndex = 0; columnIndex < this._columnNumberToAdd; columnIndex++)
                    noteMap[lineIndex].Add(new ToggleButtonNote() { LineIndex = lineIndex, ColumnIndex = columnIndex, Note = noteMap[lineIndex][0].Note });
        }

        /// <summary>
        /// Removes columns to the given note map
        /// </summary>
        /// <param name="noteMap">note map in which the columns are to be removed</param>
        public void RemoveAColumnFromThisNoteMap(ObservableCollection<ObservableCollection<IToggleButtonNote>> noteMap)
        {
            if (noteMap == null || noteMap[0].Count <= 1)
                return;
            for (var lineIndex = 0; lineIndex < noteMap.Count; lineIndex++)
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

            for (var lineIndex = 0; lineIndex < noteMap.Count; lineIndex++)
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

        /// <summary>
        /// Converts the OrpheeTrack's NoteMap to a list of OrpheeNoteMessage
        /// </summary>
        /// <param name="noteMap">Note map to be converted</param>
        /// <param name="channel">Channel related to the note map</param>
        /// <param name="trackLength">Length of the OrpheeTrack containing the given note map</param>
        /// <returns>Returns a list of OrpheeNoteMessage</returns>
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

        /// <summary>
        /// Converts the given orpheeNoteMessage list to a note map
        /// </summary>
        /// <param name="orpheeNoteMessageLists">List of OrpheeNoteMessage contained in the calling OrpheeTrack</param>
        /// <returns>Return a note map</returns>
        public ObservableCollection<ObservableCollection<IToggleButtonNote>> ConvertOrpheeMessageListToNoteMap(IList<IOrpheeNoteMessage> orpheeNoteMessageLists)
        {
            this._columnNumberToAdd = orpheeNoteMessageLists.Sum(message => message.DeltaTime / 48);
            var lowerOctave = ((int) orpheeNoteMessageLists.Min(message => message.Note) / 12) - 1;
            var higherOctave = ((int) orpheeNoteMessageLists.Max(message => message.Note) / 12) - 1;
            this._lineNumberToAdd = ((higherOctave - lowerOctave) + 1) * 12;
            var noteMap = GenerateNoteMap(lowerOctave);
            var columnIndex = 0;

            foreach (var noteMessage in orpheeNoteMessageLists)
            {
                var lineIndex = NoteNameListManager.GetNoteLineIndex(noteMessage.Note, lowerOctave);
                columnIndex += GetColumnIndexFromDeltaTime(noteMessage.DeltaTime);
                if ((noteMessage.MessageCode & 0x90) == 0x90)
                {
                    noteMap[lineIndex][columnIndex].IsChecked = true;
                    noteMap[lineIndex][columnIndex].Note = noteMessage.Note;
                }
            }
            this._columnNumberToAdd = 50;
            return noteMap;
        }

        private int GetColumnIndexFromDeltaTime(int deltaTime)
        {
            return deltaTime / 48;
        }
    }
}
