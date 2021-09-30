using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Encounters
{
    public class MovementAction
    {
        public Vector3 StartLocation { get; set; }
        public Vector3 EndLocation { get; set; }
        //public Vector3 EndLocation2 { get; set; }
        //public Vector3 Target { get; set; }
        public string[,,] FOVMap { get; set; }
        public string FOVMapString { get; set; }
        public List<EncounterResult> OverwatchEncounterResults { get; set; }
        public List<string> Log { get; set; }
        //public string LogString
        //{
        //    get
        //    {
        //        StringBuilder result = new StringBuilder();
        //        result.Append(Environment.NewLine);
        //        foreach (string item in Log)
        //        {
        //            result.Append(item);
        //            result.Append(Environment.NewLine);
        //        }
        //        return result.ToString();
        //    }
        //}

    }

    public enum ActionType
    {
        Unknown = 0,
        Movement = 1,
        Attack = 2
    }
}
