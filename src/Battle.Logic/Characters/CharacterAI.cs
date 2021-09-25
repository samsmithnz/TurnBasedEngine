using Battle.Logic.Encounters;
using Battle.Logic.Utility;
using System.Collections.Generic;

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

            //1. Get a list of all possible moves

            //2. Assign a value to each possible tile

            //3. Assign a move based on the intelligence check

            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomInt = diceRolls.Dequeue();
            if ((100 - character.Intelligence) <= randomInt)
            {
                log.Add("Successful intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll successful
            }
            else
            {
                log.Add("Failed intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll failed
            }

            character.InFullCover = true;
            return new ActionResult()
            {
                Log = log,
                StartLocation = new System.Numerics.Vector3(15, 0, 15),
                EndLocation = new System.Numerics.Vector3(20, 0, 19)
            };
        }
    }
}
