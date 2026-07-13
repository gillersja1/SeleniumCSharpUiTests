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
        var previousCount = GetCartCount();

        // Find the inventory item by its displayed name to avoid text-matching the whole item block.
        var item = InventoryItems.First(i => i.FindElement(By.ClassName("inventory_item_name")).Text.Contains(itemName));
        var button = item.FindElement(By.TagName("button"));
        button.Click();

        // Wait until the cart badge increments to reflect the added item. Increase timeout to reduce flakiness.
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        wait.Until(d =>
        {
            try
            {
                return GetCartCount() > previousCount;
            }
            catch
            {
                return false;
            }
        });
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
