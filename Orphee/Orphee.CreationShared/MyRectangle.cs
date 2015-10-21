using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Annotations;

namespace Orphee.CreationShared
{
    public class MyRectangle : IMyRectangle
    {
        private Visibility _isSelecionRectangleVisible;

        public Visibility IsSelectionRectangleVisible
        {
            get { return this._isSelecionRectangleVisible; }
            set
            {
                if (this._isSelecionRectangleVisible != value)
                {
                    this._isSelecionRectangleVisible = value;
                    OnPropertyChanged(nameof(IsSelectionRectangleVisible));
                }
            }
        }

        private double _isRectangleVisible;
        public double IsRectangleVisible
        {
            get { return this._isRectangleVisible; }
            set
            {
                if (this._isRectangleVisible != value)
                {
                    this._isRectangleVisible = value;
                    OnPropertyChanged(nameof(IsRectangleVisible));
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
