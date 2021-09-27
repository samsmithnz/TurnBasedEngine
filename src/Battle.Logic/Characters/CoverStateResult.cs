namespace Battle.Logic.Characters
{
    public class CoverStateResult
    {
        public bool InFullCover { get; set; }
        public bool InHalfCover { get; set; }        

        public bool InNorthFullCover { get; set; }
        public bool InEastFullCover { get; set; }
        public bool InSouthFullCover { get; set; }
        public bool InWestFullCover { get; set; }
        public bool InNorthHalfCover { get; set; }
        public bool InEastHalfCover { get; set; }
        public bool InSouthHalfCover { get; set; }
        public bool InWestHalfCover { get; set; }
    }
}
