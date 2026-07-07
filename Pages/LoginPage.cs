using OpenQA.Selenium;

namespace SeleniumCSharpUiTests.Pages;

/// <summary>
/// Page object for the saucedemo.com login page.
/// </summary>
public class LoginPage
{
    private readonly IWebDriver _driver;

    private IWebElement UsernameInput => _driver.FindElement(By.Id("user-name"));
    private IWebElement PasswordInput => _driver.FindElement(By.Id("password"));
    private IWebElement LoginButton => _driver.FindElement(By.Id("login-button"));
    private IWebElement ErrorMessage => _driver.FindElement(By.CssSelector("[data-test='error']"));

    public LoginPage(IWebDriver driver)
    {
        _driver = driver;
    }

    public void Goto()
    {
        _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
    }

    public void Login(string username, string password)
    {
        UsernameInput.Clear();
        UsernameInput.SendKeys(username);
        PasswordInput.Clear();
        PasswordInput.SendKeys(password);
        LoginButton.Click();
    }

    public string GetErrorMessage() => ErrorMessage.Text;
}
