using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;



namespace ProvinceMapper
{
	class CountryReader
	{
		public CountryReader(string HoI4Location, List<HoI4Mod> mods)
		{
			getCountryFiles(HoI4Location, mods);
			getCountryColors(HoI4Location, mods);
		}

		private void getCountryFiles(string HoI4Location, List<HoI4Mod> mods)
		{
			countryFiles = new Dictionary<string, string>();
			string tagsPath = Path.Combine(HoI4Location, "common\\country_tags\\00_countries.txt");
			getCountryFilesFromTagsFile(tagsPath);
			foreach (var mod in mods)
			{
				tagsPath = Path.Combine(mod.location, "common\\country_tags\\00_countries.txt");
				if (File.Exists(tagsPath))
				{
					getCountryFilesFromTagsFile(tagsPath);
				}
			}
		}

		private void getCountryFilesFromTagsFile(string tagsPath)
		{
			StreamReader sr = new StreamReader(tagsPath);
			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine();
				line = line.Trim();
				if (line.Length > 3)
				{
					string tag = line.Substring(0, 3);
					int firstQuote = line.IndexOf("\"");
					string file = line.Substring(firstQuote);
					file = file.Trim('\"');
					countryFiles[tag] = file;
				}
			}

			sr.Close();
		}

		private void getCountryColors(string HoI4Location, List<HoI4Mod> mods)
		{
			countries = new Dictionary<String, Color>();

			foreach (KeyValuePair<String, String> countryFile in countryFiles)
			{
				string countryFilePath = Path.Combine(HoI4Location, "common", countryFile.Value);
				if (File.Exists(countryFilePath))
				{
					getCountryColor(countryFile.Key, countryFilePath);
				}
				foreach (HoI4Mod mod in mods)
				{
					countryFilePath = Path.Combine(mod.location, "common", countryFile.Value);
					if (File.Exists(countryFilePath))
					{
						getCountryColor(countryFile.Key, countryFilePath);
					}
				}
			}
		}

		private void getCountryColor(string tag, string location)
		{
			StreamReader sr = new StreamReader(location);
			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine();
				if (line.Length == 0)
				{
					continue;
				}
				else if (line.Substring(0, 1) == "#")
				{
					continue;
				}
				else if (!line.Contains("color"))
				{
					continue;
				}

				int[] digits = getDigitsfromLine(line);
				Color countryColor = Color.FromArgb(0, digits[0], digits[1], digits[2]);
				countries[tag] = countryColor;
			}
			sr.Close();
		}

		private int[] getDigitsfromLine(string line)
		{
			int[] digits = { 0, 0, 0 };
			int whichDigit = 0;
			string[] tokens = line.Split(' ');
			foreach (string token in tokens)
			{
				if (token == "color")
				{
					continue;
				}
				else if (token == "=")
				{
					continue;
				}
				else if (token == "{")
				{
					continue;
				}
				else if (token == "}")
				{
					continue;
				}
				else if (token == "")
				{
					continue;
				}
				digits[whichDigit] = Convert.ToInt32(token);
				whichDigit++;
				if (whichDigit == 3)
				{
					break;
				}
			}

			return digits;
		}

		private Dictionary<String, String> countryFiles;
		public Dictionary<String, Color> countries;
	}
}