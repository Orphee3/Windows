using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class OrpheeFileExporter : IOrpheeFileExporter
    {
        private BinaryWriter _writer;
        private StorageFile _storageFile;
        private readonly IFileHeaderWriter _fileHeaderWriter;
        private readonly ITrackHeaderWriter _trackHeaderWriter;
        private readonly INoteMessageWriter _noteMessageWriter;

        public OrpheeFileExporter(IFileHeaderWriter fileHeaderWriter, ITrackHeaderWriter trackHeaderWriter, INoteMessageWriter noteMessageWriter)
        {
            this._fileHeaderWriter = fileHeaderWriter;
            this._trackHeaderWriter = trackHeaderWriter;
            this._noteMessageWriter = noteMessageWriter;
        }

        public void ConvertTracksNoteMapToOrpheeNoteMessageList(IOrpheeFile orpheeFile)
        {
            foreach (var track in orpheeFile.OrpheeTrackList)
            {
                var trackLength = track.TrackLength;
                track.OrpheeNoteMessageList = NoteMapManager.Instance.ConvertNoteMapToOrpheeNoteMessageList(track.NoteMap, (int) track.Channel, ref trackLength);
            }
        }

        public bool SaveOrpheeFile(IOrpheeFile orpheeFile)
        {
            return false;
        }

        private async Task<bool> GetTheSaveFilePicker(IOrpheeFile orpheeFile)
        {
            var savePicker = new FileSavePicker()
            {
                SuggestedStartLocation = PickerLocationId.MusicLibrary,
                SuggestedFileName = orpheeFile.FileName,
            };
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { orpheeFile.FileName.Split('.')[1] == "loop" ? ".loop" : ".orph" });
            this._storageFile = await savePicker.PickSaveFileAsync();
            if (this._storageFile != null)
            {
                CachedFileManager.DeferUpdates(this._storageFile);
                WriteEventsInFile(orpheeFile);
                return true;
            }
            return false;
        }

        public async void SaveOrpheeTrack(IOrpheeTrack orpheeTrack)
        {
            var trackLength = orpheeTrack.TrackLength;
            orpheeTrack.OrpheeNoteMessageList = NoteMapManager.Instance.ConvertNoteMapToOrpheeNoteMessageList(orpheeTrack.NoteMap, (int) orpheeTrack.Channel, ref trackLength);
            orpheeTrack.TrackLength = trackLength;
            var orpheeFile = new OrpheeFile()
            {
                FileName = "loop1.loop",
            };
            orpheeFile.AddNewTrack(orpheeTrack);
            orpheeFile.UpdateOrpheeFileParameters();
            var result2 = await GetTheSaveFilePicker(orpheeFile);
            orpheeTrack.TrackLength = (uint) (orpheeTrack.TrackPos == 0 ? 22 : 7);
        }

        private void WriteEventsInFile(IOrpheeFile orpheeFile)
        {
            using (this._writer = new BinaryWriter(this._storageFile.OpenStreamForWriteAsync().Result))
            {
                this._fileHeaderWriter.WriteFileHeader(this._writer, orpheeFile.OrpheeFileParameters);
                foreach (var orpheeTrack in orpheeFile.OrpheeTrackList)
                {
                    this._trackHeaderWriter.WriteTrackHeader(this._writer, orpheeTrack.PlayerParameters, orpheeTrack.TrackLength);
                    this._noteMessageWriter.WriteNoteMessages(this._writer, orpheeTrack.OrpheeNoteMessageList, (int)orpheeTrack.Channel, orpheeTrack.CurrentInstrument);
                }
            }
        }
    }
}
