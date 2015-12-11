using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels
{
    public class CreationPageMenuMessageDialogViewModel : ViewModel
    {
        private TaskCompletionSource<bool> _taskCompletionSource;
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

        public ObservableCollection<Creation> GroupList { get; private set; } 
        public DelegateCommand RadioButtonCommand { get; private set; }
        public DelegateCommand ValidationCommand { get; private set; }
        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return this._selectedIndex; }
            set
            {
                if (this._selectedIndex != value)
                    SetProperty(ref this._selectedIndex, value);
            }
        }
        public bool IsSoloRadioButtonChecked { get; private set; }
        public bool IsCollaborateRadioButtonChecked { get; private set; }
        private Visibility _groupListvisibility;

        public Visibility GroupListVisibility
        {
            get { return this._groupListvisibility; }
            set
            {
                if (this._groupListvisibility != value)
                    SetProperty(ref this._groupListvisibility, value);
            }
        }

        public CreationPageMenuMessageDialogViewModel()
        {
            this.GroupList = new ObservableCollection<Creation>()
            {
                new Creation() {Name = "The Extreme Intro"},
                new Creation() {Name = "Force Your Way"},
                new Creation() {Name = "Create a new group" }
            };
            this.GroupListVisibility = Visibility.Collapsed;
            this.IsCollaborateRadioButtonChecked = false;
            this.IsSoloRadioButtonChecked = true;
            this.RadioButtonCommand = new DelegateCommand(RadioButtonCommandExec);
            this.ValidationCommand = new DelegateCommand(Close);
        }

        public Task<bool> ShowAsync()
        {
            this._taskCompletionSource = new TaskCompletionSource<bool>();

            this.IsOpen = true;
            return this._taskCompletionSource.Task;
        }

        public Creation GetCreationType()
        {
            return this.IsSoloRadioButtonChecked ? null : this.GroupList[this._selectedIndex];
        }

        private void Close()
        {
            this.IsOpen = false;
            this._taskCompletionSource.SetResult(true);
        }

        private void RadioButtonCommandExec()
        {
            if (this.IsCollaborateRadioButtonChecked)
            {
                this.IsCollaborateRadioButtonChecked = false;
                this.IsSoloRadioButtonChecked = true;
                this.GroupListVisibility = Visibility.Collapsed;
            }
            else
            {
                this.IsCollaborateRadioButtonChecked = true;
                this.IsSoloRadioButtonChecked = false;
                this.GroupListVisibility = Visibility.Visible;
            }
        }
    }
}
