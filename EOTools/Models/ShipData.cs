using EOTools.Tools;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EOTools.Models
{
    public class ShipData : INotifyPropertyChanged
    {
        public string NameJP { get; set; }

        public string NameEN { get; set; }

        public int ShipId { get; set; }
        public string RessourceName { get; set; } = "";

        public bool RPCExists
        {
            get
            {
                if (string.IsNullOrEmpty(AppSettings.ShipIconFolder)) return false;

                string _path = Path.Combine(AppSettings.ShipIconFolder, ShipId.ToString() + ".png");

                return File.Exists(_path);
            }
        }

        public ImageSource shipIcon = null;

        public ImageSource ShipIcon
        {
            get {  return shipIcon; }
            set 
            {  
                shipIcon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// X, Y coordinate of first point for wedding screen (used for RPC)
        /// </summary>
        public (int, int, int, int) FaceData {  get; set; }

        public ShipData()
        {
        }

        public ShipData(string _nameJP, string _nameEN)
        {
            NameJP = _nameJP;
            NameEN = _nameEN;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ImageSource GetTempShipIcon()
        {
            if (string.IsNullOrEmpty(AppSettings.ShipIconFolder)) return null;

            string _path = Path.Combine(AppSettings.ShipIconFolder, ShipId.ToString() + "_temp.png");
            if (!File.Exists(_path)) return null;

            FileStream _fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            BitmapImage _img = new BitmapImage();

            _img.BeginInit();
            _img.StreamSource = _fileStream;
            _img.EndInit();

            return _img;
        }

        public ImageSource GetShipIcon()
        {
            if (string.IsNullOrEmpty(AppSettings.ShipIconFolder)) return null;


            string _path = Path.Combine(AppSettings.ShipIconFolder, ShipId.ToString()+".png");
            if (!File.Exists(_path)) return null;

            FileStream _fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            BitmapImage _img = new BitmapImage();

            _img.BeginInit();
            _img.StreamSource = _fileStream;
            _img.EndInit();

            return _img;

        }

        public string GetIconFileName()
        {
            string _filename = KancolleTools.GetFileName(ShipId, "full", "", "", "ship", RessourceName);

            _filename += "*";

            DirectoryInfo _dirInfo = new DirectoryInfo(AppSettings.GetShipFullPath);
            FileInfo[] _files = _dirInfo.GetFiles(_filename);

            if (_files.Length == 0) return "";

            return _files[0].FullName;

            /*FileStream _fileStream = new FileStream(_finalPath, FileMode.Open, FileAccess.Read);

            BitmapImage _img = new BitmapImage();
            _img.BeginInit();
            _img.StreamSource = _fileStream;
            _img.EndInit();

            BitmapImage _img2 = new BitmapImage();

            int _x1 = Math.Max(FaceData.Item1, 0);
            int _y1 = Math.Max(FaceData.Item2, 0);

            int _width = Math.Max(FaceData.Item3, 0);
            int _height = Math.Max(FaceData.Item4, 0);

            Int32Rect _rect = new Int32Rect(_x1, _y1, _width, _height);

            try
            {
                return new CroppedBitmap(_img, _rect);
            }
            catch
            {
                return null;
            }*/
        }
    }
}
