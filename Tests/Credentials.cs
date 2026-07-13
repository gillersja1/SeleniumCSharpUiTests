using System;

namespace SeleniumCSharpUiTests;

/// <summary>
/// Centralized test credentials. Password can be overridden via the
/// TEST_PASSWORD environment variable to avoid embedding sensitive or
/// easily-searchable values in the codebase.
///
/// Note: saucedemo.com expects the default password "secret_sauce" for
/// the provided demo users. If you change the password here, update the
/// test account on the target site or set the TEST_PASSWORD env var in CI.
/// </summary>
public static class Credentials
{
    public static string StandardUser => Environment.GetEnvironmentVariable("TEST_STANDARD_USER") ?? "standard_user";

    // Allows overriding the password without recompiling. Falls back to the
    // demo site password so tests keep working by default.
    public static string Password => Environment.GetEnvironmentVariable("TEST_PASSWORD") ?? "secret_sauce";
}
