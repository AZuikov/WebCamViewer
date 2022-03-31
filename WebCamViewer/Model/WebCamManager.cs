using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;
using WebCamViewer;
using WebCamViewer.Annotations;
using WebCamViewer.Model;

public class WebCamManager : INotifyPropertyChanged
{
    private FilterInfoCollection videoDevices { get; set; }

    private FilterInfo _selectedWebCam;
    private WebCamera _webCam;
    private WebCamViewerCommand _getNewFrameCommand;

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
            // enumerate video devices
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            ObservableCollection<FilterInfo> devices = new ObservableCollection<FilterInfo>();
            foreach (FilterInfo videoDevice in videoDevices)
            {
                devices.Add(videoDevice);
            }

            return devices;
        }
    }

    public bool IsHaveDevices() => FindedVideoDevices.Count > 0;


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

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}