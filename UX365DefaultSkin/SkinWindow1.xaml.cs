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
using System.Windows.Shapes;

namespace UX365DefaultSkin
{
    /// <summary>
    /// Interaction logic for SkinWindow1.xaml
    /// </summary>
    public partial class SkinWindow1 : Window
    {
        public SkinWindow1()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            WindowDrag.Background = new SolidColorBrush(SystemColors.ActiveCaptionColor);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            WindowDrag.Background = new SolidColorBrush(SystemColors.InactiveCaptionColor);
        }

        private void WindowDrag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ButtonExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 1)
            {
            }
            else if(e.ClickCount >= 2)
            {
                this.Close();
            }
            
        }
    }
}
