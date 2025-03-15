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
    /// Interaction logic for ThemMoi.xaml
    /// </summary>
    public partial class ThemMoi : Window
    {

        public ThemMoi()
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
                List<ConstructionData> constructionList = ReadDataFromExcel(@"C:\Users\minhh\OneDrive\Tài liệu\DoAnTotNghiep\DataTest.xlsx");

                foreach (var construction in constructionList)
                {
                    // Tìm và click vào nút "Thêm mới công trình"
                    IWebElement addNewButton = chromeDriver.FindElement(By.XPath("//*[@id=\"horizontal-slider\"]/div/div[2]/div/div[1]/button[2]"));
                    addNewButton.Click();

                    // Chờ để trang thêm công trình tải
                    // wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("construction-name-input"))); // Chờ ô nhập tên công trình

                    // Điền dữ liệu vào các trường
                    chromeDriver.FindElement(By.Id("CONG_TRINH")).SendKeys(construction.CongTrinh);
                    chromeDriver.FindElement(By.Id("X_UTM")).SendKeys(construction.X_UTM);
                    chromeDriver.FindElement(By.Id("Y_UTM")).SendKeys(construction.Y_UTM);
                    chromeDriver.FindElement(By.Id("outlined-select-currency")).SendKeys(construction.Ma);
                    chromeDriver.FindElement(By.Id("outlined-select-currency")).SendKeys(construction.QTVH);
                    chromeDriver.FindElement(By.Id("So_Hieu_Gieng_Dap_chinh__Dap_Phu_Hang_muc_lay_nuoc")).SendKeys(construction.SoHieuGieng);
                    chromeDriver.FindElement(By.Id("Ma_dap_gan_theo_ma_song")).SendKeys(construction.MaDap);
                    chromeDriver.FindElement(By.Id("Muc_dich_KTSD")).SendKeys(construction.MucDichKTSD);
                    chromeDriver.FindElement(By.Id("Toa_do_X")).SendKeys(construction.ToaDoX);
                    chromeDriver.FindElement(By.Id("Toa_do_Y")).SendKeys(construction.ToaDoY);
                    chromeDriver.FindElement(By.Id("Xa")).SendKeys(construction.Xa);
                    chromeDriver.FindElement(By.Id("Huyen")).SendKeys(construction.Huyen);
                    chromeDriver.FindElement(By.Id("Tinh")).SendKeys(construction.Tinh);
                    chromeDriver.FindElement(By.Id("Ma_song")).SendKeys(construction.MaSong);
                    chromeDriver.FindElement(By.Id("Nguon_nuoc_khai_thac")).SendKeys(construction.NguonNuoc);
                    chromeDriver.FindElement(By.Id("Chay_ra")).SendKeys(construction.ChayRa);
                    chromeDriver.FindElement(By.Id("Luu_vuc_song")).SendKeys(construction.LVS);
                    chromeDriver.FindElement(By.Id("Phuong_thuc_Duong_dan_Sau_dap_TL")).SendKeys(construction.PhuongThuc);
                    chromeDriver.FindElement(By.Id("Q_tt_m3_s_Sau_dap")).SendKeys(construction.Qtt_SauDap);
                    chromeDriver.FindElement(By.Id("Q_tt_m3_s_Sau_Cong_trinh")).SendKeys(construction.Qtt_SauCongTrinh);
                    chromeDriver.FindElement(By.Id("Q_tt_m3_s_Quy_dinh_khac")).SendKeys(construction.Qtt_QuyDinhKhac);
                    chromeDriver.FindElement(By.Id("Cong_bo_DCTT")).SendKeys(construction.CongBo);
                    chromeDriver.FindElement(By.Id("Whitrieu_m3")).SendKeys(construction.Whi);
                    chromeDriver.FindElement(By.Id("Wtbtrieu_m3")).SendKeys(construction.Wtb);
                    chromeDriver.FindElement(By.Id("Wpltrieu_m3")).SendKeys(construction.Wpl);
                    chromeDriver.FindElement(By.Id("MNDBTm")).SendKeys(construction.MNDTB);
                    chromeDriver.FindElement(By.Id("MNCm")).SendKeys(construction.MNC);
                    chromeDriver.FindElement(By.Id("Cong_suatMW")).SendKeys(construction.CongSuat);
                    chromeDriver.FindElement(By.Id("Eotrieu_kWh")).SendKeys(construction.Eo);
                    chromeDriver.FindElement(By.Id("Q_f_dien_maxm3_s")).SendKeys(construction.Qf_dien_max);
                    chromeDriver.FindElement(By.Id("Q_kthac_maxm3_s")).SendKeys(construction.Q_kthac_max_s);
                    chromeDriver.FindElement(By.Id("Q_kthac_maxm3_ngay_dem")).SendKeys(construction.Q_kthac_max_ngaydem);
                    chromeDriver.FindElement(By.Id("Dien_tich_tuoi_thiet_ke_ha")).SendKeys(construction.S_tuoiThietke);
                    chromeDriver.FindElement(By.Id("Dien_tich_tuoi_thuc_te_ha")).SendKeys(construction.S_tuoiThucTe);
                    chromeDriver.FindElement(By.Id("Do_thi_duoc_cap")).SendKeys(construction.DoThiDuocCap);
                    chromeDriver.FindElement(By.Id("Tang_chua_nuoc_khai_thac")).SendKeys(construction.TangChuaNuoc);
                    chromeDriver.FindElement(By.Id("So_luong_cong_trinh")).SendKeys(construction.SoLuongCongTrinh);
                    chromeDriver.FindElement(By.Id("So_luong_gieng_khai_thac")).SendKeys(construction.SoLuongGieng);
                    chromeDriver.FindElement(By.Id("So_hieu_gieng_mach_lo")).SendKeys(construction.SoHieuGieng_MachLo);
                    chromeDriver.FindElement(By.Id("Chieu_sau_dat_ong_loc_tu___den_m")).SendKeys(construction.ChieuSauDatOngLocTu);
                    chromeDriver.FindElement(By.Id("Chieu_sau_dat_ong_loc_tu___den_m")).SendKeys(construction.ChieuSauDatOngLocDen);
                    chromeDriver.FindElement(By.Id("Loai_gieng_KT_QT")).SendKeys(construction.LoaiGieng);
                    chromeDriver.FindElement(By.Id("So_GP")).SendKeys(construction.SoGP);
                    chromeDriver.FindElement(By.Id("Don_vi_cap")).SendKeys(construction.DonViCap);
                    chromeDriver.FindElement(By.Id("")).SendKeys(construction.NgayCapPhep);
                    chromeDriver.FindElement(By.Id("")).SendKeys(construction.NgayHieuLuc);
                    chromeDriver.FindElement(By.Id("Thoi_hannam_")).SendKeys(construction.ThoiHan);
                    chromeDriver.FindElement(By.Id("")).SendKeys(construction.NgayHetHan);
                    chromeDriver.FindElement(By.Id("Thoi_gian_con_lai_ngay")).SendKeys(construction.TimeConLai);
                    chromeDriver.FindElement(By.Id("So_QD_TCQ")).SendKeys(construction.SoQDTCQ);
                    chromeDriver.FindElement(By.Id("")).SendKeys(construction.NgayTCQ);
                    chromeDriver.FindElement(By.Id("Tong_so_tien_theo_QD_TCQ")).SendKeys(construction.TongSoTienQDTCQ);
                    chromeDriver.FindElement(By.Id("Ngay_van_hanh_du_kien_van_hanh")).SendKeys(construction.NgayVanHanh);
                    chromeDriver.FindElement(By.Id("Tinh_trang_van_hanh")).SendKeys(construction.TinhTrangVanHanh);
                    chromeDriver.FindElement(By.Id("")).SendKeys(construction.HTGS);
                    chromeDriver.FindElement(By.Id("Tinh_trang_truyen_du_lieu")).SendKeys(construction.TinhTrangTruyenDuLieu);
                    chromeDriver.FindElement(By.Id("Ket_noi_trang_van_hanh_ho")).SendKeys(construction.KetNoiTrangVanHanh);
                    chromeDriver.FindElement(By.Id("Nguon_du_lieu")).SendKeys(construction.NguonDuLieu);
                    chromeDriver.FindElement(By.Id("Chu_dau_tu")).SendKeys(construction.ChuDauTu);
                    chromeDriver.FindElement(By.Id("SDT")).SendKeys(construction.SDT);
                    chromeDriver.FindElement(By.Id("Dia_chi")).SendKeys(construction.DiaChi);
                    chromeDriver.FindElement(By.Id("Thanh_tra_Kiem_tra")).SendKeys(construction.ThanhTra_KiemTra);
                    chromeDriver.FindElement(By.Id("Bao_cao_dinh_ky")).SendKeys(construction.BaoCao);
                    chromeDriver.FindElement(By.Id("Other")).SendKeys(construction.Khac);


                    // Click nút "Lưu"
                    chromeDriver.FindElement(By.XPath("/html/body/div[2]/div[3]/form/div/div[3]/button[2]")).Click();

                    // Chờ một chút cho công trình được thêm vào (có thể thêm WebDriverWait để kiểm tra công trình xuất hiện trong danh sách)
                    //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//tr[td[contains(text(), '" + construction.Name + "')]]")));

                    // Kiểm tra công trình vừa thêm có tồn tại trong danh sách hay không
                    bool isAdded = CheckIfConstructionExists(chromeDriver, construction.CongTrinh);
                    if (isAdded)
                    {
                        Console.WriteLine($"Công trình {construction.CongTrinh} đã được thêm thành công.");
                    }
                    else
                    {
                        Console.WriteLine($"Công trình {construction.CongTrinh} chưa được thêm.");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Có lỗi xảy ra: " + ex.Message);
            }
            finally
            {
                // Đóng trình duyệt sau khi kiểm thử xong
                chromeDriver.Quit();
            }
        }

        // Đọc dữ liệu từ file Excel
        static List<ConstructionData> ReadDataFromExcel(string filePath)
        {
            List<ConstructionData> dataList = new List<ConstructionData>();

            // Đảm bảo rằng file Excel tồn tại
            if (File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Lấy worksheet đầu tiên

                    int rowCount = worksheet.Dimension.Rows; // Số lượng hàng trong worksheet

                    // Bắt đầu từ dòng 2 (vì dòng 1 là header)
                    for (int row = 3; row <= rowCount; row++)
                    {
                        ConstructionData data = new ConstructionData
                        {
                            CongTrinh = worksheet.Cells[row, 2].Text,
                            X_UTM = worksheet.Cells[row, 3].Text,
                            Y_UTM = worksheet.Cells[row, 4].Text,
                            Ma = worksheet.Cells[row, 5].Text,
                            QTVH = worksheet.Cells[row, 6].Text,
                            SoHieuGieng = worksheet.Cells[row, 7].Text,
                            MaDap = worksheet.Cells[row, 8].Text,
                            MucDichKTSD = worksheet.Cells[row, 9].Text,
                            ToaDoX = worksheet.Cells[row, 10].Text,
                            ToaDoY = worksheet.Cells[row, 11].Text,
                            Xa = worksheet.Cells[row, 12].Text,
                            Huyen = worksheet.Cells[row, 13].Text,
                            Tinh = worksheet.Cells[row, 14].Text,
                            MaSong = worksheet.Cells[row, 15].Text,
                            NguonNuoc = worksheet.Cells[row, 16].Text,
                            ChayRa = worksheet.Cells[row, 17].Text,
                            LVS = worksheet.Cells[row, 18].Text,
                            PhuongThuc = worksheet.Cells[row, 19].Text,
                            Qtt_SauDap = worksheet.Cells[row, 20].Text,
                            Qtt_SauCongTrinh = worksheet.Cells[row, 21].Text,
                            Qtt_QuyDinhKhac = worksheet.Cells[row, 22].Text,
                            CongBo = worksheet.Cells[row, 23].Text,
                            Whi = worksheet.Cells[row, 24].Text,
                            Wtb = worksheet.Cells[row, 25].Text,
                            Wpl = worksheet.Cells[row, 26].Text,
                            MNDTB = worksheet.Cells[row, 27].Text,
                            MNC = worksheet.Cells[row, 28].Text,
                            CongSuat = worksheet.Cells[row, 29].Text,
                            Eo = worksheet.Cells[row, 30].Text,
                            Qf_dien_max = worksheet.Cells[row, 31].Text,
                            Q_kthac_max_s = worksheet.Cells[row, 32].Text,
                            Q_kthac_max_ngaydem = worksheet.Cells[row, 33].Text,
                            S_tuoiThietke = worksheet.Cells[row, 34].Text,
                            S_tuoiThucTe = worksheet.Cells[row, 35].Text,
                            DoThiDuocCap = worksheet.Cells[row, 36].Text,
                            TangChuaNuoc = worksheet.Cells[row, 37].Text,
                            SoLuongCongTrinh = worksheet.Cells[row, 38].Text,
                            SoLuongGieng = worksheet.Cells[row, 39].Text,
                            SoHieuGieng_MachLo = worksheet.Cells[row, 40].Text,
                            ChieuSauDatOngLocTu = worksheet.Cells[row, 41].Text,
                            ChieuSauDatOngLocDen = worksheet.Cells[row, 41].Text,
                            LoaiGieng = worksheet.Cells[row, 42].Text,
                            SoGP = worksheet.Cells[row, 43].Text,
                            DonViCap = worksheet.Cells[row, 44].Text,
                            NgayCapPhep = worksheet.Cells[row, 45].Text,
                            NgayHieuLuc = worksheet.Cells[row, 46].Text,
                            ThoiHan = worksheet.Cells[row, 47].Text,
                            NgayHetHan = worksheet.Cells[row, 48].Text,
                            TimeConLai = worksheet.Cells[row, 49].Text,
                            SoQDTCQ = worksheet.Cells[row, 50].Text,
                            NgayTCQ = worksheet.Cells[row, 51].Text,
                            TongSoTienQDTCQ = worksheet.Cells[row, 52].Text,
                            NgayVanHanh = worksheet.Cells[row, 53].Text,
                            TinhTrangVanHanh = worksheet.Cells[row, 54].Text,
                            HTGS = worksheet.Cells[row, 55].Text,
                            TinhTrangTruyenDuLieu = worksheet.Cells[row, 56].Text,
                            KetNoiTrangVanHanh = worksheet.Cells[row, 57].Text,
                            NguonDuLieu = worksheet.Cells[row, 58].Text,
                            ChuDauTu = worksheet.Cells[row, 59].Text,
                            SDT = worksheet.Cells[row, 60].Text,
                            DiaChi = worksheet.Cells[row, 61].Text,
                            ThanhTra_KiemTra = worksheet.Cells[row, 62].Text,
                            BaoCao = worksheet.Cells[row, 63].Text,
                            Khac = worksheet.Cells[row, 64].Text,

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
                // Kiểm tra xem tên công trình đã được thêm vào danh sách (giả sử bảng công trình có <tr> chứa tên công trình)
                IList<IWebElement> constructionRows = driver.FindElements(By.XPath("//table//tr/td[1]")); // Giả sử tên công trình nằm ở cột thứ 1
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
                Console.WriteLine("Lỗi khi kiểm tra công trình: " + ex.Message);
            }
            return false;
        }


        // Lớp để lưu trữ dữ liệu công trình
        public class ConstructionData
        {
            public string CongTrinh { get; set; }
            public string X_UTM { get; set; }
            public string Y_UTM { get; set; }
            public string Ma { get; set; }
            public string QTVH { get; set; }
            public string SoHieuGieng { get; set; }
            public string MaDap { get; set; }
            public string MucDichKTSD { get; set; }
            public string ToaDoX { get; set; }
            public string ToaDoY { get; set; }
            public string Xa { get; set; }
            public string Huyen { get; set; }
            public string Tinh { get; set; }
            public string MaSong { get; set; }
            public string NguonNuoc { get; set; }
            public string ChayRa { get; set; }
            public string LVS { get; set; }
            public string PhuongThuc { get; set; }
            public string Qtt_SauDap { get; set; }
            public string Qtt_SauCongTrinh { get; set; }
            public string Qtt_QuyDinhKhac { get; set; }
            public string CongBo { get; set; }
            public string Whi { get; set; }
            public string Wtb { get; set; }
            public string Wpl { get; set; }
            public string MNDTB { get; set; }
            public string MNC { get; set; }
            public string CongSuat { get; set; }
            public string Eo { get; set; }
            public string Qf_dien_max { get; set; }
            public string Q_kthac_max_s { get; set; }
            public string Q_kthac_max_ngaydem { get; set; }
            public string S_tuoiThietke { get; set; }
            public string S_tuoiThucTe { get; set; }
            public string DoThiDuocCap { get; set; }
            public string TangChuaNuoc { get; set; }
            public string SoLuongCongTrinh { get; set; }
            public string SoLuongGieng { get; set; }
            public string SoHieuGieng_MachLo { get; set; }
            public string ChieuSauDatOngLocTu { get; set; }
            public string ChieuSauDatOngLocDen { get; set; }
            public string LoaiGieng { get; set; }
            public string SoGP { get; set; }
            public string DonViCap { get; set; }
            public string NgayCapPhep { get; set; }
            public string NgayHieuLuc { get; set; }
            public string ThoiHan { get; set; }
            public string NgayHetHan { get; set; }
            public string TimeConLai { get; set; }
            public string SoQDTCQ { get; set; }
            public string NgayTCQ { get; set; }
            public string TongSoTienQDTCQ { get; set; }
            public string NgayVanHanh { get; set; }
            public string TinhTrangVanHanh { get; set; }
            public string HTGS { get; set; }
            public string TinhTrangTruyenDuLieu { get; set; }
            public string KetNoiTrangVanHanh { get; set; }
            public string NguonDuLieu { get; set; }
            public string ChuDauTu { get; set; }
            public string SDT { get; set; }
            public string DiaChi { get; set; }
            public string ThanhTra_KiemTra { get; set; }
            public string BaoCao { get; set; }
            public string Khac { get; set; }
        }
    }
}

