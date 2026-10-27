# AI Action System: Design Document & Enhancement Roadmap

## Overview

This document describes all combat actions available to characters in TurnBasedEngine, their AP costs, turn-ending behavior, current AI support status, and next-step enhancement prompts. The AI returns one action at a time; movement spends its listed AP and the AI can be evaluated again with any AP left. Attacks and overwatch consume all remaining AP and end the turn.

---

## Action Reference Table

| # | Action | AP Cost | Ends Turn? | Currently in AI | `ActionTypeEnum` Value |
|---|--------|---------|-----------|-----------------|----------------------|
| 1 | **Move** | 1 (or 2 for a long move) | ❌ No | ✅ Yes (`Move`, `DoubleMove`) | `Move = 5`, `DoubleMove = 1` |
| 2 | **Shoot** | Remaining AP | ✅ Yes | ✅ Yes (`StayInPositionThenAttack`) | `StayInPositionThenAttack = 3` |
| 3 | **Overwatch** | Remaining AP | ✅ Yes | ⚠️ Partial (fallback when no enemies are visible) | `Overwatch = 6` |
| 4 | **Hunker Down** | 1 | ✅ Yes | ❌ No | ❌ Missing |
| 5 | **Grenade** | Ends turn | ✅ Yes | ❌ No | ❌ Missing |
| 6 | **MediKit** | 1 | ❌ No | ❌ No | ❌ Missing |
| 7 | **Reload** | 1 | ❌ No | ❌ No (no reload action) | ❌ Missing |
| 8 | **Special Ability** | 1 | ✅ Yes | ❌ No | ❌ Missing |

---

## Action Detail & AI Enhancement Prompts

---

### 1. 🚶 Move
**AP Cost:** 1 | **Ends Turn:** No

**Current State:** Implemented as an independent action. A single move costs 1 AP; a long move costs 2 AP. The AI scores each affordable move and returns one action, allowing it to be evaluated again with the remaining AP.

**Gaps:**
- No bonus for moving toward objectives or enemies when out of range
- No penalty for moving into overwatch zones

**Enhancement Prompts:**
```
"Add overwatch zone avoidance to AI movement scoring - apply a larger penalty 
when a tile is within an enemy's active overwatch coverage area"

"Add an 'advance toward nearest enemy' bonus when no enemies are visible - 
score tiles closer to known/last-seen enemy positions higher"
```

---

### 2. 🔫 Shoot
**AP Cost:** Remaining AP | **Ends Turn:** Yes

**Current State:** Implemented as an independent `StayInPositionThenAttack` action. The AI scores each visible target separately using a 6-tier hit-chance system; attacks consume all remaining AP and end the turn.

**Gaps:**
- No target prioritization (e.g. lowest HP, highest threat)
- The AI does not evaluate a move followed by a later attack as one plan; it returns one action and re-evaluates after movement

**Enhancement Prompts:**
```
"Create an AITargetPrioritizer class that ranks enemy targets by: 
  1. Lowest HP (finish them off), 
  2. Highest threat (most damage potential), 
  3. Closest range"
```

---

### 3. 👁️ Overwatch
**AP Cost:** Remaining AP | **Ends Turn:** Yes

**Current State:** `Overwatch` is scored as a fallback when no enemies are visible and ammunition is available. It is an independent turn-ending action, not bundled with movement.

**Gaps:**
- Overwatch is never chosen when enemies are visible
- No scoring for the quality of the overwatch zone (wide FOV, choke points)
- The `OVERWATCH_AIM_MULTIPLIER = 0.7f` and `OVERWATCH_CRITICAL_CHANCE = 0` constants exist but are not used in AI scoring

**Enhancement Prompts:**
```
"Score Overwatch as a deliberate defensive action when the AI character is in 
full cover and no high-value attack exists - apply a bonus proportional to 
the number of tiles the overwatch covers"

"Add a choke-point overwatch bonus - increase overwatch score when the 
character's shooting range covers a narrow passage or high-traffic corridor"

"When scoring MoveThenOverwatch, apply the OVERWATCH_AIM_MULTIPLIER to 
estimate the effective hit chance, so the AI understands overwatch shots 
are weaker than direct attacks"
```

---

