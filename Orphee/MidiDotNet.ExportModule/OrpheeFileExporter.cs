using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Senders.Interfaces;

namespace MidiDotNet.ExportModule
{
    /// <summary>
    /// Class creating the MIDI file and managing all the writers and converting
    /// the view noteMessage data so that the messsages are writen accordingly to the MIDI convention
    /// </summary>
    public class OrpheeFileExporter : IOrpheeFileExporter
    {
        private BinaryWriter _writer;
        private StorageFile _storageFile;
        private readonly IChunckWriters _chunckWriters;
        private readonly IFileUploader _fileUploader;
        private readonly INoteMapGenerator _noteMapGenerator;
        private readonly IFilePickerManager _filePickerManager;

        /// <summary>
        /// Constructor initializing fileHeaderWriter, 
        /// trackHeaderWriter, noteMessageWriter and fileUploader through dependency injection
        /// </summary>
        /// <param name="fileHeaderWriter">Instance of the FileHeaderWriter class used to write the file header in the MIDI file</param>
        /// <param name="trackHeaderWriter">Instance of the TrackHeaderWriter class used to write the track header of each track in the MIDI file</param>
        /// <param name="noteMessageWriter">Instance of the NoteMessageWriter class used to write the noteMessage messages in the MIDI file</param>
        /// <param name="fileUploader">Instance of the FileUploader class used to send the saved MIDI file to the remote server</param>
        public OrpheeFileExporter(IChunckWriters chunckWriters, IFileUploader fileUploader, INoteMapGenerator noteMapGenerator, IFilePickerManager filePickerManager)
        {
            this._noteMapGenerator = noteMapGenerator;
            this._fileUploader = fileUploader;
            this._chunckWriters = chunckWriters;
            this._filePickerManager = filePickerManager;
        }

        /// <summary>
        /// Function converting view NoteMap to a list of noteMessage usable by
        /// the NoteMessageWriter class
        /// </summary>
        /// <param name="orpheeFile">Instance of the OrpheeFile class containing the graphical representation of the noteMessage messages</param>
        public void ConvertTracksNoteMapToOrpheeNoteMessageList(IOrpheeFile orpheeFile)
        {
            foreach (var track in orpheeFile.OrpheeTrackList)
            {
                var trackLength = track.TrackLength;
                track.OrpheeNoteMessageList = this._noteMapGenerator.ConvertNoteMapToOrpheeNoteMessageList(track.NoteMap, (int) track.Channel, ref trackLength);
            }
        }

        /// <summary>
        /// Function saving the actual piece to the MIDI file
        /// </summary>
        /// <param name="orpheeFile">Instance of the OrpheeFile class containing all the data needed to create the MIDI file</param>
        /// <returns>Retuns a task containing a bool that is true if the file has been saved and false if it hasn't</returns>
        public async Task<bool?> SaveOrpheeFile(IOrpheeFile orpheeFile)
        {
            foreach (var orpheeTrack in orpheeFile.OrpheeTrackList)
            {
                var trackLength = orpheeTrack.TrackLength;
                orpheeTrack.OrpheeNoteMessageList = this._noteMapGenerator.ConvertNoteMapToOrpheeNoteMessageList(orpheeTrack.NoteMap, (int) orpheeTrack.Channel, ref trackLength);
                orpheeTrack.TrackLength = trackLength;
            }
            this._storageFile = await this._filePickerManager.GetTheSaveFilePicker(orpheeFile);
            if (this._storageFile == null)
                return false;
            CachedFileManager.DeferUpdates(this._storageFile);
            WriteEventsInFile(orpheeFile);
            var result = await this._fileUploader.UploadFile(this._storageFile);
            if (!result)
                return null;
            foreach (var orpheeTrack in orpheeFile.OrpheeTrackList)
                orpheeTrack.TrackLength = (uint) ((orpheeTrack.TrackPos == 0) ? 22 : 7);
            return true;
        }

        private void WriteEventsInFile(IOrpheeFile orpheeFile)
        {
            using (this._writer = new BinaryWriter(this._storageFile.OpenStreamForWriteAsync().Result))
            {
                this._chunckWriters.FileHeaderWriter.WriteFileHeader(this._writer, orpheeFile.OrpheeFileParameters);
                foreach (var orpheeTrack in orpheeFile.OrpheeTrackList)
                {
                    this._chunckWriters.TrackHeaderWriter.WriteTrackHeader(this._writer, orpheeTrack.PlayerParameters, orpheeTrack.TrackLength);
                    this._chunckWriters.NoteMessageWriter.WriteNoteMessages(this._writer, orpheeTrack.OrpheeNoteMessageList, (int)orpheeTrack.Channel, orpheeTrack.CurrentInstrument);
                }
            }
        }
    }
}
