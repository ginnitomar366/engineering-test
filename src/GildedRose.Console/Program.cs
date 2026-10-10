using System.Collections.Generic;
using GildedRose.Console.Factories;
using GildedRose.Console.Models;

namespace GildedRose.Console;

/// <summary>
/// Main application class for the Gilded Rose inventory management system.
/// Manages a collection of items and orchestrates their quality updates based on item-specific rules.
/// </summary>
/// <remarks>
/// This class serves as the entry point for the application and coordinates item updates
/// using the strategy pattern via the ItemUpdaterFactory.
/// </remarks>
public class Program
{
    /// <summary>
    /// Gets or sets the collection of items in the Gilded Rose inventory.
    /// </summary>
    /// <value>A list of Item objects representing the current inventory.</value>
    public IList<Item> Items = new List<Item>();

    static void Main(string[] args)
    {
        System.Console.WriteLine("OMGHAI!");

        var app = new Program()
                      {
                          Items = new List<Item>
                                      {
                                          new Item {Name = Constants.DexterityVest, SellIn = 10, Quality = 20},
                                          new Item {Name = Constants.AgedBrie, SellIn = 2, Quality = 0},
                                          new Item {Name = Constants.ElixirOfMongoose, SellIn = 5, Quality = 7},
                                          new Item {Name = Constants.SulfurasHandOfRagnaros, SellIn = 0, Quality = 80},
                                          new Item
                                              {
                                                  Name = Constants.BackstagePassConcert,
                                                  SellIn = 15,
                                                  Quality = 20
                                              },
                                          new Item {Name = Constants.ConjuredManaCake, SellIn = 3, Quality = 6}
                                      }

                      };

        app.UpdateQuality();

        System.Console.ReadKey();
    }

    /// <summary>
    /// Updates the quality and sell-in values of all items in the inventory.
    /// </summary>
    /// <remarks>
    /// This method iterates through all items in the inventory and applies item-specific
    /// update logic using the appropriate updater strategy obtained from ItemUpdaterFactory.
    /// Each item's quality and sell-in values are modified according to the rules defined
    /// for that item type (Normal, Aged Brie, Backstage Pass, Sulfuras, or Conjured).
    /// </remarks>
    public void UpdateQuality()
    {
        var factory = new ItemUpdaterFactory();

        foreach (var item in Items)
        {
            var updater = factory.GetUpdater(item);
            updater.Update(item);
        }
    }
}
