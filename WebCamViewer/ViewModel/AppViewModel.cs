using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;
using Microsoft.Win32;
using WebCamViewer.Annotations;
using WebCamViewer.Model;

namespace WebCamViewer.ViewModel
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public  WebCamManager WebCamManager { get; private set; }

        private BitmapImage _frameOnForm;

        public BitmapImage CurrentFrame
        {
            get => _frameOnForm;
            private set
            {
                _frameOnForm = value;
                OnPropertyChanged(nameof(CurrentFrame));
            }
        }

        public AppViewModel()
        {
            RefreshWebCamList();
            //подписываемся на событие получение нового кадра с камеры
            WebCamManager.ManagerNewFrameReceived += () => CurrentFrame = WebCamManager.Frame;
        }

        /// <summary>
        /// Список найденных устройств.
        /// </summary>
        public ObservableCollection<FilterInfo> GetDevices => WebCamManager.FindedVideoDevices;
        
        /// <summary>
        /// Выбранное устройство.
        /// </summary>
        public FilterInfo SelectedWebCamDevice
        {
            get => WebCamManager.SelectedWebCamDevice;
            set => WebCamManager.SelectedWebCamDevice = value;
        }

        public string[] Resolutions => WebCamManager.CurrentWebCam.Resolutions;

        public string CurrentResolution
        {
            get => WebCamManager.CurrentWebCam.CurrentResolution;
            set => WebCamManager.CurrentWebCam.CurrentResolution = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Сохраняет BitmapImage в файл PNG.
        /// </summary>
        /// <param name="image">ОБъект BitmapImage</param>
        /// <param name="filePath">Путь сохраняемого файла.</param>
        public void Save(string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(CurrentFrame));

            using var fileStream = new FileStream(filePath, FileMode.Create);
            encoder.Save(fileStream);
        }


        #region Commands

        public RelayCommand StopWebCam
        {
            get { return new RelayCommand(obj => { WebCamManager.CurrentWebCam?.StopCapture(); }); }
        }

        public RelayCommand StartWebCam
        {
            get { return new RelayCommand(obj => { WebCamManager.CurrentWebCam?.StartCapture(); }); }
        }

        public RelayCommand RefreshDeviceList
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (WebCamManager.CurrentWebCam != null)
                    {
                        WebCamManager.CurrentWebCam.StopCapture();
                    }
                    RefreshWebCamList();
                });
            }
        }

        /// <summary>
        /// Выполняет формирование списка камер и подписывается на событие получения кадра с камеры.
        /// </summary>
        private void RefreshWebCamList()
        {
            //todo нужно отписываться от события при пересоздании объекта
            WebCamManager = new WebCamManager();
            //подписываемся на событие получение нового кадра с камеры
            WebCamManager.ManagerNewFrameReceived += () => CurrentFrame = WebCamManager.Frame;
            SelectedWebCamDevice = GetDevices.FirstOrDefault();
            CurrentResolution = Resolutions.First();

        }

        public RelayCommand SaveCapture
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = "PNG (*.png)|*.png"
                    };
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var filePath = saveFileDialog.FileName;
                        Save(filePath);
                    }
                });
            }
        }

        #endregion
    }
}