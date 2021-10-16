using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Encounters
{
    public class AIAction
    {
        public AIAction(ActionTypeEnum actionType)
        {
            ActionType = actionType;
            IntelligenceCheckSuccessful = false;
        }
        public int Score { get; set; }
        public bool IntelligenceCheckSuccessful { get; set; }
        public ActionTypeEnum ActionType { get; set; }
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

    public enum ActionTypeEnum
    {
        Unknown = 0,
        DoubleMove = 1,
        MoveThenAttack = 2,
        StayInPositionThenAttack = 3
    }
}
