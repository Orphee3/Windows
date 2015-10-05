﻿using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;
using Orphee.RestApiManagement.Getters.Interfaces;
using System.Collections.Generic;

namespace Orphee.ViewModels
{
    public class MyCreationsPageViewModel : ViewModel, IMyCreationsPageViewModel
    {
        public ObservableCollection<Creation> CreationList { get; set; }
        public DelegateCommand BackCommand { get; private set; }
        private readonly IGetter _getter;

        public MyCreationsPageViewModel(IGetter getter)
        {
            this._getter = getter;
            InitCreationList();
            this.CreationList = new ObservableCollection<Creation>();
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        private async void InitCreationList()
        {
            if (RestApiManagerBase.Instance.IsConnected &&
                RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                var result = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/creation");
                foreach (var creation in result)
                {
                    creation.Name = creation.Name.Split('.')[0];
                    this.CreationList.Add(creation);
                }
            }
        }
    }
}
