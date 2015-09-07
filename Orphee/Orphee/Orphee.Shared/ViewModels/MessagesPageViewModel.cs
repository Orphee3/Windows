using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MidiDotNet.ExportModule;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class MessagesPageViewModel : ViewModel, IMessagesPageViewModel
    {
        public ObservableCollection<JToken> ConversationList { get; set; }
        public DelegateCommand LoginButton { get; private set; }
        public DelegateCommand RegisterButton { get; private set; }
        public DelegateCommand AddCommand { get; private set; }
        public string DisconnectedMessage { get; private set; }
        public Visibility ButtonsVisibility { get; private set; }
        public Visibility ListViewVisibility { get; private set; }

        public MessagesPageViewModel()
        {
            this.DisconnectedMessage = "To access the message functionnality you have \nto login or to create an account";
            this.ButtonsVisibility = Visibility.Visible;
            this.ListViewVisibility = Visibility.Collapsed;
            this.ConversationList = new ObservableCollection<JToken>()
            {
                new JValue("Les 4 cochons a Pekin"),
                new JValue("Le retour des violeur de poney mort"),
                new JValue("Introduction à la maçonnerie"),
            };
            if (RestApiManagerBase.Instance.IsConnected)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
            }
            this.AddCommand = new DelegateCommand(AddCommandExec);
            this.LoginButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.RegisterButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Register", null));
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Messages";
            if (RestApiManagerBase.Instance.IsConnected)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
            }
            else
            {
                this.ButtonsVisibility = Visibility.Visible;
                this.ListViewVisibility = Visibility.Collapsed;
            }
        }

        private void AddCommandExec()
        {
            
        }
    }
}
