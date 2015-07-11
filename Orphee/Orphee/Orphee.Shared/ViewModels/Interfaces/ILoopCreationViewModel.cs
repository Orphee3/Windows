using Microsoft.Practices.Prism.Commands;
using Orphee.OrpheeMidiConverter.Interfaces;

namespace Orphee.ViewModels.Interfaces
{
    public interface ILoopCreationViewModel
    {
        // Methods

        // Properties
        IOrpheeTrack DisplayedTrack { get; }
        DelegateCommand AddColumnsCommand { get; }
        DelegateCommand RemoveAColumnCommand { get; }
    }
}
