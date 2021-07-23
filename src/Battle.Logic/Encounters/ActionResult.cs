using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Encounters
{
    public class ActionResult
    {
        public Vector3 StartLocation { get; set; }
        public Vector3 EndLocation { get; set; }
        public List<EncounterResult> EncounterResults { get; set; }
        public List<string> Log { get; set; }
    }
}
