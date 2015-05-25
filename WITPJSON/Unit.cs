using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using System.Text.RegularExpressions;
namespace WITPJSON
{
    public partial class Unit
    {
        public enum Type
        {
            //Aircraft,
            //AirframeProd,
            AirGroup,
            //AirLoss,
            Base,
            //Device,
            //Engine,
            LCU,
            //Leader,
            //Pilot,
            //ShipClass,
            //ShipUpgrade,
            Ship,
            //SunkShip,
            TaskForce,
            //VP,

            SigInt,
            CombatEvent,
            AfterAction,
            OperationalReport
        }
        public string hash { get; set; }
        public string timeline_id { get { return side_str + "_" + type_str + "_" + id; } }
        public Turn.Side side;
        public string side_str { get { return side.ToString(); } }
        public Dictionary<string, string> row;
        public Dictionary<string, string> scendata = new Dictionary<string,string>(); //only really relevant scendata plz
        public string name;
        public int id;
        public int x, y;
        public DateTime date;
        public string date_string { get { return string.Format("{0:00}{1:00}{2:00}", date.Year - 1900, date.Month, date.Day); } }
        public Type type;
        public string type_str { get { return type.ToString(); } }
        public string location;
        public int owner = -1;
        public string report;
        public string ship_class;
        public List<Unit> subunits = new List<Unit>();
        public int bitmap;
        public string bitmap_name;
    }
}
