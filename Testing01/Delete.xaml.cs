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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Testing01
{
    /// <summary>
    /// Interaction logic for Delete.xaml
    /// </summary>
    public partial class Delete : Window
    {
        public Delete()
        {
            InitializeComponent();
        }

        [TestFixture]
        public class DeleteConstructionTests
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

                // Đăng nhập vào hệ thống
                driver.Navigate().GoToUrl(baseUrl);
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("email"))).SendKeys("admin");
                driver.FindElement(By.Id("password")).SendKeys("Abc123456");
                driver.FindElement(By.XPath("//button[@type='submit']")).Click();

                // Chờ trang chính load xong
                wait.Until(d => d.Url == baseUrl);
            }

            [Test]
            public void Test_DeleteConstruction()
            {
                GoToConstructionPage();
                DeleteConstruction("Công trình ABC");
                VerifyConstructionDeleted("Công trình ABC");
            }

            private void GoToConstructionPage()
            {
                // Mở menu "Công trình khai thác"
                IWebElement menu = wait.Until(d => d.FindElement(By.XPath("//li[@title='Công trình khai thác']")));
                menu.Click();
                Thread.Sleep(2000); // Chờ menu mở

                // Tìm và click vào danh sách công trình
                IWebElement listButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(text(), 'Danh sách')]")));
                listButton.Click();
            }

            private void DeleteConstruction(string constructionName)
            {
                // Tìm công trình cần xóa
                IWebElement row = wait.Until(d => d.FindElements(By.XPath("//table//tr"))
                                                 .FirstOrDefault(tr => tr.Text.Contains(constructionName)));

                Assert.IsNotNull(row, "Không tìm thấy công trình cần xóa!");

                // Click vào nút "Xóa"
                IWebElement deleteButton = row.FindElement(By.XPath(".//button[contains(@class, 'delete-button')]"));
                deleteButton.Click();

                // Xác nhận xóa
                IWebElement confirmButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(text(), 'Đồng ý')]")));
                confirmButton.Click();

                // Chờ hệ thống xử lý
                Thread.Sleep(2000);
            }

            private void VerifyConstructionDeleted(string constructionName)
            {
                // Kiểm tra công trình đã bị xóa khỏi danh sách
                bool isDeleted = wait.Until(d =>
                {
                    var rows = d.FindElements(By.XPath("//table//tr"));
                    return rows.All(tr => !tr.Text.Contains(constructionName));
                });

                Assert.IsTrue(isDeleted, "Công trình chưa được xóa!");
            }

            [TearDown]
            public void TearDown()
            {
                driver.Quit();
            }
        }
    }
}
