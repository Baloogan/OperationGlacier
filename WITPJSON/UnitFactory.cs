﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace WITPJSON
{
    partial class Unit
    {
        

        private static IEnumerable<Unit> ParseUnitsFromCSV(Type type, string filename)
        {
            Console.WriteLine("   " + Path.GetFileName(filename));
            using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new TextFieldParser(filename))
            {

                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(new string[] { "," });
                string[] header = parser.ReadFields();
                bool has_id = header.Contains("ID");
                bool has_name = header.Contains("Name");
                bool has_location = header.Contains("Location");
                bool has_owner = header.Contains("Owner");
                if (type == Type.Ship)
                {

                    header[8] = "Cargo1";
                    header[9] = "Exp";
                    header[13] = "Planes";
                    header[14] = "AirSortie";
                    header[15] = "AirTorps";
                    header[18] = "Cargo2";

                }
                if (type == Type.AirGroup)
                {
                    header[11] = "MaxPlanes";
                    header[12] = "CAP";
                    header[13] = "LRCAP";
                    header[14] = "ASW";
                    header[15] = "Search";
                    header[16] = "Train";
                    header[17] = "Rest";
                    header[19] = "Upgrade";
                    header[25] = "LeadAir";
                    header[26] = "LeadInsp";
                }
                if (type == Type.TaskForce)
                {
                    header[7] = "DockLevel";
                    header[8] = "ETAHome";
                }
                if (type == Type.Base)
                {
                    header[15] = "AirS";
                    for (int i = 0; i < header.Count(); ++i)
                    {
                        header[i] = header[i].Replace(" ", "");
                    }
                }
                if (type == Type.LCU)
                {
                    header[15] = "OpMode";
                    header[16] = "Planfor";
                    header[17] = "Plan";
                    header[22] = "BaseLoad";
                }
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
                    if (type == Type.Ship)
                    {
                        u.ship_class = ShipToClass.ShipName_To_Class(u.name);
                    }
                    if (has_location)
                    {
                        u.location = u.row["Location"];
                        if (u.location.ToLower().Contains("delay") || u.location.ToLower().Contains("loaded on"))
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
                    if (u.type == Type.TaskForce)
                    {
                        u.name = "TF" + (u.id - 8000).ToString();
                    }
                    yield return u;
                }
            }
        }
        internal static IEnumerable<Unit> ParseUnits(string directory)
        {
            Console.WriteLine("  ParseUnits...");
            if (!Directory.Exists(directory))
                return Enumerable.Empty<Unit>();


            //bases
            var bases = ParseUnitsFromCSV(Type.Base, Path.Combine(directory, "Bases.csv"));
            var air_groups = ParseUnitsFromCSV(Type.AirGroup, Path.Combine(directory, "Airgroups.csv"));
            var lcus = ParseUnitsFromCSV(Type.LCU, Path.Combine(directory, "LCUs.csv"));
            var tfs = ParseUnitsFromCSV(Type.TaskForce, Path.Combine(directory, "TFs.csv"));
            var ships = ParseUnitsFromCSV(Type.Ship, Path.Combine(directory, "Ships.csv"));

            var units = Enumerable.Empty<Unit>()
                .Concat(air_groups)
                .Concat(lcus)
                .Concat(bases)
                .Concat(tfs)
                .Concat(ships)
                ;

            return units;
        }


        internal static IEnumerable<Unit> ParseCombatEvents(string CombatEvents_filename)
        {
            Console.WriteLine("  ParseCombatEvents...");
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
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        internal static IEnumerable<Unit> ParseAfterActionReports(string AfterActionReports_filename)
        {
            Console.WriteLine("  ParseAfterActionReports...");
            string file = File.ReadAllText(AfterActionReports_filename);

            var reports = file.Split(
                new string[] { "--------------------------------------------------------------------------------\r\n" },
                StringSplitOptions.None).Skip(1);

            foreach (var a in reports)
            {
                Unit u = new Unit();
                u.type = Unit.Type.AfterAction;
                u.report = a.TrimEnd(new char[]{'\r','\n',' '});
                u.hash = CalculateMD5Hash(u.report);
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
            Console.WriteLine("  ParseSigInts...");
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
            Console.WriteLine("  ParseOperationReports...");
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
