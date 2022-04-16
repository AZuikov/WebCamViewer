using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WebCamViewer.ViewModel;

namespace WebCamViewer.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AppViewModel();
        }

        /// <summary>
        ///     Кнопка обновления списка устройств (камер).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshDeviceListButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataContext = new AppViewModel();
        }

        /// <summary>
        ///     Кнопка подключения к веб-камере.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectToWebCamButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentWebCam = ((AppViewModel) DataContext).WebCamManager.CurrentWebCam;
            currentWebCam.StartCapture();
        }

        /// <summary>
        ///     Кнопка отключения от веб-камеры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectFromWebCaButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentWebCam = ((AppViewModel) DataContext).WebCamManager.CurrentWebCam;
            currentWebCam.StopCapture();
        }


        /// <summary>
        ///     Кнопка сохранения текущего кадра.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCurrentFrameButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG (*.png)|*.png"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                ((AppViewModel) DataContext).Save(filePath);
            }
        }

       
    }
}