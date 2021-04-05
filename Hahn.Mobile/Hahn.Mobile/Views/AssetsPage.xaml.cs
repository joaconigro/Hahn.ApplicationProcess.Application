using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hahn.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssetsPage : ContentPage
    {
        public AssetsPage()
        {
            InitializeComponent();
        }

        private void OnPageAppearing(object sender, EventArgs e)
        {
            AssetsCollectionView.SelectedItem = null;
        }
    }
}