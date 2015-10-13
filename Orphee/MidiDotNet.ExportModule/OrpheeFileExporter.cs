﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Senders.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class OrpheeFileExporter : IOrpheeFileExporter
    {
        private BinaryWriter _writer;
        private StorageFile _storageFile;
        private readonly IFileHeaderWriter _fileHeaderWriter;
        private readonly ITrackHeaderWriter _trackHeaderWriter;
        private readonly INoteMessageWriter _noteMessageWriter;
        private readonly IFileUploader _fileUploader;

        public OrpheeFileExporter(IFileHeaderWriter fileHeaderWriter, ITrackHeaderWriter trackHeaderWriter, INoteMessageWriter noteMessageWriter, IFileUploader fileUploader)
        {
            this._fileUploader = fileUploader;
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

        public async Task<bool> SaveOrpheeFile(IOrpheeFile orpheeFile)
        {
            foreach (var orpheeTrack in orpheeFile.OrpheeTrackList)
            {
                var trackLength = orpheeTrack.TrackLength;
                orpheeTrack.OrpheeNoteMessageList = NoteMapManager.Instance.ConvertNoteMapToOrpheeNoteMessageList(orpheeTrack.NoteMap, (int) orpheeTrack.Channel, ref trackLength);
                orpheeTrack.TrackLength = trackLength;
            }
            var result = await GetTheSaveFilePicker(orpheeFile);
            foreach (var orpheeTrack in orpheeFile.OrpheeTrackList)
                orpheeTrack.TrackLength = (uint) ((orpheeTrack.TrackPos == 0) ? 22 : 7);
            return result;
        }

        private async Task<bool> GetTheSaveFilePicker(IOrpheeFile orpheeFile)
        {
            var savePicker = new FileSavePicker()
            {
                SuggestedStartLocation = PickerLocationId.MusicLibrary,
                SuggestedFileName = orpheeFile.FileName,
            };
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".mid" });
            this._storageFile = await savePicker.PickSaveFileAsync();
            if (this._storageFile != null)
            {
                CachedFileManager.DeferUpdates(this._storageFile);
                WriteEventsInFile(orpheeFile);
                //var result = await this._fileUploader.UploadFile(this._storageFile);
                return true;
            }
            return false;
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
