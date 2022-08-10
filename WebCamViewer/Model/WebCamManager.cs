using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;

namespace WebCamViewer.Model;

public class WebCamManager : INotifyPropertyChanged
{
    private FilterInfoCollection _videoDevices;
    private FilterInfo _selectedWebCam;
    private WebCamera _webCam;

    private BitmapImage _frame;

    public BitmapImage Frame
    {
        get => _frame;
        private set
        {
            _frame = value;
            ManagerNewFrameReceived?.Invoke();
        }
    }

    public delegate void CamManagerHandler();

    public event CamManagerHandler ManagerNewFrameReceived;

    public ObservableCollection<FilterInfo> FindedVideoDevices
    {
        get
        {
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            var devices = new ObservableCollection<FilterInfo>();
            foreach (FilterInfo videoDevice in _videoDevices) devices.Add(videoDevice);

            return devices;
        }
    }

    public bool IsHaveDevices()
    {
        return FindedVideoDevices.Count > 0;
    }

    public FilterInfo SelectedWebCamDevice
    {
        get => _selectedWebCam;
        set
        {
            _selectedWebCam = value;
            CurrentWebCam = new WebCamera(_selectedWebCam);
            OnPropertyChanged(nameof(SelectedWebCamDevice));
        }
    }

    public WebCamera CurrentWebCam
    {
        get => _webCam;
        private set
        {
            _webCam = value;
            _webCam.NewFrameReceived += () =>
            {
                Frame = _webCam.Frame;
                ManagerNewFrameReceived?.Invoke();
            };
            OnPropertyChanged(nameof(CurrentWebCam));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}