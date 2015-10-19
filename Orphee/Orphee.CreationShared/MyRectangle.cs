using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Annotations;

namespace Orphee.CreationShared
{
    public class MyRectangle : IMyRectangle
    {
        public SolidColorBrush _rectangleBackgroudColor;

        public SolidColorBrush RectangleBackgroundColor
        {
            get {return _rectangleBackgroudColor;}
            set
            {
                if (this._rectangleBackgroudColor != value)
                {
                    this._rectangleBackgroudColor = value;
                    OnPropertyChanged(nameof(this.RectangleBackgroundColor));
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
