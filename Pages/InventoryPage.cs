using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCSharpUiTests.Pages;

/// <summary>
/// Page object for the saucedemo.com inventory (product listing) page.
/// </summary>
public class InventoryPage
{
    private readonly IWebDriver _driver;

    private IWebElement PageTitle => _driver.FindElement(By.ClassName("title"));
    private IReadOnlyCollection<IWebElement> CartBadge => _driver.FindElements(By.ClassName("shopping_cart_badge"));
    private IReadOnlyCollection<IWebElement> InventoryItems => _driver.FindElements(By.ClassName("inventory_item"));
    private IWebElement SortDropdown => _driver.FindElement(By.CssSelector("[data-test='product-sort-container']"));

    public InventoryPage(IWebDriver driver)
    {
        _driver = driver;
    }

    public string GetPageTitle() => PageTitle.Text;

    public void AddItemToCart(string itemName)
    {
        var item = InventoryItems.First(i => i.Text.Contains(itemName));
        item.FindElement(By.TagName("button")).Click();
    }

    public int GetCartCount()
    {
        var badges = CartBadge;
        return badges.Count == 0 ? 0 : int.Parse(badges.First().Text);
    }

    public void SortBy(string value)
    {
        var select = new SelectElement(SortDropdown);
        select.SelectByValue(value);
    }

    public List<double> GetItemPrices()
    {
        return _driver.FindElements(By.ClassName("inventory_item_price"))
            .Select(e => double.Parse(e.Text.Replace("$", "")))
            .ToList();
    }
}
