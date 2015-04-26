﻿using System;
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
            foreach (var s in SigInts)
            {
                var a = Hexes.FirstOrDefault(hex => hex.x == s.x && hex.y == s.y);
                if (a == null)
                {
                    a = new Hex() { html = s.report + "\r\n", x = s.x, y = s.y };
                    Hexes.Add(a);
                }
                else
                {
                    a.html = a.html + s.report + "\r\n";
                }
                a.color = "blue";
            }
            
            foreach (var s in OperationReports)
            {
                var a = Hexes.FirstOrDefault(hex => hex.x == s.x && hex.y == s.y);
                if (a == null)
                {
                    a = new Hex() { html = s.report + "\r\n", x = s.x, y = s.y };
                    Hexes.Add(a);
                }
                else
                {
                    a.html = a.html + s.report + "\r\n";
                }
                a.color = "yellow";
            }
            foreach (var s in CombatEvents)
            {
                var a = Hexes.FirstOrDefault(hex => hex.x == s.x && hex.y == s.y);
                if (a == null)
                {
                    a = new Hex() { html = s.report + "\r\n", x = s.x, y = s.y };
                    Hexes.Add(a);
                }
                else
                {
                    a.html = a.html + s.report + "\r\n";
                }
                a.color = "orange";
            }
            foreach (var aar in AfterActionReports)
            {
                var a = Hexes.FirstOrDefault(hex => hex.x == aar.x && hex.y == aar.y);
                if (a == null)
                {
                    a = new Hex() { html = aar.report + "\r\n", x = aar.x, y = aar.y };
                    Hexes.Add(a);
                }
                else
                {
                    a.html = a.html + aar.report + "\r\n";
                }
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
