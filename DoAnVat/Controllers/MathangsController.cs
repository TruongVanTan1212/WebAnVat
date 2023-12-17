using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnVat.Data;
using DoAnVat.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;


namespace DoAnVat.Controllers
{
    public class MathangsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Khachhang> _pwHear;   

        public MathangsController(ApplicationDbContext context, IPasswordHasher<Khachhang> passwordHasher)
        {
            _context = context;
            _pwHear = passwordHasher;
        }


        void GetInfo()
        {
            // số lượng mặt hàng có trong giỏ   
            ViewData["solg"] = GetCartItems().Count();
            // danh sách danh mục có trong db
            ViewBag.danhmuc = _context.Danhmuc.ToList();
            // lấy thông tin người dùng
            if(HttpContext.Session.GetString("khachhang") != "")
            {
                ViewBag.khachhang = _context.Khachhang.FirstOrDefault(k => k.Email == HttpContext.Session.GetString("khachhang"));
            }
            if (HttpContext.Session.GetString("Nhanvien") != "")
            {
                ViewBag.Nhanvien = _context.Nhanvien.FirstOrDefault(k => k.Email == HttpContext.Session.GetString("Nhanvien"));
            }

        }
        // GET: Mahangs
        public async Task<IActionResult> Index()
        {
            GetInfo();
            var applicationDbContext = _context.Mathang.Include(m => m.MaDmNavigation);
            return View(await applicationDbContext.ToListAsync());
        }
        // lấy sản phẩm theo danh mục
        public async Task<IActionResult> Index1(int id)
        {
            GetInfo();
            var applicationDbContext = _context.Mathang.Where(s => s.MaDm ==id ).Include(m => m.MaDmNavigation);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Mahangs/Details/5
        public async Task<IActionResult> Details(int? id)   
        {
            if (id == null)
            {
                return NotFound();
            }

            var mahang = await _context.Mathang
                .Include(m => m.MaDmNavigation)
                .FirstOrDefaultAsync(m => m.MaMh == id);
            if (mahang == null)
            {
                return NotFound();
            }
            GetInfo();
            return View(mahang);
        }
        //đọc danh sách
        List<CartItem> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString("shopcart");
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Lưu danh sách CartItem trong giỏ hàng vào session
        void SaveCartSession(List<CartItem> list)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(list);
            session.SetString("shopcart", jsoncart);
        }

        // Xóa session giỏ hàng
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove("shopcart");
        }
        // Cho hàng vào giỏ
        public async Task<IActionResult> AddToCart(int id)
        {
            var mathang = await _context.Mathang
                .FirstOrDefaultAsync(m => m.MaMh == id);
            if (mathang == null)
            {
                return NotFound("Sản phẩm không tồn tại");
            }
            var cart = GetCartItems();
            var item = cart.Find(p => p.Mathang.MaMh == id);
            if (item != null)
            {
                item.Soluong++;
            }
            else
            {
                cart.Add(new CartItem() { Mathang = mathang, Soluong = 1 });
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(ViewCart));
        }
        // Chuyển đến view xem giỏ hàng
        public IActionResult ViewCart()
        {
            GetInfo();
            return View(GetCartItems());
        }
        public IActionResult RemoveItem(int id)
        {
            var cart = GetCartItems();
            var item = cart.Find(p => p.Mathang.MaMh == id);
            if (item != null)
            {
                cart.Remove(item);
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(ViewCart));
        }

        // Cập nhật số lượng một mặt hàng trong giỏ
        public IActionResult UpdateItem(int id, int quantity)
        {
            var cart = GetCartItems();
            var item = cart.Find(p => p.Mathang.MaMh == id);
            if (item != null)
            {
                item.Soluong = quantity;
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(ViewCart));
        }

        // Chuyển đến view thanh toán
        public IActionResult CheckOut()
        {
            GetInfo(); 
            return View(GetCartItems());
        }
        [HttpPost, ActionName("CreateBill")]

        // lưu thông tin đơn hàng
        public async Task<IActionResult> CreateBill(int id, string email, string hoten, string dienthoai, string diachi)
        {

            // xử lý thông tin khách hàng trường hợp khách hàng mới
            var kh = new Khachhang();
            if(id != 0) // khách hàng đã đăng nhập
            {
                kh.MaKh = id;
            }
            else
            {
                kh.Ten = hoten;
                kh.Email = email;
                kh.DienThoai = dienthoai;
                _context.Add(kh);
                await _context.SaveChangesAsync();
            }
            
            var hd = new Hoadon();
            hd.Ngay = DateTime.Now;
            hd.MaKh = kh.MaKh;

            _context.Add(hd);
            await _context.SaveChangesAsync();


            //them chi tiethoa don
            var cart = GetCartItems();
            int thanhtien = 0;
            int tongtien = 0;
            foreach (var i in cart)
            {
                var ct = new Cthoadon();
                ct.MaHd = hd.MaHd;
                ct.MaMh = i.Mathang.MaMh;
                thanhtien = i.Mathang.GiaBan * i.Soluong;
                tongtien += thanhtien;
                ct.DonGia = i.Mathang.GiaBan;
                ct.SoLuong = (short)i.Soluong;
                ct.ThanhTien = thanhtien;
                _context.Add(ct);

            }

            await _context.SaveChangesAsync();
            ///cap nhat
            hd.TongTien = tongtien;
            _context.Update(hd);
            await _context.SaveChangesAsync();

            //xoagiohang
            ClearCart();
            GetInfo();
            return View(hd);
        }

        // get 
        public IActionResult Login()
        {
            GetInfo();
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email,string matkhau)
        {
            var nv1 = _context.Nhanvien.FirstOrDefault(k => k.Email == email);
            var nv2 = _context.Nhanvien.FirstOrDefault(k => k.MatKhau == matkhau);
            var kh = _context.Khachhang.FirstOrDefault(k => k.Email == email);
           if(kh != null && _pwHear.VerifyHashedPassword(kh, kh.MatKhau, matkhau) == PasswordVerificationResult.Success)
            {
                HttpContext.Session.SetString("khachhang", kh.Email);
                return RedirectToAction(nameof(Customer));
            }
            else
            {
                if (nv1 != null && nv2 != null)
                {
                    HttpContext.Session.SetString("Nhanvien", nv1.Email);
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Login));

        }

        public IActionResult Customer()
        {
            GetInfo();
            return View();
        }

        public IActionResult Register()
        {

            GetInfo();
            return View();

        }


        [HttpPost]
        public IActionResult Register(string email,string matkhau,string hoten, string dienthoai)
        {
            // kiểm tra email đã tồn tại 


            // thêm khach hàng vào db
            var kh = new Khachhang();
            kh.Email = email;
            kh.MatKhau = _pwHear.HashPassword(kh, matkhau);   // mã hóa mật khẩu
            kh.Ten = hoten;
            kh.DienThoai = dienthoai;
            _context.Add(kh);
            _context.SaveChanges();


            return RedirectToAction(nameof(Login));
        }

        public IActionResult Signout()
        {
            HttpContext.Session.SetString("khachhang", "");
            HttpContext.Session.SetString("Nhanvien", "");
            GetInfo();
            return RedirectToAction(nameof(Index));
        }

    }
}
