namespace Battle.Logic.Characters
{
    public class Character
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int ChanceToHit { get; set; }
        public int Initiative { get; set; }
        public int Modifier { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public bool LevelUpIsReady { get; set; }
    }
}
