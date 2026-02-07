# TurnBasedEngine - GitHub Copilot Instructions

## Project Overview

TurnBasedEngine is a tactical turn-based combat engine inspired by XCOM mechanics, featuring:
- Grid-based tactical combat on 3D maps
- Character management with stats, abilities, and experience
- Line-of-sight and field-of-view calculations
- Cover system (half cover, full cover, flanking)
- AI opponent decision-making
- Mission-based gameplay with teams

## Target Frameworks
- .NET 10
- .NET Standard 2.1

## Project Structure

### Core Namespaces
- `TBE.Logic.Characters` - Character stats, movement, AI, and experience
- `TBE.Logic.Encounters` - Combat resolution, hit calculations, damage
- `TBE.Logic.Map` - Field of view, pathfinding, movement tiles
- `TBE.Logic.Game` - Mission, team, and game state management
- `TBE.Logic.Items` - Weapons, equipment, and item mechanics
- `TBE.Logic.AbilitiesAndEffects` - Character abilities and status effects
- `TBE.Logic.Utility` - Random number generation, helper utilities
- `TBE.Tests` - Unit tests mirroring the Logic namespace structure

## Coding Standards & Conventions

### General Guidelines
1. **Never use `var` - always use explicit types**
   - ? `var result = new EncounterResult();`
   - ? `EncounterResult result = new EncounterResult();`
   - ? `var characters = new List<Character>();`
   - ? `List<Character> characters = new List<Character>();`

2. **Always use braces `{}` for if statements and loops**
   - ? `if (character.HitpointsCurrent > 0) return true;`
   - ? `if (character.HitpointsCurrent > 0) { return true; }`
   - ? `for (int i = 0; i < count; i++) ProcessItem(i);`
   - ? `for (int i = 0; i < count; i++) { ProcessItem(i); }`
   - This applies to `if`, `else`, `for`, `foreach`, `while`, and `do-while` statements

3. **Use named constants instead of magic numbers**
   - ? `if (targetCharacterIndex >= 0)`
   - ? `if (targetCharacterIndex >= NO_TARGET_SELECTED)`
   - Create constants in a `GameConstants.cs` file or as private consts in the class

4. **Use compound assignment operators**
   - ? `HitpointsCurrent = HitpointsCurrent + 1;`
   - ? `HitpointsCurrent++;`
   - ? `item.ClipRemaining = item.ClipRemaining - 1;`
   - ? `item.ClipRemaining--;`

5. **Map representation**
   - Maps are 3D arrays: `string[,,]` with dimensions `[X, Y, Z]`
   - Y dimension is typically 1 (single layer)
   - Empty tiles: `""`
   - Player position: `"P"`
   - Use `Vector3` for locations (from System.Numerics)

6. **Character states**
   - Alive characters: `HitpointsCurrent > 0`
   - No target selected: `TargetCharacterIndex = -1`
   - First target: `TargetCharacterIndex = 0`

7. **Action Points**
   - Single move (within mobility range): costs 1 action point
   - Double move (beyond mobility range): costs 2 action points
   - Shooting/attacking: consumes all remaining action points (sets to 0)

8. **Percentage calculations**
   - All hit chances, critical chances use 0-100 scale
   - Formula pattern: `if ((100 - chanceToHit) <= randomRoll)`
   - Random numbers use `RandomNumberQueue` for deterministic testing

### Testing Patterns
- Use `RandomNumberQueue` for predictable random number sequences
- Pass `diceRolls` parameter to methods that need randomness
- This allows unit tests to be deterministic and repeatable

### Combat Mechanics Constants
- Half cover penalty: 20% (40% when hunkered)
- Full cover penalty: 40% (80% when hunkered)
- Flanking bonus: +50% critical chance
- Overwatch aim reduction: 70% of normal (multiply by 0.7f)
- Overwatch critical chance: 0%

### Field of View (FOV)
- `FOV_CanSee = ""` - Tile is currently visible
- `FOV_Unknown = "?"` - Tile has never been seen
- FOV is calculated using line-of-sight from character position
- Teams maintain cumulative FOV history

### Code Organization
- Static utility classes for calculations (e.g., `FieldOfView`, `MapCore`, `Range`)
- Instance classes for game objects (e.g., `Character`, `Weapon`, `Mission`)
- Keep business logic in `TBE.Logic`, separate from any UI concerns
- Test files mirror the Logic namespace structure in `TBE.Tests`

## What NOT to Do

1. ? Don't use `var` - always use explicit types
2. ? Don't omit braces `{}` from if statements or loops, even for single-line statements
3. ? Don't use `Random` directly - use `RandomNumberQueue` for testability
4. ? Don't hardcode magic numbers - create named constants
5. ? Don't modify map state without updating character FOV
6. ? Don't assume Y dimension > 1 (maps are typically flat)
7. ? Don't create UI dependencies in TBE.Logic namespace
8. ? Don't use `Vector3.Zero` for valid character positions (it's a special value)
9. ? Don't forget to check `HitpointsCurrent > 0` before operating on characters
10. ? Don't mutate weapon ammo without checking `AmmoCurrent > 0`
11. ? Don't use explicit assignment when compound operators are available

