using GildedRose.Console.Models;

namespace GildedRose.Console.Interfaces;

/// <summary>
/// Strategy interface for updating item quality and sell-in values.
/// Different item types implement this interface with their own update logic.
/// </summary>
public interface IItemUpdater
{
    /// <summary>
    /// Updates the given item's Quality and SellIn values based on item-specific rules.
    /// </summary>
    /// <param name="item">The item to update</param>
    void Update(Item item);
}
