using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testing01.FunctionTest
{
    internal class TestDelete
    {
        internal static void Test_DeleteConstruction()
        {
            throw new NotImplementedException();
        }

        [TestFixture]
        public class DeleteConstructionTests
        {
            private IWebDriver driver;
            private WebDriverWait wait;
            private string baseUrl = "https://lake-management.desoft.vn/";

            

            
            public void Test_DeleteConstruction()
            {
                Setup();
                GoToConstructionPage();
                DeleteConstruction();
                VerifyConstructionDeleted();
            }

            [SetUp]
            public void Setup()
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--start-maximized");

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
            public void GoToConstructionPage()
            {
                // Mở menu "Công trình khai thác"
                IWebElement menu = wait.Until(d => d.FindElement(By.XPath("//li[@title='Công trình khai thác']")));
                menu.Click();
                Thread.Sleep(2000); // Chờ menu mở

                // Tìm và click vào danh sách công trình
                IWebElement listButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(text(), 'Danh sách')]")));
                listButton.Click();
            }

            public void DeleteConstruction()
            {
                string constructionName = "Sê San 4A";
                // Tìm công trình cần xóa
                IWebElement row = wait.Until(d => d.FindElements(By.XPath("//table//tr"))
                                                 .FirstOrDefault(tr => tr.Text.Contains(constructionName)));

                Assert.That(row, Is.Not.Null, "Không tìm thấy công trình cần xóa!");

                // Click vào nút "Xóa"
                IWebElement deleteButton = row.FindElement(By.XPath(".//button[contains(@class, 'delete-button')]"));
                deleteButton.Click();

                // Xác nhận xóa
                IWebElement confirmButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(text(), 'Đồng ý')]")));
                confirmButton.Click();

                // Chờ hệ thống xử lý
                Thread.Sleep(2000);
            }

            public void VerifyConstructionDeleted()
            {
                string constructionName = "Sê San 4A";
                // Kiểm tra công trình đã bị xóa khỏi danh sách
                bool isDeleted = wait.Until(d =>
                {
                    var rows = d.FindElements(By.XPath("//table//tr"));
                    return rows.All(tr => !tr.Text.Contains(constructionName));
                });

                Assert.That(isDeleted, "Công trình chưa được xóa!");

            }

            [TearDown]
            public void TearDown()
            {
                driver.Quit();
            }
        }
    }
}

