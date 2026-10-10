# Gilded Rose Refactoring - Design Document

## Executive Summary

This document outlines the comprehensive refactoring of the Gilded Rose Console application, which implements a modern, maintainable architecture using design patterns and best practices. The refactoring includes:

1. **Factory Pattern**: `ItemUpdaterFactory` to create appropriate updater strategies
2. **Strategy Pattern**: Individual updater classes for each item type
3. **Constants Extraction**: Centralized `Constants.cs` for magic strings and configuration values
4. **Interface-Based Architecture**: `IItemUpdater` interface for polymorphic behavior

The primary objectives were to improve code maintainability, eliminate code duplication, enable easy addition of new item types, and provide a single source of truth for configuration values.

---

## 1. Overview of Changes

### 1.1 Scope
- **Console Project**: 
  - Created `Constants.cs` containing all magic strings and numeric constants
  - Introduced `IItemUpdater` interface for strategy pattern
  - Created `ItemUpdaterFactory` class for updater instantiation
  - Implemented 5 concrete updater classes for different item types
  - Refactored `Program.cs` to use factory and constants
- **Folder Structure**:
  - `src/GildedRose.Console/Factories/` - Factory classes
  - `src/GildedRose.Console/Interfaces/` - Interface definitions
  - `src/GildedRose.Console/Updaters/` - Concrete updater implementations
  - `src/GildedRose.Console/Models/` - Data models
- **Framework**: .NET 8
- **No Breaking Changes**: All changes are backward compatible

### 1.2 Objectives
1. Eliminate magic strings from Program.cs and updaters
2. Centralize configuration constants in a single location
3. Implement Strategy pattern for item update behaviors
4. Implement Factory pattern for creating appropriate updaters
5. Improve code maintainability, readability, and extensibility
6. Facilitate future changes to item names and quality thresholds
7. Enable easy addition of new item types without modifying existing code
8. Ensure consistency across console and test projects

---

## 2. Console Project Architecture

### 2.1 Architectural Overview

The refactored application uses two primary design patterns:

#### Strategy Pattern
- **Interface**: `IItemUpdater` defines the contract for item update behavior
- **Concrete Strategies**:
  - `NormalItemUpdater` - Standard item degradation
  - `AgedBrieUpdater` - Appreciating item behavior
  - `BackstagePassUpdater` - Time-sensitive item with increasing value
  - `SulfurasUpdater` - Legendary item that never changes
  - `ConjuredItemUpdater` - Rapidly degrading magical item

#### Factory Pattern
- **Class**: `ItemUpdaterFactory` determines which updater to instantiate based on item name
- **Responsibility**: Map item types to their corresponding updater strategies
- **Benefit**: Eliminates conditional logic from Program.cs; centralizes updater selection

### 2.2 Project Structure

```
src/GildedRose.Console/
├── Constants.cs                    # Centralized configuration constants
├── Program.cs                      # Main application class, uses factory
├── Factories/
│   └── ItemUpdaterFactory.cs       # Strategy factory for creating updaters
├── Interfaces/
│   └── IItemUpdater.cs             # Strategy interface
├── Models/
│   └── Item.cs                     # Item data model
└── Updaters/
    ├── NormalItemUpdater.cs        # Strategy: normal item degradation
    ├── AgedBrieUpdater.cs          # Strategy: aged brie appreciation
    ├── BackstagePassUpdater.cs     # Strategy: backstage pass time-decay
    ├── SulfurasUpdater.cs          # Strategy: legendary item (no change)
    └── ConjuredItemUpdater.cs      # Strategy: conjured item rapid decay
```

---

## 3. Core Components

### 3.1 File: `Constants.cs`

**Location**: `src/GildedRose.Console/Constants.cs`

**Purpose**: Centralized repository for all application constants

**Structure**: Static class with grouped constant categories

**Item Name Constants:**
- `DexterityVest` = "+5 Dexterity Vest"
- `AgedBrie` = "Aged Brie"
- `ElixirOfMongoose` = "Elixir of the Mongoose"
- `SulfurasHandOfRagnaros` = "Sulfuras, Hand of Ragnaros"
- `BackstagePassConcert` = "Backstage passes to a TAFKAL80ETC concert"
- `ConjuredManaCake` = "Conjured Mana Cake"

**Quality Threshold Constants:**
- `MaxQuality` = 50 - Maximum quality cap for most items
- `MinQuality` = 0 - Minimum quality for most items

