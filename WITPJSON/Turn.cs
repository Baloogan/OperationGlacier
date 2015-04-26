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

        public string side_initial
        {
            get
            {
                if (side == Side.Allies) return "a";
                else return "j";
            }
        }

        public DateTime date;

        public List<CombatEvent> CombatEvents;
        public List<AfterActionReport> AfterActionReports;
        public List<SigInt> SigInts;
        public List<OperationReport> OperationReports;
        public List<Hex> Hexes;

        public string date_string { get { return string.Format("{0:00}{1:00}{2:00}", date.Year - 1900, date.Month, date.Day); } }

        public string output_directory { get { return Path.Combine(Program.output_directory, side.ToString(), date_string); } }
        public string output_filename { get { return Path.Combine(this.output_directory, "Turn.json"); } }

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
        public string CombatEvents_filename { get { return Path.Combine(archive_directory, string.Format("Combat_Events_{0}.txt", date_string)); } }
        public string AfterActionReports_filename { get { return Path.Combine(archive_directory, string.Format("combatreport_{0}.txt", date_string)); } }
        public string SigInts_filename { get { return Path.Combine(archive_directory, string.Format("{0}sigint_{1}.txt", side_initial, date_string)); } }
        public string OperationReports_filename { get { return Path.Combine(archive_directory, string.Format("{0}operationsreport_{1}.txt", side_initial, date_string)); } }

        public Turn(Side side, DateTime date)
        {
            this.side = side;
            this.date = date;
            ParseCombatEvents();
            ParseAfterActionReports();
            ParseSigInts();
            ParseOperationReports();
            CompileHexes();
        }
        private void CompileHexes()
        {
            Hexes = new List<Hex>();
            foreach (var aar in AfterActionReports)
            {
                var a = Hexes.FirstOrDefault(hex => hex.x == aar.x && hex.y == aar.y);
                if (a == null)
                {
                    Hexes.Add(new Hex() {html = aar.report, x = aar.x, y = aar.y});
                }
                else
                {
                    a.html = a.html + aar.report;
                }
            }
        }
        private void ParseCombatEvents()
        {
            CombatEvents = new List<CombatEvent>();

        }

        private void ParseAfterActionReports()
        {
            AfterActionReports = new List<AfterActionReport>();

            string file = File.ReadAllText(AfterActionReports_filename);

            var reports = file.Split(
                new string[] { "--------------------------------------------------------------------------------\r\n" },
                StringSplitOptions.None).Skip(1);

            AfterActionReports.AddRange(reports.Select(s => new AfterActionReport(s)));

        }

        private void ParseSigInts()
        {
            SigInts = new List<SigInt>();

        }
        private void ParseOperationReports()
        {
            OperationReports = new List<OperationReport>();

        }
        public void Render()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.Indented);
            Directory.CreateDirectory(output_directory);
            File.WriteAllText(output_filename, json);
        }
    }
}
