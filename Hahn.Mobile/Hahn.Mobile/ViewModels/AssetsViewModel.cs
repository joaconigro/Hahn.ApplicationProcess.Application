using Hahn.Mobile.Dtos;
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

        public AssetsViewModel(INavService nav, IHttpService http, IDialogService dialog) : base(nav, http, dialog)
        {
            Assets = new RangeObservableCollection<AssetDto>();
            pageSize = 5;
            pageNumber = 1;
            RefreshAssets();
            nav.OnGoingBack += NavOnGoingBack;
        }

        private void NavOnGoingBack(object sender, object currentViewmodel)
        {
            if (currentViewmodel is AssetsViewModel)
            {
                RefreshAssets();
            }
        }

        public RangeObservableCollection<AssetDto> Assets { get; set; }


        Command _refreshCommand;
        Command _addCommand;
        Command _loadMoreCommand;
        private AssetDto selectedAsset;

        public Command RefreshCommand => _refreshCommand ??= new Command(RefreshAssets);
        public Command LoadMoreCommand => _loadMoreCommand ??= new Command(LoadMoreAssets);
        public Command AddCommand => _addCommand ??= new Command(async () => await AddAsset());

        public AssetDto SelectedAsset
        {
            get { return selectedAsset; }
            set
            {
                SetProperty(ref selectedAsset, value);
                NavigateOnSelection();
            }
        }

        private void NavigateOnSelection()
        {
            if (selectedAsset != null)
            {
                NavService.NavigateTo<AssetDetailViewModel, AssetDto>(selectedAsset);
            }
        }

        private async Task AddAsset()
        {
            await NavService.NavigateTo<AssetDetailViewModel, AssetDto>(null);
        }


        private void RefreshAssets()
        {
            if (IsBusy) return;

            Assets.Clear();
            IsBusy = true;
            pageNumber = 1;

            Task.Run(async () =>
            {
                var result = await LoadAssetsAsync(pageNumber);
                Assets.AddRange(result);
            });
            IsBusy = false;
        }

        private void LoadMoreAssets()
        {
            if (IsBusy) return;

            IsBusy = true;

            Task.Run(async () =>
            {
                var result = await LoadAssetsAsync(pageNumber + 1);
                if (result?.Any() == true)
                {
                    Assets.AddRange(result);
                    pageNumber += 1;
                }
            });
            IsBusy = false;
        }

        private async Task<IEnumerable<AssetDto>> LoadAssetsAsync(uint pageNum)
        {
            var param = new Dictionary<string, string>
            {
                {"pageSize" , pageSize.ToString()},
                {"pageNumber", pageNum.ToString() }
            };

            try
            {
                return await Http.GetItemsAsync<AssetDto>("asset/all", param);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
            return new List<AssetDto>();
        }
    }
}
