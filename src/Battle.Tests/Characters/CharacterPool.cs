using Battle.Logic.Characters;

namespace Battle.Tests.Characters
{
    public static class CharacterPool
    {
        public static Character CreateFred()
        {
            Character fred = new()
            {
                Name = "Fred",
                HP = 12,
                ChanceToHit = 70,
                Initiative = 10,
                Modifier = 0,
                Experience = 0,
                Level = 1,
                LevelUpIsReady = false,
                Location = new(0,0,0),
                ActionPoints = 2,
                Range = 10,
                InHalfCover=false,
                InFullCover=false
            };
            return fred;
        }

        public static Character CreateJeff()
        {
            Character fred = new()
            {
                Name = "Jeff",
                HP = 12,
                ChanceToHit = 70,
                Initiative = 10,
                Modifier = 0,
                Experience = 0,
                Level = 1,
                LevelUpIsReady = false,
                Location = new(5, 5, 5),
                ActionPoints = 2,
                Range = 10,
                InHalfCover = false,
                InFullCover = false
            };
            return fred;
        }

        public static Character CreateHarry()
        {
            Character harry = new()
            {
                Name = "Harry",
                HP = 12,
                ChanceToHit = 70,
                Initiative = 5,
                Modifier = 0,
                Experience = 0,
                Level = 1,
                LevelUpIsReady = false,
                Location = new(10,10,10),
                ActionPoints = 2,
                Range = 10,
                InHalfCover = true,
                InFullCover = false
            };
            return harry;
        }
    }
}
