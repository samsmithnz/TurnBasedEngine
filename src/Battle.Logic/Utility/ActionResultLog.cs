using Battle.Logic.Encounters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battle.Logic.Utility
{
    public static class ActionResultLog
    {
        public static string LogString(List<ActionResult> movementResults)
        {
            StringBuilder result = new StringBuilder();
            result.Append(Environment.NewLine);
            foreach (ActionResult item in movementResults)
            {
                foreach (string log in item.Log)
                {
                    result.Append(log);
                    result.Append(Environment.NewLine);
                }

            }
            return result.ToString();
        }
    }
}
