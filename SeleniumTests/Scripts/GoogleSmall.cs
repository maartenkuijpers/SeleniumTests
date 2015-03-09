using System;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumTests.Core;

namespace SeleniumTests.Scripts
{
    [TestFixture]
    public class GoogleSmall : BaseTest
    {
        [SetUp]
        public void SetupTest()
        {
            baseURL = "https://www.google.com/";
        }

        [Test]
        public void TheGoogleSmallTest()
        {
            foreach (var browser in AvailableBrowsersList)
            {
                var driver = GetWebDriver(browser);
                driver.Navigate().GoToUrl(baseURL + "/?gfe_rd=cr&ei=xrP2VOR7yK861eeB4Ak");
                driver.FindElement(By.Id("lst-ib")).Clear();
                driver.FindElement(By.Id("lst-ib")).SendKeys("small");
                driver.FindElement(By.Name("btnG")).Click();
                for (int second = 0;; second++)
                {
                    if (second >= 60) Assert.Fail("timeout");
                    try
                    {
                        if (Regex.IsMatch(driver.FindElement(By.Id("resultStats")).Text,
                            @"Ongeveer 4\.8\d{2}\.\d{3}\.\d{3} resultaten .*")) break;
                    }
                    catch (Exception)
                    {
                    }
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
