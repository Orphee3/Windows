﻿using Microsoft.Practices.Prism.Mvvm;

namespace Orphee.Views
{
    public sealed partial class FriendsPage : IView
    {
        public FriendsPage()
        {
            this.InitializeComponent();
            this.MyBottomAppBar = App.MyNavigationService.MyBottomAppBar;
        }
    }
}