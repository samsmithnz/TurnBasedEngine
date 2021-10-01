using Battle.Logic.Encounters;
using Battle.Logic.GameController;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public class CharacterAISecondPass
    {
        private List<KeyValuePair<Vector3, AIAction>> aiValues;

        public CharacterAISecondPass()
        {
            aiValues = new List<KeyValuePair<Vector3, AIAction>>();
        }

        public AIAction CalculateAIAction(string[,,] map, List<Team> teams, Character character, RandomNumberQueue diceRolls)
        {
            List<string> log = new List<string>
            {
                character.Name + " is processing AI, with intelligence " + character.Intelligence
            };
            Vector3 startLocation = character.Location;

            //1. Start with a list of all possible moves, a location, and number of movement points to that location
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, character.Location, character.MobilityRange, character.ActionPointsCurrent);

            //2. Assign values to each possible tile
            aiValues = AssignPointsToEachTile(map, teams, character, movementPossibileTiles);

            //3. Assign an action based on the intelligence check
            Vector3 endLocation;
            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomInt = diceRolls.Dequeue();
            if ((100 - character.Intelligence) <= randomInt)
            {
                log.Add("Successful intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll successful
                endLocation = aiValues[0].Key;
            }
            else
            {
                log.Add("Failed intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll failed
                endLocation = aiValues[aiValues.Count - 1].Key;
            }

            return new AIAction()
            {
                Log = log,
                StartLocation = startLocation,
                EndLocation = endLocation
            };
        }

        private List<KeyValuePair<Vector3, AIAction>> AssignPointsToEachTile(string[,,] map, List<Team> teams, Character character, List<KeyValuePair<Vector3, int>> movementPossibileTiles)
        {
            //Preprocess a list of opponent characters and a list of character locations
            List<Character> opponentCharacters = new List<Character>();
            List<Vector3> opponentLocations = new List<Vector3>();
            foreach (Team team in teams)
            {
                //Exclude the characters team (Assume all other teams are the bad guys)
                if (!team.Characters.Contains(character))
                {
                    foreach (Character item in team.Characters)
                    {
                        opponentCharacters.Add(item);
                        opponentLocations.Add(item.Location);
                    }
                }
            }

            //Loop through each movement possibility, assigning scores and then choosing the best one
            int maxActionPoints = movementPossibileTiles[movementPossibileTiles.Count - 1].Value;
            for (int i = 0; i < movementPossibileTiles.Count; i++)
            {
                KeyValuePair<Vector3, int> item = movementPossibileTiles[i];
                Vector3 location = item.Key;
                //if (location == new Vector3(5, 0, 4))
                //{
                //    int j = 123;
                //}
                //start at zero
                int currentScore = 0;
                //Move the character in a temp map to simulate the board for this situation
                string[,,] fovMap = (string[,,])map.Clone();
                fovMap[(int)character.Location.X, (int)character.Location.Y, (int)character.Location.Z] = "";
                fovMap[(int)location.X, (int)location.Y, (int)location.Z] = "";

                //Cover calculation
                CoverState coverStateResult = CharacterCover.CalculateCover(fovMap, location, opponentLocations);
                if (coverStateResult.IsFlanked)
                {
                    currentScore -= 1;
                }
                else if (coverStateResult.InFullCover)
                {
                    currentScore += 2;
                }
                else if (coverStateResult.InHalfCover)
                {
                    currentScore += 1;
                }

                //Movement points
                if (item.Value <= maxActionPoints)
                {
                    currentScore += maxActionPoints + 2 - item.Value;
                }

                //Upgrade positions that would flank opponents
                List<Character> fovCharacters = FieldOfView.GetCharactersInArea(opponentCharacters, fovMap, location, character.ShootingRange);
                foreach (Character fovCharacter in fovCharacters)
                {
                    CoverState coverStateResultOpponent = CharacterCover.CalculateCover(fovMap, fovCharacter.Location, new List<Vector3>() { location });
                    if (coverStateResultOpponent.IsFlanked)
                    {
                        //Position flanks enemy
                        currentScore += 2;
                    }

                    //Do a reverse FOV from the perspective of the character
                    List<Vector3> fovCharacterVisibleTiles = FieldOfView.GetFieldOfView(fovMap, fovCharacter.Location, fovCharacter.FOVRange);
                    //reduce the score for those tiles - they are more dangerous to move into
                    if (fovCharacterVisibleTiles.Contains(location))
                    {
                        currentScore -= 2;
                    }
                }

                //Attack, but not on the last action point
                if (maxActionPoints - item.Value >= 1)
                {
                }

                if (currentScore < 0)
                {
                    currentScore = 0;
                }

                KeyValuePair<Vector3, int> newItem = new KeyValuePair<Vector3, int>(location, currentScore);
                //aiValues[i] = newItem;
            }

            // Sort the values, highest first
            //aiValues = aiValues.OrderByDescending(x => x.Value).ToList();

            return aiValues;
        }

        //public string CreateAIMap(string[,,] map)
        //{
        //    if (aiValues == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return MapCore.GetMapStringWithItemValues(map, aiValues);
        //    }
        //}

    }
}