### 4. 🛡️ Hunker Down
**AP Cost:** 1 | **Ends Turn:** Yes

**Current State:** `HunkeredDown` property exists on `Character`. Cover penalties double when hunkered (`HUNKERED_FULL_COVER_PENALTY = 80`, `HUNKERED_HALF_COVER_PENALTY = 40`). The `Character.GetCurrentActions()` method returns hunker down only when in cover. AI has **no implementation** — was commented out.

**Gaps:**
- No `ActionTypeEnum` value for `HunkerDown`
- No AI scoring logic
- No conditions defined for when the AI should prefer hunker vs. attack

**Enhancement Prompts:**
```
"Add HunkerDown = 5 to ActionTypeEnum and add AI_HUNKER_DOWN_BONUS / 
AI_HUNKER_DOWN_THRESHOLD constants to GameConstants"

"Implement Hunker Down scoring in AssignPointsToEachTile:
  - Only score it when character is in full or half cover
  - Score it highly when character HP is low (below 30% of max)
  - Score it highly when no good attack options exist (chanceToHit < 50)"

"Add an IsLowHealth helper to Character that returns true when 
HitpointsCurrent < (HitpointsMax * 0.33) for use in AI defensive decisions"
```

---

### 5. 💣 Grenade
**AP Cost:** Ends turn | **Ends Turn:** Yes

**Current State:** Grenades are stored in `UtilityWeaponEquipped`. `Character.GetCurrentActions()` exposes a `_throw_grenade` action when `UtilityWeaponEquipped != null`. Grenades have area-effect damage (via `AreaEffectRadius`). AI has **no implementation**.

**Gaps:**
- No `ActionTypeEnum` value for `ThrowGrenade`
- No AI awareness of `UtilityWeaponEquipped` or its ammo
- No scoring based on enemy clustering (grenades are powerful against grouped enemies)
- No line-of-sight validation for grenade throws
- `AttackCharacterWithAreaOfEffect` consumes all remaining AP, so grenade actions end the turn

**Enhancement Prompts:**
```
"Add ThrowGrenade = 9 to ActionTypeEnum and add AI_GRENADE_CLUSTERING_BONUS / 
AI_GRENADE_SCORE constants to GameConstants"

"Implement Grenade scoring in AssignPointsToEachTile:
  - Only score if UtilityWeaponEquipped is a Grenade with ammo remaining
  - For each visible enemy, count how many other enemies are within 
    AreaEffectRadius of that target
  - Apply a bonus proportional to the number of enemies in the blast radius
  - Score grenades higher against clusters of enemies vs. single targets"

"Add grenade line-of-sight validation - ensure the throw path is not 
completely blocked by terrain (grenades arc over low cover)"

"Only consider grenade chaining after the combat rules are changed so a grenade
does not consume all remaining AP; under current rules, a grenade ends the turn"
```

---

### 6. 💊 MediKit
**AP Cost:** 1 | **Ends Turn:** No

**Current State:** `ItemPool.CreateMedKit()` exists, `Character.UseItem()` heals `item.Adjustment` HP and decrements `ClipRemaining`. `Character.GetCurrentActions()` exposes the medkit action when `UtilityItemEquipped` has charges. AI has **no implementation**.

**Gaps:**
- No `ActionTypeEnum` value for `UseMediKit`
- No AI awareness of `UtilityItemEquipped` 
- No HP threshold logic for deciding when to heal
- The AI does not yet score the medkit as an independent action

**Enhancement Prompts:**
```
"Add UseMediKit = 6 to ActionTypeEnum. Add AI_MEDKIT_HP_THRESHOLD and 
AI_MEDKIT_SCORE constants to GameConstants"

"Implement MediKit scoring in AssignPointsToEachTile:
  - Only consider it when UtilityItemEquipped is a MedKit with ClipRemaining > 0
  - Score it when HitpointsCurrent is below AI_MEDKIT_HP_THRESHOLD
  - Score it higher as HP gets lower (inverse relationship)
  - Score it as an independent action using the character's current state"

"Return the medkit action independently, then re-evaluate available actions"
```

---

### 7. 🔄 Reload
**AP Cost:** 1 | **Ends Turn:** No

