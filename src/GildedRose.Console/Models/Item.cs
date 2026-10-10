namespace GildedRose.Console.Models;

/// <summary>
/// Represents an item in the Gilded Rose inventory.
/// </summary>
/// <remarks>
/// Items have three key properties: Name (identifier), SellIn (days until expiration),
/// and Quality (value of the item). The behavior of these properties during updates
/// depends on the item type as determined by its Name.
/// </remarks>
public class Item
{
    /// <summary>
    /// Gets or sets the name or identifier of the item.
    /// </summary>
    /// <value>The item's name, which determines its update behavior.</value>
    /// <remarks>
    /// The Name is used by ItemUpdaterFactory to determine which update strategy to apply.
    /// Common names include "Aged Brie", "Backstage passes to a TAFKAL80ETC concert",
    /// "Sulfuras, Hand of Ragnaros", and items prefixed with "Conjured".
    /// </remarks>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the number of days remaining before the item expires.
    /// </summary>
    /// <value>The number of days until the item must be sold. Can be negative after expiration.</value>
    /// <remarks>
    /// Once this value goes below 0, the item is considered expired and may have additional
    /// quality degradation or special behavior depending on the item type.
    /// </remarks>
    public int SellIn { get; set; }

    /// <summary>
    /// Gets or sets the quality value of the item.
    /// </summary>
    /// <value>The item's quality, typically between 0 and 50.</value>
    /// <remarks>
    /// Quality represents the value or effectiveness of the item. Most items have a quality
    /// range of 0 to 50, but legendary items like Sulfuras may have different rules.
    /// Quality changes based on the item type and how many days have passed.
    /// </remarks>
    public int Quality { get; set; }
}
