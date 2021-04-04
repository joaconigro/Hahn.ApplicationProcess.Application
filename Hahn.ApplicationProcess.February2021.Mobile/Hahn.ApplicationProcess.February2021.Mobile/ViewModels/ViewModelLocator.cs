using CommonServiceLocator;
using Hahn.Mobile.Bootstrap;

namespace Hahn.Mobile.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            AppContainer.Init();
        }

        public AssetDetailViewModel RegisterViewModel => ServiceLocator.Current.GetInstance<AssetDetailViewModel>();
        public AssetsViewModel ReunionsViewModel => ServiceLocator.Current.GetInstance<AssetsViewModel>();
    }
}
