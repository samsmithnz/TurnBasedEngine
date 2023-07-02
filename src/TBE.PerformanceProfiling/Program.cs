using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.SaveGames;
using System;
using System.IO;
using System.Reflection;

namespace Battle.PerformanceProfiling
{
    internal class Program
    {
        static void Main()
        {
            string _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/Saves/";

            //Arrange
            string path = _rootPath + "Save019.json";

            //Act
            string fileContents;
            using (var streamReader = new StreamReader(path))
            {
                fileContents = streamReader.ReadToEnd();
            }
            Mission mission = GameSerialization.LoadGame(fileContents);
            mission.StartMission();
            //Character fred = mission.Teams[0].GetCharacter("Fred");
            //Character enemy1 = mission.Teams[1].Characters[0];
            Character enemy2 = mission.Teams[1].Characters[1];
            Team team1 = mission.Teams[0];
            Team team2 = mission.Teams[1];
            //Assert.IsTrue(enemy1.HitpointsCurrent <= 0);

            //Move to enemy turn
            mission.MoveToNextTurn();

            AIAction aIAction = mission.CalculateAIAction(enemy2, team2, team1);
            Console.WriteLine(aIAction.LogString);
            //Console.WriteLine(enemy1.HitpointsCurrent);
        }
    }
}