**Current State:** `Weapon.Reload()` sets `AmmoCurrent = AmmoMax`. `GetCurrentActions()` shows reload when `AmmoCurrent == 0`. The AI does not yet score reload; attack scoring is skipped when the weapon is empty.

**Gaps:**
- No `ActionTypeEnum` for `Reload`
- AI does not yet score a reload action when the gun is empty
- No logic for deciding whether to reload vs. use a fallback action
- Reload is an independent action; the AI can re-evaluate the remaining options afterward

**Enhancement Prompts:**
```
"Add Reload to ActionTypeEnum and score it as an independent action when
the weapon is empty, then re-evaluate available actions"

"Add an AI_LOW_AMMO_THRESHOLD constant (e.g. 1 round remaining) to 
GameConstants - score a tactical retreat/reload more highly when 
approaching empty rather than waiting for fully empty"
```

---

### 8. ⚡ Special Ability
**AP Cost:** 1 | **Ends Turn:** Yes

**Current State:** `AbilityPool` defines passive abilities (`ArmorPiercing`, `Shredder`, `CriticalDamage` etc.) that modify combat calculations. These are **passive stat modifiers**, not active abilities the AI explicitly chooses. No active ability action type exists. AI has **no implementation**.

**Gaps:**
- No active-use ability distinction vs. passive modifiers
- No `ActionTypeEnum` for `UseAbility`
- No AI scoring for when/how to use an active ability

**Enhancement Prompts:**
```
"Distinguish passive vs. active abilities by adding an IsActiveAbility bool
to the existing Ability class"

"Add UseSpecialAbility = 8 to ActionTypeEnum. Implement AI ability 
scoring based on ability type:
  - ArmorPiercing: prefer against high-armor targets
  - Shredder: prefer to use early in combat to weaken enemies
  - CriticalDamage: prefer when already have a high base hit chance"

"Create an AIAbilityEvaluator static class that takes a Character and 
an opponent Character and returns a score modifier for each available 
active ability, callable from AssignPointsToEachTile"
```

---

## Suggested Implementation Order

```
1. Add independent movement and attack actions
        |
        v
2. Add Reload action scoring
        |
        v
3. Add MediKit action scoring
        |
        v
4. Improve Overwatch scoring
        |
        v
5. Add Hunker Down + IsLowHealth helper
        |
        v
6. Add Grenade clustering detection and scoring
        |
        v
7. Target prioritization system
        |
        v
8. Active Special Abilities
```

---

## New `ActionTypeEnum` Target State

The legacy compound values remain in the enum for compatibility but are no longer returned by the AI. Independent action values planned for the remaining work are:

```csharp
public enum ActionTypeEnum
{
    Unknown = 0,
    DoubleMove = 1,
    MoveThenAttack = 2,
    StayInPositionThenAttack = 3,
    MoveThenOverwatch = 4,
    Move = 5,
    Overwatch = 6,
    HunkerDown = 7,
    UseMediKit = 8,
    Reload = 9,
    UseSpecialAbility = 10,
    ThrowGrenade = 11,
}
```

---

## Related Source Files

| File | Purpose |
|------|---------|
| `TBE.Logic/Characters/CharacterAI.cs` | Core AI decision logic — `CalculateAIAction`, `AssignPointsToEachTile` |
| `TBE.Logic/Encounters/AIAction.cs` | `AIAction` result object and `ActionTypeEnum` |
| `TBE.Logic/GameConstants.cs` | All AI scoring constants (`AI_FULL_COVER_BONUS`, thresholds, etc.) |
| `TBE.Logic/Characters/Character.cs` | Character state (`HunkeredDown`, `InOverwatch`, `UtilityItemEquipped`, `WeaponEquipped`) |
| `TBE.Logic/Items/Weapon.cs` | `AmmoCurrent`, `AmmoMax`, `Reload()` |
| `TBE.Logic/Items/ItemPool.cs` | `CreateMedKit()` factory |
| `TBE.Logic/AbilitiesAndEffects/AbilityPool.cs` | Passive ability definitions |
| `TBE.Logic/Map/FieldOfView.cs` | FOV and character visibility used during AI tile evaluation |
| `TBE.Logic/Encounters/EncounterCore.cs` | `GetChanceToHit()` used in AI attack scoring |
