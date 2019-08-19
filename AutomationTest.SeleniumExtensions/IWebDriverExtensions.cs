using OpenQA.Selenium;
using System;

namespace AutomationTest.SeleniumExtensions
{
    public static class IWebDriverExtensions
    {
        public static void LoadPage(this IWebDriver driver, TimeSpan timeout, string url)
        {
            driver.Manage().Timeouts().PageLoad = timeout;
            driver.Navigate().GoToUrl(url);
        }
    }
}