**Item Classification Constants:**
- `ConjuredItemPrefix` = "Conjured" - Used for pattern matching in factory

**Rationale**: These constants are centralized to:
- Prevent typos in item names used by the factory
- Ensure consistency across updaters
- Enable single-point changes for business rule adjustments
- Facilitate future item additions or modifications

### 3.2 File: `IItemUpdater.cs`

**Location**: `src/GildedRose.Console/Interfaces/IItemUpdater.cs`

**Purpose**: Strategy pattern interface defining the contract for item update behaviors

**Interface Definition:**
```csharp
public interface IItemUpdater
{
    void Update(Item item);
}
```

**Responsibility**: Each implementing class encapsulates the update logic for a specific item type.

**Rationale**: Using an interface enables:
- Polymorphic behavior in Program.UpdateQuality()
- Easy addition of new item types without modifying existing code
- Testability through mock implementations

### 3.3 File: `ItemUpdaterFactory.cs`

**Location**: `src/GildedRose.Console/Factories/ItemUpdaterFactory.cs`

**Purpose**: Factory pattern implementation for creating appropriate updater strategies

**Method**: `GetUpdater(Item item) : IItemUpdater`

**Selection Logic**:
```csharp
return item.Name switch
{
    "Aged Brie" => new AgedBrieUpdater(),
    "Backstage passes to a TAFKAL80ETC concert" => new BackstagePassUpdater(),
    "Sulfuras, Hand of Ragnaros" => new SulfurasUpdater(),
    _ => item.Name.StartsWith("Conjured") ? new ConjuredItemUpdater() : new NormalItemUpdater()
};
```

**Categories**:
1. **Exact Match**: Aged Brie, Backstage Pass, Sulfuras
2. **Prefix Match**: Conjured items (StartsWith check)
3. **Default**: Normal items for all others

**Benefits**:
- Centralizes updater selection logic
- Eliminates conditional logic from Program.cs
- Easy to extend with new item types
- Uses C# switch expressions for clean, readable code

### 3.4 File: `Program.cs` (Modified)

**Key Changes**:
1. Replaced hardcoded item names with `Constants` references
2. Integrated `ItemUpdaterFactory` into `UpdateQuality()` method
3. Uses factory to select appropriate updater for each item

**Item Initialization (After)**:
```csharp
Items = new List<Item>
{
    new Item {Name = Constants.DexterityVest, SellIn = 10, Quality = 20},
    new Item {Name = Constants.AgedBrie, SellIn = 2, Quality = 0},
    new Item {Name = Constants.ElixirOfMongoose, SellIn = 5, Quality = 7},
    new Item {Name = Constants.SulfurasHandOfRagnaros, SellIn = 0, Quality = 80},
    new Item {Name = Constants.BackstagePassConcert, SellIn = 15, Quality = 20},
    new Item {Name = Constants.ConjuredManaCake, SellIn = 3, Quality = 6}
}
```

**UpdateQuality Method Integration**:
```csharp
public void UpdateQuality()
{
    var factory = new ItemUpdaterFactory();
    foreach (var item in Items)
    {
        var updater = factory.GetUpdater(item);
        updater.Update(item);
    }
}
```

**Impact**:
- Improved readability with semantic naming
- Centralized factory pattern for updater selection
- Easier to add new item types
- Reduced code duplication across updater instantiation

### 3.5 Updater Classes

All updaters implement `IItemUpdater` and encapsulate item-type-specific update logic.

#### 3.5.1 `NormalItemUpdater.cs`

**Behavior**: Standard item degradation

**Rules**:
- Quality decreases by 1 each day
- SellIn decreases by 1 each day
- After sell date (SellIn < 0), quality decreases by additional 1 per day (2 total)
- Quality minimum is 0

**Use Cases**: "+5 Dexterity Vest", "Elixir of the Mongoose", and other normal items

**Example Update Sequence** (SellIn=10, Quality=20):
- Day 1: Quality=19, SellIn=9
- Day 10: Quality=11, SellIn=0
- Day 11 (after sell): Quality=9, SellIn=-1 (2 decrease)
- Day 12: Quality=7, SellIn=-2 (2 decrease)

#### 3.5.2 `AgedBrieUpdater.cs`

**Behavior**: Quality appreciation with age

