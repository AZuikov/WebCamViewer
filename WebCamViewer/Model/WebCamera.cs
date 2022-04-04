using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;

namespace WebCamViewer.Model
{
    /// <summary>
    ///     Web camera class
    /// </summary>
    public class WebCamera
    {
        private readonly VideoCaptureDevice _videoSource;
        private BitmapImage _frame;
        
        public delegate void NewWebCamHandler();

        public event NewWebCamHandler NewFrameReceived;

        public BitmapImage Frame
        {
            get=> _frame ?? new BitmapImage();
            private set
            {
                _frame = value;
                NewFrameReceived?.Invoke();
            }
        }

        public WebCamera(FilterInfo filterInfo)
        {
            _videoSource = new VideoCaptureDevice(filterInfo.MonikerString);
        }


        public void StartCapture()
        {
            // start the video source
            _videoSource.NewFrame += video_NewFrame;
            _videoSource.Start();
        }

        public void StopCapture()
        {
            // signal to stop
            _videoSource.SignalToStop();
            _videoSource.NewFrame -= video_NewFrame;
        }

        

        private void video_NewFrame(object sender,
            NewFrameEventArgs eventArgs)
        {
            // get new frame
            var bitmap = (Bitmap) eventArgs.Frame.Clone();
            // process the frame
            Frame = BitmapToImageSource(bitmap);
            
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
                bitmapimage.Freeze();

                return bitmapimage;
            }
        }
    }
}