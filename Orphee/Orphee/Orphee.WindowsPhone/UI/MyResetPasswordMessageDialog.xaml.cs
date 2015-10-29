using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Orphee.RestApiManagement.Posters;
using Orphee.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Orphee.UI
{
    public sealed partial class MyResetPasswordMessageDialog : UserControl
    {
        public MyResetPasswordMessageDialog()
        {
            this.InitializeComponent();
            this.DataContext = new MyResetPasswordMessageDialogViewModel(new PasswordReseter());
        }

        public async Task<bool> ShowAsync()
        {
            await ((MyResetPasswordMessageDialogViewModel)this.DataContext).ShowAsync();
            return true;
        }
    }
}
