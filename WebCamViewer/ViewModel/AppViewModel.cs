using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AForge.Video.DirectShow;
using WebCamViewer.Annotations;

namespace WebCamViewer.ViewModel
{
    public class AppViewModel:INotifyPropertyChanged
    {
        private WebCamManager _webCamManager;
        private FilterInfo selectedWebCamDevice;
        public  FilterInfo SelectedWebCamDevice
        {
            get => selectedWebCamDevice;
            set
            {
                selectedWebCamDevice = value;
                OnPropertyChanged(nameof(SelectedWebCamDevice));
            }
        }

        public AppViewModel()
        {
            _webCamManager = new WebCamManager();
        }

        public ObservableCollection<FilterInfo> GetDevices
        {
            get => _webCamManager.FindedVideoDevices;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}