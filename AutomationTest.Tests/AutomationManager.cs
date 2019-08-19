using AutomationTest.SeleniumExtensions;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using Xunit;

namespace AutomationTest.Tests
{
    public class AutomationManager
    {
        private IConfiguration _configuration;
        private IWebDriver _driver;

        public AutomationManager(IConfiguration configuration, Browser browser)
        {
            _configuration = configuration;

            string driverPath = null;

            switch (browser)
            {
                case Browser.Firefox:
                    driverPath = _configuration.GetSection("Selenium:FirefoxDriver").Value;
                   break;
                case Browser.Chrome:
                    driverPath = _configuration.GetSection("Selenium:ChromeDriver").Value;
                    break;
            }

            _driver = WebDriverFactory.CreateWebDriver(browser, driverPath);
        }

        public void GoToStartPage()
        {
            var configuration = _configuration.GetSection("Selenium:Url").Value;
            _driver.LoadPage(TimeSpan.FromSeconds(10), configuration);
        }

        public void ClosePage()
        {
            _driver.Close();
        }

        public void SelectFeaturedProduct(int productIndex)
        {
            var xPath = $"//*[@id=\"homefeatured\"]/li[{productIndex}]/div/div[1]/div/a[1]";
            _driver.Click(By.XPath(xPath));
        }

        public void AddProductToCart()
        {
            _driver.Submit(By.XPath("//*[@id=\"add_to_cart\"]/button"));
        }

        private IWebElement FindCartLayer()
        {
            // This code is required, because the "layer_cart" div exists, but is hidden.
            // When we submit the page, its visibility is set to "block", and some sort of script updates its inner contents.
            // So we should wait until this script finishes updating the content before proceeding with the assertions.
            // After the div is visible, there is no need to wait anymore for its inner items to update.
            return _driver.FindElementWithCondition(By.XPath("//*[@id=\"layer_cart\"]"), element => element.GetAttribute("style").Contains("display: block"));
        }

        public void ProceedToCheckout()
        {
            var cartLayer = FindCartLayer();
            cartLayer.Click(By.XPath("//*[@id=\"layer_cart\"]/div[1]/div[2]/div[4]/a"));
            _driver.Click(By.XPath("//*[@id=\"center_column\"]/p[2]/a[1]"));
        }

