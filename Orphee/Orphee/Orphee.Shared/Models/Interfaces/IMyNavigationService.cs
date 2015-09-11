using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace Orphee.Models.Interfaces
{
    public interface IMyNavigationService : INavigationService
    {
        Dictionary<string, SolidColorBrush> ButtonForegroundColorList { get; }
        string CurrentPageName { get; set; }
        void SetNewAppBarButtonColorValue();
    }
}
