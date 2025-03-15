using OfficeOpenXml;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Media3D;



namespace Testing01

{
    /// <summary>
    /// Interaction logic for HauKiem.xaml
    /// </summary>
    

    public partial class HauKiem : Window
    {
        public HauKiem()
        {
            InitializeComponent();
        }
        public static void Main()
        {
            string excelFile = @"C:\\Users\\minhh\\OneDrive\\Tài liệu\\DoAnTotNghiep\\DataTest.xlsx\";
            List<DataHauKiem> dataList = ReadDataFromExcel(excelFile);

            using (IWebDriver driver = InitWebDriver())
            {
                Login(driver);
                NavigateToKhaiThac(driver);
                ProcessData(driver, dataList, excelFile);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
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

            private static void ProcessData(IWebDriver driver, List<DataHauKiem> dataList, string excelFile)
                {
                    foreach (var data in dataList)
                    {
                        AddNewRecord(driver, data);
                        bool isAdded = CheckIfConstructionExists(driver, data.NoiDungVanBan);
                        Debug.WriteLine(isAdded ? $"{data.NoiDungVanBan} thêm thành công!" : $"{data.NoiDungVanBan} thêm thất bại!");
                        ExportResultToExcel(excelFile, data, isAdded ? "Pass" : "Fail");
                    }
                }

                private static void AddNewRecord(IWebDriver chromeDriver, DataHauKiem data)
                {
                    chromeDriver.FindElement(By.XPath("//button[contains(text(),'Thêm mới')]")).Click();
                    WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(5));
                    wait.Until(d => d.FindElement(By.Id("name"))).SendKeys(data.NoiDungVanBan);
                    chromeDriver.FindElement(By.Id("soVanBan"))?.SendKeys(data.SoVanBan);
                    chromeDriver.FindElement(By.Id("loai"))?.SendKeys(data.Loai);
                    chromeDriver.FindElement(By.Id("nam"))?.SendKeys(data.Nam);
                    chromeDriver.FindElement(By.Id("soQuyetDinh"))?.SendKeys(data.SoQuyetDinh);
                    chromeDriver.FindElement(By.Id("ghiChu"))?.SendKeys(data.GhiChu);
                    chromeDriver.FindElement(By.XPath("//button[contains(text(),'Lưu')]")).Click();
                }

                private static bool CheckIfConstructionExists(IWebDriver driver, string noiDungVanBan)
                {
                    try
                    {
                        return driver.FindElements(By.XPath("//table//tr/td[2]")).Any(el => el.Text.Contains(noiDungVanBan));
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static List<DataHauKiem> ReadDataFromExcel(string filePath)
                {
                    List<DataHauKiem> dataList = new List<DataHauKiem>();
                    if (!File.Exists(filePath)) return dataList;

                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[1];
                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            dataList.Add(new DataHauKiem
                            {
                                NgayThang = worksheet.Cells[row, 1].Text,
                                SoVanBan = worksheet.Cells[row, 2].Text,
                                NoiDungVanBan = worksheet.Cells[row, 3].Text,
                                Loai = worksheet.Cells[row, 4].Text,
                                Nam = worksheet.Cells[row, 5].Text,
                                SoQuyetDinh = worksheet.Cells[row, 6].Text,
                                GhiChu = worksheet.Cells[row, 7].Text,
                            });
                        }
                    }
                    return dataList;
                }

                private static void ExportResultToExcel(string filePath, DataHauKiem data, string result)
                {
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[2];
                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            if (worksheet.Cells[row, 3].Text == data.NoiDungVanBan)
                            {
                                int lastColumn = worksheet.Dimension.Columns + 1;
                                worksheet.Cells[row, lastColumn].Value = result;
                                break;
                            }
                        }
                        package.Save();
                    }
                }
            }

    public class DataHauKiem
        {
            public string NgayThang { get; set; }
            public string SoVanBan { get; set; }
            public string NoiDungVanBan { get; set; }
            public string Loai { get; set; }
            public string Nam { get; set; }
            public string SoQuyetDinh { get; set; }
            public string GhiChu { get; set; }
        }
    }
