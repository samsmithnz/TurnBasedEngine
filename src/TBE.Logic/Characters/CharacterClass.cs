namespace TBE.Logic.Characters
{
    /// <summary>
    /// Represents the combat class specialization of a character
    /// </summary>
    public enum CharacterClass
    {
        /// <summary>
        /// Unspecialized new recruit, not yet assigned a class
        /// </summary>
        Recruit = 0,

        /// <summary>
        /// Long-range specialist with high accuracy at distance
        /// </summary>
        Sniper = 1,

        /// <summary>
        /// Close-range specialist focused on aggressive tactics
        /// </summary>
        Assault = 2,

        /// <summary>
        /// Team-oriented specialist providing healing and utility
        /// </summary>
        Support = 3,

        /// <summary>
        /// Heavy weapons specialist with high damage output
        /// </summary>
        Heavy = 4
    }
}
