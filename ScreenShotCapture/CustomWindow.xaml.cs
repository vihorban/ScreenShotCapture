using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ScreenShotCapture
{
    /// <summary>
    /// Interaction logic for CustomWindow.xaml
    /// </summary>
    public partial class CustomWindow : Window
    {
        private double x;
        private double y;
        private double width;
        private double height;
        private bool isMouseDown = false;
        public Bitmap bitmap;


        public CustomWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            x = e.GetPosition(null).X;
            y = e.GetPosition(null).Y;
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.isMouseDown)
            {
                double curx = e.GetPosition(null).X;
                double cury = e.GetPosition(null).Y;

                System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
                SolidColorBrush brush = new SolidColorBrush(Colors.White);
                r.Stroke = brush;
                r.Fill = brush;
                r.StrokeThickness = 1;
                r.Width = Math.Abs(curx - x);
                r.Height = Math.Abs(cury - y);
                cnv.Children.Clear();
                cnv.Children.Add(r);
                Canvas.SetLeft(r, Math.Min(x , curx));
                Canvas.SetTop(r, Math.Min(y, cury));
                if (e.LeftButton == MouseButtonState.Released)
                {
                    cnv.Children.Clear();
                    width = Math.Abs(curx - x);
                    height = Math.Abs(cury - y);
                    bitmap = ScreenShotMaker.CaptureScreen(width, height, Math.Min(x, curx) - 7, Math.Min(y, cury) - 7);
                    this.x = this.y = 0;
                    this.isMouseDown = false;
                    this.Close();
                }
            }
        }
    }
}
