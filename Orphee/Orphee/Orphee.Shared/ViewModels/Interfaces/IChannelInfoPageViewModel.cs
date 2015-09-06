using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface IChannelInfoPageViewModel
    {
        List<string> CreationList { get; }
        DelegateCommand BackCommand { get; }
    }
}
