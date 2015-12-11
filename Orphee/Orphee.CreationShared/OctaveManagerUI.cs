using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Annotations;

namespace Orphee.CreationShared
{
    public class OctaveManagerUI : IOctaveManagerUI, INotifyPropertyChanged
    {
        private Visibility _octaveVisibility;

        public Visibility OctaveVisibility
        {
            get { return this._octaveVisibility; }
            set
            {
                if (this._octaveVisibility != value)
                {
                    this._octaveVisibility = value;
                    OnPropertyChanged(nameof(this.OctaveVisibility));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
