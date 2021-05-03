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
                //process damage
                targetCharacter.HP -= weapon.DamageRange;
                
                //process experience
                if (targetCharacter.HP <= 0)
                {
                    targetCharacter.HP = 0;
                    sourceCharacter.Experience += Experience.GetExperience(true, true);
                }
                else
                {
                    sourceCharacter.Experience += Experience.GetExperience(true);
                }
            }
            else
            {
                sourceCharacter.Experience += Experience.GetExperience(false);
            }

            //Check if the character has enough experience to level up
            sourceCharacter.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(sourceCharacter.Level, sourceCharacter.Experience);

            EncounterResult result = new()
            {
                SourceCharacter = sourceCharacter,
                TargetCharacter = targetCharacter
            };
            return result;
        }

    }
}
