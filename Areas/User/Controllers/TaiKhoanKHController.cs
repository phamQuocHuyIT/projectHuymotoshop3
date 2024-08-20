using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using doAnChuyenNghanh02.Models;
namespace doAnChuyenNghanh02.Areas.User.Controllers
{
    public class TaiKhoanKHController : Controller
    {
        string idtkkh = "idtkkh";
        MotoDBContext dBContext = new MotoDBContext(); 
        // GET: User/TaiKhoanKH
        public ActionResult Index()
        {
            if (Session[idtkkh] != null)
            {
                int session = int.Parse(Session[idtkkh].ToString());
                TAIKHOANKHACHHANG tkkh = dBContext.TAIKHOANKHACHHANGs.Include(x => x.KHACHHANG).FirstOrDefault(x => x.IDTKKH == session);
                return View(tkkh);
            }
            else
            {
                return RedirectToAction("DangNhap", "ShoppingCart");
            }
        }
        [HttpPost]
        public ActionResult UpdateKH(FormCollection field)
        {
            int session = int.Parse(Session[idtkkh].ToString());
            var idkh = dBContext.TAIKHOANKHACHHANGs.Find(session);
            if (Session[idtkkh] != null)
            {
                var khachhang = dBContext.KHACHHANGs.FirstOrDefault(x => x.IDKH == idkh.IDKH);
                if (khachhang != null)
                {
                    khachhang.TENKH = field["tenkh"];
                    khachhang.DIENTHOAI = int.Parse(field["dienthoai"]);
                    khachhang.NGAYSINH = DateTime.Parse(field["ngaysinh"]);
                    khachhang.DIACHI = field["diachi"];
                    khachhang.EMAIL = field["email"];
                    khachhang.GIOITINH = field["gioitinh"];
                    dBContext.SaveChanges();
                }
                
            }
            return RedirectToAction("Index");
        }

        public ActionResult Donhang()
        {
            List<DONHANG> listdonhang = new List<DONHANG>();
            if (Session[idtkkh]!= null)
            {
                int session = int.Parse(Session[idtkkh].ToString());
                listdonhang = dBContext.DONHANGs.Include(x => x.TAIKHOANKHACHHANG).Where(x => x.IDTKKH == session).ToList();
                return View(listdonhang);
            }
            return View(listdonhang);
            
        }
        [HttpPost]
        public ActionResult DetailDH(int? iddh)
        {
            CHITIET_DONTHANG chitiet = dBContext.CHITIET_DONTHANG.Include(x => x.SANPHAM).FirstOrDefault(x => x.IDDONHANG == iddh);
            
            return View(chitiet);
        }

        public ActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassWord(FormCollection field)
        {
            string tendp = field["tendn"];
            string pass = field["old_pass"];
            string corect = field["corect_pass"];
            var check = dBContext.TAIKHOANKHACHHANGs.FirstOrDefault(x => x.USERNAME == tendp && x.PASSWORD == pass);
            if (Session[idtkkh] != null && check != null)
            {
                    check.PASSWORD = field["new_pass"];
                    dBContext.SaveChanges();
                    return RedirectToAction("UpdateKH");
            }
            else
            {
                ViewBag.Error = "Không đúng mật khẩu hoặc tên đăng nhập";
                return RedirectToAction("ChangePass");
                
            }

        }
    }
}