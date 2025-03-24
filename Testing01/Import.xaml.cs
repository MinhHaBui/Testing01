using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

namespace Testing01
{
    [TestFixture]
    public class ImportFileTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "https://lake-management.desoft.vn/";

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Login();
        }

        private void Login()
        {
            driver.Navigate().GoToUrl(baseUrl);
            wait.Until(d => d.FindElement(By.Id("email"))).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Abc123456");
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            wait.Until(d => d.Url == baseUrl);
        }

        private void OpenImportPopup()
        {
            IWebElement menu = wait.Until(d => d.FindElement(By.XPath("//li[@title='Công trình khai thác']")));
            menu.Click();
            Thread.Sleep(1000);

            IWebElement importBtn = wait.Until(d => d.FindElement(By.XPath("//button//span[contains(text(), 'Import')]")));
            importBtn.Click();
        }

        private void UploadFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", fileName);
            Assert.IsTrue(File.Exists(filePath), $"File không tồn tại: {filePath}");

            IWebElement fileInput = wait.Until(d => d.FindElement(By.XPath("//input[@type='file']")));
            fileInput.SendKeys(filePath);

            IWebElement confirmBtn = driver.FindElement(By.XPath("//button[contains(text(), 'Xác nhận')]"));
            confirmBtn.Click();

            Thread.Sleep(3000);
        }

        [Test]
        public void Test_Import_ValidFile()
        {
            OpenImportPopup();
            UploadFile("valid_data.xlsx");
            IWebElement successMsg = wait.Until(d => d.FindElement(By.ClassName("success-message")));
            Assert.IsTrue(successMsg.Text.Contains("Import thành công"), "Import không thành công!");
        }

        [Test]
        public void Test_Import_InvalidFormat()
        {
            OpenImportPopup();
            UploadFile("invalid_format.xlsx");
            IWebElement errorMsg = wait.Until(d => d.FindElement(By.ClassName("error-message")));
            Assert.IsTrue(errorMsg.Text.Contains("Định dạng không hợp lệ"), "Không hiển thị lỗi khi import file sai định dạng!");
        }

        [Test]
        public void Test_Import_LargeFile()
        {
            OpenImportPopup();
            UploadFile("large_data.xlsx");
            IWebElement successMsg = wait.Until(d => d.FindElement(By.ClassName("success-message")));
            Assert.IsTrue(successMsg.Text.Contains("Import thành công"), "Import file lớn thất bại!");
        }

        [Test]
        public void Test_Import_MissingFields()
        {
            OpenImportPopup();
            UploadFile("missing_fields.xlsx");
            IWebElement errorMsg = wait.Until(d => d.FindElement(By.ClassName("error-message")));
            Assert.IsTrue(errorMsg.Text.Contains("Thiếu dữ liệu"), "Không hiển thị lỗi khi file thiếu dữ liệu!");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
