using Battle.Logic.Weapons;
using System.Numerics;

namespace Battle.Logic.Encounters
{

    //Largely sourced from here: https://www.ufopaedia.org/index.php/Chance_to_Hit_(EU2012)#Weapon_Range
    //But couldn't get the formulas to work, so just dumped the numbers since there is less than a dozen options per gun
    public class Range
    {
        public static (int, bool) GetDistance(Vector3 sourceLocation, Vector3 targetLocation)
        {
            int distance = (int)Vector3.Distance(sourceLocation, targetLocation);
            bool isDiagonalDirection = true;
            if (sourceLocation.X == targetLocation.X | sourceLocation.Z == targetLocation.Z)
            {
                isDiagonalDirection = false;
            }
            return new(distance, isDiagonalDirection);
        }

        public static int GetRangeModifier(Weapon weapon, int distance, bool isDiagonalDirection)
        {
            switch (weapon.Type)
            {
                case WeaponEnum.Standard:
                    return GetStandardWeaponRange(distance, isDiagonalDirection);
                case WeaponEnum.Shotgun:
                    return GetShotgunWeaponRange(distance);
                case WeaponEnum.Sniper:
                    return GetSniperWeaponRange(distance);
                default:
                    return 0;
            }

        }

        private static int GetStandardWeaponRange(int distance, bool isDiagonalDirection)
        {
            //Diagonal line/direction
            if (isDiagonalDirection == true)
            {
                switch (distance)
                {
                    case 1: return 35;
                    case 2: return 29;
                    case 3: return 23;
                    case 4: return 16;
                    case 5: return 10;
                    case 6: return 4;
                    default: return 0;
                }
            }
            else //Straight line/direction
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
        }

        private static int GetShotgunWeaponRange(int distance)
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

        private static int GetSniperWeaponRange(int distance)
        {
            //1	-24
            //2	-21
            //3	-19
            //4	-16
            //5	-13
            //6	-10
            //7	-7
            //8	-4
            //9	-1
            //10	0
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
    }
}
