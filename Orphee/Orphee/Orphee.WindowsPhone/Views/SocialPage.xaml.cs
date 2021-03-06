﻿using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;

namespace Orphee.Views
{
    public sealed partial class SocialPage : IView
    {
        public SocialPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.MyBottomAppBar.Unload();
        }

        private void User_OnItemClick(object sender, ItemClickEventArgs e)
        { 
            var user = e.ClickedItem as UserBase;
            App.MyNavigationService.Navigate("ChannelInfo", JsonConvert.SerializeObject(user));
        }
    }
}
