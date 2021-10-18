using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Characters
{
    public static class CharacterMovement
    {
        public static List<MovementAction> MoveCharacter(string[,,] map,
            Character characterMoving,
            PathFindingResult pathFindingResult,
            Team characterTeam,
            Team opponentTeam,
            RandomNumberQueue diceRolls)
        {
            List<EncounterResult> encounters = new List<EncounterResult>();
            List<MovementAction> results = new List<MovementAction>();

            //If you try to move to a square that is occupied, this can fail - return null
            if (pathFindingResult.Path.Count == 0)
            {
                return null;
            }
            //Deduct action points for the movement
            if (pathFindingResult.Tiles[pathFindingResult.Tiles.Count - 1].TraversalCost > characterMoving.MobilityRange)
            {
                characterMoving.ActionPointsCurrent -= 2;
            }
            else
            {
                characterMoving.ActionPointsCurrent -= 1;
            }
            List<Character> opponentCharacters = new List<Character>();
            if (opponentTeam != null)
            {
                opponentCharacters = opponentTeam.Characters; 
            }
            int totalActionPoints = TotalOverwatchActionPoints(opponentCharacters);
            int i = 0;
            foreach (Vector3 step in pathFindingResult.Path)
            {
                List<string> log = new List<string>
                {
                    characterMoving.Name + " is moving from " + characterMoving.Location.ToString() + " to " + step.ToString()
                };
                MovementAction result = new MovementAction();
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
                characterMoving.SetLocationAndRange(map, step, characterMoving.FOVRange, opponentCharacters);
                if (characterTeam != null)
                {
                    FieldOfView.UpdateTeamFOV(map, characterTeam);
                }
                //clone the array, so we don't create a link and capture the point in time
                if (characterTeam != null)
                {
                    result.FOVMap = (string[,,])characterTeam.FOVMap.Clone();
                }
                else
                {
                    result.FOVMap = (string[,,])characterMoving.FOVMap.Clone();
                }
                result.FOVMapString = MapCore.GetMapStringWithMapMask(map, result.FOVMap);
                if (opponentCharacters != null && totalActionPoints > 0)
                {
                    (List<EncounterResult>, bool) overWatchResult = Overwatch(map, characterMoving, diceRolls, opponentCharacters);
                    encounters.AddRange(overWatchResult.Item1);
                    if (encounters.Count > 0)
                    {
                        result.OverwatchEncounterResults = new List<EncounterResult>();
                        foreach (EncounterResult item in encounters)
                        {
                            result.OverwatchEncounterResults.Add(item);
                            log.AddRange(item.Log);
                        }
                    }
                    //is the character still alive?
                    if (!overWatchResult.Item2)
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
                        totalActionPoints = TotalOverwatchActionPoints(opponentCharacters);
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

        public static string LogString(List<MovementAction> movementResults)
        {
            StringBuilder result = new StringBuilder();
            result.Append(Environment.NewLine);
            foreach (MovementAction item in movementResults)
            {
                foreach (string log in item.Log)
                {
                    result.Append(log);
                    result.Append(Environment.NewLine);
                }
            }
            return result.ToString();
        }

        private static (List<EncounterResult>, bool) Overwatch(string[,,] map, Character characterMoving, RandomNumberQueue diceRolls, List<Character> overWatchedCharacters = null)
        {
            List<EncounterResult> results = new List<EncounterResult>();
            EncounterResult result = null;
            overWatchedCharacters = overWatchedCharacters.OrderByDescending(o => o.Speed).ToList();
            foreach (Character character in overWatchedCharacters)
            {
                List<Vector3> fov = FieldOfView.GetFieldOfView(map, character.Location, character.FOVRange);
                foreach (Vector3 fovLocation in fov)
                {
                    if (character.ActionPointsCurrent > 0 && fovLocation == characterMoving.Location)
                    {
                        //Act
                        result = Encounter.AttackCharacter(map, character, character.WeaponEquipped, characterMoving, diceRolls);
                        results.Add(result);
                        //The character uses their overwatch charge
                        character.InOverwatch = false;
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

        private static int TotalOverwatchActionPoints(List<Character> overWatchedCharacters)
        {
            int total = 0;
            if (overWatchedCharacters != null)
            {
                foreach (Character item in overWatchedCharacters)
                {
                    total += item.ActionPointsCurrent;
                }
            }
            return total;
        }
    }
}
