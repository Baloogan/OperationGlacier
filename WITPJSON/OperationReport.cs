using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WITPJSON
{
    class OperationReport
    {
        public string report;
        public int x, y;
        public OperationReport(string report)
        {
            this.report = report;
            var myRegex = new Regex(@"(\d+),(\d+)");
            var m = myRegex.Match(BaseToHex.ReplaceMatches(report));
            if (m.Success)
            {
                x = int.Parse(m.Groups[1].Value);
                y = int.Parse(m.Groups[2].Value);
            }
            else
            {
                x = -1;
                y = -1;
            }

        }
    }
}
