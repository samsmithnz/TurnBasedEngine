using System.Collections.Generic;

namespace Battle.Logic.Research
{
    public class ResearchController
    {
        public List<ResearchItem> ResearchItems { get; set; }

        public List<ResearchItem> GetAvailableResearchItems()
        {
            List<ResearchItem> filteredItems = new List<ResearchItem>();
            foreach (ResearchItem item in ResearchItems)
            {
                if (item.ResearchPrereq != null &&
                    item.ResearchPrereq.IsComplete &&
                    !item.IsComplete)
                {
                    filteredItems.Add(item);
                }
            }
            return filteredItems;
        }

        public List<ResearchItem> GetCompletedResearchItems()
        {
            List<ResearchItem> filteredItems = new List<ResearchItem>();
            foreach (ResearchItem item in ResearchItems)
            {
                if (item.IsComplete)
                {
                    filteredItems.Add(item);
                }
            }
            return filteredItems;
        }

        //public void LoadResearchItems(List<ResearchItem> researchItems)
        //{

        //}

    }
}
