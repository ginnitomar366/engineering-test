using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters;

/// <summary>
/// Updater for Aged Brie items.
/// Quality increases by 1 each day, and by 2 after sell date.
/// Quality is capped at 50.
/// </summary>
/// <remarks>
/// Aged Brie improves with age, following these rules:
/// - Quality increases by 1 on each update
/// - Once the SellIn date passes, quality increases by an additional 1 (total 2 per day)
/// - Quality is capped at a maximum of 50
/// </remarks>
public class AgedBrieUpdater : IItemUpdater
{
    /// <summary>
    /// Updates the item's quality and sell-in values according to Aged Brie appreciation rules.
    /// </summary>
    /// <param name="item">The Aged Brie item to update.</param>
    /// <remarks>
    /// The update process:
    /// 1. Increases quality by 1
    /// 2. Decreases sell-in by 1
    /// 3. If past sell date (SellIn &lt; 0), increases quality by an additional 1
    /// </remarks>
    public void Update(Item item)
    {
        IncreaseQuality(item, 1);
        item.SellIn--;

        if (item.SellIn < 0)
        {
            IncreaseQuality(item, 1);
        }
    }

    /// <summary>
    /// Increases the item's quality by the specified amount, ensuring it doesn't exceed 50.
    /// </summary>
    /// <param name="item">The item whose quality will be increased.</param>
    /// <param name="amount">The amount to increase quality by.</param>
    /// <remarks>
    /// This is a helper method that ensures quality is bounded at 50 using Math.Min.
    /// </remarks>
    protected void IncreaseQuality(Item item, int amount)
    {
        item.Quality = Math.Min(50, item.Quality + amount);
    }
}
