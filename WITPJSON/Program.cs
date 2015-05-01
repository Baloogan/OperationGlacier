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
        public static Random random = new Random();

        private static void Main(string[] args)
        {
            Console.WriteLine("WITPJSON v0.1");
            switch (System.Environment.MachineName)
            {
                case "STONEBURNER":
                    Console.WriteLine("Running on: STONEBURNER");
                    allies_archive_directory = @"B:\War in the Pacific Admiral's Edition\save\archive";
                    japan_archive_directory = @"C:\Dropbox\HistoricalGamer\archive";
                    allies_tracker_directory = @"B:\War in the Pacific Admiral's Edition\tracker\AlliesTracker";
                    japan_tracker_directory = @"B:\War in the Pacific Admiral's Edition\tracker\JapanTracker";
                    output_directory = @"C:\Dropbox\OperationGlacier\OperationGlacier\GameData\";
                    break;
                case "WIN-QPCSS4CO8PJ":
                    allies_archive_directory = @"\\VBOXSVR\archive";
                    japan_archive_directory = @"C:\Dropbox\HistoricalGamer\archive";
                    output_directory = @"C:\Dropbox\OperationGlacier\OperationGlacier\GameData\";
                    throw new PlatformNotSupportedException();
                    //break;
                default:
                    throw new PlatformNotSupportedException();
            }

            ClearOutputDirectory();

            ProcessTurns();

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
            var turns = allies_turns.Concat(japan_turns).ToList();
            foreach (var turn in turns)
            {
                turn.compute();
                turn.Render();
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
        static void ClearOutputDirectory()
        {
            Console.WriteLine("Clearing output directory");
            try
            {
                new DirectoryInfo(output_directory).Delete(true);
            }
            catch (Exception e)
            {

            }
            Directory.CreateDirectory(output_directory);
        }

    }
}
