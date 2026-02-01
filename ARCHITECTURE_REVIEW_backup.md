# Architecture Review: TurnBasedEngine

**Date**: February 2026  
**Reviewer**: GitHub Copilot  
**Repository**: samsmithnz/TurnBasedEngine  
**Project Type**: Turn-based tactical game engine (C# / .NET Standard 2.0)  
**Context**: Game engine for Unity integration, not enterprise software

---

## Executive Summary

The TurnBasedEngine is a **well-designed game engine** for XCOM/Jagged Alliance-style tactical gameplay. With 44 logic files organized into 8 game domains, the architecture is pragmatic and functional. **274 passing tests** (98.9% pass rate) demonstrate solid quality.

### Overall Assessment
- **Code Quality**: Good (⭐⭐⭐⭐☆)
- **Architecture**: **Appropriate for a game engine** - functional, testable, Unity-ready
- **Test Coverage**: Strong (274 tests, scenario-based testing)
- **Maintainability**: Good for a game project
- **Technical Debt**: Low

**Key Insight**: This is a **game engine, not an enterprise app**. Many "best practices" from web/enterprise development don't apply here. The static classes, concrete dependencies, and centralized orchestration are **actually appropriate** for game logic.

---

## Current Architecture Overview

### **1. Architectural Pattern**
**Domain-Driven Design (DDD) with Layered Structure**

The project uses a modular, domain-centric organization:

```
TBE.Logic (netstandard2.0)    - Core game engine
  ├── Game/                    - Mission orchestration, teams, objectives
  ├── Characters/              - Units, AI, movement, stats
  ├── Encounters/              - Combat resolution, damage calculation
  ├── Map/                     - Grid, pathfinding, FOV, cover
  ├── Items/                   - Weapons, equipment pools
  ├── AbilitiesAndEffects/     - Buffs, debuffs, modifiers
  ├── Research/                - Tech tree (minimal usage)
  ├── SaveGames/               - Serialization
  └── Utility/                 - Random numbers, helpers

TBE.Tests (net10.0)            - MSTest-based unit & scenario tests
TBE.PerformanceProfiling       - Performance testing
```

### **2. Key Design Patterns**

| Pattern | Implementation | Files |
|---------|---------------|-------|
| **Static Utility Classes** | Algorithms separated from data | MapCore, EncounterCore, PathFinding, FieldOfView (21 static classes) |
| **Object Pool** | Factory methods for entities | CharacterPool, WeaponPool, ItemPool, AbilityPool |
| **State Machine** | Cover state tracking | CoverState (InFullCover, InHalfCover, IsFlanked) |
| **Queue-based RNG** | Deterministic random numbers | RandomNumberQueue (enables replay/testing) |
| **Value Objects** | Immutable modifiers | Ability, Effect, DamageRange |
| **God Object** | Central orchestrator (⚠️) | Mission class (321 LOC) |

### **3. Dependencies**

**External NuGet Packages:**
- `Newtonsoft.Json` (13.0.2) - Serialization
- `System.Numerics.Vectors` (4.6.1) - Vector3 math

**No interfaces, no dependency injection** - All dependencies are concrete types.

---

## Strengths

### ✅ **1. Clean Domain Organization**
The 8-domain structure makes the codebase highly navigable:
- **Characters/** handles all unit logic
- **Map/** owns all spatial operations
- **Encounters/** centralizes combat math
- Clear separation of concerns

### ✅ **2. Deterministic Design**
The `RandomNumberQueue` pattern is excellent:
```csharp
RandomNumbers = new RandomNumberQueue(
    RandomNumber.GenerateRandomNumberList(0, 100, 0, 1000)
);
```
- Enables replay functionality
- Makes tests deterministic
- Supports save/load without RNG desync

### ✅ **3. Strong Test Coverage**
- **274 passing tests** (3 minor failures on Linux paths)
- Scenario-based testing (AI, Cover, Movement, Shooting)
- Integration tests for complex workflows
- Good edge case coverage

### ✅ **4. Static Algorithm Classes**
Separating pure algorithms from stateful objects:
```csharp
public static class PathFinding
public static class FieldOfView
public static class EncounterCore
```
- Easy to test
- No side effects
- Reusable across contexts

### ✅ **5. Field of View (FOV) System**
The FOV history tracking is well-designed:
```csharp
public HashSet<Vector3> FOVHistory { get; set; }
```
- Supports fog-of-war
- Efficient with HashSet
- Persistent across turns

---

## Improvement Opportunities

### Game Development Context

**Important**: Unlike enterprise software, game engines benefit from:
- **Performance over abstraction** - Direct calls are faster than interfaces
- **Centralized state management** - A central "game state" object is normal
- **Static utility methods** - Common for math, physics, pathfinding
- **Concrete dependencies** - Simplicity often beats flexibility in games

The following recommendations are **prioritized for game development**, not enterprise patterns.

---

### 🔴 **High Priority Issues**

#### **1. String-Based Map Representation**
**Severity**: High (actual bug risk)  
**File**: `Map/MapCore.cs`, `Mission.cs`

**Problem**:
```csharp
public string[,,] Map { get; set; }  // "■", "□", ".", "P", "T", "Y"
```

**Issues**:
- Magic strings scattered throughout code
- No type safety (can assign any string)
- Error-prone comparisons
- No IntelliSense support
- Hard to extend

**Current Constants** (CoverType.cs):
```csharp
public const string FullCover = "■";
public const string HalfCover = "□";
public const string NoCover = ".";
public const string ToggleSwitchOn = "T";
public const string ToggleSwitchOff = "Y";
```

**Recommendation**:
```csharp
// Option 1: Enum-based tiles
public enum TileType {
    Empty = 0,
    NoCover = 1,
    HalfCover = 2,
    FullCover = 3,
    Player = 4,
    ToggleSwitchOn = 5,
    ToggleSwitchOff = 6
}
public TileType[,,] Map { get; set; }

// Option 2: Rich domain model
public class MapTile {
    public Vector3 Position { get; set; }
    public TileType Type { get; set; }
    public Character OccupyingCharacter { get; set; }
    public CoverLevel CoverLevel { get; set; }
}
public MapTile[,,] Map { get; set; }
```

**Benefits**:
- Type safety
- Better IntelliSense
- Easier refactoring
- Cleaner code
- Supports more complex tile data

---

### 🟡 **High Priority Issues**

#### **4. Character Class Does Too Much**
**Severity**: Medium  
**File**: `Characters/Character.cs` (324 LOC)

**Problems**:
- 30+ public properties
- Mixed concerns: stats, location, FOV, targeting, actions
- Heavy method: `SetLocationAndRange()` does 5+ things
- Violates Single Responsibility

**Recommendation**:
```csharp
// Split into focused classes
public class CharacterStats {
    public int HitpointsMax { get; set; }
    public int HitpointsCurrent { get; set; }
    public int ActionPointsMax { get; set; }
    // ...
}

public class CharacterPosition {
    public Vector3 Location { get; private set; }
    public CoverState CoverState { get; private set; }
    public void MoveTo(Vector3 newLocation) { }
}

public class CharacterVision {
    public string[,,] FOVMap { get; set; }
    public HashSet<Vector3> FOVHistory { get; set; }
    public int FOVRange { get; set; }
}

public class Character {
    public CharacterStats Stats { get; set; }
    public CharacterPosition Position { get; set; }
    public CharacterVision Vision { get; set; }
    // ...
}
```

---

#### **5. Hardcoded Team Count Assumption**
**Severity**: Medium  
**File**: `Game/Mission.cs` lines 147-150

**Problem**:
```csharp
if (Teams.Count != 2) {
    throw new Exception("Unexpected number of teams: " + Teams.Count.ToString());
}
```

**Issues**:
- Hardcoded assumption of exactly 2 teams
- Cannot support 3+ team missions
- Cannot support FFA or co-op
- Limits game design flexibility

**Recommendation**:
```csharp
// Support N teams
public void StartMission() {
    if (Teams.Count < 2) {
        throw new InvalidOperationException("Mission requires at least 2 teams");
    }
    
    // Update all team targets against all other teams
    for (int i = 0; i < Teams.Count; i++) {
        List<Character> allOpponents = GetAllOpponentCharacters(i);
        Teams[i].UpdateTargets(Map, allOpponents);
    }
}

private List<Character> GetAllOpponentCharacters(int teamIndex) {
    var opponents = new List<Character>();
    for (int i = 0; i < Teams.Count; i++) {
        if (i != teamIndex) {
            opponents.AddRange(Teams[i].Characters);
        }
    }
    return opponents;
}
```

---

#### **6. Mutable State Everywhere**
**Severity**: Medium  
**Files**: Character.cs, Mission.cs, Team.cs

**Problem**:
- Heavy use of public setters
- Direct map manipulation
- Character state mutated from multiple places
- Hard to track state changes

**Examples**:
```csharp
public int HitpointsCurrent { get; set; }  // Can be set anywhere
public List<Team> Teams { get; set; }       // List can be modified
public string[,,] Map { get; set; }         // Map mutated directly
```

**Recommendation**:
```csharp
// Immutable state updates
public class Character {
    public int HitpointsCurrent { get; private set; }
    
    public Character TakeDamage(int damage) {
        return new Character {
            HitpointsCurrent = Math.Max(0, this.HitpointsCurrent - damage),
            // ... copy other properties
        };
    }
}

// Or use events for state changes
public event EventHandler<DamageEventArgs> DamageTaken;
public void TakeDamage(int damage) {
    HitpointsCurrent = Math.Max(0, HitpointsCurrent - damage);
    DamageTaken?.Invoke(this, new DamageEventArgs(damage));
}
```

---

#### **7. TODO Comments Indicate Unresolved Design Issues**
**File**: `Characters/Character.cs` line 51

```csharp
//TODO: Location should never be set here- but I need this for serialization. 
//Need to figure this out later
public Vector3 Location { get; set; }
```

**Problem**: Serialization concerns leak into domain model

**Recommendation**:
```csharp
// Use DTOs for serialization
public class CharacterDTO {
    public Vector3 Location { get; set; }  // Public for JSON
    // ... other serializable properties
}

public class Character {
    private Vector3 _location;
    [JsonIgnore]
    public Vector3 Location => _location;
    
    public CharacterDTO ToDTO() {
        return new CharacterDTO { Location = _location, /* ... */ };
    }
    
    public static Character FromDTO(CharacterDTO dto) {
        return new Character { _location = dto.Location, /* ... */ };
    }
}
```

---

### 🟢 **Medium Priority Issues**

#### **8. Missing Exception Handling Strategy**
**Severity**: Medium  
**Files**: Only 5 throw statements found

**Problem**:
- Generic `Exception` thrown (not specific types)
- No exception handling strategy
- Null returns instead of exceptions in some places

**Current**:
```csharp
throw new Exception("Unexpected number of teams: " + Teams.Count.ToString());
```

**Recommendation**:
```csharp
// Custom exceptions
public class InvalidMissionStateException : Exception { }
public class InvalidMovementException : Exception { }
public class CombatResolutionException : Exception { }

