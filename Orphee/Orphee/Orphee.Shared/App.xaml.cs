using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using Orphee.Models;
using Orphee.UnityModule;

namespace Orphee
{
    public sealed partial class App : MvvmAppBase
    {
        private IUnityContainer _container;
        public static INavigationService MyNavigationService { get; private set; }
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
            NavigationService.Navigate("Home", null);
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
    }
}