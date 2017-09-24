using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProvinceMapper
{
    class MapReader
    {
        public MapReader(Bitmap _map, List<Province> provinces, StatusUpdate su, Dictionary<int, string> provinceOwners, Dictionary<String, Color> countries)
        {
            su(0.0);

				foreach (Province province in provinces)
				{
					if (provinceOwners.ContainsKey(province.ID))
					{
						string owner = provinceOwners[province.ID];
						province.ownerColor = countries[owner];
					}
				}

            SortedList<int, Province> sortedProvs = new SortedList<int, Province>();
            foreach (Province p in provinces)
            {
                sortedProvs.Add(p.definitionColor.ToArgb(), p);
            }

            map = _map;

            bounds = new Rectangle(Point.Empty, map.Size);

            for (int x = 0; x < map.Width; ++x)
            {
                for (int y = 0; y < map.Height; ++y)
                {
						  Color color = map.GetPixel(x, y);
                    int argb = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
                    if (argb != (color.A << 24)) // ignore black pixels
                    {
                        Province match = null;
                        if (sortedProvs.TryGetValue(argb, out match))
                        {
                            match.area.Add(new Point(x, y));
									 map.SetPixel(x, y, match.ownerColor);
                        }
                    }
                }
                su(100.0 * x / map.Width);
            }
        }

        public Bitmap map;
        public Rectangle bounds;
    }
}
