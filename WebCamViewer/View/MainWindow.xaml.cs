using System.ComponentModel;
using System.Windows;
using WebCamViewer.ViewModel;

namespace WebCamViewer.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
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
            var currentWebCam = ((AppViewModel) DataContext).CurrentWebCam;
            currentWebCam.StartCapture();
        }

        private void DisconnectFromWebCaButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentWebCam = ((AppViewModel) DataContext).CurrentWebCam;
            currentWebCam.StopCapture();
        }
        
       

        public event PropertyChangedEventHandler PropertyChanged;

        
    }
}