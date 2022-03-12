using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WebCamViewer.ViewModel;

namespace WebCamViewer.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public BitmapImage CurrentFrame
        {
            get
            {
                var currentWebCam = ((AppViewModel)DataContext).CurrentWebCam;
                return currentWebCam.Frame;
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AppViewModel();
        }

        private void RefreshDeviceListButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataContext = new AppViewModel();
        }

        private void ConnectToWebCamButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentWebCam = ((AppViewModel)DataContext).CurrentWebCam;
            currentWebCam.StartCapture();
            
        }
    }
}