using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SamehAutomation
{
    [TestFixture]
    public class Dummy
    {
        public static string TestCasesDataFile = ConfigurationManager.AppSettings["TestCasesDataFile"];


        [Test]
        public void DummyTest()
        {



            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.amazon.com/");
            driver.Manage().Window.Maximize();

            IWebElement searchElement = driver.FindElement(By.Id("twotabsearchtextbox"));
            searchElement.SendKeys("PS4");
            searchElement.SendKeys(Keys.Enter);
            IWebElement searchLink = driver.FindElement(By.LinkText("PlayStation 4 Slim 1TB Console"));
            searchLink.Click();
            IWebElement productTitle = driver.FindElement(By.Id("productTitle"));
            string ptxt =productTitle.Text;
            

            Assert.AreEqual("PlayStation 4 Slim 1TB Console",ptxt);
            driver.Quit();


        }

        [Test]
        public void ReadTestDataFromExcel()
        {
            TestCasesParser.Init(TestCasesDataFile);

            var testCaseName = MethodBase.GetCurrentMethod().Name;
            var worksheetName = GetType().Name;

            var searchText = TestCasesParser.GetValueOf("SearchTxt", testCaseName, worksheetName);

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.amazon.com/");
            driver.Manage().Window.Maximize();

            IWebElement searchElement = driver.FindElement(By.Id("twotabsearchtextbox"));
            searchElement.SendKeys(searchText);
            searchElement.SendKeys(Keys.Enter);
            IWebElement searchLink = driver.FindElement(By.LinkText("PlayStation 4 Slim 1TB Console"));
            searchLink.Click();
            IWebElement productTitle = driver.FindElement(By.Id("productTitle"));
            string ptxt = productTitle.Text;


            Assert.AreEqual("PlayStation 4 Slim 1TB Console", ptxt);
            driver.Quit();


        }

    }
}
