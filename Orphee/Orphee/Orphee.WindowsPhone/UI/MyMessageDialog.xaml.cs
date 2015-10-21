using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels;

namespace Orphee.UI
{
    public sealed partial class MyMessageDialog : UserControl
    { 
        public MyMessageDialog()
        {
            this.InitializeComponent();
            this.DataContext = new MyMessageDialogViewModel(new InstrumentManager(), new ColorManager());
        }

        public async Task<bool> ShowAsync()
        {
            await ((MyMessageDialogViewModel) this.DataContext).ShowAsync();
            return true;
        }

        public void SetSource(IOrpheeTrack orpheeTrack)
        {
            ((MyMessageDialogViewModel)this.DataContext).SetCurrentOrpheeTrack(orpheeTrack);
        }
    }
}
