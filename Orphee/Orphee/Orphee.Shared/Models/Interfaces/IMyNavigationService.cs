using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace Orphee.Models.Interfaces
{
    /// <summary>
    /// MyNavigationService interface
    /// </summary>
    public interface IMyNavigationService : INavigationService
    {
        /// <summary>Foreground color of the BottomAppBar buttons </summary>
        Dictionary<string, SolidColorBrush> ButtonForegroundColorList { get; }
        /// <summary>Name of the curent page </summary>
        string CurrentPageName { get; set; }
        
        /// <summary>
        /// Sets the foreground color of the BottomAppBar buttons
        /// </summary>
        void SetNewAppBarButtonColorValue();
    }
}
