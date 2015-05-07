using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WITPJSON
{
    public class UnitTimeline
    {

        public int id { get { return unit_data.First().id; } }
        public string name { get { return unit_data.First().name; } }
        public Turn.Side side { get { return unit_data.First().side; } }
        public Unit.Type type { get { return unit_data.First().type; } }
        public DateTime first_seen { get { return unit_data.First().date; } }
        public DateTime last_seen { get { return unit_data.Last().date; } }
        public List<Unit> unit_data;

        public Dictionary<string, string> scendata_loc { get; set; }
        public Dictionary<string, string> scendata_shp { get; set; }
        public Dictionary<string, string> scendata_grp { get; set; }
        public Dictionary<string, string> scendata_air { get; set; }
        public Dictionary<string, string> scendata_toe { get; set; }
        public Dictionary<string, string> scendata_cls { get; set; }//the class for the ship in the latest incarnation
        public Dictionary<int,Dictionary<string, string>> scendata_dev { get; set; }//get the relevent devs for this unit as it is in its latest incarnation

        public UnitTimeline(IEnumerable<Unit> unit_data_)
        {
            unit_data = unit_data_.OrderBy(u => u.date).ToList(); // unit_data[0]= first appearance
            switch (unit_data[0].type)
            {
                case Unit.Type.Ship:
                    scendata_shp = ScenData.scendata_shp[unit_data.Last().id];
                    try
                    {
                        scendata_cls = ScenData.scendata_cls_lookup[unit_data.Last().row["Class"]];
                        foreach (var u in unit_data)
                        {
                            u.bitmap = int.Parse(scendata_cls["Bitmap"]);//this should go into turn files the way its setup
                            if(u.side ==Turn.Side.Allies){//AnSide0006.png
                                u.bitmap_name = "AnSide" + u.bitmap.ToString("0000") + ".png";
                            }else if(u.side == Turn.Side.Japan){
                                u.bitmap_name = "JnSide" + u.bitmap.ToString("0000") + ".png";
                            }
                            u.scendata["Type"] = scendata_cls["Type"];
                            u.scendata["CruiseSpeed"] = scendata_cls["CruiseSpeed"];
                            u.scendata["MaxSpeed"] = scendata_cls["MaxSpeed"];
                            u.scendata["Tonnage"] = scendata_cls["Tonnage"];
                        }
                    }
                    catch (Exception e)
                    {
                        //only a couple fail, I don't care! some brit sub typo error
                    }
                    //throw new NotImplementedException(); //get class too, parse unit type, and get me the right class!!!
                    break;
                case Unit.Type.LCU:
                    if (unit_data.Last().id >= 8500) //magic row from the spreadsheet
                        break;
                    scendata_loc = ScenData.scendata_loc[unit_data.Last().id];
                    foreach (var u in unit_data)
                    {
                        u.scendata["Suffix"] = scendata_loc["Suffix"];
                        u.scendata["Type"] = scendata_loc["Type"];
                        u.scendata["Nation"] = scendata_loc["Nation"];
                        u.scendata["HQtype"] = scendata_loc["HQtype"];
                        u.scendata["attribute"] = scendata_loc["attribute"];
                    }
                    if(scendata_loc["LCUFormationID"] != "0"){
                        scendata_toe = ScenData.scendata_loc[int.Parse(scendata_loc["LCUFormationID"])];
                    }
                    
                    break;
                case Unit.Type.Base:
                    scendata_loc = ScenData.scendata_loc[unit_data.Last().id];
                    foreach (var u in unit_data)
                    {
                        u.scendata["Type"] = scendata_loc["Type"];
                        u.scendata["Suffix"] = scendata_loc["Suffix"];
                        u.scendata["attribute"] = scendata_loc["attribute"];
                        if (scendata_loc["DailySupply"] != "0")
                            u.scendata["DailySupply"] = scendata_loc["DailySupply"];
                        if (scendata_loc["DailyFuel"] != "0")
                            u.scendata["DailyFuel"] = scendata_loc["DailyFuel"];
                        if(scendata_loc["monsoonEff"] != "0")
                            u.scendata["monsoonEff"] = scendata_loc["monsoonEff"];
                    }
                    break;
                case Unit.Type.AirGroup:
                    scendata_grp = ScenData.scendata_grp[unit_data.Last().id];
                    
                    try
                    {
                        scendata_air = ScenData.scendata_air_lookup[unit_data.Last().row["Model"]];
                        foreach (var u in unit_data)
                        {
                            u.bitmap = int.Parse(scendata_air["Bitmap"]);
                            
                            
                            //u.scendata["mvrAlt"] = scendata_air["mvrAlt"];
                            //                            u.scendata["mvrAlt2"] = scendata_air["mvrAlt2"];
                            //u.scendata["mvrAlt3"] = scendata_air["mvrAlt3"];
                            ////u.scendata["mvrAlt4"] = scendata_air["mvrAlt4"];
                            //u.scendata["mvrAlt5"] = scendata_air["mvrAlt5"];

                            ////u.scendata["attribute"] = scendata_air["attribute"];
                            //u.scendata["MaxAltitude"] = scendata_air["MaxAltitude"];
                            //////u.scendata["MaxSpeed"] = scendata_air["MaxSpeed"];
                            //u.scendata["CruiseSpeed"] = scendata_air["CruiseSpeed"];
                            //u.scendata["ClimbRate"] = scendata_air["ClimbRate"];
                            //u.scendata["Manuever"] = scendata_air["Manuever"];


                        }
                    }
                    catch (Exception e)
                    {
                        //only a couple fail, I don't care! Dinah'
                    }
                    break;
                default:
                    break;
            }
            get_devices();
            var biggest_gun = scendata_dev.OrderByDescending(f => int.Parse(f.Value["Effect"]));
            if (biggest_gun.Count() > 0)
            {
                foreach (var u in unit_data)
                {
                    u.scendata["BiggestDevice"] = biggest_gun.First().Value["Name"];
                }
            }
        }
        private void get_devices()
        {
            List<int> list = null;
            switch (unit_data[0].type)
            {
                case Unit.Type.Ship:
                    list = device_ids_ship();
                    break;
                case Unit.Type.LCU:
                    list = device_ids_loc();
                    break;
                case Unit.Type.Base:
                    list = device_ids_loc();
                    break;
                case Unit.Type.AirGroup:
                    list = device_ids_air();
                    break;
                default:
                    list = new List<int>();
                    break;
            }
            list = list.Where(l => l != 0).Where(l=>l < 2000).ToList();
            scendata_dev = new Dictionary<int, Dictionary<string, string>>();
            foreach (var dev_id in list)
            {
                scendata_dev[dev_id] = ScenData.scendata_dev[dev_id];
            }
        }
        private List<int> device_ids_ship()
        {
            if (scendata_cls == null)
                return new List<int>();
            return scendata_cls
                .Where(f => f.Key.Contains("WpnDevID"))
                .Select(f => int.Parse(f.Value))
                .ToList();
        }
        private List<int> device_ids_loc()
        {
            if (scendata_loc == null)
                return new List<int>();
            return scendata_loc
                .Where(f => f.Key.Contains("WpnDevID"))
                .Select(f => int.Parse(f.Value))
                .ToList();
        }
        private List<int> device_ids_air()
        {
            if (scendata_air == null)
                return new List<int>();
            return scendata_air
                .Where(f => f.Key.Contains("WpnDevID"))
                .Select(f => int.Parse(f.Value))
                .ToList();
        }


        internal static IEnumerable<UnitTimeline> generate_unit_timelines(IEnumerable<Turn> turns)
        {
            var unit_bag = turns
                .SelectMany(t => t.All_Units)
                .Where(u => u.type != Unit.Type.AfterAction && u.type != Unit.Type.CombatEvent && u.type != Unit.Type.SigInt && u.type != Unit.Type.OperationalReport && u.type != Unit.Type.TaskForce);

            var units = unit_bag.GroupBy(unit => unit.timeline_id);

            foreach (var a in units)
            {
                yield return new UnitTimeline(a);
            }
        }
        public void render()
        {
            if (!Directory.Exists(Program.output_timelines_directory))
                Directory.CreateDirectory(Program.output_timelines_directory);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.None);
            File.WriteAllText(Path.Combine(Program.output_timelines_directory, unit_data.First().timeline_id + ".json"), json);
        }
    }
}
