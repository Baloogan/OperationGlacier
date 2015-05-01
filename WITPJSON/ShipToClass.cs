using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITPJSON
{
    class ShipToClass
    {
        public static List<Tuple<string, string>> ship_class_defs = null;
        private static void populate_defs()
        {
            if (ship_class_defs == null)
            {
                string[] lines = System.IO.File.ReadAllLines("ShipToClass.psv");
                ship_class_defs =
                    lines.Select(
                        s => new Tuple<string, string>(
                            s.Split('|').First(),
                            s.Split('|').Last()))
                        .ToList();
            }
        }
        public static string ShipName_To_Class(string name)
        {
            populate_defs();
            string c = name.Split(' ').First();
            return ship_class_defs.First(tuple => tuple.Item1 == c).Item2;
        }
    }
}
