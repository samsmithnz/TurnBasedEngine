using Battle.Logic.Characters;
using Battle.Logic.Weapons;
using System.Collections.Generic;

namespace Battle.Logic.Encounters
{
    public class Encounter
    {
        public static EncounterResult AttackCharacter(Character sourceCharacter, Weapon weapon, Character targetCharacter, List<int> randomNumberList)
        {
            if (randomNumberList == null || randomNumberList.Count == 0)
            {
                return null;
            }
            //If the number rolled is higher than the chance to hit, the attack was successful!
            if (sourceCharacter.ChanceToHit <= randomNumberList[0])
            {
                targetCharacter.HP -= weapon.DamageRange;
            }
            EncounterResult result = new()
            {
                SourceCharacter = sourceCharacter,
                TargetCharacter = targetCharacter
            };
            return result;
        }

    }
}
