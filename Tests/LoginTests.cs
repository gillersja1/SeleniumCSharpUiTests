using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumCSharpUiTests.Pages;

namespace SeleniumCSharpUiTests.Tests;

[TestFixture]
public class LoginTests
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
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
    }

    [Test]
    public void StandardUser_CanLogin_AndSeeInventory()
    {
        _loginPage.Login("standard_user", "secret_sauce");

        // Explicit wait for the post-login navigation, rather than assuming
        // the click has fully resolved by the next line.
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.Url.Contains("inventory.html"));

        Assert.That(_inventoryPage.GetPageTitle(), Is.EqualTo("Products"));
    }

    [Test]
    public void LockedOutUser_SeesErrorMessage()
    {
        _loginPage.Login("locked_out_user", "secret_sauce");

        Assert.That(_loginPage.GetErrorMessage(), Does.Contain("locked out"));
    }

    [Test]
    public void EmptyCredentials_ShowsRequiredFieldError()
    {
        _loginPage.Login("", "");

        Assert.That(_loginPage.GetErrorMessage(), Does.Contain("Username is required"));
    }

    [Test]
    public void InvalidCredentials_AreRejected()
    {
        _loginPage.Login("invalid_user", "wrong_password");

        Assert.That(_loginPage.GetErrorMessage(), Does.Contain("do not match"));
    }
}
