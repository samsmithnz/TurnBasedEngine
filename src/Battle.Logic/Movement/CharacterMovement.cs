using Battle.Logic.Characters;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Movement
{
    public class CharacterMovement
    {
        public static Character MoveCharacter(Character character, List<Vector3> path)
        {
            foreach (Vector3 step in path)
            {
                character.Location = step;
            }

            return character;
        }
    }
}
