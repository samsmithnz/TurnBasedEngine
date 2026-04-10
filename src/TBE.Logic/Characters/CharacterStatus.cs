namespace TBE.Logic.Characters
{
    /// <summary>
    /// Represents the current availability status of a character
    /// </summary>
    public enum CharacterStatus
    {
        /// <summary>
        /// Character is available for missions
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Character is injured and recovering for a number of days
        /// </summary>
        Injured = 1,

        /// <summary>
        /// Character is unavailable for a number of days
        /// </summary>
        Unavailable = 2
    }
}
