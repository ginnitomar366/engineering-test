using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;
using GildedRose.Console.Updaters;

namespace GildedRose.Console.Factories;

/// <summary>
/// Factory for creating the appropriate IItemUpdater strategy for a given item.
/// Uses item names to determine which updater to instantiate.
/// </summary>
/// <remarks>
/// This factory implements the Strategy pattern, allowing different update behaviors
/// for different item types. The item type is determined by matching the item's Name
/// against known item names and patterns.
/// </remarks>
public class ItemUpdaterFactory
{
    /// <summary>
    /// Gets the appropriate updater strategy for the given item.
    /// </summary>
    /// <param name="item">The item to get an updater for.</param>
    /// <returns>
    /// An IItemUpdater instance appropriate for the item type:
    /// - AgedBrieUpdater for "Aged Brie"
    /// - BackstagePassUpdater for "Backstage passes to a TAFKAL80ETC concert"
    /// - SulfurasUpdater for "Sulfuras, Hand of Ragnaros"
    /// - ConjuredItemUpdater for items starting with "Conjured"
    /// - NormalItemUpdater for all other items
    /// </returns>
    /// <remarks>
    /// The matching is case-sensitive. Items that don't match any specific category
    /// are treated as normal items and use the NormalItemUpdater.
    /// </remarks>
    public IItemUpdater GetUpdater(Item item)
    {
        return item.Name switch
        {
            "Aged Brie" => new AgedBrieUpdater(),
            "Backstage passes to a TAFKAL80ETC concert" => new BackstagePassUpdater(),
            "Sulfuras, Hand of Ragnaros" => new SulfurasUpdater(),
            _ => item.Name.StartsWith("Conjured") ? new ConjuredItemUpdater() : new NormalItemUpdater()
        };
    }
}
