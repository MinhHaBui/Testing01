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
using OfficeOpenXml;

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


                // Đọc dữ liệu từ file Excel
                List<DataHauKiem> haukiemlist = ReadDataFromExcel("DataTest.xlsx");

                foreach (var HK in haukiemlist)
                {
                    // Tìm và click vào nút "Thêm mới"
                    IWebElement addNewButton = chromeDriver.FindElement(By.XPath("//*[@id=\"horizontal-slider\"]/div/div[2]/div/div[1]/button[2]"));
                    addNewButton.Click();

                    // Chờ để trang thêm công trình tải
                    // wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("construction-name-input"))); // Chờ ô nhập tên công trình

                    // Điền dữ liệu vào các trường
                    chromeDriver.FindElement(By.Id("")).SendKeys(HK.NgayThang);
                    chromeDriver.FindElement(By.Id("//div[contains(@class:'MuiInputBase-colorPrimary')]//input")).SendKeys(HK.SoVanBan);
                    chromeDriver.FindElement(By.Id("name")).SendKeys(HK.NoiDungVanBan);
                    chromeDriver.FindElement(By.Id("")).SendKeys(HK.Loai);
                    chromeDriver.FindElement(By.Id("")).SendKeys(HK.Nam);
                    chromeDriver.FindElement(By.Id("")).SendKeys(HK.SoQuyetDinh);
                    chromeDriver.FindElement(By.XPath("//div[contains(@class: 'MuiInputBase-colorPrimary')]")).SendKeys(HK.GhiChu);

                    // Click nút "Lưu"
                    chromeDriver.FindElement(By.XPath("/html/body/div[2]/div[3]/form/div/div[3]/button[2]")).Click();

                    // Chờ một chút cho công trình được thêm vào (có thể thêm WebDriverWait để kiểm tra công trình xuất hiện trong danh sách)
                    //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//tr[td[contains(text(), '" + construction.Name + "')]]")));

                    // Kiểm tra công trình vừa thêm có tồn tại trong danh sách hay không
                    bool isAdded = CheckIfConstructionExists(chromeDriver, HK.NoiDungVanBan);
                    if (isAdded)
                    {
                        Console.WriteLine($"Văn bản {HK.NoiDungVanBan} đã được thêm thành công.");
                    }
                    else
                    {
                        Console.WriteLine($"Văn bản {HK.NoiDungVanBan} chưa được thêm.");
                    }
                }




            }

            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi: " + ex.Message);
            }


        }


        // Đọc dữ liệu từ file Excel
        static List<DataHauKiem> ReadDataFromExcel(string filePath)
        {
            List<DataHauKiem> dataList = new List<DataHauKiem>();

            // Đảm bảo rằng file Excel tồn tại
            if (File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[1]; // Lấy worksheet đầu tiên

                    int rowCount = worksheet.Dimension.Rows; // Số lượng hàng trong worksheet

                    // Bắt đầu từ dòng 2 (vì dòng 1 là header)
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataHauKiem data = new DataHauKiem
                        {
                            NgayThang = worksheet.Cells[row, 1].Text,
                            SoVanBan = worksheet.Cells[row, 2].Text,
                            NoiDungVanBan = worksheet.Cells[row, 3].Text,
                            Loai = worksheet.Cells[row, 4].Text,
                            Nam = worksheet.Cells[row, 5].Text,
                            SoQuyetDinh = worksheet.Cells[row, 6].Text,
                            GhiChu = worksheet.Cells[row, 7].Text,

                        };
                        dataList.Add(data);


                    }
                }
            }

            else
            {
                Console.WriteLine("File Excel không tồn tại.");
            }

            return dataList;
        }

        // Kiểm tra công trình có tồn tại trong danh sách hay không
        static bool CheckIfConstructionExists(IWebDriver driver, string constructionName)
        {
            try
            {
                // Kiểm tra xem văn bản đã được thêm vào danh sách (giả sử bảng văn bản có <tr> chứa tên văn bản)
                IList<IWebElement> constructionRows = driver.FindElements(By.XPath("//table//tr/td[2]")); // Giả sử tên văn bản nằm ở cột thứ 2
                foreach (var row in constructionRows)
                {
                    if (row.Text.Contains(constructionName))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi kiểm tra van ban: " + ex.Message);
            }
            return false;
        }

        // Lớp để lưu trữ dữ liệu công trình
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
}
