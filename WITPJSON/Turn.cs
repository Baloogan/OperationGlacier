using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace WITPJSON
{
    public class Turn //more a TurnCoordinator, this guy knows where stuff gotta go.
    {
        public enum Side
        {
            Allies,
            Japan
        }

        public int file_index;

        public Side side;

        public string side_string { get { if (side == Side.Allies) return "Allies"; else if (side == Side.Japan) return "Japan"; throw new Exception(); } }

        public DateTime date;

        public List<Hex> hexes;


        [JsonIgnore]
        public string side_initial
        {
            get
            {
                if (side == Side.Allies) return "a";
                else return "j";
            }
        }

        [JsonIgnore]
        public List<Unit> Units;

        public string date_string { get { return string.Format("{0:00}{1:00}{2:00}", date.Year - 1900, date.Month, date.Day); } }

        [JsonIgnore]
        public string tracker_directory
        {
            get
            {
                if (side == Side.Allies)
                    return Path.Combine(Program.allies_tracker_directory, date_string);
                else if (side == Side.Japan)
                    return Path.Combine(Program.japan_tracker_directory, date_string);
                else
                    throw new NotImplementedException();
            }
        }

        [JsonIgnore]
        public string output_directory { get { return Path.Combine(Program.output_turns_directory, side.ToString(), date_string); } }
        [JsonIgnore]
        public string output_filename { get { return Path.Combine(this.output_directory, "Turn"); } }

        [JsonIgnore]
        public string archive_directory
        {
            get
            {
                if (side == Side.Allies)
                    return Program.allies_archive_directory;
                else if (side == Side.Japan)
                    return Program.japan_archive_directory;
                else
                    throw new NotImplementedException();
            }
        }
        [JsonIgnore]
        public string CombatEvents_filename { get { return Path.Combine(archive_directory, string.Format("Combat_Events_{0}.txt", date_string)); } }
        [JsonIgnore]
        public string AfterActionReports_filename { get { return Path.Combine(archive_directory, string.Format("combatreport_{0}.txt", date_string)); } }
        [JsonIgnore]
        public string SigInts_filename { get { return Path.Combine(archive_directory, string.Format("{0}sigint_{1}.txt", side_initial, date_string)); } }
        [JsonIgnore]
        public string OperationReports_filename { get { return Path.Combine(archive_directory, string.Format("{0}operationsreport_{1}.txt", side_initial, date_string)); } }

        public Turn(Side side, DateTime date)
        {
            this.side = side;
            this.date = date;
        }
        public void compute()
        {

            Console.WriteLine(" Computing " + side.ToString() + " " + date_string);
            Units = new List<Unit>();
            Units.AddRange(Unit.ParseCombatEvents(CombatEvents_filename));
            Units.AddRange(Unit.ParseAfterActionReports(AfterActionReports_filename));
            Units.AddRange(Unit.ParseSigInts(SigInts_filename));
            Units.AddRange(Unit.ParseOperationReports(OperationReports_filename));
            Units.AddRange(Unit.ParseUnits(tracker_directory));

            foreach (Unit u in Units)
            {
                u.date = date;
                u.side = side;
                if (u.owner == -1)
                {
                    if (u.side == Side.Allies)
                    {
                        u.owner = 1;
                    }
                    if (u.side == Side.Japan)
                    {
                        u.owner = 0;
                    }
                }
            }

            Units = Units.Where(u =>
                string.IsNullOrEmpty(u.location)
                || !u.location.ToLower().Contains("delay")).ToList();

            foreach (var u in Units.Where(unit => unit.type == Unit.Type.AirGroup).ToArray())
            {
                if (!BaseToHex.is_base(u.location)) // put aircraft into ships
                {
                    //these ships might be subunits, so do before putting ships into tfs
                    Unit parent =
                        Units.First(unit => unit.type == Unit.Type.Ship && unit.name == u.location);
                    Units.Remove(u);
                    parent.subunits.Add(u);
                }
            }
            foreach (var u in Units.Where(unit => unit.type == Unit.Type.Ship).ToArray())
            {
                if (u.location.Contains("TF")) // put ships into tfs
                {
                    Unit parent =
                        Units.First(unit => unit.type == Unit.Type.TaskForce && unit.id == int.Parse(u.location.Substring(3)));
                    Units.Remove(u);
                    parent.subunits.Add(u);
                }
            }
            Console.WriteLine(" Units: " + Units.Count);
            Console.WriteLine(" Subunits: " + Units.Sum(u => u.subunits.Count() + u.subunits.Sum(su => su.subunits.Count())));
            CompileHexes();
            Console.WriteLine(" Compute complete!");
        }


        private Hex getHex(int x, int y)
        {
            var a = hexes.FirstOrDefault(hex => hex.x == x && hex.y == y);
            if (a == null)
            {
                a = new Hex() { x = x, y = y };
                hexes.Add(a);
                return a;
            }
            else
                return a;
        }
        private void CompileHexes()
        {
            Console.WriteLine(" CompileHexes...");
            hexes = new List<Hex>();

            foreach (var s in Units)
                getHex(s.x, s.y).units.Add(s);

            hexes = hexes.Where(h => h.x != -1 && h.y != -1).ToList();
        }
        public void Render()
        {

            Directory.CreateDirectory(output_directory);

            const int parts = 16;//don't change lightly, this number shows up in cshtmls
            Console.WriteLine(" Rendering to " + parts + "...");

            List<IEnumerable<Hex>> groups = hexes.OrderBy(hex => Program.random.NextDouble())
                                                 .Select((hex, i) => new { hex, i })
                                                 .GroupBy(x => x.i % parts)
                                                 .Select(x => x.Select(y => y.hex))
                                                 .ToList();
            var turns = groups.Select(h => new Turn(side, date) { hexes = h.ToList() })
                              .Select((turn, i) => new { turn, i });
            foreach (var t in turns)
            {
                var turn = t.turn;
                turn.file_index = t.i;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(turn, Formatting.None);
                File.WriteAllText(output_filename + turn.file_index.ToString() + ".json", json);
            }
            Console.WriteLine(" Rendering complete!");

        }
    }
}
