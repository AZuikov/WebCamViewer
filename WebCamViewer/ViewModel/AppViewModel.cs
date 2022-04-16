using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;
using WebCamViewer.Annotations;

namespace WebCamViewer.ViewModel
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public  WebCamManager WebCamManager { get; }

        private BitmapImage _frameOnForm;

        public BitmapImage CurrentFrame
        {
            get => _frameOnForm;
            private set
            {
                _frameOnForm = value;
                OnPropertyChanged(nameof(CurrentFrame));
            }
        }

        public AppViewModel()
        {
            WebCamManager = new WebCamManager();
            //подписываемся на событие получение нового кадра с камеры
            WebCamManager.ManagerNewFrameReceived += () => CurrentFrame = WebCamManager.Frame;
        }

        /// <summary>
        /// Список найденных устройств.
        /// </summary>
        public ObservableCollection<FilterInfo> GetDevices => WebCamManager.FindedVideoDevices;
        
        /// <summary>
        /// Выбранное устройство.
        /// </summary>
        public FilterInfo SelectedWebCamDevice
        {
            get => WebCamManager.SelectedWebCamDevice;
            set => WebCamManager.SelectedWebCamDevice = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}