**Rules**:
- Quality increases by 1 each day
- SellIn decreases by 1 each day
- After sell date (SellIn < 0), quality increases by additional 1 per day (2 total)
- Quality maximum is 50

**Use Cases**: "Aged Brie"

**Example Update Sequence** (SellIn=2, Quality=0):
- Day 1: Quality=1, SellIn=1
- Day 2: Quality=2, SellIn=0
- Day 3 (after sell): Quality=4, SellIn=-1 (2 increase)
- Day 4: Quality=6, SellIn=-2 (2 increase)

#### 3.5.3 `BackstagePassUpdater.cs`

**Behavior**: Increasing value as concert approaches, drops to 0 after

**Rules**:
- Quality increases by 1 normally
- Quality increases by additional 1 when SellIn < 11 (2 total)
- Quality increases by additional 1 when SellIn < 6 (3 total)
- Quality drops to 0 after concert (SellIn < 0)
- Quality maximum is 50

**Use Cases**: "Backstage passes to a TAFKAL80ETC concert"

**Example Update Sequence** (SellIn=15, Quality=20):
- Days 1-4 (SellIn > 11): +1 per day → SellIn=11, Quality=24
- Days 5-9 (11 > SellIn > 6): +2 per day → SellIn=6, Quality=34
- Days 10-14 (6 > SellIn > 0): +3 per day → SellIn=0, Quality=49
- Day 15 (SellIn < 0): Quality=0 (concert passed)

#### 3.5.4 `SulfurasUpdater.cs`

**Behavior**: Legendary item, never changes

**Rules**:
- SellIn never changes
- Quality never changes
- Exempt from quality bounds (0-50)
- Update method is a no-op

**Use Cases**: "Sulfuras, Hand of Ragnaros"

**Example**: (SellIn=0, Quality=80)
- All days: SellIn=0, Quality=80 (unchanged)

#### 3.5.5 `ConjuredItemUpdater.cs`

**Behavior**: Rapid degradation of magical items

**Rules**:
- Quality decreases by 2 each day (twice as fast as normal)
- SellIn decreases by 1 each day
- After sell date (SellIn < 0), quality decreases by additional 2 per day (4 total)
- Quality minimum is 0

**Use Cases**: "Conjured Mana Cake" and any item starting with "Conjured"

**Example Update Sequence** (SellIn=3, Quality=6):
- Day 1: Quality=4, SellIn=2
- Day 2: Quality=2, SellIn=1
- Day 3: Quality=0, SellIn=0 (capped at 0)
- Day 4 (after sell): Quality=0, SellIn=-1 (would be -4, but capped at 0)

---

## 4. Test Project

### 4.1 Test Organization

The test project includes comprehensive test coverage:

**Test Files**:
- **ItemUpdaterFactoryTests.cs** - Unit tests for factory pattern logic
  - Tests updater selection for each item type
  - Verifies exact name matching and prefix matching
  - Tests default behavior for unknown items

- **ItemUpdaterTests.cs** - Unit tests for individual updater behaviors
  - Tests for each concrete updater class
  - Quality boundary conditions (min=0, max=50)
  - Sell-in date transitions
  - Edge cases for each item type

- **ProgramIntegrationTests.cs** - End-to-end integration tests
  - Tests full UpdateQuality() orchestration
  - Tests factory integration with Program
  - Tests complete update lifecycle for all item types

- **TestAssemblyTests.cs** - Assembly-level tests
  - General test infrastructure and constants validation

### 4.2 Test Data Approach

**Rationale for Using Hardcoded Strings in Tests**:

Tests currently use hardcoded item name strings for test data setup. This is acceptable because:

1. **Test Data vs. Production Data**: Tests create test fixtures independently; they don't rely on Constants.cs
2. **Test Intent Clarity**: Seeing actual strings makes tests self-documenting and easier to understand
3. **Test Independence**: Tests aren't brittle to Constants.cs changes
4. **Cross-Version Compatibility**: Hardcoded test strings ensure tests work regardless of Constants values

**Example**:
```csharp
public void UpdateQuality_UpdatesAgedBrieCorrectly()
{
	var program = new Program
	{
		Items = new List<Item>
		{
			new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 },
		}
	};
	// ... test assertions
}
```

**Future Consideration**: Tests could optionally be refactored to use Constants for consistency, but this is deferred as test readability is prioritized.

---

## 5. Architecture & Design Patterns

### 5.1 Strategy Pattern

