using TBE.Logic.AbilitiesAndEffects;
using TBE.Logic.Characters;
using TBE.Logic.Items;
using System.Collections.Generic;
using System.Numerics;

namespace TBE.Logic.Encounters
{
    public static class EncounterCore
    {
        /// <summary>
        /// Get chance to hit 
        /// </summary>
        /// <param name="sourceCharacter">Character who is trying to hit target</param>
        /// <param name="weapon">Weapon character is using</param>
        /// <param name="targetCharacter">Character who is being targeted</param>
        /// <returns>percentage int</returns>
        public static int GetChanceToHit(Character sourceCharacter, Weapon weapon, Character targetCharacter)
        {
            //Aim (unit stat + modifiers) - Defence (unit stat + modifiers) = total (clamped to 1%, if negative) + range modifier = final result

            //The characters base chance to hit
            int toHit = sourceCharacter.ChanceToHit;

            //Target cover adjustments
            if (targetCharacter.CoverState.InHalfCover)
            {
                if (targetCharacter.HunkeredDown)
                {
                    //When hunkered down, cover is worth double
                    toHit -= GameConstants.HUNKERED_HALF_COVER_PENALTY;
                }
                else
                {
                    toHit -= GameConstants.HALF_COVER_PENALTY;
                }
            }
            else if (targetCharacter.CoverState.InFullCover)
            {
                if (targetCharacter.HunkeredDown)
                {
                    //When hunkered down, cover is worth double
                    toHit -= GameConstants.HUNKERED_FULL_COVER_PENALTY;
                }
                else
                {
                    toHit -= GameConstants.FULL_COVER_PENALTY;
                }
            }

            //Weapon  modifiers
            toHit += weapon.ChanceToHitAdjustment;

            //Weapon range modifiers
            int distance = Range.GetDistance(sourceCharacter.Location, targetCharacter.Location);
            toHit += Range.GetRangeModifier(weapon, distance);

            //Overwatch
            int overwatchPenaltyRemoved = AggregateAbilitiesByType(sourceCharacter.Abilities, AbilityType.OverwatchPenaltyRemoved);
            if (overwatchPenaltyRemoved == GameConstants.DEAD_HITPOINTS && sourceCharacter.InOverwatch)
            {
                //reaction shots has a 0% Critical chance and reduced Aim, reduced to 70% of normal
                toHit = (int)((float)toHit * GameConstants.OVERWATCH_AIM_MULTIPLIER);
            }
            if (toHit > GameConstants.PERCENTAGE_MAX)
            {
                toHit = GameConstants.PERCENTAGE_MAX;
            }

            return toHit;
        }

        public static int GetChanceToCrit(string[,,] map, Character sourceCharacter, Weapon weapon, Character targetCharacter, bool isAreaEffectAttack)
        {
            int chanceToCrit = weapon.CriticalChance;
            CoverState coverState = CharacterCover.CalculateCover(map, targetCharacter.Location, new List<Vector3>() { sourceCharacter.Location });
            if (!isAreaEffectAttack && coverState.IsFlanked)
            {
                //Add 50% for a flank
                chanceToCrit += GameConstants.FLANKING_CRITICAL_BONUS;
            }
            chanceToCrit += AggregateAbilitiesByType(sourceCharacter.Abilities, AbilityType.CriticalChance);
            //reaction shots has a 0% Critical chance
            int overwatchPenaltyRemoved = AggregateAbilitiesByType(sourceCharacter.Abilities, AbilityType.OverwatchPenaltyRemoved);
            if (overwatchPenaltyRemoved == GameConstants.DEAD_HITPOINTS && sourceCharacter.InOverwatch)
            {
                chanceToCrit = GameConstants.OVERWATCH_CRITICAL_CHANCE;
            }
            return chanceToCrit;
        }

        /// <summary>
        /// Get damage ranges
        /// </summary>
        /// <param name="sourceCharacter">Character who is trying to hit target</param>
        /// <param name="weapon">Weapon being used</param>
        /// <returns>DamageRange: including damage and critical hit ranges</returns>
        public static DamageRange GetDamageRange(Character sourceCharacter, Weapon weapon)
        {
            DamageRange damageOptions = new DamageRange();
            int lowDamageAdjustment = 0;
            int highDamageAdjustment = 0;

            //Add abilities
            highDamageAdjustment += AggregateAbilitiesByType(sourceCharacter.Abilities, AbilityType.Damage);

            //Normal damage
            damageOptions.DamageLow = weapon.DamageRangeLow + lowDamageAdjustment;
            damageOptions.DamageHigh = weapon.DamageRangeHigh + highDamageAdjustment;

            //Critical damage
            damageOptions.CriticalDamageLow = damageOptions.DamageLow;
            damageOptions.CriticalDamageHigh = damageOptions.DamageHigh;

            damageOptions.CriticalDamageLow += weapon.CriticalDamageLow;
            damageOptions.CriticalDamageLow += AggregateAbilitiesByType(sourceCharacter.Abilities, AbilityType.CriticalDamage);
            damageOptions.CriticalDamageHigh += weapon.CriticalDamageHigh;
            damageOptions.CriticalDamageHigh += AggregateAbilitiesByType(sourceCharacter.Abilities, AbilityType.CriticalDamage);

            return damageOptions;
        }

        /// <summary>
        /// Add abilities adjustments by type
        /// </summary>
        /// <param name="abilities">List of abilities</param>
        /// <param name="type">The type of ability to aggregate</param>
        /// <returns>a total adjustment for this ability. Can be a positive or negative number</returns>
        public static int AggregateAbilitiesByType(List<Ability> abilities, AbilityType type)
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

    }
}
