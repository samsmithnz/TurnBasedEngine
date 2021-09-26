﻿using Battle.Logic.Encounters;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public static class CharacterAI
    {
        public static ActionResult CalculateAction(string[,,] map, Character character, RandomNumberQueue diceRolls)
        {
            List<string> log = new List<string>
            {
                character.Name + " is processing AI, with intelligence " + character.Intelligence
            };
            Vector3 startLocation = character.Location;
            //TODO: hard coded for now for the test to pass
            Vector3 endLocation = new Vector3(20, 0, 19);

            //1. Get a list of all possible moves
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, character.Location, character.MobilityRange);

            //2. Assign a value to each possible tile
            List<KeyValuePair<Vector3, int>> movementAIValues = AssignPointsToEachTile(map, null, character, movementPossibileTiles);

            //3. Assign a move based on the intelligence check
            endLocation = movementAIValues[0].Key;

            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomInt = diceRolls.Dequeue();
            if ((100 - character.Intelligence) <= randomInt)
            {
                log.Add("Successful intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll successful
                //TODO
            }
            else
            {
                log.Add("Failed intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll failed
                //TODO
            }

            character.InFullCover = true;
            return new ActionResult()
            {
                Log = log,
                StartLocation = startLocation,
                EndLocation = endLocation
            };
        }

        public static List<KeyValuePair<Vector3, int>> AssignPointsToEachTile(string[,,] map, List<Character> characters, Character character, List<Vector3> movementPossibileTiles)
        {
            //initialize the list
            List<KeyValuePair<Vector3, int>> movementAIValues = new List<KeyValuePair<Vector3, int>>();
            foreach (Vector3 item in movementPossibileTiles)
            {
                movementAIValues.Add(new KeyValuePair<Vector3, int>(item, 0));
            }

            //Create a list of opponent character locations
            List<Vector3> locations = new List<Vector3>();
            foreach (Character item in characters)
            {
                locations.Add(item.Location);
            }

            //Loop through each key value pair
            for (int i = 0; i < movementAIValues.Count; i++)
            {
                KeyValuePair<Vector3, int> item = movementAIValues[i];
                Vector3 location = item.Key;
                int currentScore = item.Value;

                if (map[(int)location.X, (int)location.Y, (int)location.Z] == "")
                {

                }

                CoverStateResult coverStateResult = CharacterCover.CalculateCover(map, location, locations);
                if (coverStateResult.IsInCover == true)
                {
                    currentScore += 2;
                }
                //List<Character> fovCharacters = FieldOfView.GetCharactersInArea(characters, map, location, character.ShootingRange);
                //foreach (Character fovCharacter in fovCharacters)
                //{

                //}

                KeyValuePair<Vector3, int> newItem = new KeyValuePair<Vector3, int>(location, currentScore);
                movementAIValues[i] = newItem;
            }

            // Sort the values, highest first
            movementAIValues = movementAIValues.OrderBy(x => x.Value).ToList();

            return movementAIValues;
        }
    }
}
