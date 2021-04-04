using Hahn.Mobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Hahn.Mobile.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected IHttpService Http { get; }
        protected INavService NavService { get; private set; }
        protected BaseViewModel(INavService nav, IHttpService http)
        {
            Http = http;
            NavService = nav;
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public virtual void Init()
        {
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnError(Exception ex)
        {
            //Analytics.TrackError(ex);
        }

        protected void OnError(Exception ex, Action action)
        {
            OnError(ex);
            action?.Invoke();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class BaseViewModel<T> : BaseViewModel
    {
        protected BaseViewModel(INavService nav, IHttpService http) : base(nav, http)
        { }

        public override void Init()
        {
            Init(default);
        }
        public virtual void Init(T parameter)
        {
        }
    }
}
