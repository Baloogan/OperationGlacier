using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var myRegex = new Regex(@"(\d+),(\d+)");
            var m = myRegex.Match(a);
            if (m.Success)
                return a;
            foreach (var t in base_hex_defs)
            {
                if (a == t.Item1)
                {
                    a = t.Item2;
                    break;
                }
                if (a.Contains(t.Item1 + " "))
                {
                    a = a.Replace(t.Item1 + " ", t.Item2);
                    break;
                }

            }
            return a;
        }
    }
}
