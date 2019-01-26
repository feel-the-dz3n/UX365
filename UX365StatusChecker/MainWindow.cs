using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UX365StatusChecker
{
    public partial class MainWindow : Form
    {
        public MainWindow(string[] a)
        {
            InitializeComponent();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
