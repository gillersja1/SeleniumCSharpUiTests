using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumCSharpUiTests;

/// <summary>
/// Centralizes WebDriver creation so tests don't repeat browser setup.
/// Selenium Manager (built into Selenium 4.6+) resolves and downloads a
/// matching chromedriver automatically based on the installed Chrome
/// version - no manual driver management required.
/// </summary>
public static class DriverFactory
{
    public static IWebDriver CreateChromeDriver()
    {
        var options = new ChromeOptions();

        // saucedemo's shared password ("secret_sauce") appears in public
        // breach lists, so Chrome shows a "Change your password" dialog after
        // login. That dialog is browser UI, not part of the page: it doesn't
        // show in screenshots, but it swallows real clicks and key presses,
        // so the add-to-cart tests silently failed. Turn off the password
        // manager and its leak check so the dialog never appears.
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

        // Run headless automatically in CI (GitHub Actions sets CI=true),
        // or locally via: HEADLESS=true dotnet test
        var headless = Environment.GetEnvironmentVariable("CI") == "true"
                        || Environment.GetEnvironmentVariable("HEADLESS") == "true";

        if (headless)
        {
            options.AddArgument("--headless=new");
        }

        options.AddArgument("--window-size=1400,1000");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");

        var driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        return driver;
    }
}
