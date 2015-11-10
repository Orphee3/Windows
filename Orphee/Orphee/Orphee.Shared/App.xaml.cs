using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared;
using MidiDotNet.Shared.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.Models;
using Orphee.Models.Interfaces;
using Orphee.RestApiManagement.Getters;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.RestApiManagement.Posters;
using Orphee.RestApiManagement.Posters.Interfaces;
using Orphee.RestApiManagement.Senders;
using Orphee.RestApiManagement.Senders.Interfaces;
using Orphee.UnityModule;


namespace Orphee
{
    public sealed partial class App : MvvmAppBase
    {
        private IUnityContainer _container;
        private bool _isSuspending;
        public static IMyNavigationService MyNavigationService { get; private set; }
        public static InternetAvailabilityWatcher InternetAvailabilityWatcher { get; private set; }
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif
        public App()
        {
            this.InitializeComponent();
        }

        internal void InitializeMyNavigatorService()
        {
            MyNavigationService = new MyNavigationService(NavigationService);
            InternetAvailabilityWatcher = new InternetAvailabilityWatcher();
        }
   
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        { 
            this.Suspending += OnSuspending;
            MyNavigationService.Navigate("Home", null);
            return Task.FromResult<object>(null);
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            base.OnRegisterKnownTypesForSerialization();

            SessionStateService.RegisterKnownType(typeof(LoggedUser));
            SessionStateService.RegisterKnownType(typeof(List<LoggedUser>));
            SessionStateService.RegisterKnownType(typeof(Creation));
            SessionStateService.RegisterKnownType(typeof(List<Creation>));
            SessionStateService.RegisterKnownType(typeof(Comment));
            SessionStateService.RegisterKnownType(typeof(List<Comment>));
            SessionStateService.RegisterKnownType(typeof(Message));
            SessionStateService.RegisterKnownType(typeof(List<Message>));
            SessionStateService.RegisterKnownType(typeof(News));
            SessionStateService.RegisterKnownType(typeof(List<News>));
            SessionStateService.RegisterKnownType(typeof(Conversation));
            SessionStateService.RegisterKnownType(typeof(List<Conversation>));
            SessionStateService.RegisterKnownType(typeof(IGetter));
            SessionStateService.RegisterKnownType(typeof(NotificationSender));
            SessionStateService.RegisterKnownType(typeof(CommentSender));
            SessionStateService.RegisterKnownType(typeof(SoundPlayer));
            SessionStateService.RegisterKnownType(typeof(OrpheeFileImporter));
        }
        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            this._container = new UnityContainer();
            this._container.RegisterInstance<INavigationService>(NavigationService);
            this._container.RegisterInstance<ISessionStateService>(SessionStateService);
            new UnityIocInitializator(this._container);
            ViewModelLocationProvider.SetDefaultViewModelFactory((type) => this._container.Resolve(type));
            InitializeMyNavigatorService();
            return Task.FromResult<object>(null);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            this._isSuspending = true;
            try
            {
                var deferral = e.SuspendingOperation.GetDeferral();

                //Bootstrap inform navigation service that app is suspending.
                MyNavigationService.Suspending();

                // Save application state
                //await SessionStateService.SaveAsync();

                deferral.Complete();
            }
            finally
            {
                this._isSuspending = false;
            }
        }
    }
}