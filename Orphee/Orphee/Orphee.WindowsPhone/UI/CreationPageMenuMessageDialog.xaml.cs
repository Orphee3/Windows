using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Orphee.CreationShared;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.UI
{
    public sealed partial class CreationPageMenuMessageDialog : UserControl
    {
        public CreationPageMenuMessageDialog()
        {
            this.InitializeComponent();
            this.DataContext = new CreationPageMenuMessageDialogViewModel(new InstrumentManager());
        }

        public async Task<bool> ShowAsync()
        {
            await ((CreationPageMenuMessageDialogViewModel)this.DataContext).ShowAsync();
            return true;
        }

        public Creation GetCreationType()
        {
            return ((CreationPageMenuMessageDialogViewModel)this.DataContext).GetCreationType();
        }
    }
}
