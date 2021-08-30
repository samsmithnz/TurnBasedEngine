using Battle.Logic.GameController;
using Newtonsoft.Json;

namespace Battle.Logic.SaveGames
{
    public class SaveGame
    {
        public static Mission LoadGame(string json)
        {
            Mission mission = JsonConvert.DeserializeObject<Mission>(json);
            return mission;
        }
    }
}
