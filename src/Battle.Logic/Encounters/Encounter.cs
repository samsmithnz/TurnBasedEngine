using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Items;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Encounters
{
    public static class Encounter
    {
        public static EncounterResult AttackCharacterWithAreaOfEffect(Character sourceCharacter, Weapon weapon, List<Character> allCharacters, string[,,] map, Queue<int> diceRolls, Vector3 throwingTargetLocation)
        {
            int damageDealt;
            bool isCriticalHit = false;
            List<string> log = new List<string>();

            if (diceRolls == null || diceRolls.Count == 0 || weapon == null || weapon.AmmoCurrent <= 0)
            {
                return null;
            }
            log.Add(sourceCharacter.Name + " is attacking with area effect " + weapon.Name + " aimed at " + throwingTargetLocation.ToString());

            //Get the targets in the area affected
            List<Character> areaEffectTargets = FieldOfView.GetCharactersInArea(allCharacters, map, throwingTargetLocation, weapon.AreaEffectRadius);
            StringBuilder names = new StringBuilder();
            foreach (Character item in areaEffectTargets)
            {
                names.Append(' ');
                names.Append(item.Name);
                names.Append(", ");
            }
            log.Add("Characters in affected area: " + names.ToString().Substring(1, names.ToString().Length - 3));//remove the first " " and last two characters: ", "

            //Deal damage to each target
            int totalDamageDealt = 0;
            int totalArmorAbsorbed = 0;
            int totalArmorShredded = 0;
            foreach (Character targetCharacter in areaEffectTargets)
            {
                EncounterResult tempResult = ProcessCharacterDamageAndExperience(sourceCharacter, weapon, targetCharacter, map, diceRolls, log, true);
                sourceCharacter = tempResult.SourceCharacter;
                damageDealt = tempResult.DamageDealt;
                totalDamageDealt += damageDealt;
                totalArmorAbsorbed += tempResult.ArmorAbsorbed;
                totalArmorShredded += tempResult.ArmorShredded;
                isCriticalHit = tempResult.IsCriticalHit;
                log = tempResult.Log;
            }

            //Remove cover
            List<Vector3> area = MapCore.GetMapArea(map, throwingTargetLocation, weapon.AreaEffectRadius, false, true);
            foreach (Vector3 item in area)
            {
                switch (map[(int)item.X, (int)item.Y, (int)item.Z])
                {
                    //Full cover becomes low cover
                    case CoverType.FullCover:
                        map[(int)item.X, (int)item.Y, (int)item.Z] = CoverType.HalfCover;
                        log.Add("High cover downgraded to low cover at " + item.ToString());
                        break;
                    //Low cover becomes no cover
                    case CoverType.HalfCover:
                        map[(int)item.X, (int)item.Y, (int)item.Z] = CoverType.NoCover;
                        log.Add("Low cover downgraded to no cover at " + item.ToString());
                        break;
                }
            }

            //Consume source characters action points
            sourceCharacter.ActionPointsCurrent = 0;

            //Check if the character has enough experience to level up
            sourceCharacter.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(sourceCharacter.Level, sourceCharacter.Experience);

            EncounterResult result = new EncounterResult()
            {
                SourceCharacter = sourceCharacter,
                AllCharacters = allCharacters,
                ArmorAbsorbed = totalArmorAbsorbed,
                ArmorShredded = totalArmorShredded,
                DamageDealt = totalDamageDealt,
                IsCriticalHit = isCriticalHit,
                Log = log
            };
            return result;
        }

        public static EncounterResult AttackCharacter(Character sourceCharacter, Weapon weapon, Character targetCharacter, string[,,] map, Queue<int> diceRolls)
        {
            if (diceRolls == null || diceRolls.Count == 0)
            {
                return null;
            }

            int armorAbsorbed = 0;
            int armorShredded = 0;
            int damageDealt = 0;
            bool isCriticalHit = false;
            List<string> log = new List<string>
            {
                sourceCharacter.Name + " is attacking with " + weapon.Name + ", targeted on " + targetCharacter.Name.ToString()
            };

            //Don't attack if the clip is empty
            if (weapon.AmmoCurrent > 0)
            {
                sourceCharacter.TotalShots++;
                int toHitPercent = EncounterCore.GetChanceToHit(sourceCharacter, weapon, targetCharacter);

                //If the number rolled is higher than the chance to hit, the attack was successful!
                int randomToHit = diceRolls.Dequeue();

                if ((100 - toHitPercent) <= randomToHit)
                {
                    log.Add("Hit: Chance to hit: " + toHitPercent.ToString() + ", (dice roll: " + randomToHit.ToString() + ")");

                    EncounterResult tempResult = ProcessCharacterDamageAndExperience(sourceCharacter, weapon, targetCharacter, map, diceRolls, log, false);
                    sourceCharacter = tempResult.SourceCharacter;
                    targetCharacter = tempResult.TargetCharacter;
                    armorAbsorbed = tempResult.ArmorAbsorbed;
                    armorShredded = tempResult.ArmorShredded;
                    damageDealt = tempResult.DamageDealt;
                    isCriticalHit = tempResult.IsCriticalHit;
                    log = tempResult.Log;
                }
                else
                {
                    log.Add("Missed: Chance to hit: " + toHitPercent.ToString() + ", (dice roll: " + randomToHit.ToString() + ")");

                    //Work out where the shot goes
                    //get the percentage miss
                    //Randomize x,y,z coordinates.
                    //Aim and shoot at that new target and see if we hit anything
                    //do this by doubling the lines. 
                    Vector3 missedLocation = FieldOfView.MissedShot(sourceCharacter.Location, targetCharacter.Location, map);

                    //Remove cover at this location
                    if (map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z] != "")
                    {
                        switch (map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z])
                        {
                            //Full cover becomes low cover
                            case CoverType.FullCover:
                                map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z] = CoverType.HalfCover;
                                log.Add("High cover downgraded to low cover at " + missedLocation.ToString());
                                break;
                            //Low cover becomes no cover
                            case CoverType.HalfCover:
                                map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z] = CoverType.NoCover;
                                log.Add("Low cover downgraded to no cover at " + missedLocation.ToString());
                                break;
                        }
                        map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z] = "";
                    }

                    int xp = Experience.GetExperience(false);
                    sourceCharacter.Experience += xp;
                    log.Add(xp.ToString() + " XP added to character " + sourceCharacter.Name + ", for a total of " + sourceCharacter.Experience + " XP");
                }

                //Consume source characters action points
                sourceCharacter.ActionPointsCurrent = 0;
                //Consume weapon ammo
                weapon.AmmoCurrent--;

                //Check if the character has enough experience to level up
                sourceCharacter.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(sourceCharacter.Level, sourceCharacter.Experience);
                if (sourceCharacter.LevelUpIsReady == true)
                {
                    log.Add(sourceCharacter.Name + " is ready to level up");
                }
            }
            else
            {
                log.Add(weapon.Name + " has no ammo remaining and the attack cannot be completed");
            }

            EncounterResult result = new EncounterResult()
            {
                SourceCharacter = sourceCharacter,
                TargetCharacter = targetCharacter,
                ArmorAbsorbed = armorAbsorbed,
                ArmorShredded = armorShredded,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit,
                Log = log
            };
            return result;
        }


        private static EncounterResult ProcessCharacterDamageAndExperience(Character sourceCharacter, Weapon weapon, Character targetCharacter, string[,,] map, Queue<int> diceRolls, List<string> log, bool isAreaEffectAttack)
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

            //Process armor shredding. This happens in addition to piercing, and therefore happens first
            int armorShredded = EncounterCore.ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityType.ArmorShredding);
            targetCharacter.ArmorPointsCurrent -= armorShredded;

            //Now process any other damage with armor
            int armorAbsorbed = 0;
            int armorPiercing = EncounterCore.ProcessAbilitiesByType(sourceCharacter.Abilities, AbilityType.ArmorPiercing);
            if (armorPiercing > 0)
            {
                log.Add("Armor was ignored due to 'armor piercing' ability");
            }
            else
            {
                int damageAfterArmor = damageDealt - targetCharacter.ArmorPointsCurrent;
                //If the armor points are higher than the damage, we have -ve damage, we don't want to heal characters, set to 
                if (damageAfterArmor < 0)
                {
                    damageAfterArmor = 0;
                }
                armorAbsorbed = damageDealt - damageAfterArmor;
                damageDealt = damageAfterArmor;
            }

            //Apply any damage
            targetCharacter.HitpointsCurrent -= damageDealt;
            sourceCharacter.TotalDamage += damageDealt;

            //Update log
            if (armorShredded > 0)
            {
                log.Add(armorShredded.ToString() + " armor points shredded");
            }
            if (armorAbsorbed > 0)
            {
                log.Add("Armor prevented " + armorAbsorbed.ToString() + " damage to character " + targetCharacter.Name);
            }
            log.Add(damageDealt.ToString() + " damage dealt to character " + targetCharacter.Name + ", HP is now " + targetCharacter.HitpointsCurrent.ToString());

            //process experience
            int xp;
            if (targetCharacter.HitpointsCurrent <= 0)
            {
                log.Add(targetCharacter.Name + " is killed");
                xp = Experience.GetExperience(true, true);
                sourceCharacter.TotalKills++;
            }
            else
            {
                xp = Experience.GetExperience(true);
            }
            sourceCharacter.Experience += xp;
            log.Add(xp.ToString() + " XP added to character " + sourceCharacter.Name + ", for a total of " + sourceCharacter.Experience + " XP");
            sourceCharacter.TotalHits++;

            EncounterResult result = new EncounterResult()
            {
                SourceCharacter = sourceCharacter,
                TargetCharacter = targetCharacter,
                ArmorAbsorbed = armorAbsorbed,
                ArmorShredded = armorShredded,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit,
                Log = log
            };
            return result;
        }
    }
}
