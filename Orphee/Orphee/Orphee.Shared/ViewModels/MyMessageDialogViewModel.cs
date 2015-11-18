using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.CreationShared.Interfaces;

namespace Orphee.ViewModels
{
    public class MyMessageDialogViewModel : ViewModel
    {
        private TaskCompletionSource<bool> _taskCompletionSource;
        public DelegateCommand ValidationCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public IInstrumentManager InstrumentManager { get; private set; }
        public IColorManager ColorManager { get; private set; }
        public IOrpheeTrack CurrentOrpheeTrack { get; private set; }
        private int _trackColorIndex;
        public int TrackColorIndex
        {
            get { return this._trackColorIndex; }
            set
            {
                if (this._trackColorIndex != value)
                    SetProperty(ref this._trackColorIndex, value);
            }
        }
        private int _trackInstrumentIndex;
        public int TrackInstrumentIndex
        {
            get { return this._trackInstrumentIndex; }
            set
            {
                if (this._trackInstrumentIndex != value)
                    SetProperty(ref this._trackInstrumentIndex, value);
            }
        }
        private bool _isOpen;
        public bool IsOpen
        {
            get { return this._isOpen; }
            set
            {
                if (this._isOpen != value)
                    SetProperty(ref this._isOpen, value);
            }
        }

        public MyMessageDialogViewModel(IInstrumentManager instrumentManager, IColorManager colorManager)
        {
            this.CancelCommand = new DelegateCommand(Close);
            this.ValidationCommand = new DelegateCommand(UpdateTrackContent);
            this.InstrumentManager = instrumentManager;
            this.ColorManager = colorManager;
        }

        public void SetCurrentOrpheeTrack(IOrpheeTrack orpheeTrack)
        {
            this.CurrentOrpheeTrack = orpheeTrack;
            this.TrackColorIndex = this.ColorManager.GetColorIndex(orpheeTrack.GetTrackColor());
            this.TrackInstrumentIndex = this.InstrumentManager.GetInstrumentIndex(orpheeTrack.CurrentInstrument);
            this.IsOpen = true;
        }

        public Task<bool> ShowAsync()
        {
            this._taskCompletionSource = new TaskCompletionSource<bool>();

            this.IsOpen = true;
            return this._taskCompletionSource.Task;
        }

        public void UpdateTrackContent()
        {
            this.CurrentOrpheeTrack.UpdateCurrentInstrument(this.InstrumentManager.InstrumentList[this.TrackInstrumentIndex].Instrument);
            this.CurrentOrpheeTrack.SetTrackColor(this.ColorManager.ColorList[this.TrackColorIndex]);
            Close();
        }

        private void Close()
        {
            this.IsOpen = false;
            this._taskCompletionSource.SetResult(true);
        }
    }
}
