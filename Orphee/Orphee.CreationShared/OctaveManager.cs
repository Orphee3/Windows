using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OctaveManager : IOctaveManager
    {
        public ObservableCollection<ObservableCollection<IToggleButtonNote>> OctaveMap { get; set; }
        public int OctavePos { get; private set; }
        public string OctavePosString { get; private set; }
        public IOctaveManagerUI OctaveManagerUI { get; private set; }

        public OctaveManager(IOctaveManagerUI octaveManagerUI, int octavePos)
        {
            this.OctaveManagerUI = octaveManagerUI;
            this.OctavePos = octavePos;
            this.OctavePosString = octavePos.ToString();
            this.OctaveManagerUI.OctaveVisibility = this.OctavePos == 4 ? Visibility.Visible : Visibility.Collapsed;
            this.OctaveMap = new ObservableCollection<ObservableCollection<IToggleButtonNote>>();
        }
    }
}
