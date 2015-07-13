using Microsoft.Practices.Unity;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.Shared;
using MidiDotNet.Shared.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.UnityModule
{
    public class UnityIocInitializator
    {
        public void InitializeIocBindings()
        {
            UnityIocContainer.Container.RegisterType(typeof (ILoopCreationViewModel), typeof (LoopCreationViewModel),null, new ContainerControlledLifetimeManager());
            UnityIocContainer.Container.RegisterType(typeof (IOrpheeTrack), typeof (OrpheeTrack), null, new ContainerControlledLifetimeManager());
            UnityIocContainer.Container.RegisterType(typeof (IMidiLibRepository), typeof (MidiLibRepository), null, new ContainerControlledLifetimeManager());
            UnityIocContainer.Container.RegisterType(typeof (ISoundPlayer), typeof (SoundPlayer), null, new ContainerControlledLifetimeManager());
            UnityIocContainer.Container.RegisterType(typeof (IInstrumentManager), typeof (InstrumentManager), null, new ContainerControlledLifetimeManager());
            UnityIocContainer.Container.RegisterType(typeof(ISwapManager), typeof(SwapManager), null, new ContainerControlledLifetimeManager());
            //UnityIocContainer.Container.RegisterType(typeof(ITimeSignatureMessageWriter), typeof(TimeSignatureMessageWriter), null, new ContainerControlledLifetimeManager());
            //UnityIocContainer.Container.RegisterType(typeof(ITempoMessageWriter), typeof(TempoMessageWriter), null, new ContainerControlledLifetimeManager());
            //UnityIocContainer.Container.RegisterType(typeof(IProgramMessageWriter), typeof(ProgramMessageWriter), null, new ContainerControlledLifetimeManager());
        }
    }
}
