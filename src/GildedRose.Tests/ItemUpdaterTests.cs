using Xunit;
using GildedRose.Console.Models;
using GildedRose.Console.Updaters;

namespace GildedRose.Tests;

public class NormalItemUpdaterTests
{
    private readonly NormalItemUpdater _updater = new();

    [Fact]
    public void Update_DecreaseQualityByOne_WhenBeforeSellDate()
    {
        var item = new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 };

        _updater.Update(item);

        Assert.Equal(9, item.SellIn);
        Assert.Equal(19, item.Quality);
    }

    [Fact]
    public void Update_DecreaseQualityByTwo_WhenAfterSellDate()
    {
        var item = new Item { Name = "Normal Item", SellIn = 0, Quality = 20 };

        _updater.Update(item);

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(18, item.Quality);
    }

    [Fact]
    public void Update_QualityNeverNegative_WhenDegradicingBelowZero()
    {
        var item = new Item { Name = "Normal Item", SellIn = 10, Quality = 0 };

        _updater.Update(item);

        Assert.Equal(9, item.SellIn);
        Assert.Equal(0, item.Quality);
    }

    [Fact]
    public void Update_QualityNeverNegative_AfterSellDateWhenQualityIsOne()
    {
        var item = new Item { Name = "Normal Item", SellIn = -5, Quality = 1 };

        _updater.Update(item);

        Assert.Equal(-6, item.SellIn);
        Assert.Equal(0, item.Quality);
    }

    [Fact]
    public void Update_DecreaseSellInRegularly()
    {
        var item = new Item { Name = "Normal Item", SellIn = 5, Quality = 10 };

        _updater.Update(item);

        Assert.Equal(4, item.SellIn);
    }

    [Fact]
    public void Update_MaxQualityAfterSellDateDoesNotExceedBoundary()
    {
        var item = new Item { Name = "Normal Item", SellIn = -1, Quality = 2 };

        _updater.Update(item);

        Assert.Equal(0, item.Quality);
    }
}

public class ConjuredItemUpdaterTests
{
    private readonly ConjuredItemUpdater _updater = new();

    [Fact]
    public void Update_DecreaseQualityByTwo_WhenBeforeSellDate()
    {
        var item = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 };

        _updater.Update(item);

        Assert.Equal(2, item.SellIn);
        Assert.Equal(4, item.Quality);
    }

    [Fact]
    public void Update_DecreaseQualityByFour_WhenAfterSellDate()
    {
        var item = new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 10 };

        _updater.Update(item);

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(6, item.Quality);
    }

    [Fact]
    public void Update_QualityNeverNegative_WhenConjuredDegradingBelowZero()
    {
        var item = new Item { Name = "Conjured Mana Cake", SellIn = 5, Quality = 1 };

        _updater.Update(item);

        Assert.Equal(4, item.SellIn);
        Assert.Equal(0, item.Quality);
    }

    [Fact]
    public void Update_QualityNeverNegative_AfterSellDateWhenConjuredDegradingBelowZero()
    {
        var item = new Item { Name = "Conjured Mana Cake", SellIn = -1, Quality = 3 };

        _updater.Update(item);

        Assert.Equal(-2, item.SellIn);
        Assert.Equal(0, item.Quality);
    }
}

public class AgedBrieUpdaterTests
{
    private readonly AgedBrieUpdater _updater = new();

    [Fact]
    public void Update_IncreaseQualityByOne_WhenBeforeSellDate()
    {
        var item = new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 };

        _updater.Update(item);

