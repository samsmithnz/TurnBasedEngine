using Battle.Logic.Characters;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Movement
{
    public class CharacterMovement
    {
        public static Character MoveCharacter(Character character, List<Vector3> path, List<KeyValuePair<Character, List<Vector3>>> overWatchedLocations = null)
        {
            foreach (Vector3 step in path)
            {
                character.Location = step;
                if (overWatchedLocations != null)
                {
                    foreach (KeyValuePair<Character, List<Vector3>> characterFOV in overWatchedLocations)
                    {
                        foreach (Vector3 fov in characterFOV.Value)
                        {
                            if (fov == character.Location)
                            {
                                character.HP = 0;
                                break;
                            }
                        }
                    }
                }
            }

            return character;
        }
    }
}
