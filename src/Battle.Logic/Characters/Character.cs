﻿using Battle.Logic.AbilitiesAndEffects;
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
        public int Range { get; set; }
        public Weapon WeaponEquipped { get; set; }
        public bool InHalfCover { get; set; }
        public bool InFullCover { get; set; }
        public bool InOverwatch { get; set; }

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
                        case AbilityTypeEnum.FireDamage:
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
    }
}
