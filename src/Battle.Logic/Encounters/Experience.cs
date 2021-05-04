using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle.Logic.Encounters
{
    public class Experience
    {
        //Experience
        // 10xp for hit
        // 0xp for miss (TODO: consider this - is this fair? Don't you still learn when you fail?)
        // 100xp for kill

        //XCOM 1 uses experience: https://xcom.fandom.com/wiki/Soldier_(XCOM:_Enemy_Unknown)#Ranks_and_Experience
        //Rank       Enemy Unknown  Enemy Within
        //Squaddie              90	          90
        //Corporal             300	         480
        //Sergeant             510	         700
        //Lieutenant           745	         925
        //Captain             1100	        1380
        //Major               1560	        1840
        //Colonel             2150	        2550
        private const int Level1ExperienceUp = 90;
        private const int Level2ExperienceUp = 300;
        //private const int Level3ExperienceUp = 510;
        //private const int Level4ExperienceUp = 745;
        //private const int Level5ExperienceUp = 1100;
        //private const int Level6ExperienceUp = 1560;
        //private const int Level7ExperienceUp = 2150;

        private const int SuccessfulAction = 10;
        private const int KillXP = 100;

        //XCOM 2 uses a kills experience system: https://gaming.stackexchange.com/questions/255057/how-can-i-tell-how-many-kills-my-soldier-is-away-from-promotion

        public static int GetExperience(bool successfulAction, bool successfulKill = false)
        {
            //hit
            if (successfulAction == true)
            {
                if (successfulKill == true)
                {
                    return KillXP;
                }
                else
                {
                    return SuccessfulAction;
                }
            }
            else
            {
                return 0;
            }
        }

        public static bool CheckIfReadyToLevelUp(int level, int experience)
        {
            bool result = false;
            switch (level)
            {
                case 1:
                    if (experience >= Level1ExperienceUp)
                    {
                        result= true;
                    }
                    break;
                case 2:
                    if (experience >= Level2ExperienceUp)
                    {
                        result = true;
                    }
                    break;
                    //case 3:
                    //    if (experience >= 510)
                    //    {
                    //        result = true;
                    //    }
                    //    break;  
            }
            return result;
        }
    }
}
