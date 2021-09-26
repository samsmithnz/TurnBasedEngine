using Battle.Logic.Map;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public static class CharacterCover
    {
        /// <summary>
        /// Calculate if the player is in cover. 
        /// </summary>
        /// <returns>True if the player is in cover</returns>
        public static CoverStateResult CalculateCover(string[,,] map, Vector3 defenderPosition, List<Vector3> attackerLocations)
        {
            CoverStateResult result = new CoverStateResult();
            List<Vector3> coverTiles = FindAdjacentCover(map, defenderPosition);
            int coverLineNorth = -1;
            int coverLineEast = -1;
            int coverLineSouth = -1;
            int coverLineWest = -1;
            bool currentLocationIsFlanked = false;

            if (coverTiles.Count == 0)
            {
                result.IsInCover = false;
                return result;
            }
            else
            {
                // Work out where the cover is relative to the player
                foreach (Vector3 coverTileItem in coverTiles)
                {
                    if (defenderPosition.X < coverTileItem.X)
                    {
                        result.InEastCover = true;
                        coverLineEast = Convert.ToInt32(coverTileItem.X) - 0;
                    }
                    if (defenderPosition.X > coverTileItem.X)
                    {
                        result.InWestCover = true;
                        coverLineWest = Convert.ToInt32(coverTileItem.X) + 0;
                    }
                    if (defenderPosition.Z < coverTileItem.Z)
                    {
                        result.InNorthCover = true;
                        coverLineNorth = Convert.ToInt32(coverTileItem.Z) - 0;
                    }
                    if (defenderPosition.Z > coverTileItem.Z)
                    {
                        result.InSouthCover = true;
                        coverLineSouth = Convert.ToInt32(coverTileItem.Z) + 0;
                    }
                }
            }

            if (attackerLocations == null || attackerLocations.Count == 0)
            {
                result.IsInCover = true;
                return result;
            }
            else
            {
                //Work out where the enemy is relative to the cover
                foreach (Vector3 enemyItem in attackerLocations)
                {
                    //Now check over all of the levels of cover

                    //Enemy is located NorthEast
                    if (enemyItem.Z >= defenderPosition.Z && enemyItem.X >= defenderPosition.X)
                    {
                        if (result.InNorthCover == false && result.InEastCover == false) //No cover in North or East = always flanked by Northeast Enenmy
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                        else if (result.InNorthCover == true && enemyItem.Z <= coverLineNorth && result.InEastCover == false) //There is cover in the North, but the enemy is past it + no East cover
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                        else if (result.InEastCover == true && enemyItem.X <= coverLineEast && result.InNorthCover == false) //There is cover in the East, but the enemy is past it + no North cover
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                    }

                    //Enemy is located NorthWest
                    if (enemyItem.Z >= defenderPosition.Z && enemyItem.X <= defenderPosition.X)
                    {
                        if (result.InNorthCover == false && result.InWestCover == false)
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                        else if (result.InNorthCover == true && enemyItem.Z <= coverLineNorth && result.InWestCover == false)
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                        else if (result.InWestCover == true && enemyItem.X >= coverLineWest && result.InNorthCover == false)
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                    }

                    //Enemy is located SouthEast
                    if (enemyItem.Z <= defenderPosition.Z && enemyItem.X >= defenderPosition.X)
                    {
                        if (result.InSouthCover == false && result.InEastCover == false)
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                        else if (result.InSouthCover == true && enemyItem.Z >= coverLineSouth && result.InEastCover == false)
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                        else if (result.InEastCover == true && enemyItem.X <= coverLineEast && result.InSouthCover == false)
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                    }

                    //Enemy is located SouthWest
                    if (enemyItem.Z <= defenderPosition.Z && enemyItem.X <= defenderPosition.X)
                    {
                        if (result.InSouthCover == false && result.InWestCover == false)
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                        else if (result.InSouthCover == true && enemyItem.Z >= coverLineSouth && result.InWestCover == false)
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                        else if (result.InWestCover == true && enemyItem.X >= coverLineWest && result.InSouthCover == false)
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                    }

                }

                result.IsInCover = !currentLocationIsFlanked;
                return result;
            }
        }

        /// <summary>
        /// Look at adjacent squares for cover
        /// </summary>
        /// <returns>A List of Vector3 objects for each item of cover</returns>
        private static List<Vector3> FindAdjacentCover(string[,,] map, Vector3 currentLocation)
        {
            int width = map.GetLength(0);
            //int height = map.GetLength(1);
            int breadth = map.GetLength(2);
            List<Vector3> result = new List<Vector3>();
            //if (currentLocation.X > width - 1 || currentLocation.Z > breadth - 1)
            //{
            //    throw new Exception("The character is off the map");
            //}

            //Make adjustments to ensure that the search doesn't go off the edges of the map
            int xMin = Convert.ToInt32(currentLocation.X) - 1;
            if (xMin < 0)
            {
                xMin = 0;
            }
            int xMax = Convert.ToInt32(currentLocation.X) + 1;
            if (xMax > width - 1)
            {
                xMax = width - 1;
            }
            int zMin = Convert.ToInt32(currentLocation.Z) - 1;
            if (zMin < 0)
            {
                zMin = 0;
            }
            int zMax = Convert.ToInt32(currentLocation.Z) + 1;
            if (zMax > breadth - 1)
            {
                zMax = breadth - 1;
            }

            //Get possible tiles, within constraints of map, including only square titles from current position (not diagonally)
            if (map[Convert.ToInt32(currentLocation.X), 0, zMax] == CoverType.FullCover || map[Convert.ToInt32(currentLocation.X), 0, zMax] == CoverType.HalfCover)
            {
                result.Add(new Vector3(currentLocation.X, 0f, zMax));
            }
            if (map[xMax, 0, Convert.ToInt32(currentLocation.Z)] == CoverType.FullCover || map[xMax, 0, Convert.ToInt32(currentLocation.Z)] == CoverType.HalfCover)
            {
                result.Add(new Vector3(xMax, 0f, currentLocation.Z));
            }
            if (map[Convert.ToInt32(currentLocation.X), 0, zMin] == CoverType.FullCover || map[Convert.ToInt32(currentLocation.X), 0, zMin] == CoverType.HalfCover)
            {
                result.Add(new Vector3(currentLocation.X, 0f, zMin));
            }
            if (map[xMin, 0, Convert.ToInt32(currentLocation.Z)] == CoverType.FullCover || map[xMin, 0, Convert.ToInt32(currentLocation.Z)] == CoverType.HalfCover)
            {
                result.Add(new Vector3(xMin, 0f, currentLocation.Z));
            }
            return result;
        }
    }
}
