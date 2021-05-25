using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Weapons;
using System;
using System.Collections.Generic;

namespace Battle.Logic.Encounters
{
    public static class EncounterCore
    {
        public static int GetChanceToHit(Character sourceCharacter, Weapon weapon, Character targetCharacter)
        {
            //Aim (unit stat + modifiers) - Defence (unit stat + modifiers) = total (clamped to 1%, if negative) + range modifier = final result

            //The characters base chance to hit
            int toHit = sourceCharacter.ChanceToHit;

            //Target cover adjustments
            if (targetCharacter.InHalfCover == true)
            {
                if (targetCharacter.HunkeredDown == true)
                {
                    //When hunkered down, cover is worth double
                    toHit -= 40;
                }
                else
                {
                    toHit -= 20;
                }
            }
            else if (targetCharacter.InFullCover == true)
            {
                if (targetCharacter.HunkeredDown == true)
                {
                    //When hunkered down, cover is worth double
                    toHit -= 80;
                }
                else
                {
                    toHit -= 40;
                }
            }

            //Weapon  modifiers
            toHit += weapon.ChanceToHitAdjustment;

            //Weapon range modifiers
            int distance = Range.GetDistance(sourceCharacter.Location, targetCharacter.Location);
            toHit += Range.GetRangeModifier(weapon, distance);

            //Overwatch
            if (sourceCharacter.InOverwatch == true)
            {
                //reaction shots has a 0% Critical chance and reduced Aim, reduced to 70 % of normal
                toHit = (int)((float)toHit * 0.7f);
            }
            if (toHit > 100)
            {
                toHit = 100;
            }

            return toHit;
        }

        public static int GetChanceToCrit(Character sourceCharacter, Weapon weapon, Character targetCharacter, string[,] map, bool isAreaEffectAttack)
        {
            int chanceToCrit = weapon.CriticalChance;
            if (isAreaEffectAttack == false && targetCharacter != null && TargetIsFlanked(sourceCharacter, targetCharacter, map) == true)
            {
                //Add 50% for a flank
                chanceToCrit += 50;
            }
            chanceToCrit += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityType.CriticalChance);
            //reaction shots has a 0% Critical chance
            if (sourceCharacter.InOverwatch == true)
            {
                chanceToCrit = 0;
            }
            return chanceToCrit;
        }

        public static DamageOptions GetDamageRange(Character sourceCharacter, Weapon weapon)
        {
            DamageOptions damageOptions = new();
            int lowDamageAdjustment = 0;
            int highDamageAdjustment = 0;

            //Add abilities
            highDamageAdjustment += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityType.Damage);

            //Normal damage
            damageOptions.DamageLow = weapon.DamageRangeLow + lowDamageAdjustment;
            damageOptions.DamageHigh = weapon.DamageRangeHigh + highDamageAdjustment;

            //Critical damage
            damageOptions.CriticalDamageLow = damageOptions.DamageLow;
            damageOptions.CriticalDamageHigh = damageOptions.DamageHigh;

            damageOptions.CriticalDamageLow += weapon.CriticalDamageLow;
            damageOptions.CriticalDamageLow += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityType.CriticalDamage);
            damageOptions.CriticalDamageHigh += weapon.CriticalDamageHigh;
            damageOptions.CriticalDamageHigh += ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityType.CriticalDamage);

            return damageOptions;
        }

        public static int ProcessAbilitiesByType(List<Ability> abilities, AbilityType type)
        {
            int adjustment = 0;
            foreach (Ability item in abilities)
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
            CoverStateResult coverState = Characters.Characters.CalculateCover(targetCharacter.Location, map.GetLength(0), map.GetLength(1), map, new() { sourceCharacter.Location });

            return !coverState.IsInCover;
        }
    }
}
