using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace TUPETesting
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            driver.Url = "http://localhost:50705/";
            new WebDriverWait(driver, new TimeSpan(0, 0, 10)).Until(
                wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
            IWebElement registerLink = driver.FindElement(By.XPath("//a[contains(text(), 'Register')]"));
            registerLink.Click();
            Console.WriteLine("Hello World!");
        }
    }
}
