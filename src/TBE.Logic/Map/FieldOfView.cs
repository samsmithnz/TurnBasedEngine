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
                    if (character.Location == item && character.HitpointsCurrent > 0)
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
            if (opponentCharacters != null)
            {
                foreach (Character character in opponentCharacters)
                {
                    bool addedCharacter = false;
                    foreach (Vector3 fovLocation in fov)
                    {
                        if (character.Location == fovLocation && character.HitpointsCurrent > 0)
                        {
                            addedCharacter = true;
                            results.Add(character);
                            break;
                        }
                    }
                    if (!addedCharacter && character.HitpointsCurrent > 0 && CharacterLocationIsAdjacentToFOVList(map, character.Location, fov))
                    {
                        results.Add(character);
                    }
                }
            }
            return results;
        }


        //If a player is behind cover, but adjacent squares are open/in the players FOV, then the player is visible too
        private static bool CharacterLocationIsAdjacentToFOVList(string[,,] map, Vector3 location, List<Vector3> list)
        {
            //Look at the location.
            //Is the player in cover? 
            //Are adjacent spots visible? 

            foreach (Vector3 item in list)
            {
                if (map[(int)item.X, (int)item.Y, (int)item.Z] == "")
                {
                    if (item.X - 1 == location.X && item.Z == location.Z)
                    {
                        return true;
                    }
                    else if (item.X + 1 == location.X && item.Z == location.Z)
                    {
                        return true;
                    }
                    else if (item.X == location.X && item.Z - 1 == location.Z)
                    {
                        return true;
                    }
                    else if (item.X == location.X && item.Z + 1 == location.Z)
                    {
                        return true;
                    }
                }
            }
            return false;
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
            for (int i = 1; i < points.Count; i++)
            {
                Vector3 item = points[i];
                //If the item gets to the edge of the map - return the edge location
                if (((int)item.X > map.GetLength(0) - 1 || (int)item.X < 0 ||
                    (int)item.Y > map.GetLength(1) - 1 || (int)item.Y < 0 ||
                    (int)item.Z > map.GetLength(2) - 1 || (int)item.Z < 0) &&
                    points.Count > 0)
                {
                    return points[i - 1];
                }
                //If the item hits something, return that position
                else if (map[(int)item.X, (int)item.Y, (int)item.Z] != "")
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
            int xMax = map.GetLength(0);
            int yMax = map.GetLength(1);
            int zMax = map.GetLength(2);

            if (team.FOVMap == null)
            {
                team.FOVMap = MapCore.InitializeMap(xMax, yMax, zMax, FOV_Unknown);
                team.FOVHistory = new HashSet<Vector3>();
            }
            foreach (Character character in team.Characters)
            {
                foreach (Vector3 item in character.FOVHistory)
                {
                    team.FOVHistory.Add(item);
                }
                //for (int y = 0; y < 1; y++)
                //{
                //    for (int x = 0; x < xMax; x++)
                //    {
                //        for (int z = 0; z < zMax; z++)
                //        {
                //            //Set the team FOV map if the character FOV is not set

                //            //a character can see this tile, update the team tile
                //            if (character.FOVMap[x, y, z] == FOV_CanSee && team.FOVMap[x, y, z] != FOV_CanSee)
                //            {
                //                team.FOVMap[x, y, z] = FOV_CanSee;
                //            }
                //            ////If the location has been visible in the past, but not now, set it as cannot see
                //            //else if (character.FOVMap[x, y, z] == FOV_CanNotSee && team.FOVMap[x, y, z] != FOV_CanNotSee)
                //            //{
                //            //    team.FOVMap[x, y, z] = FOV_CanNotSee;
                //            //}
                //            else if (character.FOVMap[x, y, z] == FOV_Unknown && team.FOVMap[x, y, z] != FOV_Unknown && team.FOVMap[x, y, z] != FOV_CanSee) // && team.FOVMap[x, y, z] != FOV_CanNotSee
                //            {
                //                //Otherwise it's never been visible and is unknown
                //                team.FOVMap[x, y, z] = FOV_Unknown;
                //            }
                //        }
                //    }
                //}
                foreach (Vector3 item in team.FOVHistory)
                {
                    team.FOVMap[(int)item.X, (int)item.Y, (int)item.Z] = FOV_CanSee;
                }

            }
            return team;
        }

    }
}
