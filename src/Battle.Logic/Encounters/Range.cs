using Battle.Logic.Items;
using System.Numerics;

namespace Battle.Logic.Encounters
{

    //Largely sourced from here: https://www.ufopaedia.org/index.php/Chance_to_Hit_(EU2012)#Weapon_Range
    //But the formulas were overly complicated, so I just dumped the numbers since there is less than a dozen options per gun
    public static class Range
    {
        public static int GetDistance(Vector3 sourceLocation, Vector3 targetLocation)
        {
            int distance = (int)Vector3.Distance(sourceLocation, targetLocation);
            return distance;
        }

        public static int GetRangeModifier(Weapon weapon, int distance)
        {
            switch (weapon.Type)
            {
                case WeaponType.Standard:
                    return GetStandardWeaponRangeModifier(distance);
                case WeaponType.Shotgun:
                    return GetShotgunWeaponRangeModifier(distance);
                case WeaponType.SniperRifle:
                    return GetSniperWeaponRangeModifier(distance);
                default:
                    return 0;
            }

        }

        private static int GetStandardWeaponRangeModifier(int distance)//, bool isDiagonalDirection)
        {
            switch (distance)
            {
                case 1: return 37;
                case 2: return 33;
                case 3: return 28;
                case 4: return 24;
                case 5: return 19;
                case 6: return 15;
                case 7: return 10;
                case 8: return 6;
                case 9: return 1;
                default: return 0;
            }
        }

        private static int GetShotgunWeaponRangeModifier(int distance)
        {
            switch (distance)
            {
                case 1: return 52;
                case 2: return 44;
                case 3: return 40;
                case 4: return 36;
                case 5: return 28;
                case 6: return 24;
                case 7: return 16;
                case 8: return 12;
                case 9: return 4;
                case 10: return 0;
                case 11: return -8;
                case 12: return -12;
                case 13: return -20;
                case 14: return -24;
                case 15: return -32;
                case 16: return -40;
                case 17: return -40;
                default: return 0;
            }
        }

        private static int GetSniperWeaponRangeModifier(int distance)
        {
            switch (distance)
            {
                case 1: return -24;
                case 2: return -21;
                case 3: return -19;
                case 4: return -16;
                case 5: return -13;
                case 6: return -10;
                case 7: return -7;
                case 8: return -4;
                case 9: return -1;
                default: return 0;
            }
        }
    }
}
