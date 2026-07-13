using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumCSharpUiTests.Pages;

namespace SeleniumCSharpUiTests.Tests;

[TestFixture]
public class CartTests
{
    private IWebDriver _driver = null!;
    private LoginPage _loginPage = null!;
    private InventoryPage _inventoryPage = null!;

    [SetUp]
    public void SetUp()
    {
        _driver = DriverFactory.CreateChromeDriver();
        _loginPage = new LoginPage(_driver);
        _inventoryPage = new InventoryPage(_driver);
        _loginPage.Goto();
        _loginPage.Login(Credentials.StandardUser, Credentials.Password);
    }

    [TearDown]
    public void TearDown()
    {
        if (_driver != null)
        {
            try { _driver.Quit(); } catch { }
            try { _driver.Dispose(); } catch { }
        }
    }

    [Test]
    public void AddingItem_UpdatesCartBadge()
    {
        _inventoryPage.AddItemToCart("Sauce Labs Backpack");

        Assert.That(_inventoryPage.GetCartCount(), Is.EqualTo(1));
    }

    [Test]
    public void AddingMultipleItems_AccumulatesCartCount()
    {
        _inventoryPage.AddItemToCart("Sauce Labs Backpack");
        _inventoryPage.AddItemToCart("Sauce Labs Bike Light");

        Assert.That(_inventoryPage.GetCartCount(), Is.EqualTo(2));
    }

    [Test]
    public void SortingLowToHigh_OrdersItemsCorrectly()
    {
        _inventoryPage.SortBy("lohi");

        var prices = _inventoryPage.GetItemPrices();
        var sortedPrices = new List<double>(prices);
        sortedPrices.Sort();

        Assert.That(prices, Is.EqualTo(sortedPrices));
    }
}
