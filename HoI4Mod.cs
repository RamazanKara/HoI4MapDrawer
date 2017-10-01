using System.IO;



namespace ProvinceMapper
{
	class HoI4Mod
	{
		public HoI4Mod(string modfile, string modFolder)
		{
			int lastSlash = modFolder.LastIndexOf("\\");
			string HoI4DocsFolder = modFolder.Substring(0, lastSlash);

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
				else if ((line.Length > 3) && (line.Substring(0,4) == "path"))
				{
					int quotePos = line.IndexOf("\"");
					string path = line.Substring(quotePos + 1);
					quotePos = path.IndexOf("\"");
					path = path.Substring(0, quotePos);
					location = Path.Combine(HoI4DocsFolder, path);
				}
				else if ((line.Length > 6) && (line.Substring(0, 7) == "archive"))
				{
					int quotePos = line.IndexOf("\"");
					location = line.Substring(quotePos + 1);
					quotePos = location.IndexOf("\"");
					location = location.Substring(0, quotePos);
				}
			}
		}

		public string name;
		public string location;
	}
}