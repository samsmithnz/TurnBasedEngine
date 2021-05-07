﻿using Battle.Logic.Characters;
using Battle.Logic.Utility;
using Battle.Logic.Weapons;
using System.Collections.Generic;

namespace Battle.Logic.Encounters
{
    public class Encounter
    {
        //source character attacks target
        //Calculate change to hit
        //Calculate critical change
        //Add to hit and critical modifiers
        //Calculate cover for target
        //Calculate final chance to hit


        public static int GetChanceToHit(Character sourceCharacter, Weapon weapon, Character targetCharacter)
        {
            //Aim (unit stat + modifiers) - Defence (unit stat + modifiers) = total (clamped to 1%, if negative) + range modifier = final result

            //The characters base chance to hit
            int toHit = sourceCharacter.ChanceToHit;

            //TODO: character modifiers            
            //TODO: Target base defence
            //TODO: Target modifiers

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
            (int, bool) distanceAndDirection = Range.GetDistance(sourceCharacter.Location, targetCharacter.Location);
            int distance = distanceAndDirection.Item1;
            bool isDiagonalDirection = distanceAndDirection.Item2;
            toHit += Range.GetRangeModifier(weapon, distance, isDiagonalDirection);

            return toHit;
        }

        public static EncounterResult AttackCharacter(Character sourceCharacter, Weapon weapon, Character targetCharacter, List<int> randomNumberList)
        {
            int randomNumberIndex = 0;
            if (randomNumberList == null || randomNumberList.Count == 0)
            {
                return null;
            }
            int toHit = GetChanceToHit(sourceCharacter, weapon, targetCharacter);

            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomToHitNumber = randomNumberList[randomNumberIndex];
            randomNumberIndex++;
            if (toHit >= randomToHitNumber)
            {
                int damage = randomNumberList[randomNumberIndex];
                //randomNumberIndex++;

                int highDamageAdjustment = 0;
                if (sourceCharacter.Abilities.Count > 0)
                {
                    highDamageAdjustment = ProcessAbilitiesByType(sourceCharacter.Abilities,AbilityTypeEnum.Damage);
                }

                //process damage
                targetCharacter.HP -= RandomNumber.ScaleRandomNumber(weapon.LowDamageRange,
                        weapon.HighDamageRange + highDamageAdjustment,
                        damage);

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

    }
}
