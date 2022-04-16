using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;
using Microsoft.Win32;
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
            var currentWebCam = ((AppViewModel) DataContext).WebCamManager.CurrentWebCam;
            currentWebCam.StartCapture();
        }

        private void DisconnectFromWebCaButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentWebCam = ((AppViewModel) DataContext).WebCamManager.CurrentWebCam;
            currentWebCam.StopCapture();
        }


        private void SaveCurrentFrameButton_OnClick(object sender, RoutedEventArgs e)
        {
            var bitmapImage = ((AppViewModel) DataContext).CurrentFrame;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG (*.png)|*.png";
            if (saveFileDialog.ShowDialog() == true)
            {
               var filePath = saveFileDialog.FileName;
                Save(bitmapImage,filePath);
            }
        }
        
        public void Save( BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }
}