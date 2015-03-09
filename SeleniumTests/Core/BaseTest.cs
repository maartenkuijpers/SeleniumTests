using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SeleniumTests.Core
{
    public class BaseTest
    {
        // Properties defined by SeleniumIDE
        protected StringBuilder verificationErrors;
        protected string baseURL;
        protected bool acceptNextAlert = true;

        // Other properties
        protected List<String> AvailableBrowsersList;
        private List<BrowserInformation> activeDrivers;

        private string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
            return path;
        }

        public BaseTest()
        {
            AvailableBrowsersList = BuildAvailableBrowsersList();
            activeDrivers = new List<BrowserInformation>();
        }

        [SetUp]
        public void SetupBaseTest()
        {
            baseURL = "https://www.the-sub.com/";
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                foreach (var browser in activeDrivers)
                {
                    TakeScreenshot(browser);
                    browser.WebDriver.Quit();
                }
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        protected void TakeScreenshot(BrowserInformation browser)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)browser.WebDriver).GetScreenshot();
                var path = Path.Combine(GetAssemblyDirectory(), ConfigurationManager.AppSettings["ScreenshotsFolder"]);
                path = Path.Combine(path, String.Format("{0}-{1}.png", browser.Name, DateTime.Now.ToString("yyyyMMdd-HHmmss")));
                screenshot.SaveAsFile(path, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        protected IWebDriver GetWebDriver(string browserName)
        {
            var driver = InitialiseWebDriver(browserName);
            driver = SetupDriver(driver);
            activeDrivers.Add(new BrowserInformation { WebDriver = driver, Name = browserName });
            return driver;
        }

        private List<String> BuildAvailableBrowsersList()
        {
            bool useBrowserArray;
            bool.TryParse(ConfigurationManager.AppSettings["UseBrowserArray"], out useBrowserArray);
            var browserArray = useBrowserArray ? ConfigurationManager.AppSettings["WebDriverBrowserArray"] : ConfigurationManager.AppSettings["WebDriverBrowserSingle"];
            return browserArray.Split(',').ToList();
        }

        private IWebDriver InitialiseWebDriver(string webDriver)
        {
            IWebDriver thisDriver;
            var webDriversPath = Path.Combine(GetAssemblyDirectory(), ConfigurationManager.AppSettings["WebDriversPath"]);

            switch (webDriver)
            {
                case "Chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--test-type");
                    thisDriver = new ChromeDriver(webDriversPath, chromeOptions);
                    break;
                case "Firefox":
                    thisDriver = new FirefoxDriver(new FirefoxBinary(), new FirefoxProfile());
                    break;
                case "InternetExplorer":
                    var ieOptions = new InternetExplorerOptions { IntroduceInstabilityByIgnoringProtectedModeSettings = true };
                    thisDriver = new InternetExplorerDriver(webDriversPath, ieOptions);
                    break;
                case "Safari":
                    thisDriver = new SafariDriver(new SafariOptions());
                    break;
                default:
                    thisDriver = new PhantomJSDriver(webDriversPath); // The headless browser
                    break;
            }

            return thisDriver;
        }

        private IWebDriver SetupDriver(IWebDriver driver)
        {
            verificationErrors = new StringBuilder();
            driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
            return driver;
        }
    }
}
