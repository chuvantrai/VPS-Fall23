using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.MobileApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        bool isBusy = false;
        int cameraIndex = 1;
        int loadingIndex = 0;
        string areaEntry = "";
        string licensePlateEntry = "";

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string AreaEntry
        {
            get { return areaEntry; }
            set { SetProperty(ref areaEntry, value); }
        }

        public string LicensePlateEntry
        {
            get { return licensePlateEntry; }
            set { SetProperty(ref licensePlateEntry, value); }
        }

        public int CameraIndex
        {
            get { return cameraIndex; }
            set { SetProperty(ref cameraIndex, value); }
        }

        public int LoadingIndex
        {
            get { return loadingIndex; }
            set { SetProperty(ref loadingIndex, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
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
}
