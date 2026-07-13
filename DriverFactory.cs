using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

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
        // Disable Chrome password manager UI and password leak detection so
        // the browser does not show "compromised password" popups during tests.
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddArgument("--disable-features=PasswordLeakDetection,PasswordManagerOnboarding");
        options.AddArgument("--disable-save-password-bubble");
        options.AddArgument("--disable-password-generation");


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

        // Ensure a matching ChromeDriver binary is downloaded and available.
        // WebDriverManager will fetch a driver that matches the installed browser.
        new DriverManager().SetUpDriver(new ChromeConfig());

        var driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        return driver;
    }
}