// Consistent error handling
public PathFindingResult FindPath(...) {
    if (start == end) {
        throw new InvalidMovementException("Start and end positions are the same");
    }
    // ...
}
```

---

#### **9. Inconsistent Null Handling**
**Severity**: Medium  
**Files**: Multiple (CharacterMovement, Encounter)

**Problem**:
```csharp
// Sometimes returns null
if (pathFindingResult.Path.Count == 0) {
    return null;
}

// Sometimes throws
if (Teams.Count != 2) {
    throw new Exception(...);
}

// Sometimes checks parameters
if (diceRolls == null || diceRolls.Count == 0) {
    return null;
}
```

**Recommendation**:
```csharp
// Use nullable reference types (C# 8+)
#nullable enable

public PathFindingResult? FindPath(Vector3 start, Vector3 end) {
    // Returns null with compiler support
}

// Or use Result pattern
public Result<PathFindingResult> FindPath(Vector3 start, Vector3 end) {
    if (start == end) {
        return Result.Failure<PathFindingResult>("Invalid path");
    }
    return Result.Success(pathResult);
}
```

---

#### **10. FieldOfView and CharacterCover Calculate Cover Differently**
**Severity**: Medium  
**Files**: `Map/FieldOfView.cs`, `Characters/CharacterCover.cs`

**Problem**: Two different implementations of cover calculation
- FieldOfView has its own cover logic
- CharacterCover has different logic
- Inconsistency risk

**Recommendation**: Centralize cover calculation in one place, referenced by both

---

#### **11. Research System Underutilized**
**Severity**: Low-Medium  
**Files**: `Research/` folder

**Observation**: Research domain exists but minimal usage detected
- ResearchController, ResearchItem, ResearchPool defined
- Not integrated into main game loop
- No tests found for research

**Recommendation**:
- Either fully implement or remove to reduce confusion
- If keeping, document intended design
- Add tests before expanding

---

#### **12. CharacterAI Class Complexity**
**Severity**: Medium  
**File**: `Characters/CharacterAI.cs` (286 LOC)

**Problem**:
- Large `CalculateAIAction()` method
- `AssignPointsToEachTile()` has complex nested logic
- Hard to extend with new behaviors

**Recommendation**:
```csharp
// Strategy pattern for AI behaviors
public interface IAIBehavior {
    AIAction Evaluate(Character character, GameState state);
}

