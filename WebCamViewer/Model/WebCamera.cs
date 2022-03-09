using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace WebCamViewer.Model
{
    /// <summary>
    /// Web camera class
    /// </summary>
    public class WebCamera
    {
        /// <summary>
        /// Video Source
        /// </summary>
        private VideoCaptureDevice videoSource;

        public BitmapImage Frame { get; private set; }

        public WebCamera(FilterInfo filterInfo)
        {
            videoSource = new VideoCaptureDevice(filterInfo.MonikerString); 
            // set NewFrame event handler
            videoSource.NewFrame += new NewFrameEventHandler( video_NewFrame );
        }
        
        private void video_NewFrame( object sender,
            NewFrameEventArgs eventArgs )
        {
            // get new frame
             Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
             // process the frame
             Frame = BitmapToImageSource(bitmap);
             
        }

        public void StartCapture()
        {
            // start the video source
            videoSource.Start( );
        }

        public void StopCapture()
        {
            // signal to stop
            videoSource.SignalToStop( );
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
        
        
    }
}

