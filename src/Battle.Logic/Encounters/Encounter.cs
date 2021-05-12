using Battle.Logic.CharacterCover;
using Battle.Logic.Characters;
using Battle.Logic.FieldOfView;
using Battle.Logic.Utility;
using Battle.Logic.Weapons;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Encounters
{
    public class Encounter
    {
            public static EncounterResult AttackCharacterWithAreaOfEffect(Character sourceCharacter, Weapon weapon, List<Character> allCharacters, string[,] map, List<int> diceRolls, Vector3 throwingTargetLocation)
        {
            int damageDealt;
            bool isCriticalHit = false;

            int diceRollIndex = 0;
            if (diceRolls == null || diceRolls.Count == 0)
            {
                return null;
            }
            //throws always hit, process this before tohit        
            //int toHitPercent = GetChanceToHit(sourceCharacter, weapon, targetCharacter);

            //If the number rolled is higher than the chance to hit, the attack was successful!
            //int randomToHit = diceRolls[diceRollIndex];
            diceRollIndex++;

            //Get the targets in the area affected
            List<Character> areaEffectTargets = AreaEffectFieldOfView.GetCharactersInArea(allCharacters, map, throwingTargetLocation, weapon.AreaEffectRadius);

            //Get damage 
            int damageRollPercent = diceRolls[diceRollIndex];
            diceRollIndex++;
            DamageOptions damageOptions = EncounterCore.GetDamageRange(sourceCharacter, weapon);
            int lowDamage = damageOptions.DamageLow;
            int highDamage = damageOptions.DamageHigh;

            //Check if it was a critical hit
            int randomToCrit = diceRolls[diceRollIndex];
            int chanceToCrit = EncounterCore.GetChanceToCrit(sourceCharacter, weapon, null, map);
            if ((100 - chanceToCrit) <= randomToCrit)
            {
                isCriticalHit = true;
                lowDamage = damageOptions.CriticalDamageLow;
                highDamage = damageOptions.CriticalDamageHigh;
            }

            //process damage
            damageDealt = RandomNumber.ScaleRandomNumber(
                    lowDamage, highDamage,
                    damageRollPercent);

            //Deal damage to each target
            foreach (Character character in areaEffectTargets)
            {
                //Deal the damage
                character.HP -= damageDealt;

                //process experience
                if (character.HP <= 0)
                {
                    //targetCharacter.HP = 0;
                    sourceCharacter.Experience += Experience.GetExperience(true, true);
                }
                else
                {
                    sourceCharacter.Experience += Experience.GetExperience(true);
                }
            }

            //Consume source characters action points
            sourceCharacter.ActionPoints = 0;

            //Check if the character has enough experience to level up
            sourceCharacter.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(sourceCharacter.Level, sourceCharacter.Experience);

            EncounterResult result = new()
            {
                SourceCharacter = sourceCharacter,
                AllCharacters = allCharacters,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit
            };
            return result;
        }

        public static EncounterResult AttackCharacter(Character sourceCharacter, Weapon weapon, Character targetCharacter, string[,] map, List<int> diceRolls)
        {
            int damageDealt = 0;
            bool isCriticalHit = false;

            int diceRollIndex = 0;
            if (diceRolls == null || diceRolls.Count == 0)
            {
                return null;
            }
            int toHitPercent = EncounterCore.GetChanceToHit(sourceCharacter, weapon, targetCharacter);

            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomToHit = diceRolls[diceRollIndex];
            diceRollIndex++;

            if ((100 - toHitPercent) <= randomToHit)
            {
                //Get damage 
                int damageRollPercent = diceRolls[diceRollIndex];
                diceRollIndex++;
                DamageOptions damageOptions = EncounterCore.GetDamageRange(sourceCharacter, weapon);
                int lowDamage = damageOptions.DamageLow;
                int highDamage = damageOptions.DamageHigh;

                //Check if it was a critical hit
                int randomToCrit = diceRolls[diceRollIndex];
                int chanceToCrit = EncounterCore.GetChanceToCrit(sourceCharacter, weapon, targetCharacter, map);
                if ((100 - chanceToCrit) <= randomToCrit)
                {
                    isCriticalHit = true;
                    lowDamage = damageOptions.CriticalDamageLow;
                    highDamage = damageOptions.CriticalDamageHigh;
                }

                //process damage
                damageDealt = RandomNumber.ScaleRandomNumber(
                        lowDamage, highDamage,
                        damageRollPercent);

                //If the damage dealt is more than the health, set damage to be equal to health
                //if (targetCharacter.HP <= damageDealt)
                //{
                //    damageDealt = targetCharacter.HP;
                //}
                targetCharacter.HP -= damageDealt;

                //process experience
                if (targetCharacter.HP <= 0)
                {
                    //targetCharacter.HP = 0;
                    sourceCharacter.Experience += Experience.GetExperience(true, true);
                }
                else
                {
                    sourceCharacter.Experience += Experience.GetExperience(true);
                }

                //Consume source characters action points
                sourceCharacter.ActionPoints = 0;
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
                TargetCharacter = targetCharacter,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit
            };
            return result;
        }

    }
}
