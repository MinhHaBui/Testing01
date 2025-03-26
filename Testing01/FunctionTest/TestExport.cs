﻿using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;

namespace Testing01.FunctionTest
{
    internal class TestExport
    {
        public TestExport() { }

        internal static void TestExportFile()
        {
            throw new NotImplementedException();
        }

        internal static void Test_ExportFile()
        {
            throw new NotImplementedException();
        }

        [TestFixture]
        public class ExportFileTests
        {
            private IWebDriver driver;
            private WebDriverWait wait;
            
            private string downloadPath = @"C:\Users\Public\Downloads\"; // Thư mục tải file

            public void TestExportFile() 
            {
                Setup();
                Test_ExportFile();
                TearDown();
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
            public void Test_ExportFile()
            {
                GoToExportPage();
                ClickExportButton();

                // Chờ 5 giây để file tải xuống
                Thread.Sleep(5000);

                // Kiểm tra file có tồn tại trong thư mục download không
                string latestFile = GetLatestDownloadedFile();
                Assert.That(!string.IsNullOrEmpty(latestFile), "Không tìm thấy file đã tải xuống!");


                // Kiểm tra định dạng file (giả sử file xuất ra là .xlsx)
                Assert.That(latestFile.EndsWith(".xlsx"), "File export không đúng định dạng!");
            }

            public void GoToExportPage()
            {
                

                // Tìm và click vào nút Export
                IWebElement exportBtn = wait.Until(d => d.FindElement(By.CssSelector(".css-14ayiqf")));
                exportBtn.Click();
            }

            public void ClickExportButton()
            {
                // Click vào nút xác nhận Export
                IWebElement confirmExportBtn = wait.Until(d => d.FindElement(By.XPath("//button[contains(text(), 'Xác nhận')]")));
                confirmExportBtn.Click();
            }

            private string GetLatestDownloadedFile()
            {
                var directory = new DirectoryInfo(downloadPath);
                var latestFile = directory.GetFiles()
                                          .OrderByDescending(f => f.LastWriteTime)
                                          .FirstOrDefault();

                return latestFile?.FullName;
            }

            [TearDown]
            public void TearDown()
            {
                driver.Quit();
            }
        }
    }
}
