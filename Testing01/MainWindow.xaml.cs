using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Testing01
{
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenThemMoi_Click(object sender, RoutedEventArgs e)
        {
            ThemMoi themMoiWindow = new ThemMoi();
            themMoiWindow.Show();
        }
        private void OpenUpdate_Click(object sender, RoutedEventArgs e)
        {
            Update UpdateWindow = new Update();
            UpdateWindow.Show();
        }

        private void OpenDelete_Click(object sender, RoutedEventArgs e)
        {
            Delete DeleteWindow = new Delete();
            DeleteWindow.Show();
        }

        private void OpenImport_Click(object sender, RoutedEventArgs e)
        {
            Import ImportWindow = new Import();
            ImportWindow.Show();
        }

        private void OpenExport_Click(object sender, RoutedEventArgs e)
        {
            Export ExportWindow = new Export();
            ExportWindow.Show();
        }


        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
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
                Thread.Sleep(5000);
                Debug.WriteLine("Popup đã mở thành công!");

                // Tìm phần tử khác để di chuột đến (hoặc dùng tọa độ 0,0)
                IWebElement body = chromeDriver.FindElement(By.TagName("body"));
                actions.MoveToElement(body, 0, 0).Perform();  // Di chuột ra ngoài

                Debug.WriteLine("Di chuột ra khỏi menu thành công!");

                // Tìm và click vào nút "Thêm mới công trình"
                
                IWebElement addNewButton = chromeDriver.FindElement(By.XPath("//button//span[contains(text(), 'Thêm mới')]"));
                addNewButton.Click();
                Debug.WriteLine("Mo popup them moi thanh cong");

                //LỌC CÔNG TRÌNH_LOẠI CÔNG TRÌNH 

                // Tìm ô lọc Loại công trình
                IWebElement filterTypeText = chromeDriver.FindElement(By.XPath("//div[contains(@class, 'MuiAutocomplete-root')]//p[contains(text(), 'Tất cả')]"));
                filterTypeText.Click();
                Debug.WriteLine("Click loại công trình thành công!");
                IWebElement inputField = chromeDriver.FindElement(By.XPath("//div[contains(@class, 'MuiAutocomplete-root')]//input"));
                inputField.SendKeys("Thủy điện");
                Debug.WriteLine("Nhập loại công trình xong.");

                // Chờ danh sách gợi ý xuất hiện
                //WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(5));
                IWebElement firstOption = wait.Until(drv => drv.FindElement(By.XPath("//li[contains(@class, 'MuiAutocomplete-option')]")));

                firstOption.Click();
                Debug.WriteLine("Đã chọn 'Thủy điện' từ dropdown.");





                // Tìm ô lọc Loai cong trinh
                IWebElement filterProvinceDropdown = chromeDriver.FindElement(By.XPath("//div[contains(@class, 'MuiAutocomplete-endAdornment')]//button"));

                // Click vào ô lọc Loại công trình
                IWebElement filterTypeDropdown = chromeDriver.FindElement(By.XPath("//div[contains(@class, 'MuiAutocomplete-endAdornment')]//button"));
                filterTypeDropdown.Click();
                Thread.Sleep(1000); // Chờ một chút để dropdown mở ra

                // danh sách các lựa chọn trong dropdown, chọn loại công trình bằng cách tìm theo nội dung
                IWebElement option = chromeDriver.FindElement(By.XPath("//option[contains(text(), 'Thủy điện')]")); 
                option.Click();
                Thread.Sleep(2000);

                // Tìm tất cả các công trình hiển thị (Giả sử các công trình có thẻ <tr> và cột "Loại hình" có mã là "L1")
                IList<IWebElement> constructionRows = chromeDriver.FindElements(By.XPath("//table//tr")); // Tìm tất cả các dòng trong bảng công trình

                Console.WriteLine("Các công trình hiển thị sau khi lọc:");

                bool isTestPassed = true;
                foreach (var row in constructionRows)
                {
                    // Kiểm tra mỗi dòng để xem có chứa mã Loại hình là 'TĐ'
                    try
                    {
                        // Tìm cột "Loại hình" trong mỗi dòng (Giả sử cột "Loại hình" là cột thứ 3)
                        IWebElement typeColumn = row.FindElements(By.TagName("td"))[2]; // Lấy cột thứ 3 (tính từ 0)

                        // Lấy giá trị trong cột "Loại hình"
                        string typeCode = typeColumn.Text.Trim();

                        // Kiểm tra mã loại hình có phải là "TĐ" không
                        if (typeCode == "TĐ")
                        {
                            Console.WriteLine("Công trình có mã loại hình TĐ: " + row.Text);
                        }
                        else
                        {
                            Console.WriteLine("Công trình không có mã loại hình TĐ: " + row.Text);
                            isTestPassed = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi kiểm tra dòng công trình: " + ex.Message);
                    }
                }

                // Kiểm tra xem kết quả có đúng không
                if (isTestPassed)
                {
                    Console.WriteLine("Test passed: Tất cả các công trình có mã loại hình là TĐ.");
                }
                else
                {
                    Console.WriteLine("Test failed: Có công trình không có mã loại hình là TĐ.");
                }

        //LỌC CÔNG TRÌNH_TỈNH 
                // Tìm ô lọc tỉnh 
                //IWebElement filterProvinceDropdown = chromeDriver.FindElement(By.ClassName("filter-province"));

                // Click vào ô lọc tỉnh
                filterProvinceDropdown.Click();
                Thread.Sleep(1000); // Chờ một chút để dropdown hiển thị các lựa chọn

                // Tìm và chọn một tỉnh bất kỳ trong danh sách (Giả sử chọn "Hà Nội")
                IWebElement provinceOption = chromeDriver.FindElement(By.XPath("//li[contains(text(), 'Hà Nội')]")); // Thay 'Hà Nội' bằng tỉnh bạn muốn chọn
                provinceOption.Click();

                // Chờ một chút để lọc được áp dụng
                Thread.Sleep(2000);
                //tìm tất cả các công trình hiển thị(Giả sử các công trình có thẻ < tr > và tỉnh được hiển thị trong một cột cụ thể)
                IList<IWebElement> constructionRows1 = chromeDriver.FindElements(By.XPath("//table//tr")); // Tìm tất cả các dòng trong bảng công trình

                Console.WriteLine("Các công trình hiển thị sau khi lọc theo tỉnh:");

                bool isTestPassed1 = true;

                foreach (var row in constructionRows)
                {
                    try
                    {
                        // Tìm cột tỉnh trong mỗi dòng (Giả sử tỉnh nằm ở cột thứ 4)
                        IWebElement provinceColumn = row.FindElements(By.TagName("td"))[3]; // Lấy cột thứ 4 (tính từ 0)

                        // Lấy giá trị trong cột tỉnh
                        string provinceName = provinceColumn.Text.Trim();

                        // Kiểm tra tỉnh có phải là "Hà Nội" không
                        if (provinceName == "Hà Nội")
                        {
                            Console.WriteLine("Công trình thuộc tỉnh Hà Nội: " + row.Text);
                        }
                        else
                        {
                            Console.WriteLine("Công trình không thuộc tỉnh Hà Nội: " + row.Text);
                            isTestPassed1 = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi kiểm tra dòng công trình: " + ex.Message);
                    }
                }
                // Kiểm tra xem kết quả có đúng không
                if (isTestPassed1)
                {
                    Console.WriteLine("Test passed: Tất cả các công trình có tỉnh Hà Nội.");
                }
                else
                {
                    Console.WriteLine("Test failed: Có công trình không thuộc tỉnh Hà Nội.");
                }

                //LỌC CÔNG TRÌNH_TÌM KIẾM 
                // Tìm ô tìm kiếm 
                IWebElement searchInput = chromeDriver.FindElement(By.XPath("//div[contains(@class, 'MuiOutlinedInput-root')]//input"));

                // Nhập từ khóa "Th" vào ô tìm kiếm
                searchInput.SendKeys("Th");

                // Chờ một chút cho kết quả lọc được áp dụng
                Thread.Sleep(2000); 

                // Tìm tất cả các công trình hiển thị 
                IList<IWebElement> constructionItems = chromeDriver.FindElements(By.XPath("//li[@title]"));

                Console.WriteLine("Các công trình hiển thị sau khi lọc:");

                bool isTestPassed2 = true;

                // Kiểm tra từng công trình
                foreach (var item in constructionItems)
                {
                    // Lấy tiêu đề của công trình
                    string title = item.GetAttribute("title");
                    // Kiểm tra xem tiêu đề công trình có chứa từ khóa "Th" không
                    if (title.Contains("Th"))
                    {
                        Console.WriteLine("Công trình hợp lệ: " + title);
                    }
                    else
                    {
                        Console.WriteLine("Công trình không hợp lệ: " + title);
                        isTestPassed2 = false;
                    }
                }

                // Kiểm tra xem tất cả các công trình có chứa từ khóa "Th"
                if (isTestPassed2)
                {
                    Console.WriteLine("Test passed: Tất cả các công trình chứa từ khóa 'Th'.");
                }
                else
                {
                    Console.WriteLine("Test failed: Có công trình không chứa từ khóa 'Th'.");
                }
            

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi: " + ex.Message);
            }

                /*finally
                {
                    chromeDriver.Quit();
                    Debug.WriteLine("Đóng ChromeDriver.");
                }*/
            



        }    
    }
}
