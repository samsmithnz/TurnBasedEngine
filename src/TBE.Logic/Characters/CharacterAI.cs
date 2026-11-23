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

            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, character.Location, character.MobilityRange, character.ActionPointsCurrent);
            aiValues = AssignPointsToEachTile(map, character, opponentTeam, movementPossibileTiles);
            if (aiValues.Count == 0)
            {
                throw new System.Exception("AssignPointsToEachTile returned no results");
            }

            int randomInt = diceRolls.Dequeue();
            AIAction aiActionResult;
            if ((GameConstants.PERCENTAGE_MAX - character.Intelligence) <= randomInt)
            {
                log.Add("Successful intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                aiActionResult = aiValues[0].Value;
                aiActionResult.IntelligenceCheckSuccessful = true;
            }
            else
            {
                log.Add("Failed intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                int highestScore = aiValues[0].Value.Score;
                aiActionResult = null;
                for (int i = 1; i < aiValues.Count; i++)
                {
                    if (aiValues[i].Value.Score < highestScore)
                    {
                        aiActionResult = aiValues[i].Value;
                        break;
                    }
                }
                if (aiActionResult == null)
                {
                    aiActionResult = aiValues[aiValues.Count > 1 ? 1 : 0].Value;
                }
            }

            aiActionResult.MapString = CreateAIMap(map);
            aiActionResult.Log = log;
            return aiActionResult;
        }

        private List<KeyValuePair<Vector3, AIAction>> AssignPointsToEachTile(string[,,] map, Character sourceCharacter, Team opponentTeam, List<KeyValuePair<Vector3, int>> movementPossibileTiles)
        {
            List<KeyValuePair<Vector3, AIAction>> results = new List<KeyValuePair<Vector3, AIAction>>();
            List<Character> opponentCharacters = new List<Character>();
            List<Vector3> opponentLocations = new List<Vector3>();
            foreach (Character opponent in opponentTeam.Characters)
            {
                if (opponent.HitpointsCurrent > GameConstants.DEAD_HITPOINTS)
                {
                    opponentCharacters.Add(opponent);
                    opponentLocations.Add(opponent.Location);
                }
            }

            foreach (KeyValuePair<Vector3, int> movement in movementPossibileTiles)
            {
                int score = GetPositionScore(map, movement.Key, sourceCharacter, opponentCharacters, opponentLocations);
                bool isSingleMove = movement.Value == GameConstants.SINGLE_MOVE_ACTION_POINT_COST;
                score += isSingleMove ? GameConstants.AI_SINGLE_MOVE_BONUS : GameConstants.AI_DOUBLE_MOVE_BONUS;
                if (score < GameConstants.AI_MINIMUM_SCORE)
                {
                    score = GameConstants.AI_MINIMUM_SCORE;
                }

                AIAction moveAction = new AIAction(isSingleMove ? ActionTypeEnum.Move : ActionTypeEnum.DoubleMove)
                {
                    Score = score,
                    StartLocation = sourceCharacter.Location,
                    EndLocation = movement.Key,
                    ActionPointsCost = movement.Value
                };
                results.Add(new KeyValuePair<Vector3, AIAction>(movement.Key, moveAction));
            }

            if (sourceCharacter.ActionPointsCurrent > 0 && sourceCharacter.WeaponEquipped != null)
            {
                string[,,] currentMap = (string[,,])map.Clone();
                List<Character> charactersInView = FieldOfView.GetCharactersInView(
                    currentMap,
                    sourceCharacter.Location,
                    sourceCharacter.ShootingRange,
                    opponentCharacters);

                if (sourceCharacter.WeaponEquipped.AmmoCurrent > 0)
                {
                    foreach (Character target in charactersInView)
                    {
                        int score = GetPositionScore(map, sourceCharacter.Location, sourceCharacter, opponentCharacters, opponentLocations);
                        int chanceToHit = EncounterCore.GetChanceToHit(sourceCharacter, sourceCharacter.WeaponEquipped, target);
                        score += GetHitChanceScore(chanceToHit);
                        if (score < GameConstants.AI_MINIMUM_SCORE)
                        {
                            score = GameConstants.AI_MINIMUM_SCORE;
                        }

                        AIAction attackAction = new AIAction(ActionTypeEnum.StayInPositionThenAttack)
                        {
                            Score = score,
                            StartLocation = sourceCharacter.Location,
                            EndLocation = sourceCharacter.Location,
                            TargetName = target.Name,
                            TargetLocation = target.Location,
                            ActionPointsCost = sourceCharacter.ActionPointsCurrent
                        };
                        results.Add(new KeyValuePair<Vector3, AIAction>(sourceCharacter.Location, attackAction));
                    }

                    if (charactersInView.Count == 0)
                    {
                        int score = GetPositionScore(map, sourceCharacter.Location, sourceCharacter, opponentCharacters, opponentLocations)
                            + GameConstants.AI_NO_CHARACTERS_IN_VIEW_PENALTY;
                        AIAction overwatchAction = new AIAction(ActionTypeEnum.Overwatch)
                        {
                            Score = System.Math.Max(score, GameConstants.AI_MINIMUM_SCORE),
                            StartLocation = sourceCharacter.Location,
                            EndLocation = sourceCharacter.Location,
                            ActionPointsCost = sourceCharacter.ActionPointsCurrent
                        };
                        results.Add(new KeyValuePair<Vector3, AIAction>(sourceCharacter.Location, overwatchAction));
                    }
                }
            }

            return results.OrderByDescending(x => x.Value.Score).ToList();
        }

        private int GetPositionScore(string[,,] map, Vector3 location, Character sourceCharacter, List<Character> opponentCharacters, List<Vector3> opponentLocations)
        {
            string[,,] fovMap = (string[,,])map.Clone();
            fovMap[(int)sourceCharacter.Location.X, (int)sourceCharacter.Location.Y, (int)sourceCharacter.Location.Z] = GameConstants.EMPTY_TILE;
            fovMap[(int)location.X, (int)location.Y, (int)location.Z] = GameConstants.PLAYER_MAP_MARKER;

            int score = GameConstants.AI_BASE_SCORE;
            CoverState coverState = CharacterCover.CalculateCover(fovMap, location, opponentLocations);
            if (coverState.IsFlanked)
            {
                score += GameConstants.AI_FLANKED_PENALTY;
            }
            else if (coverState.InFullCover)
            {
                score += GameConstants.AI_FULL_COVER_BONUS;
            }
            else if (coverState.InHalfCover)
            {
                score += GameConstants.AI_HALF_COVER_BONUS;
            }

            List<Character> charactersInArea = FieldOfView.GetCharactersInArea(fovMap, opponentCharacters, location, sourceCharacter.ShootingRange);
            foreach (Character opponent in charactersInArea)
            {
                CoverState opponentCoverState = CharacterCover.CalculateCover(fovMap, opponent.Location, new List<Vector3>() { location });
                if (opponentCoverState.IsFlanked)
                {
                    score += GameConstants.AI_FLANKING_ENEMY_BONUS;
                }

                List<Vector3> visibleTiles = FieldOfView.GetFieldOfView(fovMap, opponent.Location, opponent.FOVRange);
                if (visibleTiles.Contains(location))
                {
                    score += GameConstants.AI_VISIBLE_TILE_PENALTY;
                }
            }

            return score;
        }

        private int GetHitChanceScore(int chanceToHit)
        {
            if (chanceToHit >= GameConstants.AI_HIT_CHANCE_EXCELLENT_THRESHOLD)
            {
                return GameConstants.AI_HIT_CHANCE_EXCELLENT_BONUS;
            }
            if (chanceToHit >= GameConstants.AI_HIT_CHANCE_VERY_GOOD_THRESHOLD)
            {
                return GameConstants.AI_HIT_CHANCE_VERY_GOOD_BONUS;
            }
            if (chanceToHit >= GameConstants.AI_HIT_CHANCE_GOOD_THRESHOLD)
            {
                return GameConstants.AI_HIT_CHANCE_GOOD_BONUS;
            }
            if (chanceToHit >= GameConstants.AI_HIT_CHANCE_DECENT_THRESHOLD)
            {
                return GameConstants.AI_HIT_CHANCE_DECENT_BONUS;
            }
            if (chanceToHit >= GameConstants.AI_HIT_CHANCE_FAIR_THRESHOLD)
            {
                return GameConstants.AI_HIT_CHANCE_FAIR_BONUS;
            }

            return GameConstants.AI_HIT_CHANCE_POOR_BONUS;
        }

        private string CreateAIMap(string[,,] map)
        {
            return MapCore.GetMapStringWithAIValuesSecond(map, aiValues);
        }
    }
}
