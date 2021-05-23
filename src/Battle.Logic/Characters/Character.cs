using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.FieldOfView;
using Battle.Logic.MainGame;
using Battle.Logic.Weapons;
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
        public int Hitpoints { get; set; }
        public int ArmorPoints { get; set; }
        public int ChanceToHit { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public bool LevelUpIsReady { get; set; }
        public int Speed { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Effect> Effects { get; set; }
        public Vector3 Location { get; set; }
        public int ActionPoints { get; set; }
        public int MovementRange { get; set; }
        public int ShootingRange { get; set; }
        public Weapon WeaponEquipped { get; set; }
        public Weapon UtilityItemEquipped { get; set; }
        public bool InHalfCover { get; set; }
        public bool InFullCover { get; set; }
        public bool InOverwatch { get; set; }
        public bool HunkeredDown { get; set; }

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
                            Hitpoints -= effect.Adjustment;
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
            if (ActionPoints > 0)
            {
                if (WeaponEquipped.ClipRemaining > 0)
                {
                    options.Add(new() { Name = "_shoot", Caption = "Shoot", KeyBinding = "1" });
                    options.Add(new() { Name = "_overwatch", Caption = "Overwatch", KeyBinding = "2" });
                }
                else
                {
                    options.Add(new() { Name = "_reload", Caption = "Reload", KeyBinding = "1" });
                }
                if (UtilityItemEquipped != null)
                {
                    options.Add(new() { Name = "_throw_grenade", Caption = "Throw grenade", KeyBinding = "3" });
                }
                options.Add(new() { Name = "_hunker_down", Caption = "Hunker down", KeyBinding = "4" });
            }

            return options;
        }

        public List<Character> GetCharactersInView(string[,] map, List<Team> teams)
        {
            List<Character> results = new();

            List<Vector3> fovVectors = FieldOfViewCalculator.GetFieldOfView(map, Location, ShootingRange);
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

        public string GetCharactersInViewMapString(string[,] map, List<Team> teams)
        {
            List<Vector3> fov = FieldOfViewCalculator.GetFieldOfView(map, Location, ShootingRange);
            string[,] mapFov = FieldOfViewCalculator.ApplyListToMap((string[,])map.Clone(), fov, "o");
            mapFov[(int)Location.X, (int)Location.Z] = "P";
            foreach (Team team in teams)
            {
                foreach (Character character in team.Characters)
                {
                    mapFov[(int)character.Location.X, (int)character.Location.Z] = "P";
                }
            }
            string mapString = FieldOfViewCalculator.GetMapString(mapFov, map.GetLength(0), map.GetLength(1));

            return mapString;
        }

        private static bool LocationIsAdjacentToList(Vector3 location, List<Vector3> list)
        {
            foreach (Vector3 item in list)
            {
                if (item.X - 1 == location.X && item.Z == location.Z)
                {
                    return true;
                }
                else if (item.X + 1 == location.X && item.Z == location.Z)
                {
                    return true;
                }
                else if (item.X == location.X && item.Z - 1 == location.Z)
                {
                    return true;
                }
                else if (item.X == location.X && item.Z + 1 == location.Z)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
