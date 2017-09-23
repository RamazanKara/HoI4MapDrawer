﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProvinceMapper
{
    class DefinitionReader
    {
        public DefinitionReader(string path, StatusUpdate su)
        {
            StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1252));
            sr.ReadLine(); // discard first line
            while (!sr.EndOfStream)
            {
                string province = sr.ReadLine();
                string[] provinceTokens = province.Split(';');
                if ( (provinceTokens[4] == "RNW") || ((provinceTokens[4].Length > 5) && (provinceTokens[4].Substring(0, 6) == "Unused")) )
                {
                    continue;
                }
                try
                {
                    Province p = new Province(provinceTokens);
                    provinces.Add(p);
                }
                catch
                {
                }
                su(100.0 * sr.BaseStream.Position / sr.BaseStream.Length);
            }
            sr.Close();
        }

        public List<Province> provinces = new List<Province>();
    }
}
