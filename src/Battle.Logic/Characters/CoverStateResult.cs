namespace Battle.Logic.Characters
{
    public class CoverStateResult
    {
        public bool IsInFullCover { get; set; }
        public bool IsInHalfCover { get; set; }        

        public bool InNorthCover { get; set; }
        public bool InEastCover { get; set; }
        public bool InSouthCover { get; set; }
        public bool InWestCover { get; set; }
    }
}
