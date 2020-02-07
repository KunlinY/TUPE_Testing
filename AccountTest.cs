using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TUPETesting
{
    class AccountTest
    {
        IWebDriver driver;
        IJavaScriptExecutor js;
        int waitSec = 30;

        [SetUp]
        public void startBrowser()
        {
            driver = new ChromeDriver(".");
            js = (IJavaScriptExecutor)driver;
            driver.Url = "http://localhost:50705/";
        }

        [Test, Order(1), NonParallelizable]
        public void testRegister()
        {
            driver.Url = "http://localhost:50705/";
            wait();
            IWebElement registerLink = driver.FindElement(By.XPath("//a[contains(text(), 'Register')]"));
            registerLink.Click();

            wait();
            var inputs = driver.FindElements(By.TagName("input"));
            Assert.IsTrue(inputs.Count == 8);

            inputs[0].SendKeys("TestFirst");
            inputs[1].SendKeys("TestSecond");
            inputs[2].SendKeys("TeastOrganization");
            inputs[3].SendKeys("TestPosition");

            inputs[4].SendKeys("WrongNumber");
            Assert.IsTrue(inputs[4].GetAttribute("value") == "");
            Assert.IsTrue(inputs[4].GetAttribute("class").Contains("invalid"));
            inputs[4].Clear();
            sendKeysSlow(inputs[4], "12345");
            // inputs[4].SendKeys("12345");
            Assert.IsTrue(inputs[4].GetAttribute("value") == "12345");
            Assert.IsTrue(inputs[4].GetAttribute("class").Contains("invalid"));
            inputs[4].Clear();
            sendKeysSlow(inputs[4], "1234567890");
            Assert.IsTrue(inputs[4].GetAttribute("value") == "1234567890");
            Assert.IsTrue(inputs[4].GetAttribute("class").Contains("valid"));

            inputs[5].SendKeys("test@");
            inputs[5].SendKeys(Keys.Return);
            Assert.IsTrue(inputs[5].GetAttribute("value") == "test@");
            Assert.IsTrue(inputs[5].GetAttribute("class").Contains("invalid"));
            inputs[5].Clear();
            inputs[5].SendKeys("test@test.test");
            inputs[5].SendKeys(Keys.Return);
            Assert.IsTrue(inputs[5].GetAttribute("value") == "test@test.test");
            Assert.IsTrue(inputs[5].GetAttribute("class").Contains("valid"));

            sendKeysSlow(inputs[6], "AAAAAAAAAAAAAAAA");
            Assert.IsTrue(inputs[6].GetAttribute("class").Contains("invalid"));
            inputs[6].Clear();
            sendKeysSlow(inputs[6], "AAAAAAAAAaaaaaaa");
            Assert.IsTrue(inputs[6].GetAttribute("class").Contains("invalid"));
            inputs[6].Clear();
            sendKeysSlow(inputs[6], "AAAAAAAAAaaaaa22");
            Assert.IsTrue(inputs[6].GetAttribute("class").Contains("invalid"));
            inputs[6].Clear();
            sendKeysSlow(inputs[6], "AAAAAAAAAaaaaa2+");
            Assert.IsTrue(inputs[6].GetAttribute("class").Contains("valid"));

            sendKeysSlow(inputs[7], "AAAAAAAAAaaaaa2++");
            IWebElement submitButton = driver.FindElement(By.XPath("//form/button"));
            submitButton.Click();
            Assert.IsTrue(driver.FindElements(By.ClassName("field-validation-error")).Count == 1);
            inputs[7].Clear();
            sendKeysSlow(inputs[7], "AAAAAAAAAaaaaa2+");
            submitButton.Click();

            wait();
            Assert.IsTrue(driver.FindElements(By.ClassName("validation-message")).Count == 1);
        }

        [Test, Order(2), NonParallelizable]
        public void testLogin()
        {
            driver.Url = "http://localhost:50705/";
            wait();
            IWebElement loginLink = driver.FindElement(By.XPath("//a[contains(text(), 'Log in')]"));
            loginLink.Click();

            wait();
            var inputs = driver.FindElements(By.TagName("input"));
            Assert.IsTrue(inputs.Count == 5);

            sendKeysSlow(inputs[0], "test@test.test");
            sendKeysSlow(inputs[1], "AAAAAAAAAaaaaa2+");
            IWebElement submitButton = driver.FindElement(By.XPath("//form/button"));
            submitButton.Click();
        }

        [TearDown]
        public void closeBrowser()
        {
            // driver.Close();
        }

        public void wait()
        {
            new WebDriverWait(driver, new TimeSpan(0, 0, waitSec)).Until(
                wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }

        public void sendKeysSlow(IWebElement element, String keys)
        {
            foreach(Char ch in keys)
            {
                element.SendKeys(ch.ToString());
                Thread.Sleep(250);
            }
        }
    }
}
