using Xunit;
using GildedRose.Console;
using GildedRose.Console.Models;
using System.Collections.Generic;

namespace GildedRose.Tests;

public class ProgramIntegrationTests
{
    [Fact]
    public void UpdateQuality_UpdatesAllItemsCorrectly()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 },
                new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 },
                new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 },
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 },
                new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 }
            }
        };

        program.UpdateQuality();

        // Verify each item was updated correctly
        Assert.Equal(9, program.Items[0].SellIn);    // Normal item: SellIn decreases
        Assert.Equal(19, program.Items[0].Quality);  // Normal item: Quality decreases by 1

        Assert.Equal(1, program.Items[1].SellIn);    // Aged Brie: SellIn decreases
        Assert.Equal(1, program.Items[1].Quality);   // Aged Brie: Quality increases by 1

        Assert.Equal(0, program.Items[2].SellIn);    // Sulfuras: SellIn stays same
        Assert.Equal(80, program.Items[2].Quality);  // Sulfuras: Quality stays same

        Assert.Equal(14, program.Items[3].SellIn);   // Backstage: SellIn decreases
        Assert.Equal(21, program.Items[3].Quality);  // Backstage: Quality increases by 1

        Assert.Equal(2, program.Items[4].SellIn);    // Conjured: SellIn decreases
        Assert.Equal(4, program.Items[4].Quality);   // Conjured: Quality decreases by 2
    }

    [Fact]
    public void UpdateQuality_HandlesMultipleUpdatesCorrectly()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Test Item", SellIn = 2, Quality = 10 }
            }
        };

        // Update twice
        program.UpdateQuality();
        program.UpdateQuality();

        Assert.Equal(0, program.Items[0].SellIn);
        Assert.Equal(8, program.Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_HandlesExpiredItemsCorrectly()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Test Item", SellIn = 1, Quality = 5 }
            }
        };

        program.UpdateQuality();  // SellIn becomes 0, Quality 5 - 1 = 4
        program.UpdateQuality();  // SellIn becomes -1, Quality 4 - 1 (before) - 1 (after expiry) = 2

        Assert.Equal(-1, program.Items[0].SellIn);
        Assert.Equal(2, program.Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_AgedBrieIncreasesBeyondExpiry()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Aged Brie", SellIn = 1, Quality = 10 }
            }
        };

        program.UpdateQuality();  // SellIn becomes 0, quality increases by 1
        program.UpdateQuality();  // SellIn becomes -1, quality increases by 2

        Assert.Equal(-1, program.Items[0].SellIn);
        Assert.Equal(13, program.Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_BackstagePassQualityDropsAfterConcert()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 45 }
            }
        };

        program.UpdateQuality();  // SellIn becomes 0, quality increases by 3
        program.UpdateQuality();  // SellIn becomes -1, quality drops to 0

        Assert.Equal(-1, program.Items[0].SellIn);
        Assert.Equal(0, program.Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_ConjuredQualityDepletesQuickly()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Conjured Mana Cake", SellIn = 2, Quality = 5 }
            }
        };

        program.UpdateQuality();  // Quality: 5 - 2 = 3
        program.UpdateQuality();  // Quality: 3 - 2 = 1, SellIn becomes 0
        program.UpdateQuality();  // Quality: 1 - 2 (before) - 2 (after expiry) = capped at 0

        Assert.Equal(-1, program.Items[0].SellIn);
        Assert.Equal(0, program.Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_AllItemsStartingAtMaxQuality()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Aged Brie", SellIn = 5, Quality = 50 },
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 50 }
            }
        };

        program.UpdateQuality();

        // Aged Brie cannot exceed 50
        Assert.Equal(50, program.Items[0].Quality);
        // Backstage passes cannot exceed 50
        Assert.Equal(50, program.Items[1].Quality);
    }

    [Fact]
    public void UpdateQuality_ConjuredNeverGoesNegative()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Conjured Mana Cake", SellIn = -10, Quality = 1 }
            }
        };

        program.UpdateQuality();

        Assert.Equal(0, program.Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_MixedItemsWithComplexScenario()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 20 },
                new Item { Name = "Aged Brie", SellIn = -1, Quality = 48 },
                new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 },
                new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 },
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 49 },
                new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 5 }
            }
        };

        program.UpdateQuality();

        // Normal item: SellIn-1, Quality-1
        Assert.Equal(4, program.Items[0].SellIn);
        Assert.Equal(19, program.Items[0].Quality);

        // Aged Brie expired: SellIn-1, Quality+2 (capped at 50)
        Assert.Equal(-2, program.Items[1].SellIn);
        Assert.Equal(50, program.Items[1].Quality);

        // Normal item: SellIn-1, Quality-1
        Assert.Equal(4, program.Items[2].SellIn);
        Assert.Equal(6, program.Items[2].Quality);

        // Sulfuras: No change
        Assert.Equal(0, program.Items[3].SellIn);
        Assert.Equal(80, program.Items[3].Quality);

        // Backstage pass (10 days): SellIn-1, Quality+2 (capped at 50)
        Assert.Equal(9, program.Items[4].SellIn);
        Assert.Equal(50, program.Items[4].Quality);

        // Conjured: SellIn-1, Quality-2
        Assert.Equal(2, program.Items[5].SellIn);
        Assert.Equal(3, program.Items[5].Quality);
    }

    [Fact]
    public void UpdateQuality_BackstagePassRisingInValue()
    {
        var program = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 11, Quality = 20 }
            }
        };

        // Day 1: 11 days out, quality +1
        program.UpdateQuality();
        Assert.Equal(10, program.Items[0].SellIn);
        Assert.Equal(21, program.Items[0].Quality);

        // Day 2: 10 days out, quality +2
        program.UpdateQuality();
        Assert.Equal(9, program.Items[0].SellIn);
        Assert.Equal(23, program.Items[0].Quality);

        // Day 3: 9 days out, quality +2
        program.UpdateQuality();
        Assert.Equal(8, program.Items[0].SellIn);
        Assert.Equal(25, program.Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_EmptyItemsListDoesNotThrow()
    {
        var program = new Program
        {
            Items = new List<Item>()
        };

        // Should not throw
        program.UpdateQuality();

        Assert.Empty(program.Items);
    }
}
