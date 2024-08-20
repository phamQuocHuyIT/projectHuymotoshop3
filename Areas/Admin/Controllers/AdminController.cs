using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doAnChuyenNghanh02.Models;
using System.Data.Entity;
namespace doAnChuyenNghanh02.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        MotoDBContext dBContext = new MotoDBContext();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (Session["idtknv"]!= null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "DangNhapAdmin");
            }
            
               
        }
    }
}