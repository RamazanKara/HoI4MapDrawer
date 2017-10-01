using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace ProvinceMapper
{
    public delegate void StatusUpdate(double amount);

	public partial class LaunchForm : Form
	{
		public LaunchForm()
		{
			InitializeComponent();

			// load settings
			tbSourceMapFolder.Text = ReadHoI4Folder();
			tbSaveLocation.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Paradox Interactive\\Hearts of Iron IV\\save games");
			modFolder.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Paradox Interactive\\Hearts of Iron IV\\mod");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			PushStatusUpdate(0.0);

			if (!File.Exists(Path.Combine(tbSourceMapFolder.Text, "hoi4.exe")))
			{
				lblStatus.Text = "HoI4 Folder was not a valid HoI4 folder";
				Application.DoEvents();
				return;
			}

			if (!File.Exists(tbSaveLocation.Text))
			{
				lblStatus.Text = "HoI4 save does not exist";
				Application.DoEvents();
				return;
			}

			getSelectedMods();

			lblStatus.Text = "Load Country Colors";
			Application.DoEvents();
			CountryReader countries = new CountryReader(tbSourceMapFolder.Text, selectedMods);
			PushStatusUpdate(20.0);

			lblStatus.Text = "Import Save";
			Application.DoEvents();
			SaveReader save;
			try
			{
				save = new SaveReader(tbSaveLocation.Text, tbSourceMapFolder.Text, selectedMods);
			}
			catch
			{
				lblStatus.Text = "Save is binary. See FAQ for solution";
				Application.DoEvents();
				return;
			}
			PushStatusUpdate(40.0);

			lblStatus.Text = "Load Source Definitions";
			Application.DoEvents();
			string sourceDefPath = Path.Combine(tbSourceMapFolder.Text, "map\\Definition.csv");
			foreach (HoI4Mod mod in selectedMods)
			{
				string possibleDefinitions = Path.Combine(mod.location, "map\\Definition.csv");
				if (File.Exists(possibleDefinitions))
				{
					sourceDefPath = possibleDefinitions;
				}
			}
			Program.sourceDef = new DefinitionReader(sourceDefPath, PushStatusUpdate);
			PushStatusUpdate(60.0);

			// pre-scale maps
			lblStatus.Text = "Scale Maps";
			Application.DoEvents();
			string sourceMapPath = Path.Combine(tbSourceMapFolder.Text, "map\\Provinces.bmp");
			foreach (HoI4Mod mod in selectedMods)
			{
				string possibleMap = Path.Combine(mod.location, "map\\Provinces.bmp");
				if (File.Exists(possibleMap))
				{
					sourceMapPath = possibleMap;
				}
			}
			Bitmap srcMap = (Bitmap)Bitmap.FromFile(sourceMapPath);
			PushStatusUpdate(80.0);
			srcMap.Tag = sourceMapPath;

			// add geo data to province lists
			lblStatus.Text = "Load Source Map";
			Application.DoEvents();
			Program.sourceMap = new MapReader(srcMap, Program.sourceDef.provinces, PushStatusUpdate, save.provinceOwners, countries.countries);
			Program.sourceMap.saveMap(tbSaveLocation.Text);
			PushStatusUpdate(100.0);

			lblStatus.Text = "Map Saved";
			Application.DoEvents();
		}

		void getSelectedMods()
		{
			selectedMods.Clear();
			HashSet<String> selectedModNames = new HashSet<string>();

			foreach (CheckBox control in modsGroup.Controls)
			{
				if ((control.GetType() == typeof(CheckBox)) && (control.Checked))
				{
					selectedModNames.Add(control.Text);
				}
			}

			foreach (HoI4Mod mod in mods)
			{
				if (selectedModNames.Contains(mod.name))
				{
					selectedMods.Add(mod);
				}
			}
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

		private void FolderBrowse_Click(object sender, EventArgs e)
		{
			var dialog = new FolderBrowserDialog();

			if (Directory.Exists(tbSourceMapFolder.Text))
			{
				dialog.SelectedPath = tbSourceMapFolder.Text;
			}

			dialog.ShowNewFolderButton = false;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				tbSourceMapFolder.Text = dialog.SelectedPath;
			}
		}

		private string ReadHoI4Folder()
		{
			var regKey =
					Registry.LocalMachine.OpenSubKey(
						@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 394360");

			if (regKey != null)
			{
				return getInstallationPathFromRegKey(regKey);
			}
			else
			{
				var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
				regKey =
				hklm.OpenSubKey(
						@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 394360");

				if (regKey != null)
				{
					return getInstallationPathFromRegKey(regKey);
				}
			}

			return string.Empty;
		}

		private string getInstallationPathFromRegKey(RegistryKey regKey)
		{
			var steamInstallationPath = regKey.GetValue("InstallLocation").ToString();

			if (!string.IsNullOrEmpty(steamInstallationPath))
			{
				if (Directory.Exists(steamInstallationPath))
				{
					return steamInstallationPath;
				}
			}

			return string.Empty;
		}

		private void saveBrowse_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();

			dialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Paradox Interactive\\Hearts of Iron IV\\save games");
			dialog.CheckFileExists = true;
			dialog.DefaultExt = "hoi4";
			dialog.Multiselect = false;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				tbSaveLocation.Text = dialog.FileName;
			}
		}

		private void DocsFolderButton_Click(object sender, EventArgs e)
		{
			var dialog = new FolderBrowserDialog();

			if (Directory.Exists(modFolder.Text))
			{
				dialog.SelectedPath = modFolder.Text;
			}

			dialog.ShowNewFolderButton = false;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				modFolder.Text = dialog.SelectedPath;
			}
		}

		private void ModFolderTextChanged(object sender, EventArgs e)
		{
			if (Directory.Exists(modFolder.Text))
			{
				getMods();
				if (mods.Count > 0)
				{
					this.Height = 196 + 26 + (17 * mods.Count);
					modsGroup = new GroupBox();
					modsGroup.SetBounds(13, 90, 649, 21 + (17 * mods.Count));
					this.Controls.Add(modsGroup);

					for (int i = 0; i < mods.Count; i++)
					{
						CheckBox checkBox = new CheckBox();
						checkBox.SetBounds(13, 13 + 17 * i, 626, 17);
						checkBox.Text = mods[i].name;
						modsGroup.Controls.Add(checkBox);
					}
				}
				else
				{
					foreach (System.Windows.Forms.Control control in modsGroup.Controls)
					{
						modsGroup.Controls.Remove(control);
					}
					this.Controls.Remove(modsGroup);
					this.Height = 196;
					if (modsGroup != null)
					{
						modsGroup = null; 
					}
				}
			}
		}

		private void getMods()
		{
			mods.Clear();
			string[] modFiles = Directory.GetFiles(modFolder.Text, "*.mod");
			foreach (string modFile in modFiles)
			{
				HoI4Mod mod = new HoI4Mod(modFile, modFolder.Text);
				mods.Add(mod);
			}
		}

		private GroupBox modsGroup;
		private List<HoI4Mod> mods = new List<HoI4Mod>();
		private List<HoI4Mod> selectedMods = new List<HoI4Mod>();
	}
}
