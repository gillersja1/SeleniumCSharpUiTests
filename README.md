# Selenium C# UI Tests

A UI test automation example using [Selenium WebDriver](https://www.selenium.dev/)
and C#/NUnit, testing [saucedemo.com](https://www.saucedemo.com/) — a
public demo store built for test automation practice.

## Why this repo exists

This portfolio already has a Playwright-based C# UI suite
(`PlaywrightTestingShowcase`). This project covers the same kind of ground
with Selenium instead, since it's still the most widely used browser
automation tool in the industry and plenty of teams' existing suites are
built on it. Having both in the portfolio shows range across the two most
common WebDriver-style tools, and makes it easy to compare how each
approaches the same problem.

Notable differences from the Playwright version worth knowing about if
you're comparing them side by side:

- Selenium has **no built-in auto-waiting** the way Playwright does —
  actions don't wait for elements to be actionable automatically. This
  project uses an implicit wait as a baseline plus one explicit
  `WebDriverWait` example for a post-navigation check, which is the more
  reliable pattern for anything timing-sensitive.
- Driver management is handled by **Selenium Manager**, built into
  Selenium 4.6+ — no separate driver binaries, NuGet driver packages, or
  PATH configuration required.

## Project structure

```
SeleniumCSharpUiTests/
├── Pages/
│   ├── LoginPage.cs        # Page object for the login page
│   └── InventoryPage.cs    # Page object for the product listing / cart
├── Tests/
│   ├── LoginTests.cs
│   └── CartTests.cs
├── DriverFactory.cs         # Centralized browser setup (headless/CI-aware)
├── SeleniumCSharpUiTests.csproj
└── .github/workflows/selenium-tests.yml
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Google Chrome installed locally (Selenium Manager handles the driver, not the browser itself)

## Running the tests

```bash
dotnet restore
dotnet test
```

By default this runs with a visible Chrome window locally. To run headless:

```bash
HEADLESS=true dotnet test        # macOS/Linux
$env:HEADLESS="true"; dotnet test  # Windows PowerShell
```

In CI, `DriverFactory` detects GitHub Actions' `CI=true` environment
variable automatically and always runs headless there.

## What's being tested

- **`LoginTests.cs`** — standard login, locked-out user, empty-field
  validation, and invalid-credential handling
- **`CartTests.cs`** — adding one or more items to the cart and verifying
  the cart badge count, plus verifying the "price low to high" sort
  actually sorts

## CI/CD

`.github/workflows/selenium-tests.yml` runs on every push/PR to `main`.
No browser install step is needed — GitHub-hosted `ubuntu-latest` runners
ship with Chrome preinstalled, and Selenium Manager resolves a matching
chromedriver automatically the first time a test runs.

## Troubleshooting

- **Clicks silently do nothing after login.** saucedemo's shared password
  appears in public breach lists, so Chrome shows a "Change your password"
  dialog. It's browser UI, so it doesn't appear in screenshots, but it
  swallows real clicks and key presses. `DriverFactory` turns off Chrome's
  password manager and leak detection so the dialog never appears.
- **`session not created: This version of ChromeDriver only supports Chrome
  version N`.** An old `chromedriver.exe` on your `PATH` takes priority over
  Selenium Manager. Remove it from `PATH` (or delete it) and Selenium Manager
  will download the matching driver automatically.

## Possible extensions

- Add a `BaseTest` fixture to remove the repeated `SetUp`/`TearDown` between `LoginTests` and `CartTests`
- Parameterize browser choice (Chrome/Firefox/Edge) via an environment variable or `.runsettings` file
- Add explicit `WebDriverWait` usage more broadly instead of relying on the implicit wait, for finer control over flaky-prone interactions
- Add Selenium Grid or a cloud grid provider (BrowserStack, Sauce Labs) for cross-browser/cross-device runs, mirroring the `BrowserStackMobileTests` project elsewhere in this portfolio
