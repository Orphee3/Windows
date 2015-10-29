using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Orphee.ViewModels
{
    public class MyDeleteAccountMessageDialogViewModel : ViewModel
    {
        private TaskCompletionSource<bool> _taskCompletionSource;
        private string _deleteMessage;

        public string DeleteMessage
        {
            get { return this._deleteMessage; }
            set
            {
                if (this._deleteMessage != value)
                    SetProperty(ref this._deleteMessage, value);
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
        public DelegateCommand ValidationCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public MyDeleteAccountMessageDialogViewModel()
        {
            this.ValidationCommand = new DelegateCommand(ValidationCommandExec);
            this.CancelCommand = new DelegateCommand(CancelCommandExec);
        }

        public Task<bool> ShowAsync()
        {
            this._taskCompletionSource = new TaskCompletionSource<bool>();

            this.IsOpen = true;
            return this._taskCompletionSource.Task;
        }

        private void ValidationCommandExec()
        {
           if (this.DeleteMessage == "DELETE")
                Close();
        }

        private void CancelCommandExec()
        {
            Close();
        }

        private void Close()
        {
            this.IsOpen = false;
            this._taskCompletionSource.SetResult(true);
        }
    }
}
