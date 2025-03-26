using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testing01.FunctionTest
{
    internal class TestExport
    {
        public TestExport() { }
        [TestFixture]
        public class ExportFileTests
        {
            private IWebDriver driver;
            private WebDriverWait wait;
            private string baseUrl = "https://lake-management.desoft.vn/";
            private string downloadPath = @"C:\Users\Public\Downloads\"; // Thư mục tải file

            [SetUp]
            public void Setup()
            {
                ChromeOptions options = new ChromeOptions();
                options.AddUserProfilePreference("download.default_directory", downloadPath);
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("disable-popup-blocking", "true");

                driver = new ChromeDriver(options);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Đăng nhập vào hệ thống
                driver.Navigate().GoToUrl(baseUrl);
                wait.Until(d => d.FindElement(By.Id("email"))).SendKeys("admin");
                driver.FindElement(By.Id("password")).SendKeys("Abc123456");
                driver.FindElement(By.XPath("//button[@type='submit']")).Click();
                wait.Until(d => d.Url == baseUrl);

                // Chờ trang chính load xong
                wait.Until(d => d.Url == baseUrl);
            }



            [Test]
            public void Test_ExportFile()
            {
                GoToExportPage();
                ClickExportButton();

                // Chờ 5 giây để file tải xuống
                Thread.Sleep(5000);

                // Kiểm tra file có tồn tại trong thư mục download không
                string latestFile = GetLatestDownloadedFile();
                Assert.That(!string.IsNullOrEmpty(latestFile), "Không tìm thấy file đã tải xuống!");


                // Kiểm tra định dạng file (giả sử file xuất ra là .xlsx)
                Assert.That(latestFile.EndsWith(".xlsx"), "File export không đúng định dạng!");
            }

            private void GoToExportPage()
            {
                // Điều hướng đến menu "Công trình khai thác"
                IWebElement menu = wait.Until(d => d.FindElement(By.XPath("//li[@title='Công trình khai thác']")));
                menu.Click();
                Thread.Sleep(2000); // Đợi menu mở

                // Tìm và click vào nút Export
                IWebElement exportBtn = wait.Until(d => d.FindElement(By.CssSelector(".css-14ayiqf")));
                exportBtn.Click();
            }

            private void ClickExportButton()
            {
                // Click vào nút xác nhận Export
                IWebElement confirmExportBtn = wait.Until(d => d.FindElement(By.XPath("//button[contains(text(), 'Xác nhận')]")));
                confirmExportBtn.Click();
            }

            private string GetLatestDownloadedFile()
            {
                var directory = new DirectoryInfo(downloadPath);
                var latestFile = directory.GetFiles()
                                          .OrderByDescending(f => f.LastWriteTime)
                                          .FirstOrDefault();

                return latestFile?.FullName;
            }

            [TearDown]
            public void TearDown()
            {
                driver.Quit();
            }
        }
    }
}
