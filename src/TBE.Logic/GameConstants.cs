namespace TBE.Logic
{
    /// <summary>
    /// Centralized game constants to avoid magic numbers throughout the codebase
    /// </summary>
    public static class GameConstants
    {
        #region Character States and Targeting

        /// <summary>
        /// Indicates no target is currently selected
        /// </summary>
        public const int NO_TARGET_SELECTED = -1;

        /// <summary>
        /// Index of the first target in a list
        /// </summary>
        public const int FIRST_TARGET_INDEX = 0;

        /// <summary>
        /// Character is dead when hitpoints reach this value
        /// </summary>
        public const int DEAD_HITPOINTS = 0;

        /// <summary>
        /// Minimum hitpoints to be considered alive
        /// </summary>
        public const int MINIMUM_ALIVE_HITPOINTS = 1;

        #endregion

        #region Map Markers

        /// <summary>
        /// Map marker for player/character position
        /// </summary>
        public const string PLAYER_MAP_MARKER = "P";

        /// <summary>
        /// Empty tile marker on the map
        /// </summary>
        public const string EMPTY_TILE = "";

        #endregion

        #region Action Points

        /// <summary>
        /// Action points consumed for a single move (within mobility range)
        /// </summary>
        public const int SINGLE_MOVE_ACTION_POINT_COST = 1;

        /// <summary>
        /// Action points consumed for a double move (beyond mobility range)
        /// </summary>
        public const int DOUBLE_MOVE_ACTION_POINT_COST = 2;

        /// <summary>
        /// Action points remaining after shooting/attacking
        /// </summary>
        public const int ACTION_POINTS_AFTER_ATTACK = 0;

        #endregion

        #region Percentages

        /// <summary>
        /// Minimum percentage value
        /// </summary>
        public const int PERCENTAGE_MIN = 0;

        /// <summary>
        /// Maximum percentage value
        /// </summary>
        public const int PERCENTAGE_MAX = 100;

        /// <summary>
        /// Percentage multiplier for scaling random numbers (0-100 scale)
        /// </summary>
        public const int PERCENTAGE_SCALE = 100;

        #endregion

        #region Cover System

        /// <summary>
        /// Hit chance penalty for half cover
        /// </summary>
        public const int HALF_COVER_PENALTY = 20;

        /// <summary>
        /// Hit chance penalty for full cover
        /// </summary>
        public const int FULL_COVER_PENALTY = 40;

        /// <summary>
        /// Multiplier for cover bonuses when hunkered down
        /// </summary>
        public const int HUNKERED_DOWN_MULTIPLIER = 2;

        /// <summary>
        /// Hit chance penalty for half cover when hunkered down (20 * 2)
        /// </summary>
        public const int HUNKERED_HALF_COVER_PENALTY = 40;

        /// <summary>
        /// Hit chance penalty for full cover when hunkered down (40 * 2)
        /// </summary>
        public const int HUNKERED_FULL_COVER_PENALTY = 80;

        #endregion

        #region Combat Mechanics

        /// <summary>
        /// Critical hit chance bonus when flanking an enemy
        /// </summary>
        public const int FLANKING_CRITICAL_BONUS = 50;

        /// <summary>
        /// Aim multiplier for overwatch shots (70% of normal aim)
        /// </summary>
        public const float OVERWATCH_AIM_MULTIPLIER = 0.7f;

        /// <summary>
        /// Critical chance for overwatch shots
        /// </summary>
        public const int OVERWATCH_CRITICAL_CHANCE = 0;

        /// <summary>
        /// Minimum damage after armor (prevent healing from negative damage)
        /// </summary>
        public const int MINIMUM_DAMAGE_AFTER_ARMOR = 0;

        #endregion

        #region AI Scoring

        /// <summary>
        /// Base AI score before modifiers
        /// </summary>
        public const int AI_BASE_SCORE = 0;

        /// <summary>
        /// AI score penalty for being flanked
        /// </summary>
        public const int AI_FLANKED_PENALTY = -5;

        /// <summary>
        /// AI score bonus for being in full cover
        /// </summary>
        public const int AI_FULL_COVER_BONUS = 8;

        /// <summary>
        /// AI score bonus for being in half cover
        /// </summary>
        public const int AI_HALF_COVER_BONUS = 4;

        /// <summary>
        /// AI score bonus for flanking an enemy
        /// </summary>
        public const int AI_FLANKING_ENEMY_BONUS = 5;

        /// <summary>
        /// AI score penalty for visible/dangerous tiles
        /// </summary>
        public const int AI_VISIBLE_TILE_PENALTY = -2;

        /// <summary>
        /// AI score bonus for single move
        /// </summary>
        public const int AI_SINGLE_MOVE_BONUS = 3;

        /// <summary>
        /// AI score bonus for double move
        /// </summary>
        public const int AI_DOUBLE_MOVE_BONUS = 2;

        /// <summary>
        /// AI score penalty when no characters in view
        /// </summary>
        public const int AI_NO_CHARACTERS_IN_VIEW_PENALTY = -2;

        /// <summary>
        /// Minimum AI score (normalized floor)
        /// </summary>
        public const int AI_MINIMUM_SCORE = 0;

        #endregion

        #region AI Hit Chance Scoring Thresholds

        /// <summary>
        /// Hit chance threshold for +5 AI score bonus
        /// </summary>
        public const int AI_HIT_CHANCE_EXCELLENT_THRESHOLD = 95;

        /// <summary>
        /// AI score bonus for excellent hit chance (>=95%)
        /// </summary>
        public const int AI_HIT_CHANCE_EXCELLENT_BONUS = 5;

        /// <summary>
        /// Hit chance threshold for +4 AI score bonus
        /// </summary>
        public const int AI_HIT_CHANCE_VERY_GOOD_THRESHOLD = 90;

        /// <summary>
        /// AI score bonus for very good hit chance (>=90%)
        /// </summary>
        public const int AI_HIT_CHANCE_VERY_GOOD_BONUS = 4;

        /// <summary>
        /// Hit chance threshold for +3 AI score bonus
        /// </summary>
        public const int AI_HIT_CHANCE_GOOD_THRESHOLD = 80;

        /// <summary>
        /// AI score bonus for good hit chance (>=80%)
        /// </summary>
        public const int AI_HIT_CHANCE_GOOD_BONUS = 3;

        /// <summary>
        /// Hit chance threshold for +2 AI score bonus
        /// </summary>
        public const int AI_HIT_CHANCE_DECENT_THRESHOLD = 65;

        /// <summary>
        /// AI score bonus for decent hit chance (>=65%)
        /// </summary>
        public const int AI_HIT_CHANCE_DECENT_BONUS = 2;

        /// <summary>
        /// Hit chance threshold for +1 AI score bonus
        /// </summary>
        public const int AI_HIT_CHANCE_FAIR_THRESHOLD = 50;

        /// <summary>
        /// AI score bonus for fair hit chance (>=50%)
        /// </summary>
        public const int AI_HIT_CHANCE_FAIR_BONUS = 1;

        /// <summary>
        /// AI score bonus for poor hit chance (<50%)
        /// </summary>
        public const int AI_HIT_CHANCE_POOR_BONUS = 0;

        #endregion

        #region Array and List Indexing

        /// <summary>
        /// First element index in arrays/lists
        /// </summary>
        public const int FIRST_INDEX = 0;

        /// <summary>
        /// Offset to get the last index from count
        /// </summary>
        public const int LAST_INDEX_OFFSET = 1;

        /// <summary>
        /// Offset to get previous index
        /// </summary>
        public const int PREVIOUS_INDEX_OFFSET = 1;

        /// <summary>
        /// Increment/decrement value for adjacent tiles
        /// </summary>
        public const int ADJACENT_TILE_OFFSET = 1;

        #endregion

        #region Map Dimensions

        /// <summary>
        /// Standard Y dimension for flat maps (single layer)
        /// </summary>
        public const int STANDARD_Y_DIMENSION = 1;

        /// <summary>
        /// X dimension index in 3D arrays
        /// </summary>
        public const int X_DIMENSION_INDEX = 0;

        /// <summary>
        /// Y dimension index in 3D arrays
        /// </summary>
        public const int Y_DIMENSION_INDEX = 1;

        /// <summary>
        /// Z dimension index in 3D arrays
        /// </summary>
        public const int Z_DIMENSION_INDEX = 2;

        #endregion

        #region Experience and Leveling

        /// <summary>
        /// Experience points awarded for no action/miss
        /// </summary>
        public const int XP_FOR_MISS = 0;

        /// <summary>
        /// Affected map value for tracking changes
        /// </summary>
        public const int AFFECTED_MAP_VALUE = 1;

        #endregion

        #region Movement Validation

        /// <summary>
        /// Indicates starting index for movement loops
        /// </summary>
        public const int MOVEMENT_START_INDEX = 1;

        /// <summary>
        /// Indicates first step in movement path
        /// </summary>
        public const int FIRST_MOVEMENT_STEP = 0;

        #endregion

        #region String Manipulation

        /// <summary>
        /// Offset for substring operations to skip first character
        /// </summary>
        public const int SUBSTRING_SKIP_FIRST_CHAR = 1;

        /// <summary>
        /// Number of characters to remove from end (", ")
        /// </summary>
        public const int SUBSTRING_REMOVE_TRAILING_COMMA = 3;

        #endregion

        #region Random Number Generation

        /// <summary>
        /// Queue threshold for regenerating random numbers
        /// </summary>
        public const int RANDOM_QUEUE_THRESHOLD = 100;

        /// <summary>
        /// Batch size for random number generation
        /// </summary>
        public const int RANDOM_BATCH_SIZE = 100;

        /// <summary>
        /// Minimum random number range for map generation
        /// </summary>
        public const int MAP_GEN_RANDOM_MIN = 1;

        /// <summary>
        /// Maximum random number range for map generation
        /// </summary>
        public const int MAP_GEN_RANDOM_MAX = 100;

        #endregion

        #region Coordinate Constants

        /// <summary>
        /// Y coordinate for flat/2D maps (always 0)
        /// </summary>
        public const int FLAT_MAP_Y_COORDINATE = 0;

        #endregion

        #region Mathematical Constants

        /// <summary>
        /// Number of decimal places for rounding line length calculations
        /// </summary>
        public const int ONE_DECIMAL_PLACE = 1;

        /// <summary>
        /// Power of 2 for distance calculations (squared)
        /// </summary>
        public const int SQUARE_POWER = 2;

        #endregion
    }
}