        Assert.Equal(1, item.SellIn);
        Assert.Equal(1, item.Quality);
    }

    [Fact]
    public void Update_IncreaseQualityByTwo_WhenAfterSellDate()
    {
        var item = new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 };

        _updater.Update(item);

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(12, item.Quality);
    }

    [Fact]
    public void Update_QualityNeverExceedsFifty_WhenBeforeSellDate()
    {
        var item = new Item { Name = "Aged Brie", SellIn = 5, Quality = 50 };

        _updater.Update(item);

        Assert.Equal(4, item.SellIn);
        Assert.Equal(50, item.Quality);
    }

    [Fact]
    public void Update_QualityNeverExceedsFifty_WhenAfterSellDate()
    {
        var item = new Item { Name = "Aged Brie", SellIn = -1, Quality = 49 };

        _updater.Update(item);

        Assert.Equal(-2, item.SellIn);
        Assert.Equal(50, item.Quality);
    }

    [Fact]
    public void Update_QualityCapsCorrectlyWhenExceedsBoundary()
    {
        var item = new Item { Name = "Aged Brie", SellIn = -1, Quality = 48 };

        _updater.Update(item);

        Assert.Equal(-2, item.SellIn);
        Assert.Equal(50, item.Quality);
    }
}

public class BackstagePassUpdaterTests
{
    private readonly BackstagePassUpdater _updater = new();

    [Fact]
    public void Update_IncreaseQualityByOne_WhenMoreThanTenDaysBeforeConcert()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 };

        _updater.Update(item);

        Assert.Equal(14, item.SellIn);
        Assert.Equal(21, item.Quality);
    }

    [Fact]
    public void Update_IncreaseQualityByTwo_WhenTenDaysOrLessTillConcert()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 };

        _updater.Update(item);

        Assert.Equal(9, item.SellIn);
        Assert.Equal(22, item.Quality);
    }

    [Fact]
    public void Update_IncreaseQualityByTwo_WhenExactlyElevenDaysTillConcert()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 11, Quality = 20 };

        _updater.Update(item);

        Assert.Equal(10, item.SellIn);
        Assert.Equal(21, item.Quality);
    }

    [Fact]
    public void Update_IncreaseQualityByThree_WhenFiveDaysOrLessTillConcert()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 };

        _updater.Update(item);

        Assert.Equal(4, item.SellIn);
        Assert.Equal(23, item.Quality);
    }

    [Fact]
    public void Update_IncreaseQualityByThree_WhenExactlySixDaysTillConcert()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 6, Quality = 20 };

        _updater.Update(item);

        Assert.Equal(5, item.SellIn);
        Assert.Equal(22, item.Quality);
    }

    [Fact]
    public void Update_QualityDropsToZero_WhenConcertHasPassed()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 20 };

        _updater.Update(item);

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(0, item.Quality);
    }

    [Fact]
    public void Update_QualityDropsToZero_AfterConcertPassed()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = -5, Quality = 50 };

        _updater.Update(item);

        Assert.Equal(-6, item.SellIn);
        Assert.Equal(0, item.Quality);
    }

    [Fact]
    public void Update_QualityNeverExceedsFifty_WhenMultiplyingIncrease()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 49 };

        _updater.Update(item);

        Assert.Equal(4, item.SellIn);
        Assert.Equal(50, item.Quality);
    }

    [Fact]
    public void Update_QualityNeverExceedsFifty_WhenIncreasedByTwo()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 49 };

        _updater.Update(item);

        Assert.Equal(9, item.SellIn);
        Assert.Equal(50, item.Quality);
    }
}

public class SulfurasUpdaterTests
{
    private readonly SulfurasUpdater _updater = new();

    [Fact]
    public void Update_NeverChangesQuality()
    {
        var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 };

        _updater.Update(item);

        Assert.Equal(0, item.SellIn);
        Assert.Equal(80, item.Quality);
    }

    [Fact]
    public void Update_NeverChangesSellIn()
    {
        var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 5, Quality = 80 };

        _updater.Update(item);

        Assert.Equal(5, item.SellIn);
        Assert.Equal(80, item.Quality);
    }

    [Fact]
    public void Update_NeverChangesWhenBeforeSellDate()
    {
        var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 10, Quality = 80 };

        _updater.Update(item);

        Assert.Equal(10, item.SellIn);
        Assert.Equal(80, item.Quality);
    }

    [Fact]
    public void Update_NeverChangesWhenAfterSellDate()
    {
        var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = -1, Quality = 80 };

        _updater.Update(item);

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(80, item.Quality);
    }
}
