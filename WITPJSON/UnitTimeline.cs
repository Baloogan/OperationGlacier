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
                    scendata_cls = ScenData.scendata_cls_lookup[unit_data.Last().row["Class"]];
                    //throw new NotImplementedException(); //get class too, parse unit type, and get me the right class!!!
                    break;
                case Unit.Type.LCU:
                    scendata_loc = ScenData.scendata_loc[unit_data.Last().id];
                    if(scendata_loc["LCUFormationID"] != "0"){
                        scendata_toe = ScenData.scendata_loc[int.Parse(scendata_loc["LCUFormationID"])];
                    }
                    
                    break;
                case Unit.Type.Base:
                    scendata_loc = ScenData.scendata_loc[unit_data.Last().id];
                    break;
                case Unit.Type.AirGroup:
                    scendata_grp = ScenData.scendata_grp[unit_data.Last().id];
                    scendata_loc = ScenData.scendata_loc[unit_data.Last().id];
                    scendata_air = ScenData.scendata_air_lookup[unit_data.Last().row["Model"]];
                    break;
                default:
                    break;
            }
            get_devices();
        }
        private void get_devices()
        {

            //throw new NotImplementedException(); 
        }

        


        internal static IEnumerable<UnitTimeline> generate_unit_timelines(IEnumerable<Turn> turns)
        {
            var unit_bag = turns.SelectMany(t => t.All_Units).Where(u => u.type != Unit.Type.AfterAction && u.type != Unit.Type.CombatEvent && u.type != Unit.Type.SigInt && u.type != Unit.Type.OperationalReport && u.type != Unit.Type.TaskForce);
            var units = unit_bag.GroupBy(unit => unit.timeline_id);
            foreach (var a in units)
            {
                yield return new UnitTimeline(a);
            }
        }
        public void render()
        {
        }
    }
}
