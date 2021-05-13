using Battle.Logic.CharacterCover;
using Battle.Logic.Characters;
using Battle.Logic.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle.Logic.Encounters
{
    public class EncounterCore
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
            toHit += Range.GetRangeModifier(weapon, distance);

            return toHit;
        }

        public static int GetChanceToCrit(Character sourceCharacter, Weapon weapon, Character targetCharacter, string[,] map)
        {
            int chanceToCrit = weapon.CriticalChance;
            if (targetCharacter != null && TargetIsFlanked(sourceCharacter, targetCharacter, map) == true)
            {
                //Add 50% for a flank
                chanceToCrit += 50;
            }
            chanceToCrit += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityTypeEnum.CriticalChance);
            return chanceToCrit;
        }

        public static DamageOptions GetDamageRange(Character sourceCharacter, Weapon weapon)
        {
            DamageOptions damageOptions = new();
            int lowDamageAdjustment = 0;
            int highDamageAdjustment = 0;

            //Add abilities
            highDamageAdjustment += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityTypeEnum.Damage);

            //Normal damage
            damageOptions.DamageLow = weapon.DamageRangeLow + lowDamageAdjustment;
            damageOptions.DamageHigh = weapon.DamageRangeHigh + highDamageAdjustment;

            //Critical damage
            damageOptions.CriticalDamageLow = damageOptions.DamageLow;
            damageOptions.CriticalDamageHigh = damageOptions.DamageHigh;

            damageOptions.CriticalDamageLow += weapon.CriticalDamageLow;
            damageOptions.CriticalDamageLow += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityTypeEnum.CriticalDamage);
            damageOptions.CriticalDamageHigh += weapon.CriticalDamageHigh;
            damageOptions.CriticalDamageHigh += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityTypeEnum.CriticalDamage);

            return damageOptions;
        }

        public static int ProcessAbilitiesByType(List<CharacterAbility> abilities, AbilityTypeEnum type)
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
