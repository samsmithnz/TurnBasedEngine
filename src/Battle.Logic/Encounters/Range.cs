using Battle.Logic.Weapons;
using System.Numerics;

namespace Battle.Logic.Encounters
{

    //Largely sourced from here: https://www.ufopaedia.org/index.php/Chance_to_Hit_(EU2012)#Weapon_Range
//But couldn't get the formulas to work, so just dumped the numbers since there is less than a dozen options per gun
    public class Range
    {
        public static int GetDistance(Vector3 sourceLocation, Vector3 targetLocation)
        {
            return (int)Vector3.Distance(sourceLocation, targetLocation);
        }

        public static int GetRangeModifier(Weapon weapon,int distance)
        {
            if (weapon.Type == WeaponEnum.Standard)
            {
                //Straight line
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
            else
            {
                return 0;
            }
        }
    }
}
