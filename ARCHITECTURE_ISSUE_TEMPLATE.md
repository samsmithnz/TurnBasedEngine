---
name: Architecture Improvements
about: Comprehensive architecture review findings and improvement roadmap
title: "[ARCHITECTURE] Improve codebase architecture and design patterns"
labels: enhancement, architecture, technical-debt
assignees: ''
---

## 📋 Architecture Review Summary

A comprehensive architecture review was conducted on the TurnBasedEngine project. The full detailed report can be found in [`ARCHITECTURE_REVIEW.md`](./ARCHITECTURE_REVIEW.md).

**Overall Assessment**: ⭐⭐⭐⭐☆ (4/5)
- Strong foundation with clean domain organization
- Excellent deterministic design and test coverage
- Some architectural improvements needed for long-term maintainability

---

## 🔴 Critical Issues (Must Address)

### 1. Mission "God Object" Anti-Pattern
**Severity**: 🔴 Critical | **File**: `Game/Mission.cs` (321 LOC)

**Problem**: The Mission class has too many responsibilities:
- Turn orchestration
- Team management  
- Movement coordination
- Attack coordination
- AI calculation
- Objective checking
- Map manipulation

**Impact**: Hard to test, violates Single Responsibility Principle, tight coupling

**Recommended Solution**:
```csharp
public class TurnManager { }           // Turn/phase management
public class CombatService { }         // Attack orchestration
public class MovementService { }       // Movement orchestration
public class ObjectiveEvaluator { }    // Win condition checking
public class MissionOrchestrator {     // Coordinates services
    private readonly TurnManager _turnManager;
    private readonly CombatService _combatService;
    // ...
}
```

**Effort**: 1-2 weeks | **Risk**: High (major refactoring)

---

### 2. No Interfaces or Dependency Injection
**Severity**: 🔴 Critical | **Files**: Entire codebase

**Problem**:
- 0 interfaces found in the codebase
- 21 static utility classes
- All dependencies are concrete types
- Cannot mock for testing
- Tight coupling between layers

**Recommended Solution**:
```csharp
// Introduce abstractions
public interface IPathFinder {
    PathFindingResult FindPath(string[,,] map, Vector3 start, Vector3 end);
}

public interface ICharacterAI {
    AIAction CalculateAction(Character character, Team sourceTeam, Team opponentTeam);
}

// Enable dependency injection
public class Mission {
    private readonly IPathFinder _pathFinder;
    private readonly ICharacterAI _aiEngine;
    
    public Mission(IPathFinder pathFinder, ICharacterAI aiEngine) {
        _pathFinder = pathFinder;
        _aiEngine = aiEngine;
    }
}
```

**Effort**: 1-2 weeks | **Risk**: High

---

### 3. String-Based Map Representation (Type Safety Issue)
**Severity**: 🔴 Critical | **Files**: `Map/MapCore.cs`, `Mission.cs`

**Problem**:
```csharp
public string[,,] Map { get; set; }  // Uses magic strings: "■", "□", ".", "P", "T"
```

**Issues**:
- No type safety - can assign any string
- Magic strings scattered throughout
- Error-prone string comparisons
- No IntelliSense support
- Hard to extend

**Recommended Solution**:
```csharp
// Option 1: Enum-based
public enum TileType {
    Empty = 0,
    NoCover = 1,
    HalfCover = 2,
    FullCover = 3,
    Character = 4,
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
```

**Effort**: 3-5 days | **Risk**: Medium (breaking change)

---

## 🟡 High Priority Issues

### 4. Character Class Does Too Much
**Severity**: 🟡 High | **File**: `Characters/Character.cs` (324 LOC)

**Problem**: 30+ properties mixing stats, location, FOV, targeting, actions

**Recommendation**: Split into focused classes:
```csharp
public class CharacterStats { /* HP, AP, etc. */ }
public class CharacterPosition { /* Location, cover */ }
public class CharacterVision { /* FOV, history */ }
public class Character {
    public CharacterStats Stats { get; set; }
    public CharacterPosition Position { get; set; }
    public CharacterVision Vision { get; set; }
}
```

**Effort**: 3-4 days | **Risk**: Medium

---

### 5. Hardcoded Two-Team Limit
**Severity**: 🟡 High | **File**: `Game/Mission.cs` lines 147-150

**Problem**:
```csharp
if (Teams.Count != 2) {
    throw new Exception("Unexpected number of teams: " + Teams.Count);
}
```

**Impact**: Cannot support 3+ team missions, FFA, or co-op modes

**Recommendation**: Support N teams with dynamic opponent resolution

**Effort**: 1 day | **Risk**: Low

---

### 6. Excessive Mutable State
**Severity**: 🟡 High | **Files**: `Character.cs`, `Mission.cs`

**Problem**: Public setters everywhere, direct map mutation, hard to track changes

**Recommendation**: 
- Use private setters with methods for state changes
- Consider immutable updates or event-based changes
- Add validation in setters

**Effort**: 1 week | **Risk**: Medium

---

### 7. Unresolved TODO Comments
**Severity**: 🟡 High | **File**: `Characters/Character.cs` line 51

```csharp
//TODO: Location should never be set here- but I need this for serialization.
public Vector3 Location { get; set; }
```

**Recommendation**: Use Data Transfer Objects (DTOs) for serialization

**Effort**: 1 day | **Risk**: Low

---

## 🟢 Medium Priority Issues

### 8. No Exception Handling Strategy
**Severity**: 🟢 Medium

