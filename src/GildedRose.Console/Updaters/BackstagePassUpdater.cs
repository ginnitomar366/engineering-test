using GildedRose.Console.Interfaces;
using GildedRose.Console.Models;

namespace GildedRose.Console.Updaters;

/// <summary>
/// Updater for Backstage passes to concerts.
/// Quality increases by 1, by 2 when 10 days or less, by 3 when 5 days or less,
/// and drops to 0 after the concert.
/// Quality is capped at 50.
/// </summary>
/// <remarks>
/// Backstage passes increase in value as the concert approaches, following these rules:
/// - Quality increases by 1 on each update (normal)
/// - Quality increases by an additional 1 when SellIn &lt; 11 (2 total per day)
/// - Quality increases by an additional 1 when SellIn &lt; 6 (3 total per day)
/// - Quality drops to 0 after the concert (SellIn &lt; 0)
/// - Quality is capped at a maximum of 50
/// </remarks>
public class BackstagePassUpdater : IItemUpdater
{
    /// <summary>
    /// Updates the item's quality and sell-in values according to backstage pass rules.
    /// </summary>
    /// <param name="item">The backstage pass to update.</param>
    /// <remarks>
    /// The update process:
    /// 1. Increases quality by 1
    /// 2. If 10 days or less until concert (SellIn &lt; 11), increases quality by 1 more
    /// 3. If 5 days or less until concert (SellIn &lt; 6), increases quality by 1 more
    /// 4. Decreases sell-in by 1
    /// 5. If concert has passed (SellIn &lt; 0), sets quality to 0
    /// </remarks>
    public void Update(Item item)
    {
        IncreaseQuality(item, 1);

        if (item.SellIn < 11)
        {
            IncreaseQuality(item, 1);
        }

        if (item.SellIn < 6)
        {
            IncreaseQuality(item, 1);
        }

        item.SellIn--;

        if (item.SellIn < 0)
        {
            item.Quality = 0;
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
