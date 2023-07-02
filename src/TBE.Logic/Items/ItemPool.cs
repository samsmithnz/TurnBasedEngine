
namespace TBE.Logic.Items
{
    public static class ItemPool
    {
        public static Item CreateMedKit()
        {
            Item medKit = new Item()
            {
                Name = "MedKit",
                Adjustment = 3,
                Range = 1,
                ClipSize = 1,
                ClipRemaining = 1,
                ActionPointsRequired = 1,
                Type = ItemType.MedKit
            };
            return medKit;
        }

    }
}