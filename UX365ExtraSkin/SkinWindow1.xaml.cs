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

namespace UX365ExtraSkin
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

        private void WindowDrag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
