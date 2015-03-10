using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumTests.Core;

namespace SeleniumTests.Scripts
{
    [TestFixture]
    public class AgeGate : BaseTest
    {
        [SetUp]
        public void SetupTest()
        {
            baseURL = "https://www.the-sub.com/";
        }

        [Test]
        public void TheAgeGateTest()
        {
            foreach (var browserName in AvailableBrowsersList)
            {
                var driver = GetWebDriver(browserName);

                driver.Navigate().GoToUrl(baseURL + "/nl/nl/Home");
                driver.FindElement(By.Id("Body_ctl00_agegatewaymaincontent_0_txtDD")).Clear();
                driver.FindElement(By.Id("Body_ctl00_agegatewaymaincontent_0_txtDD")).SendKeys("08");
                driver.FindElement(By.Id("Body_ctl00_agegatewaymaincontent_0_txtMM")).Clear();
                driver.FindElement(By.Id("Body_ctl00_agegatewaymaincontent_0_txtMM")).SendKeys("08");
                driver.FindElement(By.Id("Body_ctl00_agegatewaymaincontent_0_txtYY")).Clear();
                driver.FindElement(By.Id("Body_ctl00_agegatewaymaincontent_0_txtYY")).SendKeys("80");
                driver.FindElement(By.Id("Body_ctl00_agegatewaymaincontent_0_btnOK")).Click();
            }
        }
    }
}
