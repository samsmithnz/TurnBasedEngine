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

        //Rank       Enemy Unknown  
        //Squaddie             100	        
        //Corporal             500	        
        //Sergeant            1000	        
        //Lieutenant          1500	        
        //Captain             2000	        
        //Major               2500	        
        //Colonel             3000	        
        private const int Level1ExperienceUp = 100;
        //private const int Level2ExperienceUp = 500;
        //private const int Level3ExperienceUp = 1000;
        //private const int Level4ExperienceUp = 1500;
        //private const int Level5ExperienceUp = 2000;
        //private const int Level6ExperienceUp = 2500;
        //private const int Level7ExperienceUp = 3000;

        private const int SuccessfulAction = 10;
        private const int KillXP = 100;

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
                //case 2:
                //    if (experience >= Level2ExperienceUp)
                //    {
                //        result = true;
                //    }
                //    break;
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
