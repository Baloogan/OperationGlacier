using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using System.Text.RegularExpressions;

namespace WITPJSON
{
    class ScenData
    {
        private static Dictionary<int, Dictionary<string, string>> scendata_grp = null;
        private static void read_scendata(string filename)
        {
            using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new TextFieldParser(filename))
            {

                parser.HasFieldsEnclosedInQuotes = false;
                parser.SetDelimiters(new string[] { "," });
            }
        }
    }
}
