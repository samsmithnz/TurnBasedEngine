using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public class CharacterAI
    {
        private List<KeyValuePair<Vector3, AIAction>> aiValues;

        public CharacterAI()
        {
            aiValues = new List<KeyValuePair<Vector3, AIAction>>();
        }

        public AIAction CalculateAIAction(string[,,] map, List<Team> teams, Character character, RandomNumberQueue diceRolls)
        {
            List<string> log = new List<string>
            {
                character.Name + " is processing AI, with intelligence " + character.Intelligence
            };

            //1. Start with a list of all possible moves, a location, and number of movement points to that location
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, character.Location, character.MobilityRange, character.ActionPointsCurrent);

            //2. Assign values to each possible tile
            aiValues = AssignPointsToEachTile(map, teams, character, movementPossibileTiles);

            //3. Assign an action based on the intelligence check
            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomInt = diceRolls.Dequeue();
            AIAction aiActionResult = null;
            if ((100 - character.Intelligence) <= randomInt)
            {
                log.Add("Successful intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll successful
                aiActionResult = aiValues[0].Value;
            }
            else
            {
                log.Add("Failed intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll failed, make a sub-optimal move (the next highest ranked score)
                int highestScore = aiValues[0].Value.Score;
                for (int i = 0; i < aiValues.Count; i++)
                {
                    //Find the next highest ranking score
                    if (aiValues[i].Value.Score < highestScore)
                    {
                        aiActionResult = aiValues[i].Value;
                        break;
                    }
                }
                //Double check - if it's still not assigned, assign the last value
                if (aiActionResult == null)
                {
                    aiActionResult = aiValues[aiValues.Count - 1].Value;
                }
            }

            aiActionResult.Log = log;
            return aiActionResult;
        }

        private List<KeyValuePair<Vector3, AIAction>> AssignPointsToEachTile(string[,,] map, List<Team> teams, Character character, List<KeyValuePair<Vector3, int>> movementPossibileTiles)
        {
            List<KeyValuePair<Vector3, AIAction>> results = new List<KeyValuePair<Vector3, AIAction>>();

            //Preprocess a list of opponent characters and a list of character locations
            List<Character> opponentCharacters = new List<Character>();
            List<Vector3> opponentLocations = new List<Vector3>();
            Team opponentTeam = new Team();
            foreach (Team team in teams)
            {
                //Exclude the characters team (Assume all other teams are the bad guys)
                if (!team.Characters.Contains(character))
                {
                    opponentTeam = team;
                    foreach (Character item in team.Characters)
                    {
                        opponentCharacters.Add(item);
                        opponentLocations.Add(item.Location);
                    }
                }
            }

            //Loop through each movement possibility, assigning scores and then choosing the best one
            for (int i = 0; i < movementPossibileTiles.Count; i++)
            {
                List<AIAction> possibleOptions = new List<AIAction>();
                KeyValuePair<Vector3, int> item = movementPossibileTiles[i];
                Vector3 location = item.Key;
                string targetName = "";
                Vector3 targetLocation = Vector3.Zero;
                int baseScore = 0;
                int moveScore = 0;
                int moveLongScore = 0;
                int moveThenShootScore = 0;
                //int moveWithAllActionPointsbaseScore = 0;
                //int moveThenHunkerScore = 0;
                //int shootFromCurrentLocationScore = 0;

                //if (location == new Vector3(5, 0, 4))
                //{
                //    int j = 123;
                //}

                //Create a temp FOV map to simulate the board for this situation
                string[,,] fovMap = (string[,,])map.Clone();
                fovMap[(int)character.Location.X, (int)character.Location.Y, (int)character.Location.Z] = "";
                fovMap[(int)location.X, (int)location.Y, (int)location.Z] = "P";

                //Cover calculation
                CoverState coverStateResult = CharacterCover.CalculateCover(fovMap, location, opponentLocations);
                if (coverStateResult.IsFlanked)
                {
                    baseScore -= 1;
                }
                else if (coverStateResult.InFullCover)
                {
                    baseScore += 2;
                }
                else if (coverStateResult.InHalfCover)
                {
                    baseScore += 1;
                }

                //Upgrade positions that would flank opponents
                List<Character> fovCharacters = FieldOfView.GetCharactersInArea(fovMap, opponentCharacters, location, character.ShootingRange);
                foreach (Character fovCharacter in fovCharacters)
                {
                    CoverState coverStateResultOpponent = CharacterCover.CalculateCover(fovMap, fovCharacter.Location, new List<Vector3>() { location });
                    if (coverStateResultOpponent.IsFlanked)
                    {
                        //Position flanks enemy
                        baseScore += 2;
                    }

                    //Do a reverse FOV from the perspective of the character
                    List<Vector3> fovCharacterVisibleTiles = FieldOfView.GetFieldOfView(fovMap, fovCharacter.Location, fovCharacter.FOVRange);
                    //reduce the score for those tiles - they are more dangerous to move into
                    if (fovCharacterVisibleTiles.Contains(location))
                    {
                        baseScore -= 2;
                    }
                }

                //If there are movement points left, consider shooting options.
                if (item.Value == 1)
                {
                    //If there are no opponents in view, just return a walk
                    if (opponentCharacters.Count == 0)
                    {
                        moveScore = baseScore;
                        moveScore += 3;
                        //Normalize and record the score + target
                        if (moveScore < 0)
                        {
                            moveScore = 0;
                        }
                        possibleOptions.Add(new AIAction(ActionTypeEnum.Move)
                        {
                            Score = moveScore,
                            StartLocation = character.Location,
                            EndLocation = location,
                            TargetName = targetName,
                            TargetLocation = targetLocation
                        });
                    }
                    else
                    {
                        moveThenShootScore = baseScore;

                        //if (location == new Vector3(15,0,5))
                        //{
                        //    int hj = 234;
                        //}

                        //Calculate chance to hit
                        //List<Vector3> fov = FieldOfView.GetFieldOfView(map, location, character.ShootingRange);
                        List<Character> characters = FieldOfView.GetCharactersInView(fovMap, location, character.ShootingRange, new List<Team>() { opponentTeam });
                        if (characters.Count == 0)
                        {
                            //No characters in view, record a score of 0 - this move achieves nothing
                            moveThenShootScore = 0;
                            possibleOptions.Add(new AIAction(ActionTypeEnum.Move)
                            {
                                Score = moveThenShootScore,
                                StartLocation = character.Location,
                                EndLocation = location,
                                TargetName = targetName,
                                TargetLocation = targetLocation
                            });
                        }
                        else
                        {
                            foreach (Character opponentCharacter in characters)
                            {
                                int chanceToHit = EncounterCore.GetChanceToHit(character, character.WeaponEquipped, opponentCharacter);
                                targetName = opponentCharacter.Name;
                                targetLocation = opponentCharacter.Location;
                                if (chanceToHit >= 95)
                                {
                                    moveThenShootScore += 5;
                                }
                                else if (chanceToHit >= 90)
                                {
                                    moveThenShootScore += 4;
                                }
                                else if (chanceToHit >= 80)
                                {
                                    moveThenShootScore += 3;
                                }
                                else if (chanceToHit >= 65)
                                {
                                    moveThenShootScore += 2;
                                }
                                else if (chanceToHit >= 50)
                                {
                                    moveThenShootScore += 1;
                                }
                                else //(chanceToHit < 50)
                                {
                                    moveThenShootScore += 0;
                                }

                                //Normalize and record the score + target
                                if (moveThenShootScore < 0)
                                {
                                    moveThenShootScore = 0;
                                }
                                possibleOptions.Add(new AIAction(ActionTypeEnum.Attack)
                                {
                                    Score = moveThenShootScore,
                                    StartLocation = character.Location,
                                    EndLocation = location,
                                    TargetName = targetName,
                                    TargetLocation = targetLocation
                                });
                            }
                        }
                    }
                }
                else if (item.Value == 2)
                {
                    //double move - no bonuses
                    moveLongScore = baseScore;
                    moveLongScore += 2;

                    //Normalize and record the score + target
                    if (moveLongScore < 0)
                    {
                        moveLongScore = 0;
                    }
                    possibleOptions.Add(new AIAction(ActionTypeEnum.Move)
                    {
                        Score = moveLongScore,
                        StartLocation = character.Location,
                        EndLocation = location
                    });
                }

                //Order the final options
                possibleOptions = possibleOptions.OrderByDescending(x => x.Score).ToList();
                //Get the best first option
                KeyValuePair<Vector3, AIAction> newItem = new KeyValuePair<Vector3, AIAction>(location, possibleOptions[0]);
                results.Add(newItem);
            }

            // Sort the values, highest first
            results = results.OrderByDescending(x => x.Value.Score).ToList();

            return results;
        }


        public string CreateAIMap(string[,,] map)
        {
            if (aiValues == null)
            {
                return null;
            }
            else
            {
                return MapCore.GetMapStringWithAIValuesSecond(map, aiValues);
            }
        }

    }
}
