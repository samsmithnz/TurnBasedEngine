using Battle.Logic.Items;

namespace Battle.Logic.Research
{
    public class ResearchItem
    {
        public string Name { get; set; } 
        public ResearchItem ResearchPrereq { get; set; }
        public Item ItemPrereq { get; set; }
        public int DaysToComplete { get; set; }
        public int DaysCompleted { get; set; }
        public int ScientistsAssigned { get; set; } 
        public bool IsComplete { get; set; }  

    }
}
