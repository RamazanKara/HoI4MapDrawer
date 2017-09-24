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
            PushStatusUpdate(0.0);

            lblStatus.Text = "Load Country Colors";
            Application.DoEvents();
				CountryReader countries = new CountryReader(tbSourceMapFolder.Text);
				PushStatusUpdate(20.0);

				lblStatus.Text = "Import Save";
				Application.DoEvents();
				SaveReader save = new SaveReader(tbSaveLocation.Text, tbSourceMapFolder.Text);
				PushStatusUpdate(40.0);

				lblStatus.Text = "Load Source Definitions";
				Application.DoEvents();
				string sourceDefPath = Path.Combine(tbSourceMapFolder.Text, "map\\Definition.csv");
            Program.sourceDef = new DefinitionReader(sourceDefPath, PushStatusUpdate);
            PushStatusUpdate(60.0);

            // pre-scale maps
            lblStatus.Text = "Scale Maps";
            Application.DoEvents();
            string sourceMapPath = Path.Combine(tbSourceMapFolder.Text, "map\\Provinces.bmp");
            Bitmap srcMap = (Bitmap)Bitmap.FromFile(sourceMapPath);
            PushStatusUpdate(80.0);
            srcMap.Tag = sourceMapPath;

            // add geo data to province lists
            lblStatus.Text = "Load Source Map";
            Application.DoEvents();
            Program.sourceMap = new MapReader(srcMap, Program.sourceDef.provinces, PushStatusUpdate, save.provinceOwners, countries.countries);
            PushStatusUpdate(100.0);

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
