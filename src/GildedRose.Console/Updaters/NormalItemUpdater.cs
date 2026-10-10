using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters;

/// <summary>
/// Updater for normal items (non-special items).
/// Quality decreases by 1 each day, and by 2 after sell date.
/// </summary>
/// <remarks>
/// Normal items follow the basic degradation rules where:
/// - Quality decreases by 1 on each update
/// - Once the SellIn date passes, quality decreases by an additional 1 (total 2 per day)
/// - Quality is never negative (minimum 0)
/// </remarks>
public class NormalItemUpdater : IItemUpdater
{
    /// <summary>
    /// Updates the item's quality and sell-in values according to normal item degradation rules.
    /// </summary>
    /// <param name="item">The normal item to update.</param>
    /// <remarks>
    /// The update process:
    /// 1. Decreases quality by 1
    /// 2. Decreases sell-in by 1
    /// 3. If past sell date (SellIn &lt; 0), decreases quality by an additional 1
    /// </remarks>
    public void Update(Item item)
    {
        DecreaseQuality(item, 1);
        item.SellIn--;

        if (item.SellIn < 0)
        {
            DecreaseQuality(item, 1);
        }
    }

    /// <summary>
    /// Decreases the item's quality by the specified amount, ensuring it doesn't drop below 0.
    /// </summary>
    /// <param name="item">The item whose quality will be decreased.</param>
    /// <param name="amount">The amount to decrease quality by.</param>
    /// <remarks>
    /// This is a helper method that ensures quality is bounded at 0 using Math.Max.
    /// </remarks>
    protected void DecreaseQuality(Item item, int amount)
    {
        item.Quality = Math.Max(0, item.Quality - amount);
    }
}
