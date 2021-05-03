using System.Collections.Generic;

namespace Battle.Logic
{
    public class Encounter
    {
        public static bool AttackCharacter(Character sourceCharacter, Weapon weapon, Character targetCharacter, List<int> randomNumberList)
        {
            //If the number rolled is higher than the chance to hit, the attack was successful!
            if (sourceCharacter.ChanceToHit <= randomNumberList[0])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
