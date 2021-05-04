using System.Numerics;

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

        public Vector3 Location { get; set; }
        public int ActionPoints { get; set; }
        public int Range { get; set; }

        public bool InHalfCover { get; set; }
        public bool InFullCover { get; set; }
    }
}
