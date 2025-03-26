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
using OpenQA.Selenium.Interactions;
using System.Diagnostics;

namespace Testing01.FunctionTest
{
    internal class TestUpdate
    {
        


        [TestFixture]
            public class EditConstructionTests
        {
            private IWebDriver driver;
            private WebDriverWait wait;
            

            
            public void Test_EditConstruction()
            {
                Setup();
                OpenEditConstructionForm();
                EditConstructionDetails();
                VerifyEditedConstruction();
            }

            [SetUp]
            public void Setup()
            {
                // Khởi tạo WebDriver 
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                IWebDriver chromeDriver = new ChromeDriver(options);
                Debug.WriteLine("Khởi tạo ChromeDriver thành công.");

                try
                {
                    chromeDriver.Navigate().GoToUrl("https://lake-management.desoft.vn/");
                    Debug.WriteLine("Đi tới trang đăng nhập.");

                    WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));

                    IWebElement eleEmail = wait.Until(driver => driver.FindElement(By.Id("email")));
                    IWebElement elePwd = chromeDriver.FindElement(By.Id("password"));
                    IWebElement eleSub = chromeDriver.FindElement(By.XPath("//button[@type='submit']"));

                    Debug.WriteLine("Tìm thấy các trường đăng nhập.");

                    eleEmail.SendKeys("admin");
                    Debug.WriteLine("Nhập email xong.");

                    elePwd.SendKeys("Abc123456");
                    Debug.WriteLine("Nhập mật khẩu xong.");

                    eleSub.Click();
                    Debug.WriteLine("Click nút đăng nhập.");

                    wait.Until(driver => driver.Url == "https://lake-management.desoft.vn/");
                    Debug.WriteLine("Đăng nhập thành công!");

                    // Chờ menu xuất hiện
                    IWebElement leftMenu = wait.Until(driver => driver.FindElement(By.XPath("//ul[contains(@class, 'navBar')]")));
                    Debug.WriteLine("Đã tìm thấy menu bên trái.");

                    Actions actions = new Actions(chromeDriver);

                    // Hover vào menu
                    actions.MoveToElement(leftMenu).Perform();
                    Thread.Sleep(1000); // Chờ menu mở
                    Debug.WriteLine("Di chuyển chuột đến menu.");

                    // Tìm menu "Công trình khai thác"
                    IWebElement menuKhaiThac = wait.Until(driver => driver.FindElement(By.XPath("//li[@title='Công trình khai thác']")));
                    Debug.WriteLine("Đã tìm thấy menu Công trình khai thác.");


                    // Tìm nút mở popup trong Công trình khai thác
                    IWebElement btnKhaiThac = wait.Until(driver => driver.FindElement(By.XPath("//li[@title='Công trình khai thác']//button")));
                    Debug.WriteLine("Tìm thấy nút mở popup.");

                    // Click vào nút mở popup
                    btnKhaiThac.Click();
                    Debug.WriteLine("Đã nhấn vào nút mở popup.");

                    // Chờ popup hiển thị
                    Thread.Sleep(2000);
                    Debug.WriteLine("Popup đã mở thành công!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Có lỗi xảy ra: " + ex.Message);
                }
            }



            [Test]
            

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
        internal static void Test_EditConstruction()
        {
            throw new NotImplementedException();


        }

    }
}
    

