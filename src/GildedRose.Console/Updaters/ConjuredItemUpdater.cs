using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters;

/// <summary>
/// Updater for Conjured items.
/// Quality decreases by 2 each day, and by 2 additional days after sell date (total 4).
/// </summary>
/// <remarks>
/// Conjured items degrade much faster than normal items, following these rules:
/// - Quality decreases by 2 on each update
/// - Once the SellIn date passes, quality decreases by an additional 2 (total 4 per day)
/// - Quality is never negative (minimum 0)
/// - This rapid degradation represents the magical nature of conjured items fading
/// </remarks>
public class ConjuredItemUpdater : IItemUpdater
{
    /// <summary>
    /// Updates the item's quality and sell-in values according to conjured item degradation rules.
    /// </summary>
    /// <param name="item">The conjured item to update.</param>
    /// <remarks>
    /// The update process:
    /// 1. Decreases quality by 2 (rapid degradation of magical items)
    /// 2. Decreases sell-in by 1
    /// 3. If past sell date (SellIn &lt; 0), decreases quality by an additional 2
    /// </remarks>
    public void Update(Item item)
    {
        DecreaseQuality(item, 2);
        item.SellIn--;

        if (item.SellIn < 0)
        {
            DecreaseQuality(item, 2);
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
