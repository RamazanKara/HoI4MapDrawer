using System.IO;



namespace ProvinceMapper
{
	class HoI4Mod
	{
		public HoI4Mod(string modfile)
		{
			StreamReader sr = new StreamReader(modfile);
			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine();
				line = line.Trim();
				if ((line.Length > 3) && (line.Substring(0, 4) == "name"))
				{
					int quotePos = line.IndexOf("\"");
					name = line.Substring(quotePos + 1);
					quotePos = name.IndexOf("\"");
					name = name.Substring(0, quotePos);
				}
			}
		}

		public string name;
	}
}