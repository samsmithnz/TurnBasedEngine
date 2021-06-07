using Battle.Logic.Research;

namespace Battle.Tests.Research
{
    public static class ResearchPool
    {
        public static ResearchItem CreateAdvancedWeapons()
        {
            return new ResearchItem()
            {
                Name = "Advanced weapons",
                ResearchPrereq = null,
                ItemPrereq = null,
                DaysToComplete = 5,
                DaysCompleted = 5,
                IsComplete = true
            };
        }

        public static ResearchItem CreateLasers()
        {
            return new ResearchItem()
            {
                Name = "Laser weapons",
                ResearchPrereq = CreateAdvancedWeapons(),
                ItemPrereq = null,
                DaysToComplete = 5,
                DaysCompleted = 3,
                IsComplete = false
            };
        }

        public static ResearchItem CreatePlasma()
        {
            return new ResearchItem()
            {
                Name = "Plasma weapons",
                ResearchPrereq = CreateLasers(),
                ItemPrereq = null,
                DaysToComplete = 5,
                DaysCompleted = 0,
                IsComplete = false
            };
        }

    }
}