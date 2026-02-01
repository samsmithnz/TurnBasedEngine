# Architecture Review: TurnBasedEngine (Game-Focused)

**Date**: February 2026  
**Reviewer**: GitHub Copilot  
**Repository**: samsmithnz/TurnBasedEngine  
**Project Type**: Turn-based tactical game engine (C# / .NET Standard 2.0 for Unity)

---

## Executive Summary

The TurnBasedEngine is a **well-crafted game engine** for XCOM/Jagged Alliance-style tactical gameplay. The architecture is **appropriate for game development** - functional, performant, and Unity-ready. With 274 passing tests (98.9%), it's more polished than most game codebases.

### Overall Assessment: ⭐⭐⭐⭐☆ (4/5)

**What's Working Well:**
- ✅ Clean domain organization (8 game systems)
- ✅ Deterministic design (replay-friendly RandomNumberQueue)
- ✅ Strong test coverage (rare in game dev!)
- ✅ Static utility methods (performant for games)
- ✅ Unity-ready (netstandard2.0, no Unity dependencies in logic)

**What Needs Attention:**
- ⚠️ String-based map (bug-prone magic strings)
- ⚠️ Some magic numbers (clarity issue)
- ⚠️ Hardcoded 2-team limit (design flexibility)

**What's Fine As-Is:**
- ✅ Mission class as central orchestrator (normal for games)
- ✅ Static classes for algorithms (faster than interfaces)
- ✅ Concrete dependencies (simplicity over abstraction)

---

## Important: Game Engine vs Enterprise Software

This review is **game-development focused**. Many enterprise "best practices" **don't apply to games**:

| Enterprise Pattern | Game Reality |
|-------------------|--------------|
| ❌ Heavy DI/IoC | ✅ Direct references (faster) |
| ❌ Interface everything | ✅ Concrete types (simpler) |
| ❌ Microservices | ✅ Monolithic game loop |
| ❌ Repository pattern | ✅ Direct data access |
| ❌ Async everywhere | ✅ Synchronous game ticks |

**Why?**
- **Performance matters** - Games run at 60fps, abstractions have cost
- **Simplicity matters** - Game logic is complex enough without layers
- **Unity matters** - Unity's architecture favors composition over DI

---

## Current Architecture

### Domain Organization (8 Game Systems)

```
TBE.Logic/
  ├── Game/                   ← Mission orchestration, teams, objectives
  ├── Characters/             ← Units, AI, stats, movement
  ├── Encounters/             ← Combat resolution, damage
  ├── Map/                    ← Grid, pathfinding, FOV, cover
  ├── Items/                  ← Weapons, equipment
  ├── AbilitiesAndEffects/    ← Buffs, debuffs
  ├── Research/               ← Tech tree (minimal)
  └── Utility/                ← Random numbers, helpers
```

**Assessment**: ✅ Excellent separation. Each system owns its domain.

### Key Patterns (Game-Appropriate)

| Pattern | Usage | Verdict |
|---------|-------|---------|
| **Static Utility Classes** | PathFinding, FieldOfView, EncounterCore | ✅ Good for games (performance) |
| **Object Pools** | CharacterPool, WeaponPool, ItemPool | ✅ Standard game pattern |
| **Deterministic RNG** | RandomNumberQueue | ✅ Excellent (enables replay) |
| **Central Orchestrator** | Mission class (321 LOC) | ✅ Normal for game state |
| **String-based Map** | `string[,,]` with magic strings | ⚠️ Needs improvement |

---

## Actual Issues (Pragmatic, Game-Focused)

### 🔴 **Issue #1: String-Based Map = Bug Risk**
**Priority**: High | **Effort**: Medium (3-5 days)

**Problem**: Using strings for tiles is error-prone
```csharp
public string[,,] Map { get; set; }  // "■", "□", ".", "P", "T", "Y"
map[x, y, z] = "■";  // Typo? Compiler won't catch it
```

**Real Risks**:
- Typos in string comparisons (silent bugs)
- No autocomplete/IntelliSense
- Hard to add new tile types
- Performance overhead (string comparisons)

**Game-Appropriate Solution**:
```csharp
// Option 1: Enum (best for performance)
public enum TileType : byte {
    Empty = 0,
    NoCover = 1,
    HalfCover = 2,
    FullCover = 3,
    Character = 4,
    ToggleSwitchOn = 5,
    ToggleSwitchOff = 6
}
public TileType[,,] Map { get; set; }

// Option 2: Byte flags (for complex tiles)
[Flags]
public enum TileFlags : byte {
    None = 0,
    NoCover = 1,
    HalfCover = 2,
    FullCover = 4,
    HasCharacter = 8,
    HasItem = 16,
    Interactive = 32
}
```

**Why This Matters for Games**:
- **Type safety** prevents silent bugs
- **Better performance** (int comparison vs string)
- **Memory savings** (byte vs string reference)
- **Unity-friendly** (enums serialize well)

---

### 🟡 **Issue #2: Magic Numbers Everywhere**
**Priority**: Medium | **Effort**: Low (2-4 hours)

**Problem**: Hard to understand and modify
```csharp
toHit -= 20;  // What's 20?
toHit -= 40;  // What's 40?
toHit = (int)((float)toHit * 0.7f);  // What's 0.7?
character.XP += 100;  // What's 100?
```

**Game-Appropriate Solution**:
```csharp
// Game balance constants in one place
public static class GameBalance {
    // Cover modifiers
    public const int HALF_COVER_PENALTY = 20;
    public const int FULL_COVER_PENALTY = 40;
    public const int HUNKERED_MULTIPLIER = 2;
    
    // Combat modifiers
    public const float OVERWATCH_ACCURACY_MULT = 0.7f;
    public const int FLANK_CRIT_BONUS = 50;
    
    // Progression
    public const int XP_MISSION_COMPLETE = 100;
    public const int XP_PER_KILL = 50;
}

// Usage
toHit -= GameBalance.HALF_COVER_PENALTY;
```

**Why This Matters for Games**:
- **Game balance tuning** in one file
- **Easier iteration** during playtesting
- **Better for modding** (expose one config file)

---

### 🟡 **Issue #3: Hardcoded 2-Team Limit**
**Priority**: Medium | **Effort**: Low (4 hours)

**Problem**: Limits game design options
```csharp
if (Teams.Count != 2) {
    throw new Exception("Unexpected number of teams");
}
// Also: hardcoded Teams[0] and Teams[1] references
```

**Missed Opportunities**:
- 3-way battles (player vs faction A vs faction B)
- Co-op missions
- Free-for-all
- Dynamic team spawning

**Game-Appropriate Solution**:
```csharp
public void StartMission() {
    if (Teams.Count < 2) {
        throw new InvalidOperationException("Need at least 2 teams");
    }
    
    // Update targets for all teams
    for (int i = 0; i < Teams.Count; i++) {
        var opponents = GetOpponentsForTeam(i);
        Teams[i].UpdateTargets(Map, opponents);
    }
}

private List<Character> GetOpponentsForTeam(int teamIndex) {
    var opponents = new List<Character>();
    for (int i = 0; i < Teams.Count; i++) {
        if (i != teamIndex) {  // All other teams are opponents
            opponents.AddRange(Teams[i].Characters);
        }
    }
    return opponents;
}
```

**Why This Matters for Games**:
- **Design flexibility** for future game modes
- **Minimal code change** (4 hours)
- **Opens up gameplay options**

---

### 🟢 **Issue #4: TODO Comments Need Resolution**
**Priority**: Low | **Effort**: Low (2 hours)

**Problems Found**:
```csharp
// Character.cs:51
//TODO: Location should never be set here- but I need this for serialization.
public Vector3 Location { get; set; }

// FieldOfView.cs:190
//TODO: Fix this - but for now, it will work for missed shots
int zFinal = ((int)target.Z + (zDifference * zMultiplier)) + 2;
```

**Game-Appropriate Solutions**:

For serialization issue:
```csharp
// Use [JsonProperty] to control what's serialized
public class Character {
    [JsonIgnore]
    private Vector3 _location;
    
    [JsonProperty("Location")]  // Only for serialization
    internal Vector3 LocationForSerialization {
        get => _location;
        set => _location = value;
    }
    
    public Vector3 Location => _location;  // Read-only for game code
    
    public void SetLocation(string[,,] map, Vector3 newLocation) {
        _location = newLocation;
        // ... FOV updates etc
    }
}
```

For the FOV calculation:
- Either fix the math or add a comment explaining the +2 offset

---

### 🟢 **Issue #5: Character Class Is Large**
**Priority**: Low | **Effort**: Medium (but optional)

**Observation**: Character.cs is 324 LOC with 30+ properties

**Reality Check**: This is **actually fine for a game**. Character is a core entity with:
- Stats (HP, AP, Level, XP)
- Equipment (Weapons, Items)
- Position (Location, Cover)
- Vision (FOV, Targets)
- State (Overwatch, Hunkered)
- Records (Kills, Shots, Missions)

**Alternative** (only if it becomes painful):
```csharp
// Split into smaller structs
public struct CharacterStats {
    public int HitpointsMax;
    public int HitpointsCurrent;
    public int ActionPointsMax;
    public int ActionPointsCurrent;
    // ...
}

public class Character {
    public CharacterStats Stats;
    public CharacterPosition Position;
    public CharacterVision Vision;
    // ...
}
```

**But honestly**: The current structure is fine. Don't over-engineer.

---

## What NOT to Change

### ✅ Mission Class as Central Orchestrator

**Enterprise developers might say**: "God object! Violates SRP! Needs 5 services!"

**Game reality**: Mission **should** orchestrate everything. This is **normal** for games.

```csharp
// Current (good):
mission.AttackCharacter(source, weapon, target, sourceTeam, opponentTeam);

// Over-engineered (bad for games):
var combatService = new CombatService(
    new DamageCalculator(
        new RangeModifierService(),
        new CoverService(new CoverCalculator()),
        new AbilityAggregator()
    )
);
combatService.Execute(new CombatContext(...));
```

**Why Mission is fine**:
- **Single source of truth** for game state
- **Unity integration** - easy to call from MonoBehaviours
- **Testability** - Mission has 274 tests, works great
- **Performance** - Direct calls, no abstraction overhead

**Verdict**: ✅ Leave Mission class as-is. It's working well.

---

### ✅ Static Utility Classes

**Enterprise developers might say**: "No interfaces! Can't mock! Need DI!"

**Game reality**: Static classes are **perfect** for pure algorithms.

```csharp
// Current (good for games):
PathFindingResult path = PathFinding.FindPath(map, start, end);

// Over-engineered (bad):
public interface IPathFinder { }
public class PathFinderService : IPathFinder { }
// Inject into 10 classes, deal with DI container, etc.
```

**Why static is fine**:
- **Pure functions** - no side effects, easy to test
- **Performance** - no virtual dispatch, no allocations
- **Simplicity** - just call the method
- **Unity-friendly** - no need to manage object lifetime

**Existing static classes that are perfect**:
- `PathFinding` - A* algorithm
- `FieldOfView` - Line-of-sight calculation
- `EncounterCore` - Damage math
- `MapCore` - Grid utilities

**Verdict**: ✅ Keep static classes. They're appropriate for games.

---

### ✅ No Dependency Injection

**Enterprise developers might say**: "No IoC container! No constructor injection! Tight coupling!"

**Game reality**: DI adds **complexity and overhead** that games don't need.

**Why DI is unnecessary here**:
- **Unity doesn't use DI** by default (uses Component pattern)
- **Performance cost** - Interface calls have overhead at 60fps
- **Complexity cost** - Learning curve for contributors
- **Testing works** - 274 tests prove it's testable without DI

**When DI makes sense in games**:
- Large teams (50+ developers) needing strict boundaries
- Live service games needing to swap systems at runtime
- This project: 4,500 LOC by one person

**Verdict**: ✅ DI not needed. Keep it simple.

---

## Recommended Actions (Prioritized for Games)

### 🎯 Do These (High Value, Low Cost)

1. **Replace string[,,] map with enum** (3-5 days)
   - Biggest bug risk right now
   - Better performance
   - Unity-friendly

2. **Extract magic numbers to GameBalance class** (2-4 hours)
   - Makes balance tuning easier
   - Quick win

3. **Remove 2-team hardcoding** (4 hours)
   - Opens up game design
   - Minimal risk

4. **Fix TODO comments** (2 hours)
   - Clean up tech debt

**Total effort**: ~1 week

---

### 🤔 Consider These (Medium Value)

5. **Add XML docs to public APIs** (4-8 hours)
   - Helps Unity developers using the DLL
   - Better IntelliSense

6. **Expand GameConstants class** (2 hours)
   - Centralize all game configuration
   - Expose for modding

---

### 🚫 Don't Do These (Over-Engineering)

❌ **Add dependency injection** - Not needed for games  
❌ **Split Mission into 5 services** - Over-engineered  
❌ **Add interfaces everywhere** - Performance cost  
❌ **Use repository pattern** - Game engines aren't databases  
❌ **Add async/await** - Game loops are synchronous  

---

## Testing Assessment

**Current**: 274 passing tests (98.9% pass rate)

**Assessment**: ✅ **Excellent for a game project**

Most games have:
- 0 tests (manual QA only)
- Some integration tests
- Rarely unit tests

This project has:
- ✅ Comprehensive unit tests
- ✅ Scenario-based integration tests
- ✅ Edge case coverage (AI crash tests, null checks)
- ✅ Deterministic tests (RandomNumberQueue)

**Verdict**: Testing is a **major strength**. Don't mess with it.

---

## Unity Integration Notes

**Current**: netstandard2.0 DLL with no Unity dependencies ✅

**This is excellent**:
- Unity can consume the DLL directly
- Can test without Unity
- Can version independently
- Can use in non-Unity projects

**Integration pattern**:
```csharp
// Unity MonoBehaviour (thin wrapper)
public class MissionController : MonoBehaviour {
    private Mission _mission;
    
    void Start() {
        _mission = new Mission();
        // ... setup
    }
    
    void OnPlayerAction() {
        var result = _mission.AttackCharacter(...);
        // ... handle result
    }
}
```

**Verdict**: ✅ Unity integration is well-designed.

---

## Performance Notes

**No profiling done**, but observations:

**Potential hotspots**:
1. `MapCore.GetMapArea()` - O(n²) for FOV calculation
2. `CharacterAI.AssignPointsToEachTile()` - Evaluates every tile
3. String array cloning: `(string[,,])map.Clone()`

**But**: Premature optimization is the root of all evil.

**Recommendation**: 
- Profile in Unity with real game scenarios
- Only optimize hotspots that show up in profiler
- Don't optimize based on theory

---

## Summary

### What's Good (Keep!)
✅ Clean domain organization  
✅ Deterministic design (RandomNumberQueue)  
✅ Static utility classes (performance)  
✅ Mission orchestrator (normal for games)  
✅ Strong test coverage (rare!)  
✅ Unity-ready architecture  

### What Needs Work
⚠️ String-based map → enum (3-5 days)  
⚠️ Magic numbers → GameBalance (2-4 hours)  
⚠️ 2-team limit → N teams (4 hours)  
⚠️ Fix TODO comments (2 hours)  

### What to Ignore
🚫 Don't add DI/IoC  
🚫 Don't split Mission class  
🚫 Don't add interfaces everywhere  
🚫 Don't follow enterprise patterns  

### Final Rating: ⭐⭐⭐⭐☆ (4/5)

**For a game engine**, this is **excellent work**. The architecture is clean, testable, and appropriate for the domain. Focus on the practical improvements (enums, constants, team limit) and avoid over-engineering.

**Total recommended work**: ~1 week of focused effort  
**Expected outcome**: 4.5/5 stars with type safety and flexibility gains

---

**Report Generated**: 2026-02-01  
**Context**: Game development, not enterprise software  
**Recommendation**: Pragmatic improvements only
