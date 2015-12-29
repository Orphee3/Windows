using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Midi;
using MidiDotNet.ImportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule
{
    /// <summary>
    /// Class containing all the needed functions in order
    /// to import a MIDI file and converting its content 
    /// so that it can be used by the program
    /// </summary>
    [DataContract]
    public class OrpheeFileImporter : IOrpheeFileImporter
    {
        private readonly IFileHeaderReader _fileHeaderReader;
        private readonly ITrackHeaderReader _trackHeaderReader;
        private readonly INoteMessageReader _noteMessageReader;
        private BinaryReader _reader;
        /// <summary>File representing the actual MIDI file in the program </summary>
        public IOrpheeFile OrpheeFile { get; private set; }
        /// <summary>File representing the actual MIDI file in the device </summary>
        public IStorageFile StorageFile { get; set; }
        /// <summary>String representing the name of the actual MIDI file</summary>
        public string FileName { get; set; }

        /// <summary>
        /// Constructor initializing fileHeaderReader, trackHeaderReader
        /// and noteMessageReader through dependency injection.
        /// </summary>
        /// <param name="fileHeaderReader">Instance of the FileHeaderReader class used to read the MIDI file header</param>
        /// <param name="trackHeaderReader">Instance of the TrackHeaderReader class used to read the track header of each track</param>
        /// <param name="noteMessageReader">Instance of the NoteMessageReader class used to read the noteMessages in the MIDI file</param>
        public OrpheeFileImporter(IFileHeaderReader fileHeaderReader, ITrackHeaderReader trackHeaderReader, INoteMessageReader noteMessageReader)
        {
            this._fileHeaderReader = fileHeaderReader;
            this._trackHeaderReader = trackHeaderReader;
            this._noteMessageReader = noteMessageReader;
        }

        /// <summary>
        /// Function importing the MIDI file and converting
        /// it so it can be used in the program
        /// </summary>
        /// <param name="fileType">Value representing the actual MIDI file type</param>
        /// <returns>Returns the imported MIDI file or null if a problem occured</returns>
        public async Task<IOrpheeFile> ImportFile()
        {
            var result = await GetTheOpenFilePicker();
            return result ? this.OrpheeFile : null;
        }

        public async Task<IOrpheeFile> ImportFileFromNet(string filePath, string fileName)
        {
            var file = await KnownFolders.MusicLibrary.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
            var downloader = new BackgroundDownloader();
            var download = downloader.CreateDownload(new Uri(filePath), file);
            DownloadOperation result;
            try
            {
                result = await download.StartAsync();
            }
            catch (Exception)
            {
                return null;
            }

            if (result.ResultFile == null)
                return null;
            this.StorageFile = result.ResultFile;
            var result2 = ReadFileMessages();
            return result2 ? this.OrpheeFile : null;
        }

        private async Task<bool> GetTheOpenFilePicker()
        {
            var openPicker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.MusicLibrary,
            };
            openPicker.FileTypeFilter.Add(".mid");
            this.StorageFile = await openPicker.PickSingleFileAsync();
            if (this.StorageFile != null)
                return ReadFileMessages();
            return false;
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
            var newOrpheeTrack = new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager())
            {
                OrpheeNoteMessageList = this._noteMessageReader.OrpheeNoteMessageList,
                TrackLength = this._trackHeaderReader.TrackLength,
            };
            newOrpheeTrack.Init(trackPos, (Channel)this._trackHeaderReader.ProgramChangeMessageReader.Channel, false);
            if (trackPos == 0)
                newOrpheeTrack.PlayerParameters = this._trackHeaderReader.PlayerParameters;
            newOrpheeTrack.UpdateCurrentInstrument((Instrument)this._trackHeaderReader.ProgramChangeMessageReader.InstrumentIndex);
            this.OrpheeFile.OrpheeTrackList.Add(newOrpheeTrack);
        }

        private void InitializeOrpheeFile()
        {
            this.OrpheeFile = new OrpheeFile(new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager()))
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
