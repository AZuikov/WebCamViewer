using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using WebCamViewer.Annotations;
using WebCamViewer.Model;

namespace WebCamViewer.ViewModel
{
    public class AppViewModel :INotifyPropertyChanged
    {
        private WebCamManager _webCamManager;
        private FilterInfo _selectedWebCam;
        private WebCamera _webCam;
        private BitmapImage _frame;

        public BitmapImage CurrentFrame
        {
            get => CurrentWebCam is null ? new BitmapImage() : _frame;
            set => _frame = value;
        }

        public FilterInfo SelectedWebCamDevice
        {
            get => _selectedWebCam;
            set
            {
                _selectedWebCam = value;
                CurrentWebCam = new WebCamera(_selectedWebCam);
                OnPropertyChanged(nameof(SelectedWebCamDevice));
                OnPropertyChanged(nameof(CurrentWebCam));
            }
        }


        public WebCamera CurrentWebCam
        {
            get => _webCam;
            private set
            {
                _webCam = value;
                _webCam.AddNewFrameEventHandler(video_NewFrame);
            }
        }
        
        private void video_NewFrame(object sender,
            NewFrameEventArgs eventArgs)
        {
            // get new frame
            var bitmap = (Bitmap) eventArgs.Frame.Clone();
            // process the frame
            CurrentFrame = BitmapToImageSource(bitmap);
            OnPropertyChanged(nameof(CurrentFrame));
        }
        
        
       


        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public AppViewModel()
        {
            _webCamManager = new WebCamManager();
        }
        public ObservableCollection<FilterInfo> GetDevices => _webCamManager.FindedVideoDevices;

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName]string propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        

       
    }
}