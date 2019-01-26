using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UX365Control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string[] args)
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("UX365.log");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLoadSkin_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog o = new Microsoft.Win32.OpenFileDialog();
            o.Filter = "UX365 Extension (.NET Library) (*.dll)|*.dll|All Files (*.*)|*.*";

            if ((bool)o.ShowDialog())
            {
                UX365Core.WindowSkin skin = UX365Core.Core.LoadSkin(o.FileName);

                if (skin != null)
                {
                    UX365Core.Properties.Config.OnlyUX365Control = (bool)cbEvery.IsChecked;
                    UX365Core.Core.ApplyNewSkin(skin);
                    WindowStyle = WindowStyle.None;
                }
            }
        }

        private void btnKillSkins_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < UX365Core.Core.RegisteredWindows.Count; i++)
            {
                UX365Core.Core.RegisteredWindows[i].SetSkin(null);
            }
            WindowStyle = WindowStyle.SingleBorderWindow;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < UX365Core.Core.RegisteredWindows.Count; i++)
            {
                UX365Core.Draw.UpdateSkin(UX365Core.Core.RegisteredWindows[i]);
            }
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            btnUpdate_Click(null, null);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            btnUpdate_Click(null, null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new ThemeTweaks.PreviewWnd().Show();
        }
    }
}
