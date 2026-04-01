using TBE.Logic.Characters;
using TBE.Logic.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TBE.Logic.Map
{
    public static class FieldOfView
    {

        public const string FOV_CanSee = "";
        public const string FOV_Unknown = "▓";
        //public const string FOV_CanNotSee = "░";

        public static List<Vector3> GetFieldOfView(string[,,] map, Vector3 location, int range)
        {
            return MapCore.GetMapArea(map, location, range, true);
        }

        public static List<Character> GetCharactersInArea(string[,,] map, List<Character> characters, Vector3 location, int range)
        {
            List<Character> results = new List<Character>();
            List<Vector3> area = MapCore.GetMapArea(map, location, range, false, true);
            foreach (Character character in characters)
            {
                foreach (Vector3 item in area)
                {
                    if (character.Location == item && character.HitpointsCurrent > GameConstants.DEAD_HITPOINTS)
                    {
                        results.Add(character);
                    }
                }
            }
            return results;
        }

        public static List<Character> GetCharactersInView(string[,,] map, Vector3 location, int shootingRange, List<Character> opponentCharacters)
        {
            List<Character> results = new List<Character>();

            List<Vector3> fov = FieldOfView.GetFieldOfView(map, location, shootingRange);
            HashSet<Vector3> fovLocations = new HashSet<Vector3>(fov);
            if (opponentCharacters != null)
            {
                foreach (Character character in opponentCharacters)
                {
                    if (character.HitpointsCurrent <= GameConstants.DEAD_HITPOINTS)
                    {
                        continue;
                    }

                    if (fovLocations.Contains(character.Location))
                    {
                        results.Add(character);
                    }
                    else if (CharacterLocationIsAdjacentToFOVList(map, character.Location, fovLocations))
                    {
                        results.Add(character);
                    }
                }
            }
            return results;
        }


        //If a player is behind cover, but adjacent squares are open/in the players FOV, then the player is visible too
        private static bool CharacterLocationIsAdjacentToFOVList(string[,,] map, Vector3 location, HashSet<Vector3> fovLocations)
        {
            int x = (int)location.X;
            int y = (int)location.Y;
            int z = (int)location.Z;

            if (MapLocationIsVisibleAndEmpty(map, x - GameConstants.ADJACENT_TILE_OFFSET, y, z, fovLocations))
            {
                return true;
            }

            if (MapLocationIsVisibleAndEmpty(map, x + GameConstants.ADJACENT_TILE_OFFSET, y, z, fovLocations))
            {
                return true;
            }

            if (MapLocationIsVisibleAndEmpty(map, x, y, z - GameConstants.ADJACENT_TILE_OFFSET, fovLocations))
            {
                return true;
            }

            if (MapLocationIsVisibleAndEmpty(map, x, y, z + GameConstants.ADJACENT_TILE_OFFSET, fovLocations))
            {
                return true;
            }

            return false;
        }

        private static bool MapLocationIsVisibleAndEmpty(string[,,] map, int x, int y, int z, HashSet<Vector3> fovLocations)
        {
            if (x < GameConstants.FIRST_INDEX ||
                y < GameConstants.FIRST_INDEX ||
                z < GameConstants.FIRST_INDEX ||
                x > map.GetLength(GameConstants.X_DIMENSION_INDEX) - GameConstants.LAST_INDEX_OFFSET ||
                y > map.GetLength(GameConstants.Y_DIMENSION_INDEX) - GameConstants.LAST_INDEX_OFFSET ||
                z > map.GetLength(GameConstants.Z_DIMENSION_INDEX) - GameConstants.LAST_INDEX_OFFSET)
            {
                return false;
            }

            Vector3 adjacentLocation = new Vector3(x, y, z);
            return map[x, y, z] == GameConstants.EMPTY_TILE && fovLocations.Contains(adjacentLocation);
        }


        //Follow the missed shot to find the target
        public static Vector3 MissedShot(string[,,] map, Vector3 source, Vector3 target, int missedByPercent)
        {
            //Get the final missed location the projectile is heading down
            Vector3 finalLocation = GetMissedLocation(map, source, target, missedByPercent);

            //Get all of the points along this line to the final location
            List<Vector3> points = GetPointsOnLine(source, finalLocation);

            //Check each point to see if it goes off the map, or hits another object
            //Start at 1 to skip the source - if the source is a player, things go bad
            for (int i = GameConstants.MOVEMENT_START_INDEX; i < points.Count; i++)
            {
                Vector3 item = points[i];
                //If the item gets to the edge of the map - return the edge location
                if (((int)item.X > map.GetLength(GameConstants.X_DIMENSION_INDEX) - GameConstants.LAST_INDEX_OFFSET || (int)item.X < GameConstants.FIRST_INDEX ||
                    (int)item.Y > map.GetLength(GameConstants.Y_DIMENSION_INDEX) - GameConstants.LAST_INDEX_OFFSET || (int)item.Y < GameConstants.FIRST_INDEX ||
                    (int)item.Z > map.GetLength(GameConstants.Z_DIMENSION_INDEX) - GameConstants.LAST_INDEX_OFFSET || (int)item.Z < GameConstants.FIRST_INDEX) &&
                    points.Count > GameConstants.DEAD_HITPOINTS)
                {
                    return points[i - GameConstants.PREVIOUS_INDEX_OFFSET];
                }
                //If the item hits something, return that position
                else if (map[(int)item.X, (int)item.Y, (int)item.Z] != GameConstants.EMPTY_TILE)
                {
                    return item;
                }
            }

            return Vector3.Zero;
        }

        //Get the final location - which will usually be just off the map
        public static Vector3 GetMissedLocation(string[,,] map, Vector3 source, Vector3 target, int missedByPercent = 0)
        {
            int xMax = map.GetLength(0);
            int yMax = map.GetLength(1);
            int zMax = map.GetLength(2);

            int xDifference = (int)(target.X - source.X);
            int yDifference = (int)(target.Y - source.Y);
            int zDifference = (int)(target.Z - source.Z);

            int xAdjustment = 1;
            if (xDifference < 0)
            {
                xAdjustment = -1;
            }
            int yAdjustment = 1;
            if (yDifference < 0)
            {
                yAdjustment = -1;
            }
            int zAdjustment = 1;
            if (zDifference < 0)
            {
                zAdjustment = -1;
            }
            if (xDifference == 0)
            {
                xDifference = 1;
            }
            if (yDifference == 0)
            {
                yDifference = 1;
            }
            if (zDifference == 0)
            {
                zDifference = 1;
            }

            int xMultiplier = (int)Math.Ceiling((xMax - target.X) / xDifference);
            if (xDifference < 0)
            {
                xMultiplier = (int)Math.Ceiling((xMax - target.X) / xDifference * xAdjustment);
            }
            int yMultiplier = (int)Math.Ceiling((yMax - target.Y) / yDifference);
            if (yDifference < 0)
            {
                yMultiplier = (int)Math.Ceiling((yMax - target.Y) / yDifference * yAdjustment);
            }
            int zMultiplier = (int)Math.Ceiling((zMax - target.Z) / zDifference);
            if (zDifference < 0)
            {
                zMultiplier = (int)Math.Ceiling((zMax - target.Z) / zDifference * zAdjustment);
            }

            int xFinal = ((int)target.X + (xDifference * xMultiplier));
            int yFinal = ((int)target.Y + (yDifference * yMultiplier));
            int zFinal = ((int)target.Z + (zDifference * zMultiplier)) + 2; //TODO: Fix this - but for now, it will work for missed shots
            return new Vector3(xFinal, yFinal, zFinal);
        }

        //http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
        //https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        private static IEnumerable<Vector3> GetPointsOnLine(int x0, int z0, int x1, int z1)
        {
            bool steep = Math.Abs(z1 - z0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and z0
                x0 = z0;
                z0 = t;
                t = x1; // swap x1 and z1
                x1 = z1;
                z1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = z0; // swap z0 and z1
                z0 = z1;
                z1 = t;
            }
            int dx = x1 - x0;
            int dz = Math.Abs(z1 - z0);
            int error = dx / 2;
            int zstep = (z0 < z1) ? 1 : -1;
            int z = z0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Vector3((steep ? z : x), 0, (steep ? x : z));
                error -= dz;
                if (error < 0)
                {
                    z += zstep;
                    error += dx;
                }
            }
            yield break;
        }

        public static List<Vector3> GetPointsOnLine(Vector3 source, Vector3 target)
        {
            List<Vector3> points = GetPointsOnLine((int)source.X, (int)source.Z, (int)target.X, (int)target.Z).ToList<Vector3>();

            //Check if we need to reverse the list, we always want the source node first
            if (points.Count > 0 &&
                points[points.Count - 1].X == source.X &&
                points[points.Count - 1].Z == source.Z)
            {
                points.Reverse();
            }

            return points;
        }

        public static Team UpdateTeamFOV(string[,,] map, Team team)
        {
            if (team.FOVMap == null || team.FOVHistory == null)
            {
                int xMax = map.GetLength(0);
                int yMax = map.GetLength(1);
                int zMax = map.GetLength(2);
                team.FOVMap = MapCore.InitializeMap(xMax, yMax, zMax, FOV_Unknown);
                team.FOVHistory = new HashSet<Vector3>();
            }

            foreach (Character character in team.Characters)
            {
                foreach (Vector3 item in character.FOVHistory)
                {
                    if (team.FOVHistory.Add(item))
                    {
                        team.FOVMap[(int)item.X, (int)item.Y, (int)item.Z] = FOV_CanSee;
                    }
                }
            }
            return team;
        }

    }
}
