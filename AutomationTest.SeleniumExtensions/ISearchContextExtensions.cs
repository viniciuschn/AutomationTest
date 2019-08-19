using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace AutomationTest.SeleniumExtensions
{
    public static class ISearchContextExtensions
    {
        public static IWebElement FindElementRetrying(this ISearchContext element, By by)
        {
            IWebElement found = null;

            var count = 0;

            while (found == null)
            {
                // We should not wait indefinitely for finding the element, so we run this loop 100 times only.
                // If the element is not found within 5 seconds (100 times), an exception is thrown.
                if (count > 100) throw new Exception("Could not find element in the page.");

                count++;

                // We wait 50 milisseconds between each try so we don't overuse the processor.
                Thread.Sleep(50);

                var innerElements = element.FindElements(by);

                if (innerElements.Count > 0)
                    found = innerElements[0];
            }

            return found;
        }

        public static void SetSelectElementText(this ISearchContext element, By by, string value)
        {
            var selectElement = new SelectElement(element.FindElement(by));

            IWebElement found = null;

            var count = 0;

            while (found == null)
            {
                // We should not wait indefinitely for finding the element, so we run this loop 100 times only.
                // If the element is not found within 5 seconds (100 times), an exception is thrown.
                if (count > 100) throw new Exception("Could not find element in the page.");

                count++;

                // We wait 50 milisseconds between each try so we don't overuse the processor.
                Thread.Sleep(50);

                found = selectElement.Options.Where(option => option.Text == value).FirstOrDefault();
            }

            selectElement.SelectByText(value);
        }

        public static IWebElement FindElementWithCondition(this ISearchContext element, By by, Func<IWebElement, bool> condition)
        {
            IWebElement found = null;

            var count = 0;

            while (found == null)
            {
                // We should not wait indefinitely for finding the element, so we run this loop 100 times only.
                // If the element is not found within 5 seconds (100 times), an exception is thrown.
                if (count > 100) throw new Exception("Could not find element in the page.");

                count++;

                // We wait 50 milisseconds between each try so we don't overuse the processor.
                Thread.Sleep(50);

                var innerElement = element.FindElement(by);

                if (condition(innerElement))
                    found = innerElement;
            }

            return found;
        }

        public static string GetText(this ISearchContext element, By by)
        {
            return element.FindElement(by).Text;
        }

        public static void SetText(this ISearchContext element, By by, string text)
        {
            element.FindElement(by).SendKeys(text);
        }

        public static void Submit(this ISearchContext element, By by)
        {
            element.FindElement(by).Submit();
        }

        public static void Click(this ISearchContext element, By by)
        {
            element.FindElement(by).Click();
        }
    }
}
