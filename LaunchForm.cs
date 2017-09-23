using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ProvinceMapper
{
    public delegate void StatusUpdate(double amount);

    public partial class LaunchForm : Form
    {
        public LaunchForm()
        {
            InitializeComponent();

            // load settings
            tbSourceMapFolder.Text = Properties.Settings.Default.srcMapFolder;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // read definitions and create province lists
            lblStatus.Text = "Load Source Definitions";
            Application.DoEvents();
            string sourceDefPath = Path.Combine(tbSourceMapFolder.Text, "Definition.csv");
            Program.sourceDef = new DefinitionReader(sourceDefPath, PushStatusUpdate);

            // pre-scale maps
            lblStatus.Text = "Scale Maps";
            PushStatusUpdate(0.0);
            Application.DoEvents();
            string sourceMapPath = Path.Combine(tbSourceMapFolder.Text, "Provinces.bmp");
            Bitmap srcMap = (Bitmap)Bitmap.FromFile(sourceMapPath);
            PushStatusUpdate(33.0);
            PushStatusUpdate(67.0);
            PushStatusUpdate(100.0);
            srcMap.Tag = sourceMapPath;

            // add geo data to province lists
            lblStatus.Text = "Load Source Map";
            Application.DoEvents();
            Program.sourceMap = new MapReader(srcMap, Program.sourceDef.provinces, true, PushStatusUpdate);

            // save settings
            Properties.Settings.Default.srcMapFolder = tbSourceMapFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void PushStatusUpdate(double amount)
        {
            int actualAmt = (int)amount;
            if (actualAmt != progressBar1.Value)
            {
                // ProgressBar updates much more reliably going backwards
                if (actualAmt < 100)
                    progressBar1.Value = actualAmt + 1;
                progressBar1.Value = actualAmt;
                Application.DoEvents();
            }
        }
    }
}
