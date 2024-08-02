using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using NUnit.Framework;
using System;
using System.Threading;

namespace TestProject1
{
    internal class ReadTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            // Khởi tạo trình duyệt Chrome
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void ReadTheArticleTest()
        {
            try
            {
                // Mở trang chủ
                driver.Navigate().GoToUrl("https://tuoitre.vn/");

                // Click vào tiêu đề bài viết
                var read = WaitForElementToBeInteractable(driver, By.CssSelector(".box-content-title .box-title-text a"), TimeSpan.FromSeconds(10));
                read.Click();

                // Click vào logo để về trang chủ 
                var logo = WaitForElementToBeInteractable(driver, By.ClassName("header_top-left"), TimeSpan.FromSeconds(10));
                logo.Click();
            }
            catch (NoSuchElementException e)
            {
                Assert.Fail($"Có lỗi xảy ra trong quá trình kiểm tra: {e.Message}");
            }
        }

        private IWebElement WaitForElementToBeInteractable(IWebDriver driver, By by, TimeSpan timeout)
        {
            IWebElement element = null;
            var endTime = DateTime.Now + timeout;

            while (DateTime.Now < endTime)
            {
                try
                {
                    element = driver.FindElement(by);
                    if (element != null && element.Displayed && element.Enabled)
                    {
                        return element;
                    }
                }
                catch (NoSuchElementException)
                {
                    // Ignore exceptions and retry until timeout
                }

                Thread.Sleep(500);
            }

            throw new NoSuchElementException($"Element not found or not interactable: {by.ToString()}");
        }

        [TearDown]
        public void TearDown()
        {
            // Đóng trình duyệt sau khi hoàn thành test
            driver.Quit();
        }
    }
}
