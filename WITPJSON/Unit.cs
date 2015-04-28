﻿using System;
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
            //Aircraft,
            //AirframeProd,
            AirGroup,
            //AirLoss,
            Base,
            //Device,
            //Engine,
            //LCU,
            //Leader,
            //Pilot,
            //ShipClass,
            //ShipUpgrade,
            Ship,
            //SunkShip,
            TF,
            //VP,

            SigInt,
            CombatEvent,
            AfterAction,
            OperationalReport
        }
        public bool is_report()
        {
            return type == Type.SigInt || type == Type.CombatEvent || type == Type.AfterAction ||
                   type == Type.OperationalReport;
        }
        public Dictionary<string, string> row;
        public string name;
        public int id;
        public int x, y;
        public Type type;
        public string type_str { get { return type.ToString(); } }
        public string location;
        public int owner;
        public string report;

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
        internal static IEnumerable<Unit> ParseUnits(string directory)
        {
            if (!Directory.Exists(directory))
                return Enumerable.Empty<Unit>();

            //bases
            var bases = ParseUnitsFromCSV(Type.Base, Path.Combine(directory, "Bases.csv"));
            var air_groups = ParseUnitsFromCSV(Type.AirGroup, Path.Combine(directory, "Airgroups.csv"));
            return bases.Concat(air_groups);
        }


        internal static IEnumerable<Unit> ParseCombatEvents(string CombatEvents_filename)
        {
            string file = File.ReadAllText(CombatEvents_filename);

            var reports = file.Split(
                new string[] { "\r\n" },
                StringSplitOptions.None).Skip(2);

            var a = reports.Where(s => !s.Contains(" arrives at "));
            //this is duplicated in operation reports, WITP being WITP :/
            foreach (var b in a)
            {
                Unit u = new Unit();
                u.report = b;
                u.type = Unit.Type.CombatEvent;

                var myRegex = new Regex(@"(\d+),(\d+)");
                var m = myRegex.Match(BaseToHex.ReplaceMatches(b));
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
                yield return u;
            }
        }

        internal static IEnumerable<Unit> ParseAfterActionReports(string AfterActionReports_filename)
        {
            string file = File.ReadAllText(AfterActionReports_filename);

            var reports = file.Split(
                new string[] { "--------------------------------------------------------------------------------\r\n" },
                StringSplitOptions.None).Skip(1);

            foreach (var a in reports)
            {
                Unit u = new Unit();
                u.type = Unit.Type.AfterAction;
                u.report = a;
                var lines = a.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                lines = lines.Where(s => s != " ").ToArray();
                var myRegex = new Regex(@"(\d+),(\d+)");
                var m = myRegex.Match(lines[0]);
                u.x = int.Parse(m.Groups[1].Value);
                u.y = int.Parse(m.Groups[2].Value);
                yield return u;
            }

        }

        internal static IEnumerable<Unit> ParseSigInts(string SigInts_filename)
        {
            string file = File.ReadAllText(SigInts_filename);

            var reports = file.Split(
                new string[] { "\r\n" },
                StringSplitOptions.None).Skip(2);

            foreach (var a in reports)
            {
                Unit u = new Unit();
                u.type = Unit.Type.SigInt;
                u.report = a;
                var myRegex = new Regex(@"(\d+),(\d+)");
                var m = myRegex.Match(BaseToHex.ReplaceMatches(a));
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
                yield return u;
            }
        }
        internal static IEnumerable<Unit> ParseOperationReports(string OperationReports_filename)
        {
            string file = File.ReadAllText(OperationReports_filename);

            var reports = file.Split(
                new string[] { "\r\n" },
                StringSplitOptions.None).Skip(2);

            foreach (var a in reports)
            {
                Unit u = new Unit();
                u.type = Unit.Type.OperationalReport;
                u.report = a;
                var myRegex = new Regex(@"(\d+),(\d+)");
                var m = myRegex.Match(BaseToHex.ReplaceMatches(a));
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
                yield return u;
            }
        }

    }
}
