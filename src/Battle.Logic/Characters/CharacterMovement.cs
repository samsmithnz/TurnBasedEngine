﻿using Battle.Logic.Encounters;
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
            List<string> log = new List<string>();
            List<EncounterResult> encounters = new List<EncounterResult>();
            int totalActionPoints = TotalOverwatchActionPoints(overWatchedCharacters);
            foreach (Vector3 step in pathFindingResult.Path)
            {
                log.Add(characterMoving.Name + " is moving from " + characterMoving.Location.ToString() + " to " + step.ToString());
                characterMoving.Location = step;
                if (overWatchedCharacters != null && totalActionPoints > 0)
                {
                    (List<EncounterResult>, bool) overWatchResult = Overwatch(characterMoving, map, diceRolls, log, overWatchedCharacters);
                    encounters.AddRange(overWatchResult.Item1);
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

            if (log.Count > 0)
            {
                encounters.Add(new EncounterResult()
                {
                    Log = log
                });
            }

            return encounters;
        }

        private static (List<EncounterResult>, bool) Overwatch(Character characterMoving, string[,,] map, Queue<int> diceRolls, List<string> log, List<KeyValuePair<Character, List<Vector3>>> overWatchedCharacters = null)
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
                        //Insert any existing logs to the beginning of the log
                        if (log.Count > 0)
                        {
                            result.Log.InsertRange(0, log);
                            log = new List<string>();
                        }
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