**Problem**: Generic `Exception` thrown, inconsistent null vs exception returns

**Recommendation**: 
- Create custom exception types
- Use consistent error handling
- Consider nullable reference types (C# 8+)

---

### 9. CharacterAI Class Complexity
**Severity**: 🟢 Medium | **File**: `Characters/CharacterAI.cs` (286 LOC)

**Problem**: Large methods, complex nested logic, hard to extend

**Recommendation**: Strategy pattern for AI behaviors
```csharp
public interface IAIBehavior {
    AIAction Evaluate(Character character, GameState state);
}
```

---

### 10. Magic Numbers Scattered Throughout
**Severity**: 🟢 Medium

**Examples**:
```csharp
toHit -= 20;  // Half cover penalty - what does 20 mean?
toHit -= 40;  // Full cover penalty
character.XP += 100;  // XP for mission
```

**Recommendation**: Create `GameConstants` class

---

## 🔵 Low Priority / Quality of Life

### 11. Long Parameter Lists
**Example**: `MoveCharacter(map, character, pathResult, team1, team2, dice)` - 6 parameters

**Recommendation**: Use parameter objects/contexts

---

### 12. Test Organization
**Recommendation**: Organize into Unit/Integration/Scenarios folders

---

### 13. Commented-Out Code
**Files**: `Character.cs` (lines 138-162, 275-305)

**Recommendation**: Remove and rely on git history

---

### 14. No Structured Logging
**Recommendation**: Add logging abstraction for debugging and combat logs

---

### 15. Research System Underutilized
**Recommendation**: Either fully implement or remove the Research domain

---

## 📊 Code Metrics

| Metric | Value | Status |
|--------|-------|--------|
| Total Lines (Logic) | ~4,500 | ✅ Manageable |
| Test Pass Rate | 98.9% (274/277) | ✅ Excellent |
| Static Classes | 21 | ⚠️ Too many |
| Interfaces | 0 | ❌ Critical gap |
| Largest File | 352 LOC | ✅ Acceptable |
| External Dependencies | 2 | ✅ Minimal |

---

## 🗺️ Recommended Refactoring Roadmap

### Phase 1: Foundation (1-2 days, Low Risk) ✅
- [ ] Extract magic numbers to constants
- [ ] Add XML documentation to public APIs
- [ ] Remove commented code
- [ ] Fix TODO items
- [ ] Centralize exception types

### Phase 2: Type Safety (3-5 days, Medium Risk) 🔶
- [ ] Replace `string[,,]` map with enum-based system
- [ ] Add nullable reference types (`#nullable enable`)
- [ ] Create DTOs for serialization

### Phase 3: Architecture (1-2 weeks, High Risk) 🔴
- [ ] Introduce interfaces (IPathFinder, IFieldOfView, ICombatResolver)
- [ ] Refactor Mission god object into services
- [ ] Split Character class into focused components
- [ ] Add dependency injection support

### Phase 4: Flexibility (3-5 days, Medium Risk) 🔶
- [ ] Support N teams (remove 2-team hardcoding)
- [ ] Strategy pattern for AI behaviors
- [ ] Parameter objects for complex methods

### Phase 5: Polish (2-3 days, Low Risk) ✅
- [ ] Add logging framework
- [ ] Expand performance profiling
- [ ] Reorganize test structure
- [ ] Consider netstandard2.1 upgrade

---

## 🎯 Quick Wins (Can Do Immediately)

1. **Extract Constants** - 2 hours
   ```csharp
   public static class GameConstants {
       public const int HALF_COVER_PENALTY = 20;
       public const int FULL_COVER_PENALTY = 40;
   }
   ```

2. **Remove Hardcoded Team Limit** - 4 hours
   - Allows 3+ team missions

3. **Fix TODO Comments** - 2 hours
   - Add DTO for Character serialization

4. **Add XML Docs** - 4 hours
   - Document public APIs

---

## 💪 Strengths to Maintain

- ✅ **Clean domain organization** - Keep the 8-domain structure
- ✅ **Deterministic RNG** - RandomNumberQueue is excellent for replay
- ✅ **Strong test coverage** - 274 tests, scenario-based
- ✅ **Pure logic layer** - No Unity dependencies (good for portability)
- ✅ **FOV history tracking** - Well-designed fog-of-war system

---

## 📚 Resources

**Full Report**: See [`ARCHITECTURE_REVIEW.md`](./ARCHITECTURE_REVIEW.md) for detailed analysis

**Recommended Reading**:
- *Clean Architecture* by Robert C. Martin
- *Domain-Driven Design* by Eric Evans
- *Dependency Injection Principles* by Steven van Deursen

**Recommended Tools**:
- SonarQube (already configured)
- BenchmarkDotNet (performance profiling)
- NDepend (code metrics)

---

## 🤝 Contributing

This is a comprehensive list. Contributions are welcome for any of these improvements!

**Priority Order**: 
1. Critical issues (god object, interfaces, type safety)
2. High priority (Character class, team limit, mutable state)
3. Medium priority (exceptions, AI, constants)
4. Low priority (quality of life improvements)

**Next Steps**:
1. Review and discuss priorities
2. Create individual issues for accepted improvements
3. Start with Phase 1 (low-risk foundation work)
4. Prototype Phase 3 changes before committing

---

**Generated**: 2026-02-01  
**Total Issues**: 19 (Critical: 3, High: 6, Medium: 6, Low: 4)
