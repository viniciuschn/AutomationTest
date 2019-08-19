using AutomationTest.SeleniumExtensions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;

namespace AutomationTest.Tests
{
    public class AutomationTests
    {
        private IConfiguration _configuration;

        public AutomationTests()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json");
            _configuration = builder.Build();
        }

        [Theory]
        [InlineData(Browser.Firefox)]
        [InlineData(Browser.Chrome)]
        public void Buy_Single_Product(Browser browser)
        {
            var manager = new AutomationManager(_configuration, browser);

            manager.GoToStartPage();

            try
            {
                manager.SelectFeaturedProduct(1);
                manager.AddProductToCart();
                manager.ValidateAddedProduct("Faded Short Sleeve T-shirts", "Orange, S", 1, 16.51m);
                manager.ValidatedAddedProductCart(1, 16.51m, 2m);
                manager.ProceedToCheckout();

                /// E-mail address should change within each test
                var account = new AccountDetails
                {
                    Address = "Rua Coronel Lisboa, 146",
                    AddressAlias = "Casa",
                    City = "São Paulo",
                    Country = "United States",
                    Email = $"vinicius{Guid.NewGuid().ToString("N")}@teste.com",
                    FirstName = "Vinicius",
                    LastName = "Lopes",
                    Mobile = "99 99999-99999",
                    Password = "umdoistres",
                    State = "Alabama",
                    ZipCode = "12345"
                };

                manager.CreateAccount(account);
                manager.CheckAddress(account);
                manager.AcceptTermsOfService();
                manager.CheckSaleTotalAmount(19.25m);
                manager.PayByBankWire(19.25m);
            }
            finally
            {
                manager.ClosePage();
            }
        }
    }
}
