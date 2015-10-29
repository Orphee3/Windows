using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Orphee.ViewModels;

namespace Orphee.UI
{
    public sealed partial class MyDeleteAccountMessageDialog : UserControl
    {
        public MyDeleteAccountMessageDialog()
        {
            this.InitializeComponent();
            this.DataContext = new MyDeleteAccountMessageDialogViewModel();
        }

        public async Task<bool> ShowAsync()
        {
            await ((MyDeleteAccountMessageDialogViewModel)this.DataContext).ShowAsync();
            return true;
        }
    }
}
