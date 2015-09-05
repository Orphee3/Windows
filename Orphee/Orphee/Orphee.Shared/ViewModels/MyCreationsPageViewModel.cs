using System.Collections.ObjectModel;
using Windows.Storage;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.FileManagement.Interfaces;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class MyCreationsPageViewModel : ViewModel, IMyCreationsPageViewModel
    {
        public ObservableCollection<IStorageFile> CreationList { get; set; }
        public DelegateCommand BackCommand { get; private set; }
        private readonly IOrpheeFilesGetter _orpheeFilesGetter;

        public MyCreationsPageViewModel(IOrpheeFilesGetter orpheeFilesGetter)
        {
            this._orpheeFilesGetter = orpheeFilesGetter;
            InitCreationList();
            this.CreationList = new ObservableCollection<IStorageFile>();
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        private async void InitCreationList()
        {
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                var result = await this._orpheeFilesGetter.RetrieveOrpheeFiles();
                foreach (var creation in result)
                    this.CreationList.Add(creation);
            }
        }
    }
}
