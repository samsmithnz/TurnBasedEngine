---
name: Architecture Improvements (Game-Focused)
about: Pragmatic improvements for game engine, not enterprise patterns
title: "[ARCHITECTURE] Game-focused improvements"
labels: enhancement, architecture
assignees: ''
---

## 📋 Architecture Review Summary

A game-focused architecture review was conducted on the TurnBasedEngine project. Full report: [`ARCHITECTURE_REVIEW_GAME_FOCUSED.md`](./ARCHITECTURE_REVIEW_GAME_FOCUSED.md)

**Overall Assessment**: ⭐⭐⭐⭐☆ (4/5) - Excellent for a game engine

**Key Finding**: This architecture is **appropriate for games**. Static classes, central orchestrator, and concrete dependencies are **correct choices** for game performance and simplicity.

---

## 🎯 Recommended Improvements (Game-Focused)

### 🔴 High Priority (Do These)

#### 1. Replace String-Based Map with Enum
**Priority**: 🔴 High | **Effort**: 3-5 days | **Risk**: Bug prevention

**Problem**: Magic strings are bug-prone
```csharp
public string[,,] Map { get; set; }  // "■", "□", ".", "P", "T"
map[x, y, z] = "■";  // Typo = silent bug
```

**Solution**:
```csharp
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
```

**Benefits**:
- Type safety (compiler catches errors)
- Better performance (int vs string comparison)
- Memory savings (byte vs string reference)
- Unity-friendly (enums serialize well)

---

#### 2. Extract Magic Numbers to GameBalance
**Priority**: 🟡 Medium | **Effort**: 2-4 hours | **Risk**: None

**Problem**: Hard to tune game balance
```csharp
toHit -= 20;  // What's 20?
toHit -= 40;  // What's 40?
character.XP += 100;  // What's 100?
```

**Solution**:
```csharp
public static class GameBalance {
    // Cover
    public const int HALF_COVER_PENALTY = 20;
    public const int FULL_COVER_PENALTY = 40;
    
    // Combat
    public const float OVERWATCH_ACCURACY_MULT = 0.7f;
    public const int FLANK_CRIT_BONUS = 50;
    
    // Progression
    public const int XP_MISSION_COMPLETE = 100;
}
```

**Benefits**:
- Easy balance tuning
- Better for modding
- Clear intent

---

#### 3. Remove 2-Team Hardcoding
**Priority**: 🟡 Medium | **Effort**: 4 hours | **Risk**: Design flexibility

**Problem**: Limits game modes
```csharp
if (Teams.Count != 2) {
    throw new Exception("Unexpected number of teams");
}
```

**Solution**: Support N teams
```csharp
public void StartMission() {
    if (Teams.Count < 2) {
        throw new InvalidOperationException("Need at least 2 teams");
    }
    
    for (int i = 0; i < Teams.Count; i++) {
        var opponents = GetOpponentsForTeam(i);
        Teams[i].UpdateTargets(Map, opponents);
    }
}
```

**Benefits**:
- 3-way battles
- Co-op missions
- Future game modes

---

#### 4. Fix TODO Comments
**Priority**: 🟢 Low | **Effort**: 2 hours

**TODOs Found**:
```csharp
// Character.cs:51
//TODO: Location should never be set here- but I need this for serialization.

// FieldOfView.cs:190  
//TODO: Fix this - but for now, it will work for missed shots
```

**Solution**: Use `[JsonProperty]` for serialization, fix or document FOV math

---

## 🚫 Don't Do These (Over-Engineering)

### ❌ Add Dependency Injection
**Why not**: 
- Performance cost (interface dispatch at 60fps)
- Complexity (unnecessary for 4,500 LOC)
- Unity doesn't use DI by default
- **Current testing works fine** (274 tests)

**Verdict**: Static classes and concrete types are **correct for games**

---

### ❌ Split Mission into Services
**Why not**:
- Mission orchestrator is **normal for games**
- Single source of truth for game state
- Easy Unity integration
- Already well-tested (274 tests)

**Verdict**: Mission class is **appropriate as-is**

---

### ❌ Add Interfaces Everywhere
**Why not**:
- Performance overhead (virtual dispatch)
- Unnecessary abstraction
- Static utility classes are **perfect for pure algorithms** (PathFinding, FieldOfView)

**Verdict**: Keep static classes for algorithms

---

## ✅ What's Already Good (Don't Change)

1. **Mission orchestrator** - Normal for game state management
2. **Static utility classes** - Performance benefit for games
3. **Concrete dependencies** - Simplicity over abstraction
4. **Strong test coverage** - 274 tests (rare in games!)
5. **Clean domain organization** - 8 game systems
6. **Deterministic RNG** - RandomNumberQueue is excellent
7. **Unity-ready** - netstandard2.0, no Unity dependencies

---

## 📊 Metrics (For Context)

| Metric | Value | Game Context |
|--------|-------|--------------|
| Test Coverage | 274/277 (98.9%) | ✅ Rare in games |
| Static Classes | 21 | ✅ Good for performance |
| DI/Interfaces | 0 | ✅ Appropriate for games |
| Mission LOC | 321 | ✅ Normal for orchestrator |
| External Deps | 2 | ✅ Minimal |

---

## 🗺️ Recommended Action Plan

### Week 1: Type Safety
- [ ] Replace `string[,,]` map with `TileType` enum
- [ ] Update MapCore, FieldOfView, PathFinding
- [ ] Update all tests
- [ ] Verify performance (should be better)

### Day 1: Constants & Cleanup  
- [ ] Create GameBalance class with all constants
- [ ] Replace magic numbers
- [ ] Fix TODO comments
- [ ] Remove 2-team hardcoding

### Optional: Polish
- [ ] Add XML docs to public APIs (for Unity devs)
- [ ] Expand GameConstants for modding
- [ ] Document Unity integration patterns

**Total Effort**: ~1 week  
**Expected Result**: 4.5/5 stars

---

## 🎮 Game Development Context

**Important**: This is a **game engine**, not enterprise software.

**What's different**:
- Performance > Abstraction
- Simplicity > Flexibility  
- Direct calls > Layers
- Concrete > Interfaces
- Static utils > Services

**Why Mission class is fine**:
- Games need central orchestration
- Unity integration is simpler
- Testing already works (274 tests)
- Performance is better

**Why static classes are fine**:
- Pure algorithms (PathFinding, FieldOfView)
- No side effects
- Better performance
- Easy to test

**Why no DI is fine**:
- Unity doesn't use DI by default
- Overhead at 60fps matters
- Simplicity for contributors
- Already testable

---

## 🚀 Quick Wins (Can Start Today)

1. **Extract constants** (2h) - Easy balance tuning
2. **Remove 2-team limit** (4h) - Opens game design
3. **Fix TODOs** (2h) - Clean up tech debt

---

## 📚 Full Report

See [`ARCHITECTURE_REVIEW_GAME_FOCUSED.md`](./ARCHITECTURE_REVIEW_GAME_FOCUSED.md) for:
- Detailed explanations
- Code examples
- Unity integration notes
- Performance considerations
- What NOT to change

---

**Generated**: 2026-02-01  
**Context**: Game development, not enterprise  
**Focus**: Pragmatic improvements only  
**Total Issues**: 4 practical improvements (not 19 enterprise patterns)
