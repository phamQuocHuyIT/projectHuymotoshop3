using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using doAnChuyenNghanh02.Models;
namespace doAnChuyenNghanh02.Areas.User.Controllers
{
    public class UserController : Controller
    {
        private MotoDBContext dbContext = new MotoDBContext();
        // GET: User/User
        public ActionResult Index()
        {
            return View();
        }
        // Trang sản phẩm của người dùng User
        public ActionResult Products(int iddmsp = 0)
        {
            List<SANPHAM> listSP;
            if (iddmsp != 0)
            {
                listSP = dbContext.SANPHAMs.Include(x => x.DANHMUC).Where(x => x.IDDM == iddmsp).ToList();
            }
            else
            {
                listSP = dbContext.SANPHAMs.Include(x => x.DANHMUC).ToList();
            }
            return View(listSP);
        }
        //chi tiết sản phẩm
        public ActionResult Detail(int idsp)
        {
            SANPHAM sanpham = dbContext.SANPHAMs.Include(x => x.DANHMUC).FirstOrDefault(x => x.IDSP == idsp);
            return View(sanpham);
        }

        
    }
}