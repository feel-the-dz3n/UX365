using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ThemeTweaks
{
    public partial class PreviewWnd : Form
    {
        public PreviewWnd()
        {
            InitializeComponent();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void PreviewWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void PreviewWnd_Load(object sender, EventArgs e)
        {

            if (!UX365Core.DWM.CheckAeroEnabled())
                button2.Enabled = false;

        }

        private void PreviewWnd_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                PreviewWnd_Load(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UX365Core.DWM.Glass(Handle);
            this.AllowTransparency = true;
            this.TransparencyKey = Color.Black;
            this.BackColor = Color.Black;
        }
    }
}
