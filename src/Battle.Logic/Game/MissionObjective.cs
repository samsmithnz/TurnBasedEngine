namespace Battle.Logic.Game
{
    public class MissionObjective
    {
        public MissionObjective(MissionObjectiveType type, bool objectiveComplete)
        {
            Type = type;
            ObjectiveIsComplete = objectiveComplete;
        }

        public MissionObjectiveType Type { get; set; }
        public bool ObjectiveIsComplete { get; set; }
    }

    public enum MissionObjectiveType
    {
        EliminateAllOpponents = 0,
        //GetItem = 1,
        //TriggerSwitch = 2,
        //GetToLocation = 3,
        ExtractTroops = 4
    }
}
