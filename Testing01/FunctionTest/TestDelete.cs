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
using OfficeOpenXml;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using System.IO;
using static Microsoft.IO.RecyclableMemoryStreamManager;


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
            public IWebDriver driver;
            public WebDriverWait wait;
            string baseUrl = "https://lake-management.desoft.vn/";

            [SetUp]
            public static void Setup()
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

                    DeleteConstruction(chromeDriver);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Có lỗi xảy ra: " + ex.Message);
                }
            }

            [Test]
            

            public static void DeleteConstruction(IWebDriver chromeDriver)
            {
               
                string constructionName = "Kiem tra";
                // Tìm công trình cần xóa
                IWebElement row = chromeDriver.FindElements(By.XPath("//table//tr"))
                                                 .FirstOrDefault(tr => tr.Text.Contains(constructionName));

                Assert.That(row, Is.Not.Null, "Khong tim thay cong trinh can xoa!");

                // Click vào nút "Xóa"
                IWebElement deleteButton = row.FindElement(By.CssSelector("data-testid='DeleteForeverOutlinedIcon'"));
                deleteButton.Click();

                // Xác nhận xóa
                IWebElement confirmButton = chromeDriver.FindElement(By.CssSelector(".css-p7k7eh"));
                confirmButton.Click();

                // Chờ hệ thống xử lý
                Thread.Sleep(2000);

                //VerifyConstructionDeleted(wait);
            }

            public static void VerifyConstructionDeleted(WebDriverWait wait)
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

