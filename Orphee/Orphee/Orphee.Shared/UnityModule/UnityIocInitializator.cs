using Microsoft.Practices.Unity;
using Orphee.OrpheeMidiConverter;
using Orphee.OrpheeMidiConverter.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.UnityModule
{
    public class UnityIocInitializator
    {
        public void InitializeIocBindings()
        {
            UnityIocContainer.Container.RegisterType(typeof (ILoopCreationViewModel), typeof (LoopCreationViewModel),null, new ContainerControlledLifetimeManager());
            UnityIocContainer.Container.RegisterType(typeof (IOrpheeTrack), typeof(OrpheeTrack), null, new ContainerControlledLifetimeManager());
        }
    }
}
