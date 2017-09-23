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
            tbSourceTag.Text = Properties.Settings.Default.srcTag;
            tbMappingsFile.Text = Properties.Settings.Default.mappingFile;
            cbScale.Checked = Properties.Settings.Default.fitMaps;
            cbNamesFrom.SelectedItem = Properties.Settings.Default.namesFrom;
            ckInvertSource.Checked = Properties.Settings.Default.invertSource;
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
            if (cbScale.Checked)
            {
                int h = srcMap.Height;
                int w = srcMap.Width;
                if (srcMap.Height < h || srcMap.Width < w)
                {
                    srcMap = Program.CleanResizeBitmap(srcMap, w, h);
                }
            }
            PushStatusUpdate(100.0);
            srcMap.Tag = sourceMapPath;

            // add geo data to province lists
            lblStatus.Text = "Load Source Map";
            Application.DoEvents();
            Program.sourceMap = new MapReader(srcMap, Program.sourceDef.provinces, ckInvertSource.Checked, PushStatusUpdate);

            // load localizations, if desired
            if (cbNamesFrom.SelectedItem.ToString() == "Localization")
            {
                lblStatus.Text = "Load Source Localization";
                Application.DoEvents();
                LocalizationReader lr = new LocalizationReader(tbSourceMapFolder.Text, Program.sourceDef.provinces, PushStatusUpdate);
            }

            // read existing mappings (if any)
            string mappingFile = tbMappingsFile.Text.Trim();
            if (mappingFile != String.Empty && File.Exists(mappingFile))
            {
                lblStatus.Text = "Parse Existing Mappings";
                Application.DoEvents();
            }

            // save settings
            Properties.Settings.Default.srcMapFolder = tbSourceMapFolder.Text;
            Properties.Settings.Default.srcTag = tbSourceTag.Text;
            Properties.Settings.Default.mappingFile = tbMappingsFile.Text;
            Properties.Settings.Default.fitMaps = cbScale.Checked;
            Properties.Settings.Default.namesFrom = cbNamesFrom.SelectedItem.ToString();
            Properties.Settings.Default.invertSource = ckInvertSource.Checked;
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
