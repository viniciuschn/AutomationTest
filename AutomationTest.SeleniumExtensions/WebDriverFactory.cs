using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;

namespace AutomationTest.SeleniumExtensions
{
    /// <summary>
    /// Factory used to isntantiate Selenium WebDrivers.
    /// </summary>
    public static class WebDriverFactory
    {
        /// <summary>
        /// Creates a Selenium WebDriver using the specified browser and driver path.
        /// </summary>
        /// <param name="browser">The browser to use to run the driver.</param>
        /// <param name="driverPath">The path of the driver on the disk.</param>
        /// <returns></returns>
        public static IWebDriver CreateWebDriver(Browser browser, string driverPath)
        {
            IWebDriver driver = null;

            switch (browser)
            {
                case Browser.Firefox:
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(driverPath);
                    driver = new FirefoxDriver(service);
                    break;
                case Browser.Chrome:
                    driver = new ChromeDriver(driverPath);
                    break;
            }

            return driver;
        }
    }
}
