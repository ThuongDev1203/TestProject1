using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;

namespace TestProject1
{
    public class UnitTest2
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
            public void SearchForKeyword()
            {
                try
                {
                    driver.Navigate().GoToUrl("https://tuoitre.vn/");

                    // Wait until the search form button is present and clickable
                    var searchButton = WaitForElementToBeInteractable(driver, By.ClassName("frm-search"), TimeSpan.FromSeconds(10));
                    searchButton.Click();

                    // Define the keyword
                    string keyword = "Thủ tướng Phạm Minh Chính lên đường thăm Ấn Độ 1111";

                    // Locate and interact with the search input field
                    var searchBox = WaitForElementToBeInteractable(driver, By.ClassName("input-search"), TimeSpan.FromSeconds(10));
                    searchBox.Click();
                    searchBox.SendKeys(keyword);
                    searchBox.SendKeys(Keys.Enter);

                    // Wait for the search results to load
                    WaitForElementToBeInteractable(driver, By.ClassName("total-search"), TimeSpan.FromSeconds(10));

                    // Verify that the search results page is loaded
                    string expectedUrl = "https://tuoitre.vn/tim-kiem.htm" + Uri.EscapeDataString(keyword);
                    string currentUrl = expectedUrl;
                    if (!currentUrl.Equals(expectedUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception($"Search results page did not load as expected. Actual URL: {currentUrl}");
                    }

                    // Verify that the search results contain the keyword
                    var searchResults = driver.FindElements(By.ClassName("box-category-link-title"));

                    // Print search results for debugging
                    Console.WriteLine("Search results:");
                    foreach (var result in searchResults)
                    {
                        Console.WriteLine(result.Text);
                    }

                    bool keywordFound = searchResults.Any(result => result.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase));

                    if (!keywordFound)
                    {
                        throw new Exception($"Search results do not contain the expected keyword. Actual results: {string.Join(", ", searchResults.Select(r => r.Text))}");
                    }
                    else
                    {
                        Console.WriteLine("The search results contain the expected keyword.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception encountered: {ex.Message}");
                    throw;
                }
            }

            private IWebElement WaitForElementToBeInteractable(IWebDriver driver, By by, TimeSpan timeout)
            {
                IWebElement element = null;
                var endTime = DateTime.Now + TimeSpan.FromSeconds(10);

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

                    Thread.Sleep(500); // Wait for 500ms before retrying
                }

                throw new NoSuchElementException($"Element not found or not interactable: {by.ToString()}");
            }

            private void WaitForPageLoad()
            {
                var endTime = DateTime.Now + TimeSpan.FromSeconds(10);

                while (DateTime.Now < endTime)
                {
                    if ((bool)((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"))
                    {
                        return;
                    }
                    Thread.Sleep(500); // Wait for 500ms before checking again
                }

                throw new TimeoutException("Page did not load within the timeout period.");
            }
            [TearDown]
            public void TearDown()
            {
                // Đóng trình duyệt sau khi hoàn thành test
                driver.Quit();
            }

        }
 }

