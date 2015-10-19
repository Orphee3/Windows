using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Orphee.UI
{
    public sealed partial class TrackOptionsMessageDialog : UserControl
    {
        private TaskCompletionSource<bool> _taskCompletionSource;
        public TrackOptionsMessageDialog()
        {
            this.InitializeComponent();
        }

        public Task<bool> ShowAsync()
        {
            this._taskCompletionSource = new TaskCompletionSource<bool>();

            this.MyPopUp.IsOpen = true;
            return this._taskCompletionSource.Task;
        }

        public void Close()
        {
            this.MyPopUp.IsOpen = false;
            this._taskCompletionSource.SetResult(true);
        }
    }
}
