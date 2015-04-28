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
    class Turn //more a TurnCoordinator, this guy knows where stuff gotta go.
    {
        public enum Side
        {
            Allies,
            Japan
        }

        public Side side;

        [JsonIgnore]
        public string side_initial
        {
            get
            {
                if (side == Side.Allies) return "a";
                else return "j";
            }
        }

        public DateTime date;

        [JsonIgnore]
        public List<CombatEvent> CombatEvents;
        [JsonIgnore]
        public List<AfterActionReport> AfterActionReports;
        [JsonIgnore]
        public List<SigInt> SigInts;
        [JsonIgnore]
        public List<OperationReport> OperationReports;

        public List<Hex> Hexes;

        [JsonIgnore]
        public List<Unit> Units;

        [JsonIgnore]
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
        public string output_directory { get { return Path.Combine(Program.output_directory, side.ToString(), date_string); } }
        [JsonIgnore]
        public string output_filename { get { return Path.Combine(this.output_directory, "Turn.json"); } }

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
            ParseCombatEvents();
            ParseAfterActionReports();
            ParseSigInts();
            ParseOperationReports();
            ParseUnits();
            CompileHexes();
        }

        private void ParseUnits()
        {
            if (Directory.Exists(tracker_directory))
                Units = Unit.ParseUnits(tracker_directory);

        }
        private Hex getHex(int x, int y)
        {
            var a = Hexes.FirstOrDefault(hex => hex.x == x && hex.y == y);
            if (a == null)
            {
                a = new Hex() {x = x, y = y};
                Hexes.Add(a);
                return a;
            }
            else
            {
                return a;
            }
        }
        private void CompileHexes()
        {
            Hexes = new List<Hex>();

            if (Units != null)
            {
                foreach (var s in Units)
                {
                    var a = getHex(s.x, s.y);
                    a.html = a.html + s.report + "\r\n";

                    a.color = "black";
                        
                    
                }
            }
            foreach (var s in SigInts)
            {
                var a = getHex(s.x, s.y);
                a.html = a.html + s.report + "\r\n";
                a.color = "blue";
            }

            foreach (var s in OperationReports)
            {
                var a = getHex(s.x, s.y);
                a.html = a.html + s.report + "\r\n";
                a.color = "yellow";
            }
            foreach (var s in CombatEvents)
            {
                var a = getHex(s.x, s.y);
                a.html = a.html + s.report + "\r\n";
                a.color = "orange";
            }
            foreach (var s in AfterActionReports)
            {
                var a = getHex(s.x, s.y);
                a.html = a.html + s.report + "\r\n";
                a.color = "red";

            }

            Hexes = Hexes.Where(h => h.x != -1 && h.y != -1).ToList();

            foreach (var a in Hexes)
            {
                a.radius = 0.1F;
                a.fillOpacity = a.html.Length / 1000.0F;
                if (a.fillOpacity < 0.1) a.fillOpacity = 0.1F;
                if (a.fillOpacity > 1) a.fillOpacity = 1;
                a.fillColor = a.color;
            }

        }
        private void ParseCombatEvents()
        {
            CombatEvents = new List<CombatEvent>();

            string file = File.ReadAllText(CombatEvents_filename);

            var reports = file.Split(
                new string[] { "\r\n" },
                StringSplitOptions.None).Skip(2);

            CombatEvents = reports
                .Where(s => !s.Contains(" arrives at ")) //this is duplicated in operation reports, WITP being WITP :/
                .Select(s => new CombatEvent(s)).ToList();
        }

        private void ParseAfterActionReports()
        {
            string file = File.ReadAllText(AfterActionReports_filename);

            var reports = file.Split(
                new string[] { "--------------------------------------------------------------------------------\r\n" },
                StringSplitOptions.None).Skip(1);

            AfterActionReports = reports.Select(s => new AfterActionReport(s)).ToList();
        }

        private void ParseSigInts()
        {
            string file = File.ReadAllText(SigInts_filename);

            var reports = file.Split(
                new string[] { "\r\n" },
                StringSplitOptions.None).Skip(2);

            SigInts = reports.Select(s => new SigInt(s)).ToList();
        }
        private void ParseOperationReports()
        {
            string file = File.ReadAllText(OperationReports_filename);

            var reports = file.Split(
                new string[] { "\r\n" },
                StringSplitOptions.None).Skip(2);

            OperationReports = reports.Select(s => new OperationReport(s)).ToList();
        }
        public void Render()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.Indented);
            Directory.CreateDirectory(output_directory);
            File.WriteAllText(output_filename, json);
        }
    }
}
