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
        Available = 0,

        /// <summary>
        /// Character is currently on a mission
        /// </summary>
        OnMission = 1,

        /// <summary>
        /// Character is injured and recovering for a number of days
        /// </summary>
        Injured = 2,

        /// <summary>
        /// Character is in training for a number of days
        /// </summary>
        Training = 3,

        /// <summary>
        /// Character is killed in action (permanent)
        /// </summary>
        Deceased = 4
    }
}
