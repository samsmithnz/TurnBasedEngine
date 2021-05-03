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
                Modifier = 0
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
                Modifier = 0
            };
            return fred;
        }
    }
}
