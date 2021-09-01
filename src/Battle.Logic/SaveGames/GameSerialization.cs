using Battle.Logic.GameController;
using Newtonsoft.Json;

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
    }
}
