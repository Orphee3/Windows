using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Orphee.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreationInfoPage : IView
    {
        public CreationInfoPage()
        {
            this.InitializeComponent();
        }

        public void UserPicture_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var channel = sender;
            App.MyNavigationService.Navigate("ChannelInfo", channel);
        }
    }
}
