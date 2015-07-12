using Microsoft.Practices.Unity;
using Orphee.Models;
using Orphee.Models.Interfaces;
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
            UnityIocContainer.Container.RegisterType(typeof(IMidiLibRepository), typeof(MidiLibRepository), null, new ContainerControlledLifetimeManager());
            UnityIocContainer.Container.RegisterType(typeof(ISoundPlayer), typeof(SoundPlayer), null, new ContainerControlledLifetimeManager());
            UnityIocContainer.Container.RegisterType(typeof(IInstrumentManager), typeof(InstrumentManager), null, new ContainerControlledLifetimeManager());
        }
    }
}
