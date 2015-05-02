using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WITPJSON
{
    public class UnitTimeline
    {
        private static string timeline_directory { get { return Path.Combine(Program.output_directory, "Timelines"); } }
        private static string timeline_directory_allies { get { return Path.Combine(timeline_directory, "Allies"); } }
        private static string timeline_directory_japan { get { return Path.Combine(timeline_directory, "Japan"); } }
        


        internal static IEnumerable<UnitTimeline> generate_unit_timelines(IEnumerable<Turn> turns)
        {
            Directory.CreateDirectory(timeline_directory_allies);
            Directory.CreateDirectory(timeline_directory_japan);
            foreach (var turn in turns)
            {
                
            }
            return null;
        }
        public void render()
        {

        }
    }
}
