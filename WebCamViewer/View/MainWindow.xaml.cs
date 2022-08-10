using System;
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
           
        }

        protected override void OnClosed(EventArgs e)
        {
            ((AppViewModel)DataContext).WebCamManager.CurrentWebCam?.StopCapture();
            base.OnClosed(e);
        }
    }
}