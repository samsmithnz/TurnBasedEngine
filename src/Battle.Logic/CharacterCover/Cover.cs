using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.CharacterCover
{
    public static class Cover
    {
        /// <summary>
        /// Calculate if the player is in cover. 
        /// </summary>
        /// <returns>True if the player is in cover</returns>
        public static CoverState CalculateCover(Vector3 defenderPosition, int width, int height, string[,] validTiles, List<Vector3> attackerLocations)
        {
            //TODO: Why is attackerpositions a list?
            CoverState result = new();
            List<Vector3> coverTiles = FindAdjacentCover(defenderPosition, width, height, validTiles);
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
                    //NOTE: I don't think I need this now that I have cover lines
                    //Check to see if Enemy is right on top of the player, neutralizing each others cover and causing a flank
                    int xPosition = Convert.ToInt32(defenderPosition.X - enemyItem.X);
                    if (xPosition < 0)
                    {
                        xPosition *= -1;
                    }
                    int zPosition = Convert.ToInt32(defenderPosition.Z - enemyItem.Z);
                    if (zPosition < 0)
                    {
                        zPosition *= -1;
                    }

                    //Now check over all of the levels of cover

                    //Enemy is located NorthEast
                    if (enemyItem.Z >= defenderPosition.Z && enemyItem.X >= defenderPosition.X)
                    {
                        if (result.InNorthCover == false && result.InEastCover == false) //No cover in North or East = always flanked by Northeast Enenmy
                        {
                            currentLocationIsFlanked = true;
                            break;
                        }
                        //else if (result.InNorthCover == true && enemyItem.Y < coverLineNorth && result.InEastCover == true && enemyItem.X < coverLineEast)
                        //{
                        //    currentLocationIsFlanked = true;
                        //    break;
                        //}
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
                        //else if (result.InNorthCover != true && enemyItem.Y <= coverLineNorth && result.InWestCover != true && enemyItem.X >= coverLineWest)
                        //{
                        //    currentLocationIsFlanked = true;
                        //    break;
                        //}
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
                        //else if (result.InSouthCover != true && enemyItem.Y >= coverLineSouth && result.InEastCover != true && enemyItem.X <= coverLineEast)
                        //{
                        //    currentLocationIsFlanked = true;
                        //    break;
                        //}
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
                        //else if (result.InSouthCover != true && enemyItem.Y >= coverLineSouth && result.InWestCover != true && enemyItem.X >= coverLineWest)
                        //{
                        //    currentLocationIsFlanked = true;
                        //    break;
                        //}
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
        private static List<Vector3> FindAdjacentCover(Vector3 currentLocation, int width, int height, string[,] validTiles)
        {
            List<Vector3> result = new();
            if (currentLocation.X > width - 1 || currentLocation.Z > height - 1)
            {
                throw new Exception("The character is off the map");
            }
            if (currentLocation.X == 1000)
            {
                int i = 2;
                int j = 3 + i;
            }

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
            if (zMax > height - 1)
            {
                zMax = height - 1;
            }

            //Get possible tiles, within constraints of map, including only square titles from current position (not diagonally)
            if (validTiles[Convert.ToInt32(currentLocation.X), Convert.ToInt32(zMax)] == "W")
            {
                result.Add(new Vector3(currentLocation.X, 0f, zMax));
            }
            if (validTiles[Convert.ToInt32(xMax), Convert.ToInt32(currentLocation.Z)] == "W")
            {
                result.Add(new Vector3(xMax, 0f, currentLocation.Z));
            }
            if (validTiles[Convert.ToInt32(currentLocation.X), Convert.ToInt32(zMin)] == "W")
            {
                result.Add(new Vector3(currentLocation.X, 0f, zMin));
            }
            if (validTiles[Convert.ToInt32(xMin), Convert.ToInt32(currentLocation.Z)] == "W")
            {
                result.Add(new Vector3(xMin, 0f, currentLocation.Z));
            }
            return result;
        }
    }
}
