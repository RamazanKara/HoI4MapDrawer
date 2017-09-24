using System;
using System.Collections.Generic;
using System.IO;



namespace ProvinceMapper
{
	class SaveReader
	{
		public SaveReader(string saveName)
		{
			StreamReader theSave = new StreamReader(saveName);
			while (!theSave.EndOfStream)
			{
				string line = theSave.ReadLine();
				if (line == "states={")
				{
					readStates(theSave);
				}
			}
			theSave.Close();
		}

		private void readStates(StreamReader theSave)
		{
			stateOwners = new Dictionary<int, string>();

			while (!theSave.EndOfStream)
			{
				string line = theSave.ReadLine();
				if (line.Contains("={"))
				{
					int numEnd = line.IndexOf("=");
					string numString = line.Substring(0, numEnd);
					int stateNum = Convert.ToInt32(numString);
					readState(stateNum, theSave);
				}
				if (line.Contains("}"))
				{
					return;
				}
			}
		}

		private void readState(int stateNum, StreamReader theSave)
		{
			int bracesLevel = 0;
			while (!theSave.EndOfStream)
			{
				string line = theSave.ReadLine();
				if (line.Contains("{"))
				{
					bracesLevel++;
				}
				if (line.Contains("}"))
				{
					if (bracesLevel == 0)
					{
						return;
					}
					bracesLevel--;
				}
				if (line.Contains("owner"))
				{
					int equalsPos = line.IndexOf('=');
					string ownerTag = line.Substring(equalsPos + 1);
					ownerTag = ownerTag.Trim();
					ownerTag = ownerTag.Trim('\"');
					stateOwners.Add(stateNum, ownerTag);
				}
			}
		}

		private Dictionary<int, string> stateOwners;
	}
}