using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;
using WebCamViewer.Annotations;
using WebCamViewer.Model;

namespace WebCamViewer.ViewModel
{
    public class AppViewModel : INotifyPropertyChanged
    {
        private WebCamManager _webCamManager;
        private FilterInfo selectedWebCam;
        private WebCamera _webCam;

        public FilterInfo SelectedWebCamDevice
        {
            get => selectedWebCam;
            set
            {
                selectedWebCam = value;
                OnPropertyChanged(nameof(SelectedWebCamDevice));
                CurrentWebCam = new WebCamera(selectedWebCam);
            }
        }

        

        public WebCamera CurrentWebCam
        {
            get => _webCam;
            private set => _webCam = value;
        }

        public AppViewModel()
        {
            _webCamManager = new WebCamManager();
        }

        public ObservableCollection<FilterInfo> GetDevices => _webCamManager.FindedVideoDevices;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
    }
}