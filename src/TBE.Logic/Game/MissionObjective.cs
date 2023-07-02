using System.Numerics;

namespace TBE.Logic.Game
{
    public class MissionObjective
    {
        public MissionObjective(MissionObjectiveType type, bool objectiveComplete)
        {
            Type = type;
            ObjectiveIsComplete = objectiveComplete;
            Location = Vector3.Zero;
        }

        public MissionObjective(MissionObjectiveType type, bool objectiveComplete, Vector3 location)
        {
            Type = type;
            ObjectiveIsComplete = objectiveComplete;
            Location = location;
        }

        public MissionObjectiveType Type { get; set; }
        public bool ObjectiveIsComplete { get; set; }
        public Vector3 Location { get; set; } //Only used for item, toggle switch, and get to location
    }

    public enum MissionObjectiveType
    {
        EliminateAllOpponents = 0,
        //GetItem = 1,
        ToggleSwitch = 2,
        //GetToLocation = 3,
        ExtractTroops = 4
    }
}
