
namespace TBE.Logic.Research
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
                ScientistsAssigned = 0,
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
                ScientistsAssigned = 2,
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
                ScientistsAssigned = 0,
                IsComplete = false
            };
        }

    }
}