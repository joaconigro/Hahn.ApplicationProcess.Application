using Hahn.Mobile.Dtos;
using Hahn.Mobile.Helpers;
using Hahn.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Hahn.Mobile.ViewModels
{
    public class AssetsViewModel : BaseViewModel
    {
        private readonly uint pageSize;
        private uint pageNumber;

        public AssetsViewModel(INavService nav, IHttpService http) : base(nav, http)
        {
            Assets = new RangeObservableCollection<AssetDto>();
            pageSize = 20;
            pageSize = 1;
            RefreshAssets();
        }

        public RangeObservableCollection<AssetDto> Assets { get; set; }


        Command _refreshCommand;
        Command _addCommand;
        private AssetDto selectedAsset;

        public Command RefreshCommand => _refreshCommand ??= new Command(RefreshAssets);

        public Command AddCommand => _addCommand ??= new Command(async() => await AddAsset());

        public AssetDto SelectedAsset
        {
            get { return selectedAsset; }
            set
            {
                SetProperty(ref selectedAsset, value);
                Task.Run(NavigateOnSelection);
            }
        }

        private async Task NavigateOnSelection()
        {
            if (selectedAsset != null)
            {
               await NavService.NavigateTo<AssetDetailViewModel, AssetDto>(selectedAsset);
            }
        }

        private async Task AddAsset()
        {
                await NavService.NavigateTo<AssetDetailViewModel, AssetDto>(null);
        }


        private void RefreshAssets()
        {
            if (IsBusy) return;

            IsBusy = true;
            pageNumber = 1;

            Task.Run(LoadReunionsAsync);
            IsBusy = false;
        }

        private async Task LoadReunionsAsync()
        {
            var param = new Dictionary<string, string>
            {
                {"pageSize" , pageSize.ToString()},
                {"pageNumber", pageNumber.ToString() }
            };

            try
            {
                var r = await Http.GetItemsAsync<AssetDto>("asset/all", param);
                if (r?.Any() == true)
                {
                    Assets.AddRange(r);
                    pageNumber += 1;
                }
            }
            catch (HttpException http)
            {
                //TODO: display a message with the error

            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }
    }
}
