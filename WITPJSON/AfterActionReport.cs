using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WITPJSON
{
    class AfterActionReport
    {
        public string report;
        public string[] lines;
        public int x, y;
        public AfterActionReport(string report)
        {
            this.report = report;
            lines = report.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            lines = lines.Where(s => s != " ").ToArray();
            var myRegex = new Regex(@"(\d+),(\d+)");
            var m = myRegex.Match(lines[0]);
            x = int.Parse(m.Groups[1].Value);
            y = int.Parse(m.Groups[2].Value);
        }
    }
}
