using Battle.Logic.GameController;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Battle.Logic.SaveGames
{
    public static class GameSerialization
    {
        public static Mission LoadGame(string json)
        {
            Mission mission = JsonConvert.DeserializeObject<Mission>(json);
            return mission;
        }

        public static string SaveGame(Mission mission)
        {
            string json = JsonConvert.SerializeObject(mission);
            return json;
            //string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //string path = systemPath + @"\my games\TBS\Saves\";
            //if (Directory.Exists(path) == false)
            //{
            //    Directory.CreateDirectory(path);
            //}
            //int number = new DirectoryInfo(path).GetFiles().Length;
            //string fileName = $"Save{number:000}.json";
            //while (File.Exists(path + fileName) == true)
            //{
            //    number++;
            //    fileName = $"Save{number:000}.json";
            //    if (number > 1000)
            //    {
            //        //we don't want an infinite loop
            //        break;
            //    }
            //}
            //File.WriteAllText(path + fileName, json);
            //return true;
        }
    }
}
