using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Orphee.Models;
using Orphee.Models.Interfaces;
using Orphee.UnityModule;
using Orphee.Views;

namespace Orphee
{
    public sealed partial class App : MvvmAppBase
    {
        private IUnityContainer _container;
        public bool IsSuspending;
        public static IMyNavigationService MyNavigationService { get; private set; }
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
        }
   
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        { 
            this.Suspending += OnSuspending;
            MyNavigationService.Navigate("Home", null);
            return Task.FromResult<object>(null);
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            this._container = new UnityContainer();
            new UnityIocInitializator(this._container);
            ViewModelLocationProvider.SetDefaultViewModelFactory((type) => this._container.Resolve(type));
            InitializeMyNavigatorService();
            return Task.FromResult<object>(null);
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            IsSuspending = true;
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
                IsSuspending = false;
            }
        }
    }
}