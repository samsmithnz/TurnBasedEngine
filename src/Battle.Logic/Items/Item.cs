namespace Battle.Logic.Items
{
    public class Item
    {
        public string Name { get; set; }
        public int Adjustment { get; set; }
        public int Range { get; set; }
        public int ClipSize { get; set; }
        public int ClipRemaining { get; set; }
        public int ActionPointsRequired { get; set; }
        public ItemType Type { get; set; }

    }

    public enum ItemType
    {
        Unknown = 0,
        MedKit = 6
    }
}
