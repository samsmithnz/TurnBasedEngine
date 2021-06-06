using Battle.Logic.Encounters;
using Battle.Logic.Map;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public static class CharacterMovement
    {
        public static List<EncounterResult> MoveCharacter(Character characterMoving, string[,,] map, PathFindingResult pathFindingResult, Queue<int> diceRolls, List<KeyValuePair<Character, List<Vector3>>> overWatchedCharacters = null)
        {
            if (pathFindingResult.Tiles[pathFindingResult.Tiles.Count - 1].TraversalCost > characterMoving.MobilityRange)
            {
                characterMoving.ActionPointsCurrent -= 2;
            }
            else
            {
                characterMoving.ActionPointsCurrent -= 1;
            }
            List<EncounterResult> encounters = new List<EncounterResult>();
            int totalActionPoints = TotalOverwatchActionPoints(overWatchedCharacters);
            foreach (Vector3 step in pathFindingResult.Path)
            {
                characterMoving.Location = step;
                if (overWatchedCharacters != null && totalActionPoints > 0)
                {
                    (EncounterResult, bool) overWatchResult = Overwatch(characterMoving, map, diceRolls, overWatchedCharacters);
                    encounters.Add(overWatchResult.Item1);
                    //is the character still alive?
                    if (overWatchResult.Item2 == false)
                    {
                        break;
                    }
                    else
                    {
                        totalActionPoints = TotalOverwatchActionPoints(overWatchedCharacters);
                    }
                }
            }

            return encounters;
        }

        private static (EncounterResult, bool) Overwatch(Character characterMoving, string[,,] map, Queue<int> diceRolls, List<KeyValuePair<Character, List<Vector3>>> overWatchedCharacters = null)
        {
            EncounterResult result = null;
            overWatchedCharacters = overWatchedCharacters.OrderByDescending(o => o.Key.Speed).ToList();
            foreach (KeyValuePair<Character, List<Vector3>> characterFOV in overWatchedCharacters)
            {
                foreach (Vector3 fovLocation in characterFOV.Value)
                {
                    if (characterFOV.Key.ActionPointsCurrent > 0 && fovLocation == characterMoving.Location)
                    {
                        //Act
                        result = Encounter.AttackCharacter(characterFOV.Key, characterFOV.Key.WeaponEquipped, characterMoving, map, diceRolls);
                        //The character uses their overwatch charge
                        characterFOV.Key.InOverwatch = false;
                        if (result.TargetCharacter.HitpointsCurrent <= 0)
                        {
                            //Return the encounter result and if the character is still alive
                            return (result, false);
                        }
                    }
                }
            }
            //Return the encounter result and if the character is still alive
            return (result, true);
        }

        private static int TotalOverwatchActionPoints(List<KeyValuePair<Character, List<Vector3>>> overWatchedCharacters)
        {
            int total = 0;
            if (overWatchedCharacters != null)
            {
                foreach (KeyValuePair<Character, List<Vector3>> item in overWatchedCharacters)
                {
                    total += item.Key.ActionPointsCurrent;
                }
            }
            return total;
        }
    }
}
