﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WITPJSON
{
    class Turn //more a TurnCoordinator, this guy knows where stuff gotta go.
    {
        public DateTime date;
        
        public List<CombatEvent> CombatEvents;
        public List<AfterActionReport> AfterActionReports;
        public List<SigInt> SigInts;
        public List<OperationReport> OperationReports;
        
        public string date_string { get { return string.Format("{0}{1}{2}", date.Year, date.Month, date.Day); } }
        
        public string output_directory { get { return Path.Combine(Program.output_directory, date_string); } }
        public string output_filename { get { return Path.Combine(this.output_directory, "Turn.json"); } }
        
        public string CombatEvents_filename { get { return Path.Combine(Program.archive_directory, string.Format("Combat_Events_{0}.txt", date_string)); } }
        public string AfterActionReports_filename { get { return Path.Combine(Program.archive_directory, string.Format("combatreport_{0}.txt", date_string)); } }
        public string SigInts_filename { get { return Path.Combine(Program.archive_directory, string.Format("asigint_{0}.txt", date_string)); } }
        public string OperationReports_filename { get { return Path.Combine(Program.archive_directory, string.Format("asigint_{0}.txt", date_string)); } }
        
        public Turn(DateTime date)
        {
            this.date = date;
            ParseCombatEvents();
            ParseAfterActionReports();
            ParseSigInts();
            ParseOperationReports();
        }

        private void ParseOperationReports()
        {
            
        }
        private void ParseSigInts()
        {
            SigInts = new List<SigInt>();

        }
        private void ParseAfterActionReports()
        {
            AfterActionReports = new List<AfterActionReport>();

        }
        private void ParseCombatEvents()
        {
            CombatEvents = new List<CombatEvent>();
            
        }
        public void Render()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            File.WriteAllText(output_filename,json);
        }
    }
}
