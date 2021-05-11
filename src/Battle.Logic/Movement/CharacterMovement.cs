using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Movement
{
    public class CharacterMovement
    {
        public static Character MoveCharacter(Character characterMoving, string[,] map, List<Vector3> path, List<int> diceRolls, List<KeyValuePair<Character, List<Vector3>>> overWatchedLocations = null)
        {
            foreach (Vector3 step in path)
            {
                characterMoving.Location = step;
                if (overWatchedLocations != null)
                {
                    bool targetIsStillAlive = Overwatch(characterMoving, map, diceRolls, overWatchedLocations);
                    if (targetIsStillAlive == false)
                    {
                        break;
                    }
                }
            }

            return characterMoving;
        }

        private static bool Overwatch(Character characterMoving, string[,] map, List<int> diceRolls, List<KeyValuePair<Character, List<Vector3>>> overWatchedLocations = null)
        {
            foreach (KeyValuePair<Character, List<Vector3>> characterFOV in overWatchedLocations)
            {
                foreach (Vector3 fovLocation in characterFOV.Value)
                {
                    if (characterFOV.Key.ActionPoints > 0 & fovLocation == characterMoving.Location)
                    {
                        //Act
                        EncounterResult result = Encounter.AttackCharacter(characterFOV.Key, characterFOV.Key.WeaponEquipped, characterMoving, map, diceRolls, null);

                        if (result.TargetCharacter.HP <= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
