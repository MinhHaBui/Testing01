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
                [SetUp]
                public static void Update()
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

                        OpenEditConstructionForm(chromeDriver);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Có lỗi xảy ra: " + ex.Message);
                    }
                }



                [Test]
                public static void OpenEditConstructionForm(IWebDriver chromeDriver)
                {
                    string constructionName = "Sê San 4A";
                    // Tìm công trình cần chỉnh sửa
                    var rows = chromeDriver.FindElements(By.ClassName("fixedDataTableCellLayout_wrap"));
                    // Duyệt qua từng hàng để tìm công trình theo tên
                    IWebElement rowToUpdate = null;

                    foreach (var row in rows)
                    {
                        if (row.Text.Trim().Equals(constructionName, StringComparison.OrdinalIgnoreCase))
                        {
                            rowToUpdate = row;
                            break;
                        }
                    }

                    Assert.That(rowToUpdate, Is.Not.Null, "Không tìm thấy công trình cần chỉnh sửa!");
                    Debug.WriteLine("Tim thay cong trinh can sua.");

                // Click vào nút "Sửa"
                IWebElement editButton = rowToUpdate.FindElement(By.CssSelector("[data-testid='ModeEditOutlinedIcon']"));
                    editButton.Click();
                EditConstructionDetails(chromeDriver);

                }

                public static void EditConstructionDetails(IWebDriver chromeDriver)
                {
                    string newName = "Kiểm tra";
                    // Chờ trang chỉnh sửa mở
                    IWebElement nameInput = chromeDriver.FindElement(By.Id("name"));
                    nameInput.Clear();
                    nameInput.SendKeys(newName);

                    // Click nút "Lưu"
                    IWebElement saveButton = chromeDriver.FindElement(By.CssSelector(".css-m8gy81"));
                    saveButton.Click();

                    // Chờ thay đổi được lưu
                    Thread.Sleep(2000);
                VerifyEditedConstruction(chromeDriver);
                

                }

            public static void VerifyEditedConstruction(IWebDriver chromeDriver)
            {
                string expectedName = "Kiểm tra";
                var rows = chromeDriver.FindElements(By.ClassName("fixedDataTableCellLayout_wrap"));
                // Duyệt qua từng hàng để tìm công trình theo tên
                IWebElement updatedRow = null;

                foreach (var row in rows)
                {
                    if (row.Text.Trim().Equals(expectedName, StringComparison.OrdinalIgnoreCase))
                    {
                        updatedRow = row;
                        break;
                    }
                }

                Assert.That(updatedRow, Is.Not.Null, "Khong tim thay cong trinh vua chinh sua");
                Debug.WriteLine("Tim thay cong trinh vua chinh sua.");
                Assert.That(updatedRow, Is.Not.Null, "Công trình chưa được cập nhật!");
                TearDown(chromeDriver);
            }

            [TearDown]
            public static void TearDown(IWebDriver chromeDriver)
            {
                chromeDriver.Quit();
            }

        }
        

    }
}
    

