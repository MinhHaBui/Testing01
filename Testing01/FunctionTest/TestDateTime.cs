using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chromium;

namespace Testing01.FunctionTest
{
    internal class TestDateTime
    {


        public static void DateTime()
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

                // Tìm và click vào nút "Thêm mới công trình"

                IWebElement addNewButton = chromeDriver.FindElement(By.CssSelector(".MuiButton-colorPrimary.css-boomvj"));
                addNewButton.Click();
                Debug.WriteLine("Mo popup them moi thanh cong");

                //Test nhap ngay thang vao o
                //Click vao o Ngay cap phep
                IWebElement DateButton = chromeDriver.FindElement(By.CssSelector(".css-1pfnmo6"));
                DateButton.Click();
                DateButton.SendKeys("18/07/2024");
                Debug.WriteLine("Nhap ngay thang vao o thanh cong");

                //Click vao o Ngay hieu luc
                IWebElement NHLBut = chromeDriver.FindElement(By.CssSelector("css - 1pfnmo6")); 
                NHLBut.Click();
                IWebElement NHBut = chromeDriver.FindElement(By.CssSelector(".css-ihdtdm"));
                NHBut.Click();
                Debug.WriteLine("Mo chon lich");


            }
            catch (Exception ex)
            {
                Console.WriteLine("Có lỗi xảy ra: " + ex.Message);
            }
        }
    }
}
