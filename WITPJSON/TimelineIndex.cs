using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace WITPJSON
{
    public class TimelineIndex
    {
        public Dictionary<string, string> timeline_id_to_name;
        public TimelineIndex(IEnumerable<UnitTimeline> timelines)
        {
            timeline_id_to_name = timelines.ToDictionary(t => t.unit_data[0].timeline_id, t => t.name);
        }
        public void Render()
        {
            if (!Directory.Exists(Program.output_directory))
                Directory.CreateDirectory(Program.output_directory);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.None);
            File.WriteAllText(Path.Combine(Program.output_directory, "TimelineIndex.json"), json);
        }
    }
}
