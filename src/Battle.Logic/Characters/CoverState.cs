namespace Battle.Logic.Characters
{
    public class CoverState
    {
        public bool InFullCover { get; set; }
        public bool InHalfCover { get; set; }
        private bool _isInFullCover;
        public bool IsFlanked
        {
            get
            {
                return _isInFullCover;
            }
            set
            {
                _isInFullCover = true;
                //If the player is flanked, ignore all cover bonuses
                if (_isInFullCover)
                {
                    InFullCover = false;
                    InHalfCover = false;
                    InNorthFullCover = false;
                    InEastFullCover = false;
                    InSouthFullCover = false;
                    InWestFullCover = false;
                    InNorthHalfCover = false;
                    InEastHalfCover = false;
                    InSouthHalfCover = false;
                    InWestHalfCover = false;
                }
            }
        }

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