public class AggressiveBehavior : IAIBehavior { }
public class DefensiveBehavior : IAIBehavior { }
public class FlankingBehavior : IAIBehavior { }

public class CharacterAI {
    private readonly List<IAIBehavior> _behaviors;
    
    public AIAction CalculateAIAction(...) {
        var scores = _behaviors.Select(b => b.Evaluate(character, state));
        return ChooseBestAction(scores);
    }
}
```

---

### 🔵 **Low Priority / Quality of Life**

#### **13. Magic Numbers**
**Files**: Multiple

**Examples**:
```csharp
toHit -= 20;  // Half cover penalty
toHit -= 40;  // Full cover penalty
toHit = (int)((float)toHit * 0.7f);  // Overwatch penalty
character.XP += 100;  // XP for surviving mission
```

**Recommendation**:
```csharp
public static class GameConstants {
    public const int HALF_COVER_PENALTY = 20;
    public const int FULL_COVER_PENALTY = 40;
    public const float OVERWATCH_ACCURACY_MULTIPLIER = 0.7f;
    public const int XP_MISSION_COMPLETE = 100;
}
```

---

#### **14. Method Parameter Counts**
**Examples**:
```csharp
public static List<MovementAction> MoveCharacter(
    string[,,] map,
    Character characterMoving,
    PathFindingResult pathFindingResult,
    Team characterTeam,
    Team opponentTeam,
    RandomNumberQueue diceRolls)  // 6 parameters
