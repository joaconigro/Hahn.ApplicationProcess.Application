using Xamarin.Forms;

namespace Hahn.Mobile.Views
{
    public partial class AssetDetailPage : ContentPage
    {
        public AssetDetailPage()
        {
            InitializeComponent();
        }

        private void BrokenLabelTapped(object sender, System.EventArgs e)
        {
            IsBrokenCheckbox.IsChecked = !IsBrokenCheckbox.IsChecked;
        }
    }
}
