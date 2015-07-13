using Orphee.ViewModels.Interfaces;

namespace Orphee.UnityModule
{
    public class ViewModelLocator
    {
        public ILoopCreationViewModel LoopCreationViewModel { get { return UnityIocContainer.Get<ILoopCreationViewModel>(); } }
    }
}
