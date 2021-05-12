using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Logic.Movement
{
    public class CharacterMovement
    {
        public static Character MoveCharacter(Character characterMoving, string[,] map, List<Vector3> path, Queue<int> diceRolls, List<KeyValuePair<Character, List<Vector3>>> overWatchedCharacters = null)
        {
            foreach (Vector3 step in path)
            {
                characterMoving.Location = step;
                if (overWatchedCharacters != null)
                {
                    bool targetIsStillAlive = Overwatch(characterMoving, map, diceRolls, overWatchedCharacters);
                    if (targetIsStillAlive == false)
                    {
                        break;
                    }
                }
            }

            return characterMoving;
        }

        private static bool Overwatch(Character characterMoving, string[,] map, Queue<int> diceRolls, List<KeyValuePair<Character, List<Vector3>>> overWatchedCharacters = null)
        {
            overWatchedCharacters = overWatchedCharacters.OrderByDescending(o => o.Key.Speed).ToList();
            foreach (KeyValuePair<Character, List<Vector3>> characterFOV in overWatchedCharacters)
            {
                foreach (Vector3 fovLocation in characterFOV.Value)
                {
                    if (characterFOV.Key.ActionPoints > 0 & fovLocation == characterMoving.Location)
                    {
                        //Act
                        EncounterResult result = Encounter.AttackCharacter(characterFOV.Key, characterFOV.Key.WeaponEquipped, characterMoving, map, diceRolls);
                        //The character uses their overwatch charge
                        characterFOV.Key.InOverwatch = false;
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
