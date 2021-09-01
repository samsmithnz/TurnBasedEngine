using Battle.Logic.Characters;
using Battle.Logic.Utility;
using System.Collections.Generic;

namespace Battle.Logic.GameController
{
    public class Mission
    {
        public Mission()
        {
            Teams = new List<Team>();
            Objective = Mission.MissionType.EliminateAllOpponents;
            TurnNumber = 1;
            RandomNumbers = new Queue<int>(RandomNumber.GenerateRandomNumberList(0, 100, 0, 1000));
        }

        public int TurnNumber { get; set; }
        public List<Team> Teams { get; set; }
        public string[,,] Map { get; set; }
        public MissionType Objective { get; set; }
        public Queue<int> RandomNumbers { get; set; }

        //Start mission

        //End mission, add records
        public void EndMission()
        {
            foreach (Team team in Teams)
            {
                foreach (Character character in team.Characters)
                {
                    //If the character is still alive, incrememt the missions completed metric
                    if (character.HitpointsCurrent > 0)
                    {
                        character.MissionsCompleted++;
                    }
                }
            }
        }

        public enum MissionType
        {
            EliminateAllOpponents = 0
        }
    }
}
