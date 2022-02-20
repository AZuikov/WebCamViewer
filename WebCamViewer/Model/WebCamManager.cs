using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AForge.Video.DirectShow;
using WebCamViewer.Annotations;

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

    

    public bool IsHaveDivices() => videoDevices.Count > 0;

   
}