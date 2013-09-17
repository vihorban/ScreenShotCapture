using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using Com.CurtisRutland.WpfHotkeys;
using Delay;

namespace ScreenShotCapture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private MyDataContext myDataContext;
        private Hotkey globalHotkey;
        private CustomWindow customWindow;

        public MainWindow()
        {
            InitializeComponent();
            myDataContext = new MyDataContext();
            folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.DataContext = myDataContext;
            MinimizeToTray.Enable(this);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            globalHotkey = new Hotkey(Modifiers.NoMod, Keys.PrintScreen, this, true);
            globalHotkey.HotkeyPressed += makeFullScreen;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            globalHotkey.Dispose();
        }

        private void selectFolder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                myDataContext.Folder = folderBrowserDialog.SelectedPath;
            }
        }

        private void makeFullScreen(object sender, HotkeyEventArgs e)
        {
            System.Drawing.Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            System.Drawing.Bitmap image = ScreenShotMaker.CaptureScreen(resolution.Width, resolution.Height);
            saveImage(image);
        }

        private void saveImage(System.Drawing.Bitmap image)
        {
            createDirectory();
            string path = System.IO.Path.Combine(myDataContext.Folder, DateTime.Now.ToString("yyyyMMdd-HHmmss")).ToString() + ".png";
            image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }

        private void createDirectory()
        {
            try
            {
                if (Directory.Exists(myDataContext.Folder))
                {
                    return;
                }
                DirectoryInfo di = Directory.CreateDirectory(myDataContext.Folder);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("The process failed: " + e.ToString());
            }
            finally { }
        }

        private void createCustomScreenShot(object sender, RoutedEventArgs e)
        {
            customWindow = new CustomWindow();
            customWindow.Closing += CustomWindow_Closed;
            customWindow.Show();
        }

        private void CustomWindow_Closed(object sender, EventArgs e)
        {
            saveImage(customWindow.bitmap);
        }
    }
}
