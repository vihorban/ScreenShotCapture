using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ObservableBinding;

namespace ScreenShotCapture
{
    public class MyDataContext : Observable
    {
        public string Folder
        {
            get
            {
                if (Properties.Settings.Default.FisrtTimeUsed)
                {
                    string newPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\ScreenShots";
                    Properties.Settings.Default.Folder = newPath;
                    Properties.Settings.Default.FisrtTimeUsed = false;
                    Properties.Settings.Default.Save();
                    NotifyPropertyChanged("Folder");
                }
                return Properties.Settings.Default.Folder;
            }
            set
            {
                Properties.Settings.Default.Folder = value;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("Folder");
            }
        }
    }
}
