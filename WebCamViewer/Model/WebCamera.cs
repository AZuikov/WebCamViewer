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
        /// <summary>
        ///     Video Source
        /// </summary>
        private readonly VideoCaptureDevice videoSource;

        public BitmapImage Frame { get; private set; }

        public WebCamera(FilterInfo filterInfo)
        {
            videoSource = new VideoCaptureDevice(filterInfo.MonikerString);
            
        }

        public void AddNewFrameEventHandler(NewFrameEventHandler eventHandler)
        {
            videoSource.NewFrame += eventHandler;
        }

       


        public void StartCapture()
        {
            // start the video source
            videoSource.Start();
        }

        public void StopCapture()
        {
            // signal to stop
            videoSource.SignalToStop();
        }
    }
}