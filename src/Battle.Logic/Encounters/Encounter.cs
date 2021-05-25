using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using Battle.Logic.Weapons;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Encounters
{
    public static class Encounter
    {
        public static EncounterResult AttackCharacterWithAreaOfEffect(Character sourceCharacter, Weapon weapon, List<Character> allCharacters, string[,] map, Queue<int> diceRolls, Vector3 throwingTargetLocation)
        {
            int damageDealt;
            bool isCriticalHit = false;
            List<string> log = new();

            if (diceRolls == null || diceRolls.Count == 0 || weapon == null || weapon.ClipRemaining <= 0)
            {
                return null;
            }
            log.Add(sourceCharacter.Name + " is attacking with area effect " + weapon.Name + " aimed at " + throwingTargetLocation.ToString());

            //Get the targets in the area affected
            List<Character> areaEffectTargets = FieldOfView.GetCharactersInArea(allCharacters, map, throwingTargetLocation, weapon.AreaEffectRadius);
            StringBuilder names = new();
            foreach (Character item in areaEffectTargets)
            {
                names.Append(' ');
                names.Append(item.Name);
                names.Append(", ");
            }
            log.Add("Characters in affected area: " + names.ToString()[1..^2]);//remove the first " " and last two characters: ", "

            //Deal damage to each target
            int totalDamageDealt = 0;
            foreach (Character targetCharacter in areaEffectTargets)
            {
                EncounterResult tempResult = ProcessCharacterDamageAndExperience(sourceCharacter, weapon, targetCharacter, map, diceRolls, log, true);
                sourceCharacter = tempResult.SourceCharacter;
                damageDealt = tempResult.DamageDealt;
                totalDamageDealt += damageDealt;
                isCriticalHit = tempResult.IsCriticalHit;
                log = tempResult.Log;
            }

            //Remove cover
            List<Vector3> area = MapCore.GetMapArea(map, throwingTargetLocation, weapon.AreaEffectRadius, false, true);
            foreach (Vector3 item in area)
            {
                if (map[(int)item.X, (int)item.Z] == CoverType.FullCover || map[(int)item.X, (int)item.Z] == CoverType.HalfCover)
                {
                    map[(int)item.X, (int)item.Z] = "";
                    log.Add("Cover removed from " + item.ToString());
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
                DamageDealt = totalDamageDealt,
                //DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit,
                Log = log
            };
            return result;
        }

        public static EncounterResult AttackCharacter(Character sourceCharacter, Weapon weapon, Character targetCharacter, string[,] map, Queue<int> diceRolls)
        {
            int damageDealt = 0;
            bool isCriticalHit = false;
            List<string> log = new();
            log.Add(sourceCharacter.Name + " is attacking with " + weapon.Name + ", targeted on " + targetCharacter.Name.ToString());

            if (diceRolls == null || diceRolls.Count == 0)
            {
                return null;
            }
            int toHitPercent = EncounterCore.GetChanceToHit(sourceCharacter, weapon, targetCharacter);

            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomToHit = diceRolls.Dequeue();

            if ((100 - toHitPercent) <= randomToHit)
            {
                log.Add("Hit: Chance to hit: " + toHitPercent.ToString() + ", (dice roll: " + randomToHit.ToString() + ")");

                EncounterResult tempResult = ProcessCharacterDamageAndExperience(sourceCharacter, weapon, targetCharacter, map, diceRolls, log, false);
                sourceCharacter = tempResult.SourceCharacter;
                targetCharacter = tempResult.TargetCharacter;
                damageDealt = tempResult.DamageDealt;
                isCriticalHit = tempResult.IsCriticalHit;
                log = tempResult.Log;
            }
            else
            {
                log.Add("Missed: Chance to hit: " + toHitPercent.ToString() + ", (dice roll: " + randomToHit.ToString() + ")");

                int xp = Experience.GetExperience(false);
                sourceCharacter.Experience += xp;
                log.Add(xp.ToString() + " XP added to character " + sourceCharacter.Name + ", for a total of " + sourceCharacter.Experience + " XP");
            }

            //Consume source characters action points
            sourceCharacter.ActionPoints = 0;

            //Check if the character has enough experience to level up
            sourceCharacter.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(sourceCharacter.Level, sourceCharacter.Experience);
            if (sourceCharacter.LevelUpIsReady == true)
            {
                log.Add(sourceCharacter.Name + " is ready to level up");
            }

            EncounterResult result = new()
            {
                SourceCharacter = sourceCharacter,
                TargetCharacter = targetCharacter,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit,
                Log = log
            };
            return result;
        }


        private static EncounterResult ProcessCharacterDamageAndExperience(Character sourceCharacter, Weapon weapon, Character targetCharacter, string[,] map, Queue<int> diceRolls, List<string> log, bool isAreaEffectAttack)
        {
            //Get damage 
            int damageRollPercent = diceRolls.Dequeue();
            DamageOptions damageOptions = EncounterCore.GetDamageRange(sourceCharacter, weapon);
            int lowDamage = damageOptions.DamageLow;
            int highDamage = damageOptions.DamageHigh;
            log.Add("Damage range: " + lowDamage.ToString() + "-" + highDamage.ToString() + ", (dice roll: " + damageRollPercent + ")");

            //Check if it was a critical hit
            bool isCriticalHit = false;
            if (targetCharacter.HunkeredDown == false) // player can't be critically hit if hunkered down
            {
                int randomToCrit = diceRolls.Dequeue();
                int chanceToCrit = EncounterCore.GetChanceToCrit(sourceCharacter, weapon, targetCharacter, map, isAreaEffectAttack);
                if ((100 - chanceToCrit) <= randomToCrit)
                {
                    isCriticalHit = true;
                    lowDamage = damageOptions.CriticalDamageLow;
                    highDamage = damageOptions.CriticalDamageHigh;
                }
                log.Add("Critical chance: " + chanceToCrit.ToString() + ", (dice roll: " + randomToCrit.ToString() + ")");
                if (isCriticalHit == true)
                {
                    log.Add("Critical damage range: " + lowDamage.ToString() + "-" + highDamage.ToString() + ", (dice roll: " + damageRollPercent + ")");
                }
            }
            else
            {
                log.Add("Critical chance: 0, hunkered down");
            }

            //process damage
            int damageDealt = RandomNumber.ScaleRandomNumber(
                        lowDamage, highDamage,
                        damageRollPercent);

            int armorPiercing = EncounterCore.ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityType.ArmorPiercing);
            if (armorPiercing > 0)
            {
                log.Add("Armor was ignored due to 'armor piercing' ability");
            }
            else
            {
                damageDealt -= targetCharacter.ArmorPoints;
            }
            //If the armor points are higher than the damage, we have -ve damage, we don't want to heal characters, set to 0
            if (damageDealt < 0)
            {
                damageDealt = 0;
            }
            targetCharacter.Hitpoints -= damageDealt;

            //Process armor shredding
            int armorShredderDamage = EncounterCore.ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityType.ArmorShredding);
            targetCharacter.ArmorPoints -= armorShredderDamage;

            //Update log
            if (armorShredderDamage > 0)
            {
                log.Add(armorShredderDamage.ToString() + " armor points shredded");
            }
            if (targetCharacter.ArmorPoints > 0 && armorPiercing == 0)
            {
                log.Add("Armor prevented " + targetCharacter.ArmorPoints.ToString() + " damage to character " + targetCharacter.Name);
            }
            log.Add(damageDealt.ToString() + " damage dealt to character " + targetCharacter.Name + ", HP is now " + targetCharacter.Hitpoints.ToString());

            //process experience
            int xp;
            if (targetCharacter.Hitpoints <= 0)
            {
                log.Add(targetCharacter.Name + " is killed");
                xp = Experience.GetExperience(true, true);
            }
            else
            {
                xp = Experience.GetExperience(true);
            }
            sourceCharacter.Experience += xp;
            log.Add(xp.ToString() + " XP added to character " + sourceCharacter.Name + ", for a total of " + sourceCharacter.Experience + " XP");

            EncounterResult result = new()
            {
                SourceCharacter = sourceCharacter,
                TargetCharacter = targetCharacter,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit,
                Log = log
            };
            return result;
        }
    }
}
