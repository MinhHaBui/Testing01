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
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        public Update()
        {
            InitializeComponent();
        }

        [TestFixture]
        public class EditConstructionTests
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
            public void Test_EditConstruction()
            {
                GoToConstructionPage();
                OpenEditConstructionForm("Công trình ABC");
                EditConstructionDetails("Công trình ABC - Đã chỉnh sửa");
                VerifyEditedConstruction("Công trình ABC - Đã chỉnh sửa");
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

            private void OpenEditConstructionForm(string constructionName)
            {
                // Tìm công trình cần chỉnh sửa
                IWebElement row = wait.Until(d => d.FindElements(By.XPath("//table//tr"))
                                                 .FirstOrDefault(tr => tr.Text.Contains(constructionName)));

                Assert.IsNotNull(row, "Không tìm thấy công trình cần chỉnh sửa!");

                // Click vào nút "Sửa"
                IWebElement editButton = row.FindElement(By.XPath(".//button[contains(@class, 'edit-button')]"));
                editButton.Click();
            }

            private void EditConstructionDetails(string newName)
            {
                // Chờ trang chỉnh sửa mở
                IWebElement nameInput = wait.Until(d => d.FindElement(By.Id("name")));
                nameInput.Clear();
                nameInput.SendKeys(newName);

                // Click nút "Lưu"
                IWebElement saveButton = driver.FindElement(By.XPath("//button[contains(text(), 'Lưu')]"));
                saveButton.Click();

                // Chờ thay đổi được lưu
                Thread.Sleep(2000);
            }

            private void VerifyEditedConstruction(string expectedName)
            {
                // Kiểm tra công trình đã được chỉnh sửa
                IWebElement updatedRow = wait.Until(d => d.FindElements(By.XPath("//table//tr"))
                                                          .FirstOrDefault(tr => tr.Text.Contains(expectedName)));

                Assert.IsNotNull(updatedRow, "Công trình chưa được cập nhật!");
            }

            [TearDown]
            public void TearDown()
            {
                driver.Quit();
            }
        }
    }
}
