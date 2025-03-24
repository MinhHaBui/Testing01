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
using NUnit.Framework;
using System.IO;
using System.Diagnostics;
using OpenQA.Selenium.Interactions;

namespace Testing01
{
    /// <summary>
    /// Interaction logic for Import.xaml
    /// </summary>
    public partial class Import : Window
    {
        public Import()
        {
            InitializeComponent();
        }
        [TestFixture]
        public class ImportFileTests
        {
            private IWebDriver driver;
            private WebDriverWait wait;
            private string baseUrl = "https://lake-management.desoft.vn/";

            [SetUp]
            public void Setup()
            {

                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                IWebDriver chromeDriver = new ChromeDriver(options);
                Debug.WriteLine("Khởi tạo ChromeDriver thành công.");

                // Đăng nhập vào hệ thống
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

                // Chờ trang chính load xong
                wait.Until(d => d.Url == baseUrl);

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
                Thread.Sleep(5000);
                Debug.WriteLine("Popup đã mở thành công!");


            }

            [Test]
            public void Test_Import_ValidFile()
            {
                GoToImportPage();
                UploadFile("valid_data.xlsx");

                // Kiểm tra thông báo thành công
                IWebElement successMsg = wait.Until(d => d.FindElement(By.ClassName("success-message")));
                Assert.IsTrue(successMsg.Text.Contains("Import thành công"), "Import không thành công!");
            }



            [Test]
            public void Test_Import_InvalidFormat()
            {
                GoToImportPage();
                UploadFile("invalid_format.xlsx");

                // Kiểm tra thông báo lỗi
                IWebElement errorMsg = wait.Until(d => d.FindElement(By.ClassName("error-message")));
                Assert.IsTrue(errorMsg.Text.Contains("Định dạng không hợp lệ"), "Không hiển thị lỗi khi import file sai định dạng!");
            }

            [Test]
            public void Test_Import_LargeFile()
            {
                GoToImportPage();
                UploadFile("large_data.xlsx");

                // Kiểm tra hệ thống có treo không
                IWebElement successMsg = wait.Until(d => d.FindElement(By.ClassName("success-message")));
                Assert.IsTrue(successMsg.Text.Contains("Import thành công"), "Import file lớn thất bại!");
            }

            [Test]
            public void Test_Import_MissingFields()
            {
                GoToImportPage();
                UploadFile("missing_fields.xlsx");

                // Kiểm tra thông báo lỗi
                IWebElement errorMsg = wait.Until(d => d.FindElement(By.ClassName("error-message")));
                Assert.IsTrue(errorMsg.Text.Contains("Thiếu dữ liệu"), "Không hiển thị lỗi khi file thiếu dữ liệu!");
            }

            private void GoToImportPage()
            {
                // Điều hướng đến menu "Công trình khai thác"
                IWebElement menu = wait.Until(d => d.FindElement(By.XPath("//li[@title='Công trình khai thác']")));
                menu.Click();
                Thread.Sleep(2000); // Đợi menu mở

                // Click vào nút Import
                IWebElement importBtn = wait.Until(d => d.FindElement(By.XPath("//button[contains(text(), 'Nhập dữ liệu')]")));
                importBtn.Click();
            }

            private void UploadFile(string fileName)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", fileName);

                // Kiểm tra file có tồn tại không
                Assert.IsTrue(File.Exists(filePath), $"File không tồn tại: {filePath}");

                // Upload file
                IWebElement fileInput = wait.Until(d => d.FindElement(By.XPath("//input[@type='file']")));
                fileInput.SendKeys(filePath);

                // Click nút "Tải lên"
                IWebElement uploadBtn = driver.FindElement(By.XPath("//button[contains(text(), 'Tải lên')]"));
                uploadBtn.Click();

                Thread.Sleep(3000); // Đợi hệ thống xử lý
            }

            [TearDown]
            public void TearDown()
            {
                driver.Quit();
            }
        }
    }
