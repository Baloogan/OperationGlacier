using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace WITPJSON
{
    class ScenData
    {
        public static Dictionary<int, Dictionary<string, string>> scendata_air = null;
        public static Dictionary<string, Dictionary<string, string>> scendata_air_lookup = null;
        public static Dictionary<int, Dictionary<string, string>> scendata_cls = null;
        public static Dictionary<string, Dictionary<string, string>> scendata_cls_lookup = null;
        public static Dictionary<int, Dictionary<string, string>> scendata_dev = null;
        public static Dictionary<int, Dictionary<string, string>> scendata_grp = null;
        public static Dictionary<int, Dictionary<string, string>> scendata_loc = null;
        public static Dictionary<int, Dictionary<string, string>> scendata_shp = null;

        private static Dictionary<int, Dictionary<string, string>> read_scendata(string filename)
        {
            Dictionary<int, Dictionary<string, string>> scendata = new Dictionary<int, Dictionary<string, string>>();
            using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new TextFieldParser(filename))
            {

                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(new string[] { "," });
                string[] header = parser.ReadFields();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    var row = header.Zip(fields, (s, s1) => new Tuple<string, string>(s, s1))
                                  .ToDictionary(t => t.Item1, t => t.Item2);
                    scendata[int.Parse(row["UnitNumber"])] = row;
                }
            }
            return scendata;
        }
        private static bool has_init = false;
        public static void init()
        {
            if (has_init)
                return;
            has_init = true;

            scendata_air = read_scendata(@"B:\War in the Pacific Admiral's Edition\SCEN\WITPair001.csv");
            foreach (var a in scendata_air)
            {
                //placeholder for replacements and such

            }
            scendata_air_lookup = new Dictionary<string, Dictionary<string, string>>();
            foreach (var a in scendata_air)
            {
                if (string.IsNullOrWhiteSpace(a.Value["Name"]))
                    continue;

                scendata_air_lookup[a.Value["Name"]] = a.Value;

            }



            scendata_cls = read_scendata(@"B:\War in the Pacific Admiral's Edition\SCEN\WITPcls001.csv");
            foreach (var a in scendata_cls)
            {
                //placeholder for replacements and such

            }
            scendata_cls_lookup = new Dictionary<string, Dictionary<string, string>>();
            foreach (var a in scendata_cls)
            {
                if (string.IsNullOrWhiteSpace(a.Value["Name"]))
                    continue;
                if (scendata_cls_lookup.ContainsKey(a.Value["Name"]))
                {
                    //first one already done, append date to classname "Wasp 6/42" "Essex 'Short Hull' 10/43"
                    scendata_cls_lookup[a.Value["Name"] + " " + a.Value["AvailMonth"] + "/" + a.Value["AvailYear"]] = a.Value;
                }
                else
                {
                    //this is the first one! simple addition!
                    scendata_cls_lookup[a.Value["Name"]] = a.Value;
                }
            }


            scendata_dev = read_scendata(@"B:\War in the Pacific Admiral's Edition\SCEN\WITPdev001.csv");
            scendata_grp = read_scendata(@"B:\War in the Pacific Admiral's Edition\SCEN\WITPgrp001.csv");



            scendata_loc = read_scendata(@"B:\War in the Pacific Admiral's Edition\SCEN\WITPloc001.csv");
            Dictionary<string, string> Nation_lookup = new Dictionary<string, string>() { { "0", "none" }, { "1", "IJ Army" }, { "2", "IJ Navy" }, { "4", "US Navy" }, { "5", "US Army" }, { "6", "US Marines" }, { "7", "Australian" }, { "8", "New Zealand" }, { "9", "British" }, { "10", "French" }, { "11", "Dutch" }, { "12", "Chinese" }, { "13", "Soviet" }, { "14", "Indian" }, { "15", "Commonwealth" }, { "16", "Philippines" }, { "17", "----" }, { "18", "Canada" }, { "19", "----" } };
            foreach (var a in scendata_loc)
            {
                a.Value["Nation"] = Nation_lookup[a.Value["Nation"]];

            }

            scendata_shp = read_scendata(@"B:\War in the Pacific Admiral's Edition\SCEN\WITPshp001.csv");
            foreach (var a in scendata_shp)
            {
                a.Value["Nationality"] = Nation_lookup[a.Value["Nationality"]];

            }

        }
        private static void RenderDict(string name, object o)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(o, Formatting.None);
            File.WriteAllText(Path.Combine(Program.output_scendata_directory, name + ".json"), json);

        }
        public static void Render()
        {
            if (!Directory.Exists(Program.output_scendata_directory))
                Directory.CreateDirectory(Program.output_scendata_directory);

            RenderDict("air", scendata_air);
            RenderDict("air_lookup", scendata_air_lookup);
            RenderDict("cls", scendata_cls);
            RenderDict("cls_lookup", scendata_cls_lookup);
            RenderDict("dev", scendata_dev);
            RenderDict("grp", scendata_grp);
            RenderDict("loc", scendata_loc);
            RenderDict("shp", scendata_shp);
        }
    }
}
