using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Orphee.ViewModels.Interfaces
{
    public interface IInternetConnexionAware
    {
        // Properties
        Visibility ConnexionUnavailableTextBlockVisibility { get; }
        
        // Methods
        void OnConnexionRetreived();
    }
}
