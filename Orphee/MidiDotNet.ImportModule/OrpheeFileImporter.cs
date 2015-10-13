using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Midi;
using MidiDotNet.ImportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class OrpheeFileImporter : IOrpheeFileImporter
    {
        private readonly IFileHeaderReader _fileHeaderReader;
        private readonly ITrackHeaderReader _trackHeaderReader;
        private readonly INoteMessageReader _noteMessageReader;
        private BinaryReader _reader;
        public IOrpheeFile OrpheeFile { get; private set; }
        public IStorageFile StorageFile { get; set; }
        public string FileName { get; set; }

        public OrpheeFileImporter(IFileHeaderReader fileHeaderReader, ITrackHeaderReader trackHeaderReader, INoteMessageReader noteMessageReader)
        {
            this._fileHeaderReader = fileHeaderReader;
            this._trackHeaderReader = trackHeaderReader;
            this._noteMessageReader = noteMessageReader;
        }

        private async Task<bool> GetTheOpenFilePicker(string fileType)
        {
            var openPicker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.MusicLibrary,
            };
            openPicker.FileTypeFilter.Add(fileType);
            this.StorageFile = await openPicker.PickSingleFileAsync();
            if (this.StorageFile != null)
                return ReadFileMessages();
            return false;
        }

        public async Task<IOrpheeFile> ImportFile(string fileType)
        {
            var result = await GetTheOpenFilePicker(fileType);
            return result ? this.OrpheeFile : null;
        }

        private bool ReadFileMessages()
        {
            using (this._reader = new BinaryReader(this.StorageFile.OpenStreamForReadAsync().Result))
            {
                if (!this._fileHeaderReader.ReadFileHeader(this._reader))
                    return false;
                InitializeOrpheeFile();
                for (var iterator = 0; iterator < this._fileHeaderReader.NumberOfTracks; iterator++)
                {
                    if (!this._trackHeaderReader.ReadTrackHeader(this._reader, iterator) || !this._noteMessageReader.ReadNoteMessage(this._reader, this._trackHeaderReader.TrackLength))
                        return false;
                    AddNewTrackToOrpheeFile(iterator);
                }
            }
            return true;
        }

        private void AddNewTrackToOrpheeFile(int trackPos)
        {
            var newOrpheeTrack = new OrpheeTrack(trackPos, (Channel)this._trackHeaderReader.ProgramChangeMessageReader.Channel, false)
            {
                OrpheeNoteMessageList = this._noteMessageReader.OrpheeNoteMessageList,
                TrackLength = this._trackHeaderReader.TrackLength,
            };
            if (trackPos == 0)
                newOrpheeTrack.PlayerParameters = this._trackHeaderReader.PlayerParameters;
            newOrpheeTrack.UpdateCurrentInstrument((Instrument)this._trackHeaderReader.ProgramChangeMessageReader.InstrumentIndex);
            this.OrpheeFile.OrpheeTrackList.Add(newOrpheeTrack);
        }

        private void InitializeOrpheeFile()
        {
            this.OrpheeFile = new OrpheeFile()
            {
                FileName = this.StorageFile.Name,
                OrpheeFileParameters = new OrpheeFileParameters()
                {
                    OrpheeFileType = this._fileHeaderReader.FileType,
                    NumberOfTracks = this._fileHeaderReader.NumberOfTracks,
                    DeltaTicksPerQuarterNote = this._fileHeaderReader.DeltaTicksPerQuarterNote,
                },
                OrpheeTrackList = new ObservableCollection<IOrpheeTrack>(),
            };
        }
    }
}