## Common Patterns

### Character Movement
```csharp
// Always update location with map reference
character.SetLocationAndRange(map, newLocation, character.FOVRange, opponentCharacters);

// Update team FOV after movement
FieldOfView.UpdateTeamFOV(map, team);
```

### Combat Resolution
```csharp
// Get hit chance
int chanceToHit = EncounterCore.GetChanceToHit(sourceCharacter, weapon, targetCharacter);

// Roll to hit
int randomRoll = diceRolls.Dequeue();
if ((100 - chanceToHit) <= randomRoll) 
{
    // Hit logic
}
```

### Experience Gain
```csharp
// Award XP and check for level up
sourceCharacter.XP += Experience.GetExperience(successfulHit, wasKill);
if (sourceCharacter.LevelUpIsReady)
{
    // Level up available
}
```

### Using Constants Instead of Magic Numbers
```csharp
// Define constants
private const int NO_TARGET_SELECTED = -1;
private const int FIRST_TARGET_INDEX = 0;
private const string PLAYER_MAP_MARKER = "P";

// Use them
TargetCharacterIndex = NO_TARGET_SELECTED;
if (TargetCharacterIndex >= FIRST_TARGET_INDEX)
{
    map[x, y, z] = PLAYER_MAP_MARKER;
}
```

## Domain-Specific Terms

- **Overwatch** - Reaction shot triggered when enemy moves within sight
- **Hunker Down** - Defensive action that doubles cover bonuses
- **Flanking** - Attacking from a position where target's cover doesn't apply
- **FOV (Field of View)** - Area visible to a character based on line-of-sight
- **Mobility Range** - Distance character can move with 1 action point
- **Shooting Range** - Maximum distance for attacks
- **Critical Hit** - Extra damage attack (flanking gives +50% chance)
- **Armor Piercing** - Ability that ignores target armor
- **Armor Shredding** - Ability that reduces target armor before damage

## AI Scoring System

When generating AI code:
- Higher scores = better moves
- Base score starts at 0
- Cover bonuses: Full cover +8, Half cover +4
- Penalties: Flanked -5, Visible to enemy -2
- Chance to hit bonuses: 95%?+5, 90%?+4, 80%?+3, 65%?+2, 50%?+1
- AI attempts best move if intelligence check passes, otherwise next-best
- **Note**: These scoring values should be extracted to named constants

## Map Tiles

Common tile types in `CoverType` class:
- `NoCover = ""`
- `HalfCover = "?"`
- `FullCover = "?"`
- `ToggleSwitchOn` / `ToggleSwitchOff` - Interactive elements

## Weapon Range Modifiers

Range modifiers vary by weapon type:
- **Standard weapons**: Bonus decreases with distance (1?+37, 9?+1)
- **Shotguns**: High bonus at close range, penalty at long range (1?+52, 17?-40)
- **Sniper rifles**: Penalty at close range, neutral at long range (1?-24, 9?0)

These values are in `Range.cs` and should ideally be moved to configuration data.

## Example Class Structure

When creating new features, follow these patterns:
- Static classes for calculations/utilities (no state)
- Instance classes for game entities (with state)
- Keep methods focused and single-purpose
- Return `EncounterResult` objects for combat outcomes
- Include logging via `List<string> log` in results
- Use `Vector3` for all positions, even on 2D maps

## Testing Philosophy

- All game logic must be deterministic when using `RandomNumberQueue`
- Tests should cover edge cases (e.g., dead characters, empty ammo, out of bounds)
- Use descriptive test names: `AttackCharacter_WithNoAmmo_ReturnsNull()`
- Verify both successful and failure paths
- Check that state changes persist (HP, ammo, XP, action points)

## Known Technical Debt

1. **Range.cs weapon modifiers** - Contains 27+ hardcoded weapon range modifiers
   - Consider moving to JSON/XML configuration file for easier game balance tuning
   - Current values: Standard weapon (37 to 1), Shotgun (52 to -40), Sniper (-24 to 0)
2. **FieldOfView.cs line 190** - Has TODO comment about `+ 2` offset for missed shot calculation
3. **Commented code** - Several files contain large blocks of commented code that should be cleaned up
   - Character.cs lines 269-305 (old FOV calculation logic)
   - Consider removing if no longer needed or documenting why it's preserved
4. **WeaponPool.cs, ItemPool.cs, CharacterPool.cs** - Hardcoded game balance data
   - Consider moving to JSON configuration files for easier modification without recompilation

---

**When in doubt**: Check existing patterns in similar classes, prefer named constants over literals, use compound assignment operators, and ensure deterministic behavior for testing.