```

**Recommendation**: Use parameter objects
```csharp
public class MovementContext {
    public string[,,] Map { get; set; }
    public Character Character { get; set; }
    public PathFindingResult Path { get; set; }
    public Team CharacterTeam { get; set; }
    public Team OpponentTeam { get; set; }
    public RandomNumberQueue DiceRolls { get; set; }
}

public static List<MovementAction> MoveCharacter(MovementContext context) { }
```

---

#### **15. Test Organization**
**Current**:
- Tests in flat structure
- Mix of unit tests and scenarios
- Some scenarios are integration tests

**Recommendation**:
```
TBE.Tests/
  ├── Unit/           // Pure unit tests
  ├── Integration/    // Multi-component tests
  └── Scenarios/      // End-to-end scenarios
```

---

#### **16. .NET 10 Target for Tests vs netstandard2.0 for Logic**
**Files**: TBE.Tests.csproj, TBE.Logic.csproj

**Current State**:
- Logic: `netstandard2.0` (for Unity compatibility)
- Tests: `net10.0` (latest .NET)

**Observation**: Large version gap (netstandard 2.0 was .NET Core 2.0 era)

**Recommendation**:
- Consider `netstandard2.1` for Logic (more features, still Unity-compatible)
- Or dual-target: `<TargetFrameworks>netstandard2.0;net8.0</TargetFrameworks>`

---

#### **17. Commented-Out Code**
**Files**: Character.cs (lines 138-162, 275-305)

**Problem**: Large blocks of commented code
- Adds noise
- May confuse developers
- Unclear if code should be kept

**Recommendation**: Remove and rely on git history, or document why preserved

---

#### **18. Logging/Observability**
**Current**: No structured logging

**Observation**: Many methods build `List<string> log` but no logging framework

**Recommendation**:
```csharp
// Add structured logging
public interface IGameLogger {
    void LogCombat(string attacker, string defender, int damage);
    void LogMovement(string character, Vector3 from, Vector3 to);
}