        private string GetPriceText(decimal price)
        {
            return price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        public void ValidateAddedProduct(string productName, string attributes, int quantity, decimal total)
        {
            var cartLayer = FindCartLayer();
            Assert.Equal(productName, cartLayer.GetText(By.XPath("//*[@id=\"layer_cart_product_title\"]")));
            Assert.Equal(quantity.ToString(), cartLayer.GetText(By.Id("layer_cart_product_quantity")));
            Assert.Equal(attributes, cartLayer.GetText(By.Id("layer_cart_product_attributes")));
            Assert.Equal(GetPriceText(total), cartLayer.GetText(By.Id("layer_cart_product_price")));
        }

        public void ValidatedAddedProductCart(int itemsInCart, decimal totalProducts, decimal totalShipping)
        {
            var cartLayer = FindCartLayer();
            var itemsInCartText = itemsInCart == 1 ? "There is 1 item in your cart." : $"There are {itemsInCart} items in your cart.";
            Assert.Equal(itemsInCartText, cartLayer.GetText(By.XPath("//*[@id=\"layer_cart\"]/div[1]/div[2]/h2/span[2]")));
            Assert.Equal(GetPriceText(totalProducts), cartLayer.GetText(By.XPath("//*[@id=\"layer_cart\"]/div[1]/div[2]/div[1]/span")));
            Assert.Equal(GetPriceText(totalShipping), cartLayer.GetText(By.XPath("//*[@id=\"layer_cart\"]/div[1]/div[2]/div[2]/span")));
            Assert.Equal(GetPriceText(totalProducts + totalShipping), cartLayer.GetText(By.XPath("//*[@id=\"layer_cart\"]/div[1]/div[2]/div[3]/span")));
        }

        public void CreateAccount(AccountDetails details)
        {
            _driver.SetText(By.XPath("//*[@id=\"email_create\"]"), details.Email);
            _driver.Submit(By.XPath("//*[@id=\"SubmitCreate\"]"));
            var form = _driver.FindElementRetrying(By.XPath("//*[@id=\"account-creation_form\"]/div[1]"));
            form.SetText(By.XPath("//*[@id=\"customer_firstname\"]"), details.FirstName);
            form.SetText(By.XPath("//*[@id=\"customer_lastname\"]"), details.LastName);
            form.SetText(By.XPath("//*[@id=\"passwd\"]"), details.Password);
            form.SetText(By.XPath("//*[@id=\"address1\"]"), details.Address);
            form.SetText(By.XPath("//*[@id=\"city\"]"), details.City);
            form.SetSelectElementText(By.XPath("//*[@id=\"id_state\"]"), details.State);
            form.SetText(By.XPath("//*[@id=\"postcode\"]"), details.ZipCode);
            form.SetSelectElementText(By.XPath("//*[@id=\"id_country\"]"), details.Country);
            form.SetText(By.XPath("//*[@id=\"phone_mobile\"]"), details.Mobile);
            form.SetText(By.XPath("//*[@id=\"alias\"]"), details.AddressAlias);
            _driver.Submit(By.XPath("//*[@id=\"submitAccount\"]"));
        }

        public void CheckAddress(AccountDetails details)
        {
            var form = _driver.FindElementRetrying(By.XPath("//*[@id=\"center_column\"]/form"));

            Assert.Equal(details.FirstName + " " + details.LastName, form.GetText(By.XPath("//*[@id=\"address_delivery\"]/li[2]")));
            Assert.Equal(details.Address, form.GetText(By.XPath("//*[@id=\"address_delivery\"]/li[3]")));
            Assert.Equal(details.City + ", " + details.State + " " + details.ZipCode, form.GetText(By.XPath("//*[@id=\"address_delivery\"]/li[4]")));
            Assert.Equal(details.Country, form.GetText(By.XPath("//*[@id=\"address_delivery\"]/li[5]")));
            Assert.Equal(details.Mobile, form.GetText(By.XPath("//*[@id=\"address_delivery\"]/li[6]")));

            Assert.Equal(details.FirstName + " " + details.LastName, form.GetText(By.XPath("//*[@id=\"address_invoice\"]/li[2]")));
            Assert.Equal(details.Address, form.GetText(By.XPath("//*[@id=\"address_invoice\"]/li[3]")));
            Assert.Equal(details.City + ", " + details.State + " " + details.ZipCode, form.GetText(By.XPath("//*[@id=\"address_invoice\"]/li[4]")));
            Assert.Equal(details.Country, form.GetText(By.XPath("//*[@id=\"address_invoice\"]/li[5]")));
            Assert.Equal(details.Mobile, form.GetText(By.XPath("//*[@id=\"address_invoice\"]/li[6]")));

            _driver.Submit(By.XPath("//*[@id=\"center_column\"]/form/p/button"));
        }

        public void AcceptTermsOfService()
        {
            var form = _driver.FindElementRetrying(By.XPath("//*[@id=\"form\"]"));

            form.Click(By.XPath("//*[@id=\"cgv\"]"));

            _driver.Submit(By.XPath("//*[@id=\"form\"]/p/button"));
        }

        public void CheckSaleTotalAmount(decimal totalAmount)
        {
            Assert.Equal(GetPriceText(totalAmount),_driver.GetText(By.XPath("//*[@id=\"total_price\"]")));
        }

        public void PayByBankWire(decimal totalAmount)
        {
            _driver.Click(By.XPath("//*[@id=\"HOOK_PAYMENT\"]/div[1]/div/p/a"));
            _driver.Submit(By.XPath("//*[@id=\"cart_navigation\"]/button"));
            Assert.Equal("Your order on My Store is complete.", _driver.GetText(By.XPath("//*[@id=\"center_column\"]/div/p/strong")));
            Assert.Equal(GetPriceText(totalAmount), _driver.GetText(By.XPath("//*[@id=\"center_column\"]/div/span/strong")));
        }
    }
}
