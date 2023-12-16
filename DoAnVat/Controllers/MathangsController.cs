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

namespace DoAnVat.Controllers
{
    public class MathangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MathangsController(ApplicationDbContext context)
        {
            _context = context;
        }


        void GetInfo()
        {
            ViewData["solg"] = GetCartItems().Count();

            ViewBag.danhmuc = _context.Danhmuc.ToList();


        }
        // GET: Mahangs
        public async Task<IActionResult> Index()
        {
            GetInfo();
            var applicationDbContext = _context.Mathang.Include(m => m.MaDmNavigation);
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
        public async Task<IActionResult> CreateBill(string email, string hoten, string dienthoai, string diachi)
        {
            var kh = new Khachhang();
            kh.Ten = hoten;
            kh.Email = email;
          
            kh.DienThoai = dienthoai;
            _context.Add(kh);
            await _context.SaveChangesAsync();

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



    }
}
