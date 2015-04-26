using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITPJSON
{
    static class BaseToHex
    {
        public static List<Tuple<string, string>> base_hex_defs = null;
        public static string ReplaceMatches(string a)
        {

            if (base_hex_defs == null)
            {
                string[] lines = System.IO.File.ReadAllLines("BaseToHex.psv");
                base_hex_defs =
                    lines.Select(
                        s => new Tuple<string, string>(
                            s.Split('|').First(),
                            s.Split('|').Last()))
                        .ToList();
            }
            foreach (var t in base_hex_defs)
            {
                a = a.Replace(t.Item1, t.Item2);
            }
            return a;
        }
    }
}
