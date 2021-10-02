using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Encounters
{
    public class AIAction
    {
        public int Score { get; set; }
        public ActionType ActionType { get; set; }
        public string TargetName { get; set; }
        public Vector3 TargetLocation { get; set; }
        public Vector3 StartLocation { get; set; }
        public Vector3 EndLocation { get; set; }
        public List<string> Log { get; set; }
        public string LogString
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append(Environment.NewLine);
                foreach (string item in Log)
                {
                    result.Append(item);
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
        }

    }

    public enum ActionType
    {
        Unknown = 0,
        Movement = 1,
        Attack = 2
    }
}
