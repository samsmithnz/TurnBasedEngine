namespace Battle.Logic.Characters
{
    public static class Experience
    {
        //Experience
        // 0xp for miss
        // 10xp for hit
        // 100xp for kill

        //Rank       Enemy Unknown
        //1. Rookie                 0
        //2. Squaddie             100	        
        //3. Corporal             500	        
        //4. Sergeant            1000	        
        //5. Lieutenant          1500	        
        //6. Captain             2000	        
        //7. Major               2500	        
        //8. Colonel             3000	        
        private const int Level2ExperienceNeeded = 100;
        private const int Level3ExperienceNeeded = 500;
        private const int Level4ExperienceNeeded = 1000;
        private const int Level5ExperienceNeeded = 1500;
        private const int Level6ExperienceNeeded = 2000;
        private const int Level7ExperienceNeeded = 2500;
        private const int Level8ExperienceNeeded = 3000;

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
                    if (experience >= Level2ExperienceNeeded)
                    {
                        result = true;
                    }
                    break;
                case 2:
                    if (experience >= Level3ExperienceNeeded)
                    {
                        result = true;
                    }
                    break;
                case 3:
                    if (experience >= Level4ExperienceNeeded)
                    {
                        result = true;
                    }
                    break;
                case 4:
                    if (experience >= Level5ExperienceNeeded)
                    {
                        result = true;
                    }
                    break;
                case 5:
                    if (experience >= Level6ExperienceNeeded)
                    {
                        result = true;
                    }
                    break;
                case 6:
                    if (experience >= Level7ExperienceNeeded)
                    {
                        result = true;
                    }
                    break;
                case 7:
                    if (experience >= Level8ExperienceNeeded)
                    {
                        result = true;
                    }
                    break;

            }
            return result;
        }
    }
}