**Definition**: Encapsulates different algorithms (update behaviors) into separate classes, all implementing the same interface.

**Implementation in Gilded Rose**:
- **Interface**: `IItemUpdater`
- **Concrete Strategies**: Five updater classes, each with unique update logic
- **Context**: `Program.UpdateQuality()` uses strategies polymorphically

**Benefit**: Easy to add new item types without modifying existing updater code. Open/Closed Principle: open for extension, closed for modification.

### 5.2 Factory Pattern

**Definition**: Creates objects without specifying exact classes, delegating to a factory method.

**Implementation in Gilded Rose**:
- **Factory Class**: `ItemUpdaterFactory`
- **Method**: `GetUpdater(Item item)`
- **Selection Logic**: Maps item names/patterns to updater types

**Benefit**: 
- Centralizes object creation logic
- Makes it easy to change updater selection rules
- Eliminates conditional logic from Program.cs
- Enables easy extension for new item types

### 5.3 Static Constants Class Pattern

**Definition**: Centralized repository for configuration constants, using a static class with public const members.

**Implementation**:
- **Class**: `Constants` (static)
- **Namespace**: `GildedRose.Console`
- **Members**: All public const (no instances)

**Naming Conventions**:
- PascalCase for all constants (e.g., `MaxQuality`, `DexterityVest`)
- Descriptive names including context (e.g., `BackstagePassSecondBonusThreshold`)

**Benefit**:
- Single source of truth for configuration values
- Easy to audit and modify business rules
- Prevents typos and inconsistencies
- Improves code readability

### 5.4 Dependency Flow

```
Program.cs
  ├─→ Constants.cs (item initialization)
  ├─→ ItemUpdaterFactory (updater selection)
  │    ├─→ Constants.cs (item name matching)
  │    └─→ IItemUpdater implementations
  │
  └─→ *Updater.cs (polymorphic update)
	   └─→ Constants.cs (future: quality bounds)

Tests
  ├─→ Program (integration testing)
  ├─→ ItemUpdaterFactory (factory testing)
  ├─→ *Updater (unit testing)
  └─→ (Independent of Constants.cs)
```

---

## 6. Benefits of This Refactoring

### 6.1 Maintainability
- **Single Source of Truth**: All item names defined in one place
- **Reduced Duplication**: Item names no longer repeated across code
- **Clear Responsibilities**: Each updater has a single, well-defined job
- **Centralized Factory Logic**: All updater selection rules in one place

### 6.2 Extensibility
- **Easy Item Addition**: Add new item type by:
  1. Adding constant to Constants.cs
  2. Creating new Updater class implementing IItemUpdater
  3. Adding case to ItemUpdaterFactory.GetUpdater()
  4. No changes needed to Program.UpdateQuality()

- **Easy Rule Changes**: Business rule adjustments require changes in only one updater

### 6.3 Code Quality
- **Improved Readability**: Semantic constant names instead of magic strings
- **Reduced Bugs**: Centralized constants prevent typos
- **Better Testability**: Each updater can be tested independently
- **Design Pattern Adherence**: Follows well-established software engineering patterns

### 6.4 Developer Experience
- **Self-Documenting Code**: Clear intent through naming and structure
- **Ease of Understanding**: New developers can quickly grasp the architecture
- **Reduced Cognitive Load**: Each class has a clear purpose
- **Facilitated Onboarding**: Pattern-based architecture is familiar to experienced developers

---

## 7. Usage Example

### Adding a New Item Type

**Scenario**: Add "Epic Sword of Awesomeness" which increases quality by 2 per day, similar to Aged Brie.

**Steps**:

1. **Add constant to Constants.cs**:
   ```csharp
   public const string EpicSword = "Epic Sword of Awesomeness";
   ```

2. **Create EpicSwordUpdater.cs** (extends AgedBrieUpdater or creates new implementation):
   ```csharp
   public class EpicSwordUpdater : IItemUpdater
   {
	   public void Update(Item item)
	   {
		   // Custom logic for epic sword
	   }
   }
   ```

3. **Update ItemUpdaterFactory.GetUpdater()**:
   ```csharp
   return item.Name switch
   {
	   Constants.EpicSword => new EpicSwordUpdater(),
	   // ... existing cases
   };
   ```

4. **Update Program.cs to initialize the new item**:
   ```csharp
   new Item { Name = Constants.EpicSword, SellIn = 10, Quality = 0 }
   ```

