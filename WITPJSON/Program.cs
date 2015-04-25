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
        internal static string archive_directory = null;
        internal static string output_directory = null;

        static void Main(string[] args)
        {

            switch (System.Environment.MachineName)
            {
                case "STONEBURNER":
                    archive_directory = @"B:\War in the Pacific Admiral's Edition\save\archive";
                    output_directory = @"C:\Dropbox\OperationGlacier\OperationGlacier\App_Data\archive";
                    break;
                case "WIN-QPCSS4CO8PJ":
                    archive_directory = @"\\VBOXSVR\archive";
                    output_directory = @"C:\Dropbox\OperationGlacier\OperationGlacier\App_Data\archive";
                    break;
                default:
                    throw new PlatformNotSupportedException();
            }

            ClearOutputDirectory();

            var days = GetDays();
            var turns = days.Select(t => new Turn(t));
            //var json_turns = turns.Select(t => Newtonsoft.Json.JsonConvert.SerializeObject(t));
            foreach (var turn in turns)
            {
                turn.Render();
            }
            
        }
        static IEnumerable<DateTime> GetDays()
        {
            var dir = Directory.EnumerateFiles(archive_directory).ToList();
            var myRegex = new Regex(@"_(\d\d)(\d\d)(\d\d)\.txt");
            foreach (var filename in dir)
            {
                var m = myRegex.Match(filename);
                var year = int.Parse(m.Groups[0].Value);
                var month = int.Parse(m.Groups[1].Value);
                var day = int.Parse(m.Groups[2].Value);
                yield return new DateTime(year, month, day);
            }
        }
        static void ClearOutputDirectory()
        {
            Directory.Delete(archive_directory);
            Directory.CreateDirectory(archive_directory);
        }
        
    }
}
