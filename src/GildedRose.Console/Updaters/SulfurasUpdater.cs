using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters;

/// <summary>
/// Updater for Sulfuras, the legendary hand of Ragnaros.
/// This is a legendary item that never decreases in quality or sell-in value.
/// </summary>
/// <remarks>
/// Sulfuras is a legendary item with special properties:
/// - It never changes in quality or sell-in value
/// - It is exempt from the normal quality bounds (0-50)
/// - The update method is effectively a no-op for this item type
/// </remarks>
public class SulfurasUpdater : IItemUpdater
{
    /// <summary>
    /// Updates the Sulfuras item (performs no operation as legendary items never change).
    /// </summary>
    /// <param name="item">The Sulfuras legendary item (not modified).</param>
    /// <remarks>
    /// Sulfuras is a legendary item and never changes. This method is intentionally
    /// empty as the item should never be modified, maintaining its legendary status.
    /// </remarks>
    public void Update(Item item)
    {
        // Sulfuras never changes
    }
}
