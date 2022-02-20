using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
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

        public Bitmap Frame { get; private set; }

        public WebCamera(VideoCaptureDevice videoCaptureDevice)
        {
            videoSource = videoCaptureDevice; 
            // set NewFrame event handler
            videoSource.NewFrame += new NewFrameEventHandler( video_NewFrame );
            
        }
        
        private void video_NewFrame( object sender,
            NewFrameEventArgs eventArgs )
        {
            // get new frame
             Bitmap bitmap = eventArgs.Frame;
             // process the frame
             Frame = bitmap;
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
    }
}

