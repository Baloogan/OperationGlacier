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

        public UnitTimeline(IEnumerable<Unit> unit_data_)
        {
            unit_data = unit_data_.OrderBy(u => u.date).ToList();
        }


        internal static IEnumerable<UnitTimeline> generate_unit_timelines(IEnumerable<Turn> turns)
        {
            var unit_bag = turns.SelectMany(t => t.Units).Where(u => u.type != Unit.Type.AfterAction && u.type != Unit.Type.CombatEvent && u.type != Unit.Type.SigInt && u.type != Unit.Type.OperationalReport);
            var units = unit_bag.GroupBy(unit => unit.timeline_id);
            foreach (var a in units)
            {
                yield return new UnitTimeline(a);
            }
        }
        public void render()
        {

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.None);
            File.WriteAllText(Path.Combine(Program.output_timelines_directory, unit_data.First().timeline_id + ".json"), json);
        }
    }
}
