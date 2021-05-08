using Battle.Logic.CharacterCover;
using Battle.Logic.Characters;
using Battle.Logic.Utility;
using Battle.Logic.Weapons;
using System;
using System.Collections.Generic;

namespace Battle.Logic.Encounters
{
    public class Encounter
    {

        public static int GetChanceToHit(Character sourceCharacter, Weapon weapon, Character targetCharacter)
        {
            //Aim (unit stat + modifiers) - Defence (unit stat + modifiers) = total (clamped to 1%, if negative) + range modifier = final result

            //The characters base chance to hit
            int toHit = sourceCharacter.ChanceToHit;

            //Target cover adjustments
            if (targetCharacter.InHalfCover == true)
            {
                toHit -= 20;
            }
            else if (targetCharacter.InFullCover == true)
            {
                toHit -= 40;
            }

            //Weapon  modifiers
            toHit += weapon.ChanceToHitAdjustment;

            //Weapon range modifiers
            int distance = Range.GetDistance(sourceCharacter.Location, targetCharacter.Location);
            //int distance = distanceAndDirection.Item1;
            //bool isDiagonalDirection = distanceAndDirection.Item2;
            toHit += Range.GetRangeModifier(weapon, distance);//, isDiagonalDirection);

            return toHit;
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
            int toHitPercent = GetChanceToHit(sourceCharacter, weapon, targetCharacter);

            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomToHit = diceRolls[diceRollIndex];
            diceRollIndex++;
            if ((100 - toHitPercent) <= randomToHit)
            {
                //Setup damage
                int damagePercent = diceRolls[diceRollIndex];
                diceRollIndex++;
                int lowDamageAdjustment = 0;
                int highDamageAdjustment = 0;
                highDamageAdjustment += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityTypeEnum.Damage);

                //Check if it was a critical hit
                int randomToCrit = diceRolls[diceRollIndex];
                int chanceToCrit = weapon.CriticalChance;
                if (TargetIsFlanked(sourceCharacter, targetCharacter, map) == true)
                {
                    //Add 50% for a flank
                    chanceToCrit += 50;
                }
                chanceToCrit += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityTypeEnum.CriticalChance);
                if ((100 - chanceToCrit) <= randomToCrit)
                {
                    isCriticalHit = true;
                    lowDamageAdjustment += weapon.CriticalDamageLow;
                    highDamageAdjustment += weapon.CriticalDamageHigh;
                    lowDamageAdjustment += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityTypeEnum.CriticalDamage);
                    highDamageAdjustment += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityTypeEnum.CriticalDamage);
                }

                //process damage
                damageDealt = RandomNumber.ScaleRandomNumber(
                        weapon.DamageRangeLow + lowDamageAdjustment,
                        weapon.DamageRangeHigh + highDamageAdjustment,
                        damagePercent);

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

        private static int ProcessAbilitiesByType(List<CharacterAbility> abilities, AbilityTypeEnum type)
        {
            int adjustment = 0;
            foreach (CharacterAbility item in abilities)
            {
                if (item.Type == type)
                {
                    adjustment += item.Adjustment;
                }
            }
            return adjustment;
        }

        private static bool TargetIsFlanked(Character sourceCharacter, Character targetCharacter, string[,] map)
        {
            Console.WriteLine(map.Length);
            Console.WriteLine(sourceCharacter.Location);
            Console.WriteLine(targetCharacter.Location);
            //This is where we will call the cover calculation
            CoverState coverState = Cover.CalculateCover(targetCharacter.Location, map.GetLength(0), map.GetLength(1), map, new() { sourceCharacter.Location });

            return !coverState.IsInCover;
        }

    }
}
