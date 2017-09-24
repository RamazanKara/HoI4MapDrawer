using System;
using System.Collections.Generic;
using System.IO;



namespace ProvinceMapper
{
	class StateReader
	{
		public StateReader(string HoI4Location)
		{
			stateToProvinces = new Dictionary<int, List<int>>();

			string statesPath = Path.Combine(HoI4Location, "history\\states");
			string[] stateFiles = Directory.GetFiles(statesPath);
			foreach (string stateFile in stateFiles)
			{
				int id = -1;
				List<int> provinces = new List<int>();

				StreamReader sr = new StreamReader(stateFile);
				while (!sr.EndOfStream)
				{
					string line = sr.ReadLine();
					if (line.Contains("#"))
					{
						int hashPos = line.IndexOf("#");
						line = line.Substring(0, hashPos);
					}
					if (line.Contains("id"))
					{
						int equalsPos = line.IndexOf("=");
						string num = line.Substring(equalsPos + 1);
						id = Convert.ToInt32(num);
					}

					if (line.Contains("provinces"))
					{
						while(!line.Contains("{"))
						{
							line = sr.ReadLine();
						}
						line = line.Substring(line.IndexOf("{") + 1);
						provinces.AddRange(readInProvinces(line, sr));
					}
				}

				sr.Close();
				if ((id != -1) && (provinces.Count > 0))
				{
					stateToProvinces.Add(id, provinces);
				}
			}
		}

		private List<int> readInProvinces(string line, StreamReader sr)
		{
			List<int> provinces = new List<int>();
			provinces.AddRange(processLine(line));
			if (line.Contains("}"))
			{
				return provinces;
			}

			while (!sr.EndOfStream)
			{
				string newLine = sr.ReadLine();
				provinces.AddRange(processLine(newLine));

				if (newLine.Contains("}"))
				{
					break;
				}
			}

			return provinces;
		}

		private List<int> processLine(string line)
		{
			List<int> provinces = new List<int>();

			string[] tokens = line.Split();
			foreach (string token in tokens)
			{
				if ((token.Length > 0) && (token != "}"))
				{
					provinces.Add(Convert.ToInt32(token));
				}
			}

			return provinces;
		}

		public Dictionary<int, List<int>> stateToProvinces;
	}
}