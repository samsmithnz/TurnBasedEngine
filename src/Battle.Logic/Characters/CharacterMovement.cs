using Battle.Logic.Encounters;
using Battle.Logic.Map;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public static class CharacterMovement
    {
        public static List<ActionResult> MoveCharacter(Character characterMoving, string[,,] map, PathFindingResult pathFindingResult, Queue<int> diceRolls, List<KeyValuePair<Character, List<Vector3>>> overWatchedCharacters = null)
        {
            List<EncounterResult> encounters = new List<EncounterResult>();
            List<ActionResult> results = new List<ActionResult>();

            if (pathFindingResult.Tiles[pathFindingResult.Tiles.Count - 1].TraversalCost > characterMoving.MobilityRange)
            {
                characterMoving.ActionPointsCurrent -= 2;
            }
            else
            {
                characterMoving.ActionPointsCurrent -= 1;
            }
            int totalActionPoints = TotalOverwatchActionPoints(overWatchedCharacters);
            int i = 0;
            foreach (Vector3 step in pathFindingResult.Path)
            {
                List<string> log = new List<string>
                {
                    characterMoving.Name + " is moving from " + characterMoving.Location.ToString() + " to " + step.ToString()
                };
                ActionResult result = new ActionResult();
                if (i == 0)
                {
                    result.StartLocation = characterMoving.Location;
                }
                else
                {
                    result.StartLocation = pathFindingResult.Path[i - 1];
                }
                result.EndLocation = step;

                //Move to the next step
                characterMoving.SetLocation(step, map);
                if (overWatchedCharacters != null && totalActionPoints > 0)
                {
                    (List<EncounterResult>, bool) overWatchResult = Overwatch(characterMoving, map, diceRolls, overWatchedCharacters);
                    encounters.AddRange(overWatchResult.Item1);
                    if (encounters.Count > 0)
                    {
                        result.EncounterResults = new List<EncounterResult>();
                        foreach (EncounterResult item in encounters)
                        {
                            result.EncounterResults.Add(item);
                            log.AddRange(item.Log);
                        }
                    }
                    //is the character still alive?
                    if (overWatchResult.Item2 == false)
                    {
                        if (log.Count > 0)
                        {
                            result.Log = log;
                        }
                        results.Add(result);
                        break;
                    }
                    else
                    {
                        totalActionPoints = TotalOverwatchActionPoints(overWatchedCharacters);
                    }
                }
                if (log.Count > 0)
                {
                    result.Log = log;
                }
                results.Add(result);
                i++;
            }

            return results;
        }

        private static (List<EncounterResult>, bool) Overwatch(Character characterMoving, string[,,] map, Queue<int> diceRolls, List<KeyValuePair<Character, List<Vector3>>> overWatchedCharacters = null)
        {
            List<EncounterResult> results = new List<EncounterResult>();
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
                        results.Add(result);
                        //The character uses their overwatch charge
                        characterFOV.Key.InOverwatch = false;
                        if (characterMoving.HitpointsCurrent <= 0)
                        {
                            //Return the encounter result and if the character is still alive
                            return (results, false);
                        }
                    }
                }
            }
            //Return the encounter result and if the character is still alive
            return (results, true);
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
