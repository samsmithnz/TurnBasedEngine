# Battle
A POC to build the battle logic for a turn based game idea, similar to [XCOM](https://en.wikipedia.org/wiki/XCOM) and [Jagged Alliance](https://en.wikipedia.org/wiki/Jagged_Alliance_(series)).  

[![.NET Build](https://github.com/samsmithnz/Battle/actions/workflows/dotnet.yml/badge.svg)](https://github.com/samsmithnz/Battle/actions/workflows/dotnet.yml)
[![Coverage Status](https://coveralls.io/repos/github/samsmithnz/Battle/badge.svg?branch=main)](https://coveralls.io/github/samsmithnz/Battle?branch=main)
![Current Release](https://img.shields.io/github/release/samsmithnz/Battle/all.svg)

Currently supports
- Basic map generation, in 2d world, with x and z axis (y axis to come later)
- Basic pathfinding, movement and identification of possible movement tiles
- Basic attacks between multiple characters, with weapons, special abilities, and range taken into account
- Basic high and low cover, with cover destruction
- Grenades/area effects 
- Basic field of view
- Reactions/ overwatch
- A very basic AI, that attempts to move, flank and attack a character

Current workflow
1. Start team turn
2. Select first player
3. Player calculates field of view
4. Player calculates possible movement tiles
5. Player calculates opponenents in view
6. Player selects first opponent as their current target
7.  
