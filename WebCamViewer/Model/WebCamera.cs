using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;

namespace WebCamViewer.Model;

/// <summary>
///     Web camera class
/// </summary>
public class WebCamera
{
    public delegate void NewWebCamHandler();

    private readonly VideoCaptureDevice _videoSource; 
    //todo добавить инверсию картинки по горизонтали
    private BitmapImage _frame;
    private Dictionary<string, VideoCapabilities> _videoCapabilitiesMap;

    public WebCamera(FilterInfo filterInfo)
    {
        _videoSource = new VideoCaptureDevice(filterInfo.MonikerString);
        _videoSource.VideoResolution =
            _videoSource.VideoCapabilities.OrderBy(q => q.FrameSize.Height * q.FrameSize.Width).First();
        _videoCapabilitiesMap = _videoSource.VideoCapabilities.ToDictionary(x=>$"{x.FrameSize.Width}*{x.FrameSize.Height}",q=>q);
    }

    public string[] Resolutions => _videoCapabilitiesMap.Keys.Select(x=>x.ToString()).OrderBy(x=>x.Split('*')[0]).ToArray();

    public string CurrentResolution
    {
        get
        {
            var frameSize = _videoSource.VideoResolution.FrameSize;
            return $"{frameSize.Width}*{frameSize.Height}";
        }
        set
        {
            if (_videoCapabilitiesMap.ContainsKey(value) &&
                _videoCapabilitiesMap.TryGetValue(value, out VideoCapabilities resolution))
            {

                
                _videoSource.VideoResolution = resolution;
                if (_videoSource.IsRunning)
                {
                    _videoSource.Stop();
                    _videoSource.Start();
                }
               
            }
        }
    }

    public BitmapImage Frame
    {
        get => _frame ?? new BitmapImage();
        private set
        {
            _frame = value;
            NewFrameReceived?.Invoke();
        }
    }

    public event NewWebCamHandler NewFrameReceived;


    public void StartCapture()
    {
        // start the video source
        _videoSource.NewFrame += video_NewFrame;
        _videoSource.Start();
    }

    public void StopCapture()
    {
        // signal to stop
        if (_videoSource.IsRunning)
        {
            _videoSource.SignalToStop();
            _videoSource.NewFrame -= video_NewFrame;
        }
    }


    private void video_NewFrame(object sender,
        NewFrameEventArgs eventArgs)
    {
        // get new frame
        var bitmap = (Bitmap)eventArgs.Frame.Clone();
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