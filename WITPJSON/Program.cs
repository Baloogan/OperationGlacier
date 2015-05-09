using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WITPJSON
{
    class Program
    {
        internal static string allies_archive_directory = null;
        internal static string allies_tracker_directory = null;
        internal static string japan_archive_directory = null;
        internal static string japan_tracker_directory = null;
        internal static string output_directory = null;
        internal static string output_turns_directory { get { return Path.Combine(output_directory, "Turns"); } }
        internal static string output_timelines_directory { get { return Path.Combine(output_directory, "Timeline"); } }
        internal static string output_scendata_directory { get { return Path.Combine(output_directory, "Scendata"); } }
        public static Random random = new Random();
        private static IEnumerable<Turn> turns;
        private static IEnumerable<UnitTimeline> timelines;
        private static void Main(string[] args)
        {
            Console.WriteLine("WITPJSON v0.1");
            const int selection = 1;
            switch (System.Environment.MachineName)
            {
                case "STONEBURNER":
                    Console.WriteLine("Running on: STONEBURNER");
                    if (selection == 1)
                    {
                        allies_archive_directory = @"B:\War in the Pacific Admiral's Edition\save\archive";
                        japan_archive_directory = @"C:\Dropbox\HistoricalGamer\archive";
                        allies_tracker_directory = @"B:\War in the Pacific Admiral's Edition\tracker\AlliesTracker";
                        japan_tracker_directory = @"C:\Dropbox\HistoricalGamer\tracker";
                        output_directory = @"C:\maps\gamedata_main\GameData\";
                    }
                    break;
                case "WIN-QPCSS4CO8PJ":
                    throw new PlatformNotSupportedException();
            }
            
            ProcessScendata();
            ProcessTurns();
            ProcessTimelines();

            //RenderScendata();
            //RenderTurns();
            //RenderTimelines();

            Game game = new Game(turns, timelines);
            game.Render();

            TimelineIndex ti = new TimelineIndex(timelines);
            ti.Render();
        }
        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            foreach (string directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
        }
        static void ProcessScendata()
        {
            Console.WriteLine("Reading scendata");
            ScenData.init();
        }
        static void RenderScendata()
        {
            Console.WriteLine("Rendering scendata");
            DeleteDirectory(output_timelines_directory);
            if (!Directory.Exists(Program.output_timelines_directory))
                Directory.CreateDirectory(Program.output_timelines_directory);
            ScenData.Render();
        }
        static void RenderTurns()
        {
            DeleteDirectory(output_turns_directory);
            Console.WriteLine("Rendering turns.");
            foreach (var turn in turns)
            {
                turn.Render();
            }
        }
        static void RenderTimelines()
        {
            DeleteDirectory(output_timelines_directory);
            
            Console.WriteLine("Rendering " + timelines.Count() + " timelines.");
            foreach (var timeline in timelines)
            {
                timeline.render();
            }
        }
        static void ProcessTimelines()
        {
            Console.WriteLine("Computing timelines");
            timelines = UnitTimeline.generate_unit_timelines(turns).ToList();
        }
        static void ProcessTurns()
        {
            Console.WriteLine("Processing turns");
            var allies_days = GetDays(allies_archive_directory).Distinct();
            var allies_turns = allies_days.Select(t => new Turn(Turn.Side.Allies, t) { file_index = -1 });
            Console.WriteLine(allies_turns.Count() + " allied turns found");
            var japan_days = GetDays(japan_archive_directory).Distinct();
            var japan_turns = japan_days.Select(t => new Turn(Turn.Side.Japan, t) { file_index = -1 });
            Console.WriteLine(japan_turns.Count() + " japan turns found");
            turns = allies_turns.Concat(japan_turns).ToList();
            foreach (var turn in turns)
            {
                turn.compute();
            }
        }
        static IEnumerable<DateTime> GetDays(string directory)
        {
            var dir = Directory.EnumerateFiles(directory).ToList();
            var myRegex = new Regex(@"_(\d\d)(\d\d)(\d\d)\.txt");
            foreach (var filename in dir)
            {
                var m = myRegex.Match(filename);
                var year = int.Parse(m.Groups[1].Value) + 1900;
                var month = int.Parse(m.Groups[2].Value);
                var day = int.Parse(m.Groups[3].Value);
                yield return new DateTime(year, month, day);
            }
        }
    }
}