// Use in tests for debugging, in Unity for combat log
```

---

#### **19. Performance Profiling Project Underutilized**
**File**: TBE.PerformanceProfiling/

**Observation**: Project exists but minimal
- Could benchmark pathfinding
- Could benchmark FOV calculations
- Could identify bottlenecks

**Recommendation**: Expand with BenchmarkDotNet

---

## Recommended Refactoring Roadmap

### **Phase 1: Foundation (High Priority, Low Risk)**
1. **Extract constants** for magic numbers
2. **Add XML documentation** to public APIs
3. **Remove commented code**
4. **Fix TODO items**
5. **Centralize exception types**

**Effort**: 1-2 days  
**Risk**: Low  
**Impact**: Improves code clarity

---

### **Phase 2: Type Safety (High Priority, Medium Risk)**
1. **Replace string[,,] map with enum-based system**
   - Create `TileType` enum
   - Refactor MapCore, FieldOfView, PathFinding
   - Update tests
2. **Add nullable reference types** (`#nullable enable`)

**Effort**: 3-5 days  
**Risk**: Medium (breaking change)  
**Impact**: Eliminates entire class of bugs

---

### **Phase 3: Architecture Refactoring (Critical, High Risk)**
1. **Introduce interfaces**
   - `IPathFinder`, `IFieldOfView`, `ICombatResolver`
   - Extract interface from existing static classes
2. **Refactor Mission god object**
   - Create `TurnManager`, `CombatService`, `MovementService`
   - Use constructor injection
3. **Split Character class**
   - `CharacterStats`, `CharacterPosition`, `CharacterVision`

**Effort**: 1-2 weeks  
**Risk**: High (major refactoring)  
**Impact**: Testability, maintainability, extensibility

---

### **Phase 4: Flexibility Enhancements (Medium Priority)**
1. **Support N teams** (remove hardcoded 2-team limit)
2. **Strategy pattern for AI**
3. **Parameter objects** for complex methods

**Effort**: 3-5 days  
**Risk**: Medium  
**Impact**: Game design flexibility

---

### **Phase 5: Polish (Low Priority)**
1. **Add logging framework**
2. **Expand performance profiling**
3. **Reorganize tests**
4. **Consider upgrading to netstandard2.1**

**Effort**: 2-3 days  
**Risk**: Low  
**Impact**: Developer experience

---

## Architecture Smells Detected

| Smell | Severity | Location | Fix Effort |
|-------|----------|----------|------------|
| God Object | 🔴 High | Mission.cs | High |
| Primitive Obsession | 🔴 High | string[,,] map | Medium |
| Feature Envy | 🟡 Medium | Character manipulated externally | Medium |
| Shotgun Surgery | 🟡 Medium | Map changes scattered | Low |
| Duplicated Code | 🟢 Low | Cover calculation | Low |
| Magic Numbers | 🟢 Low | Throughout | Low |
| Long Method | 🟡 Medium | CharacterAI.AssignPointsToEachTile | Medium |
| Long Parameter List | 🟢 Low | MoveCharacter, AttackCharacter | Low |

---

## Code Metrics Summary

| Metric | Value | Assessment |
|--------|-------|------------|
| **Total LOC (Logic)** | ~4,500 | Manageable |
| **Largest File** | 352 LOC (Encounter.cs) | Acceptable |
| **Static Classes** | 21 | Too many |
| **Interfaces** | 0 | ⚠️ Critical gap |
| **Test Pass Rate** | 98.9% (274/277) | Excellent |
| **External Dependencies** | 2 | Minimal, good |
| **Cyclomatic Complexity** | Moderate | Some complex methods |

---

## Testing Observations

### ✅ **Strengths**
- 274 passing tests
- Scenario-based integration tests
- Deterministic testing via RandomNumberQueue
- Good edge case coverage (AI crash tests, null checks)

