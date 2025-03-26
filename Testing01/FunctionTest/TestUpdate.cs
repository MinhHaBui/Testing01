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
    internal class TestUpdate
    {
        internal static void Test_EditConstruction()
        {
            throw new NotImplementedException();
        }

        /* public TestUpdate() 
{*/


        [TestFixture]
            public class EditConstructionTests
        {
            private IWebDriver driver;
            private WebDriverWait wait;
            private string baseUrl = "https://lake-management.desoft.vn/";

            
            public void Test_EditConstruction()
            {
                Setup();
                GoToConstructionPage();
                OpenEditConstructionForm();
                EditConstructionDetails();
                VerifyEditedConstruction();
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

            public void OpenEditConstructionForm()
            {
                string constructionName = "Sê San 4A";
                // Tìm công trình cần chỉnh sửa
                IWebElement row = wait.Until(d => d.FindElements(By.XPath("//table//tr"))
                                                 .FirstOrDefault(tr => tr.Text.Contains(constructionName)));

                Assert.That(row, Is.Not.Null, "Không tìm thấy công trình cần chỉnh sửa!");

                // Click vào nút "Sửa"
                IWebElement editButton = row.FindElement(By.CssSelector("[data-testid='ModeEditOutlinedIcon']"));
                editButton.Click();
            }

            public void EditConstructionDetails()
            {
                string newName = "Kiểm tra";
                // Chờ trang chỉnh sửa mở
                IWebElement nameInput = wait.Until(d => d.FindElement(By.Id("name")));
                nameInput.Clear();
                nameInput.SendKeys(newName);

                // Click nút "Lưu"
                IWebElement saveButton = driver.FindElement(By.CssSelector(".css-m8gy81"));
                saveButton.Click();

                // Chờ thay đổi được lưu
                Thread.Sleep(2000);
            }

            public void VerifyEditedConstruction()
            {
                string expectedName = "Kiểm tra";
                // Kiểm tra công trình đã được chỉnh sửa
                IWebElement updatedRow = wait.Until(d => d.FindElements(By.XPath("//table//tr"))
                                                          .FirstOrDefault(tr => tr.Text.Contains(expectedName)));

                Assert.That(updatedRow, Is.Not.Null, "Công trình chưa được cập nhật!");

            }

            [TearDown]
            public void TearDown()
            {
                driver.Quit();
            }
        }
    }
}
    

