using System.Collections.ObjectModel;
using AForge.Video.DirectShow;

public class WebCamManager
{
    private FilterInfoCollection videoDevices{get;  set; }
     
    
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

   
}