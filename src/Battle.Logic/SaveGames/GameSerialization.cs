using Battle.Logic.GameController;
using Newtonsoft.Json;
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
        }

        public static bool CreateSaveGameFile(string path, string json, int number = 0)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            int fileNumber = new DirectoryInfo(path).GetFiles().Length;
            if (number > 0)
            {
                fileNumber = number;
            }
            string fileName = $"Save{fileNumber:000}.json";
            while (File.Exists(path + fileName))
            {
                fileNumber++;
                fileName = $"Save{fileNumber:000}.json";
                if (fileNumber > 1000)
                {
                    //we don't want an infinite loop
                    break;
                }
            }
            File.WriteAllText(path + fileName, json);
            return true;
        }
    }
}
