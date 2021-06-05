using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Map;
using Battle.Logic.GameController;
using Battle.Logic.Items;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public class Character
    {
        public Character()
        {
            Abilities = new();
            Effects = new();
        }

        public string Name { get; set; }
        public int HitpointsMax { get; set; }
        public int HitpointsCurrent { get; set; }
        public int ArmorPointsMax { get; set; }
        public int ArmorPointsCurrent { get; set; }
        public int ActionPointsMax { get; set; }
        public int ActionPointsCurrent { get; set; }

        public int ChanceToHit { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public bool LevelUpIsReady { get; set; }
        public int Speed { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Effect> Effects { get; set; }
        public Vector3 Location { get; set; }
        public int MobilityRange { get; set; }
        public int ShootingRange { get; set; }
        public Weapon WeaponEquipped { get; set; }
        public Weapon UtilityWeaponEquipped { get; set; }
        public Item UtilityItemEquipped { get; set; }
        public bool InHalfCover { get; set; }
        public bool InFullCover { get; set; }
        public bool InOverwatch { get; set; }
        public bool HunkeredDown { get; set; }

        //Records & statistics
        public int MissionsCompleted { get; set; }
        //public int DaysWounded { get; set; }
        public int TotalKills { get; set; }
        public int TotalShots { get; set; }
        public int TotalHits { get; set; }
        public int TotalDamage { get; set; }

        public void ProcessEffects(int currentTurn)
        {
            List<int> itemIndexesToRemove = new();
            for (int i = 0; i < Effects.Count; i++)
            {
                Effect effect = Effects[i];
                //If the effect is expiring this turn ,remove it
                if (effect.TurnExpiration <= currentTurn)
                {
                    itemIndexesToRemove.Add(i);
                }
                else //the effect is still active, process it
                {
                    switch (effect.Type)
                    {
                        case AbilityType.FireDamage:
                            HitpointsCurrent -= effect.Adjustment;
                            break;
                    }
                }
            }
            for (int i = itemIndexesToRemove.Count - 1; i >= 0; i--)
            {
                Effects.RemoveAt(i);
            }
        }

        public List<CharacterAction> GetCurrentActions()
        {
            List<CharacterAction> options = new();
            if (ActionPointsCurrent > 0)
            {
                if (WeaponEquipped.AmmoCurrent > 0)
                {
                    options.Add(new() { Name = "_shoot", Caption = "Shoot", KeyBinding = "1" });
                    options.Add(new() { Name = "_overwatch", Caption = "Overwatch", KeyBinding = "2" });
                }
                else
                {
                    options.Add(new() { Name = "_reload", Caption = "Reload", KeyBinding = "1" });
                }
                if (UtilityWeaponEquipped != null)
                {
                    options.Add(new() { Name = "_throw_grenade", Caption = "Throw grenade", KeyBinding = "3" });
                }
                if (UtilityItemEquipped != null && UtilityItemEquipped.Type == ItemType.MedKit && UtilityItemEquipped.ClipRemaining > 0)
                {
                    options.Add(new() { Name = "_use_medkit", Caption = "Use medkit", KeyBinding = "4" });
                }

                options.Add(new() { Name = "_hunker_down", Caption = "Hunker down", KeyBinding = "5" });
            }

            return options;
        }

        public void UseItem(Item item)
        {
            if (item != null && item.Type == ItemType.MedKit && item.ClipRemaining > 0)
            {
                HitpointsCurrent += item.Adjustment;
                item.ClipRemaining -= 1;
            }
        }

        public List<Character> GetCharactersInView(string[,,] map, List<Team> teams)
        {
            List<Character> results = new();

            List<Vector3> fovVectors = FieldOfView.GetFieldOfView(map, Location, ShootingRange);
            foreach (Team team in teams)
            {
                foreach (Character character in team.Characters)
                {
                    bool addedCharacter = false;
                    foreach (Vector3 location in fovVectors)
                    {
                        if (character.Location == location)
                        {
                            addedCharacter = true;
                            results.Add(character);
                            break;
                        }
                    }
                    if (addedCharacter == false && LocationIsAdjacentToList(character.Location, fovVectors) == true)
                    {
                        results.Add(character);
                    }
                }
            }

            return results;
        }

        public string GetCharactersInViewMapString(string[,,] map, List<Team> teams)
        {
            List<Vector3> fov = FieldOfView.GetFieldOfView(map, Location, ShootingRange);
            string[,,] mapFov = MapCore.ApplyListToMap((string[,,])map.Clone(), fov, "o");
            mapFov[(int)Location.X, (int)Location.Y, (int)Location.Z] = "P";
            foreach (Team team in teams)
            {
                foreach (Character character in team.Characters)
                {
                    mapFov[(int)character.Location.X, (int)character.Location.Y, (int)character.Location.Z] = "P";
                }
            }
            string mapString = MapCore.GetMapString(mapFov);

            return mapString;
        }

        private static bool LocationIsAdjacentToList(Vector3 location, List<Vector3> list)
        {
            foreach (Vector3 item in list)
            {
                //if (item.X - 1 == location.X && item.Z == location.Z)
                //{
                //    return true;
                //}
                //else 
                if (item.X + 1 == location.X && item.Z == location.Z)
                {
                    return true;
                }
                //else if (item.X == location.X && item.Z - 1 == location.Z)
                //{
                //    return true;
                //}
                else if (item.X == location.X && item.Z + 1 == location.Z)
                {
                    return true;
                }
            }
            return false;
        }

        public bool LevelUpCharacter()
        {
            if (LevelUpIsReady == true)
            {
                LevelUpIsReady = false;
                HitpointsCurrent++;
                HitpointsMax++;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
