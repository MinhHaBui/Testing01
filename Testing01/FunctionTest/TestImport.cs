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
using System.IO;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace Testing01.FunctionTest
{
    internal class TestImport
    {

        [TestFixture]
        public class ImportFileTests
        {
            
            [SetUp]
            public static void ImportFile()
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
                    Debug.WriteLine("Popup CTKT đã mở thành công!");

                    // Mo popup Import
                    IWebElement importBtn = chromeDriver.FindElement(By.CssSelector(".css-1dz4pif"));
                    importBtn.Click();
                    Debug.WriteLine("Popup import đã mở thành công!");

                    /*
                    IWebElement subImportBtn = chromeDriver.FindElement(By.CssSelector(".css-lvjg27"));
                    
                    Debug.WriteLine("Click subButon thanh cong");*/

                    Test_Import_ValidFile();
                    Test_Import_InvalidFormat();
                    Test_Import_LargeFile();
                    Test_Import_MissingFields();
                    TearDown(chromeDriver);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Có lỗi xảy ra: " + ex.Message);
                }
            }



            public static void UploadFile(string fileName)
            {
                IWebDriver chromeDriver = new ChromeDriver();
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", fileName);               
                Assert.That(File.Exists(filePath), Is.True, $"File không tồn tại: {filePath}");

                //Click sub button Import trong popup import
                IWebElement subImportBtn = chromeDriver.FindElement(By.CssSelector(".css-lvjg27"));
                subImportBtn.Click();
                subImportBtn.SendKeys(filePath);
                //Click button Xac Nhan
                IWebElement XacNhanBtn = chromeDriver.FindElement(By.CssSelector(".css-epn919"));
                XacNhanBtn.Click();

                Thread.Sleep(3000);
            }

            [Test]
            public static void Test_Import_ValidFile()
            {
                //imoprt file hop le
                UploadFile("000. Cong trinh khai thac su dung_Ghep-Gui-23-10 (có mã).xlsx");
                //IWebElement successMsg = wait.Until(d => d.FindElement(By.ClassName("success-message")));
                //Assert.That(successMsg.Text.Contains("Import thành công"), "Import không thành công!");
            }

            [Test]
            public static void Test_Import_InvalidFormat()
            {
                //import file sai dinh dang
                UploadFile("invalid_format.xlsx");
                //IWebElement errorMsg = wait.Until(d => d.FindElement(By.ClassName("error-message")));
                //Assert.That(errorMsg.Text.Contains("Định dạng không hợp lệ"), "Không hiển thị lỗi khi import file sai định dạng!");
            }

            [Test]
            public static void Test_Import_LargeFile()
            {
                //import file dung luong lon
                UploadFile("large_data.xlsx");
                //IWebElement successMsg = wait.Until(d => d.FindElement(By.ClassName("success-message")));
                //Assert.That(successMsg.Text.Contains("Import thành công"), "Import file lớn thất bại!");
            }

            [Test]
            public static void Test_Import_MissingFields()
            {
                //import file thieu du lieu
                UploadFile("missing_fields.xlsx");
                //IWebElement errorMsg = wait.Until(d => d.FindElement(By.ClassName("error-message")));
                //Assert.That(errorMsg.Text.Contains("Thiếu dữ liệu"), "Không hiển thị lỗi khi file thiếu dữ liệu!");
            }

            [TearDown]
            public static void TearDown(IWebDriver chromeDriver)
            {
                chromeDriver.Quit();
            }
        }
    }
}