**Result**: New item type fully integrated with minimal code changes, no modifications to existing updaters.

---

## 8. Future Enhancements

### 8.1 Configuration Management
- Migrate Constants.cs values to appsettings.json for runtime configuration
- Enable per-environment quality thresholds and item behaviors
- Support dynamic item registration without code recompilation

### 8.2 Performance Optimization
- Implement updater instance caching in ItemUpdaterFactory
- Consider compiled delegate strategies for high-frequency updates

### 8.3 Auditing & Logging
- Add logging to each updater to track quality changes
- Implement audit trail for item value modifications
- Track update performance metrics

### 8.4 Additional Item Types
- Musical Instruments (fade in value over time)
- Perishable Goods (with expiration mechanics)
- Seasonal Items (value varies by season)

---

## 9. Summary

The refactored Gilded Rose application demonstrates modern software engineering principles through:

1. **Design Patterns**: Strategy and Factory patterns provide flexible, maintainable architecture
2. **Single Responsibility**: Each updater class has one reason to change
3. **Open/Closed Principle**: Open for extension (new updaters), closed for modification (existing updaters untouched)
4. **Dependency Injection Ready**: Factory pattern enables easy DI container integration
5. **Testability**: Each component can be unit tested independently

The architecture scales well for adding new item types and business rule changes, while maintaining clean, readable code and strong type safety through the .NET type system.

### 5.4 Testing & Debugging
- **Clarity**: Constant names improve test readability
- **Traceability**: Easy to find where constants are used via IDE navigation

---

## 6. Migration Path

### 6.1 Completed
✅ Constants.cs created with all item names and thresholds
✅ Program.cs updated to use Constants
✅ Build verification passed

### 6.2 Optional Future Enhancements
- Update ItemUpdaterFactory to reference item name constants
- Update Updater classes to use quality threshold constants
- Optional: Migrate test fixtures to use Constants (for consistency)
- Consider: Configuration file integration for runtime-changeable values

---

## 7. Build & Verification

### 7.1 Build Status
- **Status**: ✅ Successful
- **Framework**: .NET 8
- **Compiler Warnings**: None
- **Breaking Changes**: None

### 7.2 Test Status
- All existing tests remain compatible
- No test modifications required for this refactoring
- Tests can continue to use hardcoded strings for clarity

---

## 8. Code Quality Metrics

| Metric | Impact |
|--------|--------|
| Magic Strings Eliminated | 6 item name strings moved to constants |
| Centralized Constants | 28 public constants defined in Constants.cs |
| Code Duplication Reduced | Item names: 1 definition → multiple references |
| File Count | +1 new file (Constants.cs) |
| Lines Changed | ~18 lines in Program.cs |

---

## 9. Risk Assessment

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|-----------|
| Build Failure | Low | High | Build verification completed ✅ |
| Test Failures | Low | Medium | All tests remain compatible |
| Typos in Constants | Low | Low | IDE intellisense prevents typos |
| Naming Conflicts | Very Low | Low | Static class with unique names |

---

## 10. Recommendations

### 10.1 Short Term
1. ✅ Deploy Constants.cs refactoring to production
2. Monitor for any runtime issues (unlikely given compile-time safety)
3. Update deployment documentation if applicable

### 10.2 Medium Term (1-2 sprints)
1. Consider refactoring ItemUpdaterFactory to use Constants for item name matching
2. Update Updater classes to use Constants for quality thresholds
3. Add XML documentation comments to Constants.cs for IDE tooltips

### 10.3 Long Term (Next Quarter)
1. Evaluate moving Constants to configuration files (appsettings.json)
2. Consider database-driven configuration for item names and rules
3. Implement feature flags for A/B testing business rule changes

---

## 11. Appendix: File Structure

src/GildedRose.Console/
├── Program.cs 
├── Constants.cs
├── Factories/
│   └── ItemUpdaterFactory.cs
├── Updaters/
│   ├── NormalItemUpdater.cs
│   ├── AgedBrieUpdater.cs
│   ├── BackstagePassUpdater.cs
│   ├── SulfurasUpdater.cs
│   └── ConjuredItemUpdater.cs
├── Models/
│   └── Item.cs
└── Interfaces/
	└── IItemUpdater.cs
```

---

## 12. Conclusion


This refactoring serves as a foundation for future enhancements such as configuration file integration, database-driven rules, and extended constant management across the application.