### ⚠️ **Gaps**
- 3 failing tests (Linux path separator issues - minor)
- No tests for Research domain
- Limited tests for serialization edge cases
- No performance regression tests

---

## Security Considerations

### ✅ **Good Practices**
- No SQL injection risk (no database)
- No user input handling (game engine)
- Serialization uses Newtonsoft.Json (trusted library)

### ⚠️ **Watch Areas**
- **Serialization**: Ensure save files validated before deserialization
- **Path Traversal**: If adding file I/O, validate paths
- **Exception Information**: Don't expose stack traces to players

---

## Unity Integration Considerations

**Current**: `netstandard2.0` target is Unity-compatible ✅

**Recommendations for Unity**:
1. **Keep Logic layer pure** (no Unity dependencies) ✅ Already doing this
2. **Add event system** for Unity to subscribe to game events
3. **Consider IL2CPP compatibility** - avoid reflection where possible
4. **Test on mobile** - avoid large allocations in hot paths

---

## Performance Considerations

**Not Analyzed in Detail** (requires profiling), but observations:

### Potential Bottlenecks:
1. **String array cloning**: `(string[,,])map.Clone()` - frequent allocations
2. **FOV recalculation**: Called every movement step
3. **AI tile evaluation**: O(n²) in some cases
4. **LINQ usage**: Some allocations in hot paths

### Recommendations:
1. **Object pooling** for commonly allocated objects
2. **Cache FOV results** when possible
3. **Benchmark critical paths** with BenchmarkDotNet
4. **Consider struct-based value types** for Vector3-heavy code

---

## Maintainability Score

| Category | Score | Notes |
|----------|-------|-------|
| Code Organization | 8/10 | Clean domain structure |
| Readability | 7/10 | Some complex methods |
| Testability | 6/10 | Limited by static classes |
| Modularity | 6/10 | Good separation, but tight coupling |
| Documentation | 5/10 | Some XML docs, needs more |
| Extensibility | 5/10 | Hard to extend without refactoring |
| **Overall** | **6.5/10** | **Solid foundation, needs architecture work** |

---

## Comparison to Industry Standards

**Compared to typical Unity game engines:**
- ✅ Better separation of concerns than most
- ✅ Excellent determinism (rare in game dev)
- ✅ Strong test coverage (rare in game dev)
- ⚠️ Lacks dependency injection (common in Unity)
- ⚠️ Primitive obsession (strings for tiles)

**Compared to enterprise .NET applications:**
- ⚠️ No interfaces or DI
- ⚠️ Static classes instead of services
- ⚠️ No repository pattern
- ✅ Clean domain organization
- ✅ Good test coverage

---

## Conclusion

The TurnBasedEngine is a **well-crafted foundation** for a tactical game engine with:
- ✅ Clean domain organization
- ✅ Excellent deterministic design
- ✅ Strong test coverage
- ✅ Pure logic layer (Unity-ready)

**However**, it would benefit significantly from:
1. **Architecture refactoring** to break up the Mission god object
2. **Introducing interfaces and DI** for testability
3. **Type-safe map representation** instead of strings
4. **Splitting the Character class** into focused components

### Recommended Next Steps:
1. ✅ **Accept current architecture** for immediate use
2. 🔄 **Plan Phase 1-2 refactoring** (low-risk improvements)
3. 🔄 **Prototype interface-based design** before committing to Phase 3
4. 📊 **Run performance profiling** before optimization

---

## Additional Resources

**Recommended Reading:**
- *Clean Architecture* by Robert C. Martin (SOLID principles)
- *Domain-Driven Design* by Eric Evans (domain modeling)
- *Dependency Injection Principles, Practices, and Patterns* by Steven van Deursen

**Recommended Tools:**
- **SonarQube** - Static analysis (already configured but disabled)
- **BenchmarkDotNet** - Performance profiling
- **NDepend** - Code metrics and dependency analysis
- **ReSharper** - Code quality analysis

---

**Report Generated**: 2026-02-01  
**Total Issues Identified**: 19  
**Critical**: 3 | **High**: 9 | **Medium**: 4 | **Low**: 3
