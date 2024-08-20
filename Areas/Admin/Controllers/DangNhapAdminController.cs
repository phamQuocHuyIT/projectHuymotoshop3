using doAnChuyenNghanh02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doAnChuyenNghanh02.Models;
using System.Data.Entity;
namespace doAnChuyenNghanh02.Areas.Admin.Controllers
{
    public class DangNhapAdminController : Controller
    {
        MotoDBContext dBContext = new MotoDBContext();
        string idtknv = "idtknv";
        // GET: Admin/DangNhapAdmin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckDangNhap(FormCollection field)
        {
            string tendn = field["tendn"];
            string pass = field["password"];
            var check = dBContext.TAIKHOANNHANVIENs.FirstOrDefault(x => x.TAIKHOAN == tendn && x.MATKHAU == pass);

            if (check != null)
            {
                Session[idtknv] = check.IDTKNV;
                Session["tennv"] = check.NHANVIEN.TENNV;
                return RedirectToAction("Index", "DonHang");
            }
            else
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View("Index"); // Return to the same view to display the error message
            }
        }

        public ActionResult DetailTKNV()
        {

            if (Session[idtknv] != null)
            {
                int detail = int.Parse(Session[idtknv].ToString());
                TAIKHOANNHANVIEN chitiet = dBContext.TAIKHOANNHANVIENs.Include(x => x.NHANVIEN.PHONGBAN).Include(x=> x.NHANVIEN.CHUCVU).FirstOrDefault(x => x.IDTKNV == detail);
                return View(chitiet);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }
        
        public ActionResult ChangPass()
        {
            if (Session[idtknv] != null)
            {
               
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult ChangePassTKNV(FormCollection field)
        {
            string tendp = field["tendn"];
            string pass = field["old_pass"];
            string corect = field["corect_pass"];
            var check = dBContext.TAIKHOANNHANVIENs.FirstOrDefault(x => x.TAIKHOAN == tendp && x.MATKHAU == pass);
            if (Session[idtknv] != null && check != null)
            {
                check.MATKHAU = field["new_pass"];
                dBContext.SaveChanges();
                return RedirectToAction("DetailTKNV");
            }
            else
            {
                ViewBag.Error = "Không đúng mật khẩu hoặc tên đăng nhập";
                return RedirectToAction("ChangePass");

            }

        }
    }
}