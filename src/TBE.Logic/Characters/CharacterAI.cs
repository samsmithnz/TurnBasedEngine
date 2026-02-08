using TBE.Logic.Encounters;
using TBE.Logic.Game;
using TBE.Logic.Map;
using TBE.Logic.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TBE.Logic.Characters
{
    public class CharacterAI
    {
        private List<KeyValuePair<Vector3, AIAction>> aiValues;

        public CharacterAI()
        {
            aiValues = new List<KeyValuePair<Vector3, AIAction>>();
        }

        public AIAction CalculateAIAction(string[,,] map, Character character, Team sourceTeam, Team opponentTeam, RandomNumberQueue diceRolls)
        {
            List<string> log = new List<string>
            {
                character.Name + " is processing AI, with intelligence " + character.Intelligence
            };

            //1. Start with a list of all possible moves, a location, and number of movement points to that location
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, character.Location, character.MobilityRange, character.ActionPointsCurrent);

            //2. Assign values to each possible tile
            aiValues = AssignPointsToEachTile(map, character, sourceTeam, opponentTeam, movementPossibileTiles);
            if (aiValues.Count == 0)
            {
                throw new System.Exception("AssignPointsToEachTile returned no results");
            }

            //3. Assign an action based on the intelligence check
            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomInt = diceRolls.Dequeue();
            AIAction aiActionResult = null;
            if ((GameConstants.PERCENTAGE_MAX - character.Intelligence) <= randomInt)
            {
                log.Add("Successful intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll successful
                aiActionResult = aiValues[0].Value;
                aiActionResult.IntelligenceCheckSuccessful = true;
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
                ////Double check - if it's still not assigned, assign the last value
                //if (aiActionResult == null)
                //{
                //    aiActionResult = aiValues[aiValues.Count - 1].Value;
                //}
            }

            if (aiActionResult != null)
            {
                aiActionResult.MapString = CreateAIMap(map);
                aiActionResult.Log = log;
            }
            return aiActionResult;
        }

        private List<KeyValuePair<Vector3, AIAction>> AssignPointsToEachTile(string[,,] map, Character sourceCharacter, Team sourceTeam, Team opponentTeam, List<KeyValuePair<Vector3, int>> movementPossibileTiles)
        {
            List<KeyValuePair<Vector3, AIAction>> results = new List<KeyValuePair<Vector3, AIAction>>();

            //Preprocess a list of opponent characters and a list of character locations
            List<Character> opponentCharacters = new List<Character>();
            List<Vector3> opponentLocations = new List<Vector3>();
            foreach (Character item in opponentTeam.Characters)
            {
                if (item.HitpointsCurrent > 0)
                {
                    opponentCharacters.Add(item);
                    opponentLocations.Add(item.Location);
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
                int baseScore = GameConstants.AI_BASE_SCORE;
                int moveScore = GameConstants.AI_BASE_SCORE;
                int moveLongScore = GameConstants.AI_BASE_SCORE;
                int moveThenShootScore = GameConstants.AI_BASE_SCORE;
                //int moveWithAllActionPointsbaseScore = 0;
                //int moveThenHunkerScore = 0;
                //int shootFromCurrentLocationScore = 0;

                //Create a temp FOV map to simulate the board for this situation
                string[,,] fovMap = (string[,,])map.Clone();
                fovMap[(int)sourceCharacter.Location.X, (int)sourceCharacter.Location.Y, (int)sourceCharacter.Location.Z] = GameConstants.EMPTY_TILE;
                fovMap[(int)location.X, (int)location.Y, (int)location.Z] = GameConstants.PLAYER_MAP_MARKER;

                //Cover calculation
                CoverState coverStateResult = CharacterCover.CalculateCover(fovMap, location, opponentLocations);
                if (coverStateResult.IsFlanked)
                {
                    baseScore += GameConstants.AI_FLANKED_PENALTY;
                }
                else if (coverStateResult.InFullCover)
                {
                    baseScore += GameConstants.AI_FULL_COVER_BONUS;
                }
                else if (coverStateResult.InHalfCover)
                {
                    baseScore += GameConstants.AI_HALF_COVER_BONUS;
                }

                //Upgrade positions that would flank opponents
                List<Character> fovCharacters = FieldOfView.GetCharactersInArea(fovMap, opponentCharacters, location, sourceCharacter.ShootingRange);
                foreach (Character fovCharacter in fovCharacters)
                {
                    CoverState coverStateResultOpponent = CharacterCover.CalculateCover(fovMap, fovCharacter.Location, new List<Vector3>() { location });
                    if (coverStateResultOpponent.IsFlanked)
                    {
                        //Position flanks enemy
                        baseScore += GameConstants.AI_FLANKING_ENEMY_BONUS;
                    }

                    //Do a reverse FOV from the perspective of the character
                    List<Vector3> fovCharacterVisibleTiles = FieldOfView.GetFieldOfView(fovMap, fovCharacter.Location, fovCharacter.FOVRange);
                    //reduce the score for those tiles - they are more dangerous to move into
                    if (fovCharacterVisibleTiles.Contains(location))
                    {
                        baseScore += GameConstants.AI_VISIBLE_TILE_PENALTY;
                    }
                }

                //If there are movement points left, consider shooting options.
                if (item.Value == GameConstants.SINGLE_MOVE_ACTION_POINT_COST)
                {
                    //If there are no opponents in view, just return a walk
                    if (opponentCharacters.Count == GameConstants.DEAD_HITPOINTS)
                    {
                        moveScore = baseScore;
                        moveScore += GameConstants.AI_SINGLE_MOVE_BONUS;
                        //Normalize and record the score + target
                        if (moveScore < GameConstants.AI_MINIMUM_SCORE)
                        {
                            moveScore = GameConstants.AI_MINIMUM_SCORE;
                        }
                        possibleOptions.Add(new AIAction(ActionTypeEnum.DoubleMove)
                        {
                            Score = moveScore,
                            StartLocation = sourceCharacter.Location,
                            EndLocation = location,
                            TargetName = targetName,
                            TargetLocation = targetLocation
                        });

                    }
                    else
                    {
                        moveThenShootScore = baseScore;

                        //Calculate chance to hit
                        List<Character> characters = FieldOfView.GetCharactersInView(fovMap, location, sourceCharacter.ShootingRange, opponentTeam.Characters);
                        if (characters.Count == GameConstants.DEAD_HITPOINTS)
                        {
                            //No characters in view, deduct some more points - this move achieves very little
                            moveThenShootScore += GameConstants.AI_NO_CHARACTERS_IN_VIEW_PENALTY;
                            if (moveThenShootScore < GameConstants.AI_MINIMUM_SCORE)
                            {
                                moveThenShootScore = GameConstants.AI_MINIMUM_SCORE;
                            }
                            possibleOptions.Add(new AIAction(ActionTypeEnum.MoveThenAttack)
                            {
                                Score = moveThenShootScore,
                                StartLocation = sourceCharacter.Location,
                                EndLocation = location,
                                TargetName = targetName,
                                TargetLocation = targetLocation
                            });
                        }
                        else
                        {
                            foreach (Character opponentCharacter in characters)
                            {
                                if (sourceCharacter.HitpointsCurrent > GameConstants.DEAD_HITPOINTS)
                                {
                                    int chanceToHit = EncounterCore.GetChanceToHit(sourceCharacter, sourceCharacter.WeaponEquipped, opponentCharacter);
                                    targetName = opponentCharacter.Name;
                                    targetLocation = opponentCharacter.Location;
                                    if (chanceToHit >= GameConstants.AI_HIT_CHANCE_EXCELLENT_THRESHOLD)
                                    {
                                        moveThenShootScore += GameConstants.AI_HIT_CHANCE_EXCELLENT_BONUS;
                                    }
                                    else if (chanceToHit >= GameConstants.AI_HIT_CHANCE_VERY_GOOD_THRESHOLD)
                                    {
                                        moveThenShootScore += GameConstants.AI_HIT_CHANCE_VERY_GOOD_BONUS;
                                    }
                                    else if (chanceToHit >= GameConstants.AI_HIT_CHANCE_GOOD_THRESHOLD)
                                    {
                                        moveThenShootScore += GameConstants.AI_HIT_CHANCE_GOOD_BONUS;
                                    }
                                    else if (chanceToHit >= GameConstants.AI_HIT_CHANCE_DECENT_THRESHOLD)
                                    {
                                        moveThenShootScore += GameConstants.AI_HIT_CHANCE_DECENT_BONUS;
                                    }
                                    else if (chanceToHit >= GameConstants.AI_HIT_CHANCE_FAIR_THRESHOLD)
                                    {
                                        moveThenShootScore += GameConstants.AI_HIT_CHANCE_FAIR_BONUS;
                                    }
                                    else //(chanceToHit < 50)
                                    {
                                        moveThenShootScore += GameConstants.AI_HIT_CHANCE_POOR_BONUS;
                                    }

                                    //Normalize and record the score + target
                                    if (moveThenShootScore < GameConstants.AI_MINIMUM_SCORE)
                                    {
                                        moveThenShootScore = GameConstants.AI_MINIMUM_SCORE;
                                    }
                                    possibleOptions.Add(new AIAction(ActionTypeEnum.MoveThenAttack)
                                    {
                                        Score = moveThenShootScore,
                                        StartLocation = sourceCharacter.Location,
                                        EndLocation = location,
                                        TargetName = targetName,
                                        TargetLocation = targetLocation
                                    });
                                }
                            }
                        }
                    }
                }
                else if (item.Value == GameConstants.DOUBLE_MOVE_ACTION_POINT_COST)
                {
                    //double move - no bonuses
                    moveLongScore = baseScore;
                    moveLongScore += GameConstants.AI_DOUBLE_MOVE_BONUS;

                    //Normalize and record the score + target
                    if (moveLongScore < GameConstants.AI_MINIMUM_SCORE)
                    {
                        moveLongScore = GameConstants.AI_MINIMUM_SCORE;
                    }
                    possibleOptions.Add(new AIAction(ActionTypeEnum.DoubleMove)
                    {
                        Score = moveLongScore,
                        StartLocation = sourceCharacter.Location,
                        EndLocation = location
                    });
                }

                //Order the final options
                possibleOptions = possibleOptions.OrderByDescending(x => x.Score).ToList();
                //Get the best first option
                KeyValuePair<Vector3, AIAction> newItem = new KeyValuePair<Vector3, AIAction>(location, possibleOptions[GameConstants.FIRST_INDEX]);
                results.Add(newItem);
            }

            // Sort the values, highest first
            results = results.OrderByDescending(x => x.Value.Score).ToList();

            return results;
        }


        private string CreateAIMap(string[,,] map)
        {
            return MapCore.GetMapStringWithAIValuesSecond(map, aiValues);
        }

    }
}
