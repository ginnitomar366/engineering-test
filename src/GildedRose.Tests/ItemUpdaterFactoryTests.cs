using Xunit;
using GildedRose.Console.Models;
using GildedRose.Console.Factories;
using GildedRose.Console.Updaters;


public class ItemUpdaterFactoryTests
{
    private readonly ItemUpdaterFactory _factory = new();

    [Fact]
    public void GetUpdater_ReturnsNormalItemUpdater_ForRegularItem()
    {
        var item = new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 };

        var updater = _factory.GetUpdater(item);

        Assert.IsType<NormalItemUpdater>(updater);
    }

    [Fact]
    public void GetUpdater_ReturnsAgedBrieUpdater_ForAgedBrie()
    {
        var item = new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 };

        var updater = _factory.GetUpdater(item);

        Assert.IsType<AgedBrieUpdater>(updater);
    }

    [Fact]
    public void GetUpdater_ReturnsBackstagePassUpdater_ForBackstagePasses()
    {
        var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 };

        var updater = _factory.GetUpdater(item);

        Assert.IsType<BackstagePassUpdater>(updater);
    }

    [Fact]
    public void GetUpdater_ReturnsSulfurasUpdater_ForSulfuras()
    {
        var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 };

        var updater = _factory.GetUpdater(item);

        Assert.IsType<SulfurasUpdater>(updater);
    }

    [Fact]
    public void GetUpdater_ReturnsConjuredItemUpdater_ForConjuredItems()
    {
        var item = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 };

        var updater = _factory.GetUpdater(item);

        Assert.IsType<ConjuredItemUpdater>(updater);
    }

    [Fact]
    public void GetUpdater_ReturnsConjuredItemUpdater_ForConjuredPrefixVariation()
    {
        var item = new Item { Name = "Conjured Sword", SellIn = 3, Quality = 6 };

        var updater = _factory.GetUpdater(item);

        Assert.IsType<ConjuredItemUpdater>(updater);
    }

    [Fact]
    public void GetUpdater_ReturnsConjuredItemUpdater_ForConjuredArmor()
    {
        var item = new Item { Name = "Conjured Armor", SellIn = 3, Quality = 6 };

        var updater = _factory.GetUpdater(item);

        Assert.IsType<ConjuredItemUpdater>(updater);
    }

    [Fact]
    public void GetUpdater_ReturnsNormalItemUpdater_ForUnknownItem()
    {
        var item = new Item { Name = "Unknown Item", SellIn = 5, Quality = 10 };

        var updater = _factory.GetUpdater(item);

        Assert.IsType<NormalItemUpdater>(updater);
    }

    [Fact]
    public void GetUpdater_ReturnsNormalItemUpdater_ForElixirOfTheMongoose()
    {
        var item = new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 };

        var updater = _factory.GetUpdater(item);

        Assert.IsType<NormalItemUpdater>(updater);
    }

    [Fact]
    public void GetUpdater_CaseSensitive_DifferentiatesBrieFromOthers()
    {
        var item = new Item { Name = "aged brie", SellIn = 2, Quality = 0 };

        var updater = _factory.GetUpdater(item);

        // Case-sensitive match should return NormalItemUpdater, not AgedBrieUpdater
        Assert.IsType<NormalItemUpdater>(updater);
    }
}
