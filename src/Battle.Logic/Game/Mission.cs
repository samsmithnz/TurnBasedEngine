using Battle.Logic.Characters;
using Battle.Logic.Utility;
using System.Collections.Generic;

namespace Battle.Logic.Game
{
    public class Mission
    {
        public Mission()
        {
            Teams = new List<Team>();
            Objective = Mission.MissionType.EliminateAllOpponents;
            TurnNumber = 1;
            CurrentTeamIndex = 0;
            RandomNumbers = new RandomNumberQueue(RandomNumber.GenerateRandomNumberList(0, 100, 0, 1000));
            RandomNumbers.Queue[0] = 8; //First shot always misses in this scenario.
        }

        public int TurnNumber { get; set; }
        public int CurrentTeamIndex { get; set; }
        public List<Team> Teams { get; set; }
        public string[,,] Map { get; set; }
        public MissionType Objective { get; set; }
        public RandomNumberQueue RandomNumbers { get; set; }

        //Start mission

        //End mission, add records
        public void EndMission()
        {
            foreach (Team team in Teams)
            {
                foreach (Character character in team.Characters)
                {
                    //If the character is still alive
                    if (character.HitpointsCurrent > 0)
                    {
                        //incrememt the missions completed metric
                        character.MissionsCompleted++;
                        //Add some experience for surviving
                        character.XP += 100;
                    }
                }
            }
        }

        public enum MissionType
        {
            EliminateAllOpponents = 0
        }

        public bool CheckIfCurrentTeamIsDoneTurn()
        {
            int totalActionPoints = 0;
            foreach (Character character in Teams[CurrentTeamIndex].Characters)
            {
                totalActionPoints += character.ActionPointsCurrent;
            }
            if (totalActionPoints == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MoveToNextTurn()
        {
            //Move to the next team
            if (CurrentTeamIndex == Teams.Count - 1)
            {
                CurrentTeamIndex = 0;
                //Move to the next turn
                TurnNumber++;
            }
            else
            {
                CurrentTeamIndex++;
            }

            //Reset the characters action points for the new turn
            foreach (Character character in Teams[CurrentTeamIndex].Characters)
            {
                character.ActionPointsCurrent = character.ActionPointsMax;
            }
            //Refresh targets
            UpdateTargetsForAllTeams();
        }

        public bool CheckIfMissionIsCompleted()
        {
            bool result = false;
            foreach (Team team in Teams)
            {
                int totalHPs = 0;
                foreach (Character character in team.Characters)
                {
                    if (character.HitpointsCurrent > 0)
                    {
                        totalHPs += character.HitpointsCurrent;
                    }
                }
                if (totalHPs <= 0)
                {
                    return true;
                }
            }

            return result;
        }

        public void UpdateTargetsForAllTeams()
        {
            if (Teams.Count == 2)
            {
                Teams[0].UpdateTargets(Map, Teams[1].Characters);
                Teams[1].UpdateTargets(Map, Teams[0].Characters);
            }
        }
    }
}
