using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Orphee.UI;

namespace Orphee.Models.Interfaces
{
    public interface IMyNavigationService : INavigationService
    {
        MyBottomAppBar MyBottomAppBar { get; }
        Dictionary<string, SolidColorBrush> ButtonForegroundColorList { get; }
        string CurrentPageName { get; set; }
        void SetNewAppBarButtonColorValue();
    }
}
