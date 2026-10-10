namespace GildedRose.Console;

/// <summary>
/// Contains all application-wide constants used throughout the Gilded Rose system.
/// </summary>
/// <remarks>
/// This class serves as the single source of truth for magic strings and numeric constants,
/// facilitating easy maintenance and modification of item names, quality thresholds, and
/// quality change amounts without modifying business logic code.
/// </remarks>
public static class Constants
{
    // ========== Item Names ==========

    /// <summary>
    /// Name of the "+5 Dexterity Vest" normal item.
    /// </summary>
    public const string DexterityVest = "+5 Dexterity Vest";

    /// <summary>
    /// Name of the "Aged Brie" item that increases in quality over time.
    /// </summary>
    public const string AgedBrie = "Aged Brie";

    /// <summary>
    /// Name of the "Elixir of the Mongoose" normal item.
    /// </summary>
    public const string ElixirOfMongoose = "Elixir of the Mongoose";

    /// <summary>
    /// Name of the "Sulfuras, Hand of Ragnaros" legendary item that never changes.
    /// </summary>
    public const string SulfurasHandOfRagnaros = "Sulfuras, Hand of Ragnaros";

    /// <summary>
    /// Name of the backstage pass item for the TAFKAL80ETC concert.
    /// </summary>
    public const string BackstagePassConcert = "Backstage passes to a TAFKAL80ETC concert";

    /// <summary>
    /// Name of the "Conjured Mana Cake" item that degrades rapidly.
    /// </summary>
    public const string ConjuredManaCake = "Conjured Mana Cake";

    // ========== Quality Thresholds ==========

    /// <summary>
    /// Maximum quality value for most items.
    /// </summary>
    public const int MaxQuality = 50;

    /// <summary>
    /// Minimum quality value for items (quality cannot drop below this).
    /// </summary>
    public const int MinQuality = 0;

    // ========== Backstage Pass Thresholds (in days) ==========

    /// <summary>
    /// Number of days or fewer when backstage passes receive a second quality bonus.
    /// </summary>
    public const int BackstagePassSecondBonusThreshold = 11;

    /// <summary>
    /// Number of days or fewer when backstage passes receive a third quality bonus.
    /// </summary>
    public const int BackstagePassThirdBonusThreshold = 6;

    // ========== Quality Change Amounts ==========

    /// <summary>
    /// Quality decrement per day for normal items.
    /// </summary>
    public const int NormalItemQualityDecrement = 1;

    /// <summary>
    /// Quality increment per day for Aged Brie.
    /// </summary>
    public const int AgedBrieQualityIncrement = 1;

    /// <summary>
    /// Quality increment for backstage passes on a normal day.
    /// </summary>
    public const int BackstagePassInitialIncrement = 1;

    /// <summary>
    /// Quality increment bonus when backstage pass is within second threshold.
    /// </summary>
    public const int BackstagePassSecondIncrement = 1;

    /// <summary>
    /// Quality increment bonus when backstage pass is within third threshold.
    /// </summary>
    public const int BackstagePassThirdIncrement = 1;

    /// <summary>
    /// Quality decrement per day for conjured items (rapid degradation).
    /// </summary>
    public const int ConjuredItemQualityDecrement = 2;

    // ========== Item Classification Patterns ==========

    /// <summary>
    /// Prefix used to identify conjured items in the factory.
    /// Items starting with this prefix are treated as conjured and use the ConjuredItemUpdater.
    /// </summary>
    public const string ConjuredItemPrefix = "Conjured";
}
