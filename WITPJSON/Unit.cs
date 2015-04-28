using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using System.Text.RegularExpressions;
namespace WITPJSON
{
    class Unit
    {
        public enum Type
        {
            Aircraft,
            AirframeProd,
            AirGroup,
            AirLoss,
            Base,
            Device,
            Engine,
            LCU,
            Leader,
            Pilot,
            ShipClass,
            ShipUpgrade,
            SunkShip,
            TF,
            VP
        }

        public Dictionary<string, string> row;
        public string name;
        public int id;
        public int x, y;
        public Type type;
        public string type_str { get { return type.ToString(); } }
        public string location;
        public string color;
        public int owner;
        public string report
        {
            get { return name; }
        }

        private static IEnumerable<Unit> ParseUnitsFromCSV(Type type, string filename)
        {
            using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new TextFieldParser(filename))
            {
                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(new string[] { "," });
                string[] header = parser.ReadFields();
                bool has_id = header.Contains("ID");
                bool has_name = header.Contains("Name");
                bool has_location = header.Contains("Location");
                bool has_owner = header.Contains("Owner");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    Unit u = new Unit();
                    u.type = type;
                    u.row = header.Zip(fields, (s, s1) => new Tuple<string, string>(s, s1))
                                  .ToDictionary(t => t.Item1, t => t.Item2);
                    if (has_id)
                        u.id = int.Parse(u.row["ID"]);
                    if (has_name)
                        u.name = u.row["Name"];
                    if (has_owner)
                        u.owner = int.Parse(u.row["Owner"]);

                    if (has_location)
                    {
                        u.location = u.row["Location"];
                        if (u.location.ToLower().Contains("delay"))
                        {
                            u.x = -1;
                            u.y = -1;
                        }
                        else
                        {
                            var myRegex = new Regex(@"(\d+),(\d+)");
                            var m = myRegex.Match(BaseToHex.ReplaceMatches(u.location));
                            if (m.Success)
                            {
                                u.x = int.Parse(m.Groups[1].Value);
                                u.y = int.Parse(m.Groups[2].Value);
                            }
                            else
                            {
                                u.x = -1;
                                u.y = -1;
                            }
                        }
                    }
                    yield return u;
                }
            }
        }
        internal static List<Unit> ParseUnits(string directory)
        {
            //bases
            var bases = ParseUnitsFromCSV(Type.Base, Path.Combine(directory, "Bases.csv"));
            var air_groups = ParseUnitsFromCSV(Type.AirGroup, Path.Combine(directory, "Airgroups.csv"));
            var a = new List<Unit>();
            a.AddRange(bases);
            a.AddRange(air_groups);
            return a;
        }

    }
}
