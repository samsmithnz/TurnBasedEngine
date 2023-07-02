namespace TBE.Logic.AbilitiesAndEffects
{
    public static class AbilityPool
    {
        public static Ability ArmorPiercingAbility()
        {
            return new Ability("Armor Piercing", AbilityType.ArmorPiercing, 10);
        }

        public static Ability BiggestBoomsAbility1()
        {
            return new Ability("Biggest Booms", AbilityType.CriticalDamage, 2);
        }

        public static Ability BiggestBoomsAbility2()
        {
            return new Ability("Biggest Booms", AbilityType.CriticalChance, 20);
        }

        public static Ability BringEmOnAbility()
        {
            return new Ability("Bring Em On", AbilityType.CriticalDamage, 3);
        }

        public static Ability OpportunistAbility()
        {
            return new Ability("Opportunist", Logic.AbilitiesAndEffects.AbilityType.OverwatchPenaltyRemoved, 1);
        }

        public static Ability PlatformStabilityAbility()
        {
            return new Ability("Platform Stability", AbilityType.CriticalChance, 10);
        }

        public static Ability SharpShooterAbility()
        {
            return new Ability("Sharp Shooter", AbilityType.Damage, 10);
        }

        public static Ability ShredderAbility()
        {
            return new Ability("Shredder", AbilityType.ArmorShredding, 2);
        }

    }
}
