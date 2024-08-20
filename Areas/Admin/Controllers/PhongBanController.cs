using doAnChuyenNghanh02.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using doAnChuyenNghanh02.Models;
namespace doAnChuyenNghanh02.Areas.Admin.Controllers
{
    public class PhongBanController : Controller
    {
        MotoDBContext dBContext1 = new MotoDBContext();
        string idtknv = "idtknv";
        // GET: Admin/PhongBan
        public ActionResult Index(IEnumerable<PHONGBAN> listphongban)
        {
            if (Session[idtknv] != null)
            {
                if (listphongban == null)
                {
                    listphongban = dBContext1.PHONGBANs.ToList();
                }
                return View(listphongban);
            }
            else
            {
                return RedirectToAction("Index", "DangNhapAdmin");
            }
            
        }
        public ActionResult PhongBan(string idpb)
        {
            List<PHONGBAN> listphongban;
            if(idpb == null)
            {
                listphongban = dBContext1.PHONGBANs.ToList();
            }
            else
            {
                listphongban = dBContext1.PHONGBANs.Where(x => x.IDPHONGBAN == idpb).ToList();
            }
            
            return View("Index", listphongban);
        }
        public ActionResult FillerPB(FormCollection field)
        {
            string idpb = field["phongban"];
           
            return RedirectToAction("PhongBan", new { idpb = idpb});
        }

        
        //Cập nhật nhân viên
        [HttpPost]
        public ActionResult UpdatePB(FormCollection field)
        {
            string idpb = field["idpb1"];
            string tenpb = field["tenpb1"];
           

            var phongBan = dBContext1.PHONGBANs.FirstOrDefault(nv => nv.IDPHONGBAN == idpb);

            if (phongBan != null)
            {
                phongBan.TENPB = tenpb;
                
               

                // Cập nhật chức vụ và phòng ban nếu cần
                
                dBContext1.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        //thêm nhân viên mới
        [HttpPost]
        public ActionResult CreatePB(FormCollection field)
        {
            string mapb = field["mapb"];
            string tenpb = field["tenpb"];


            var phongban = new doAnChuyenNghanh02.Models.PHONGBAN()
            {

                IDPHONGBAN = mapb,
                TENPB = tenpb
            };
            // Cập nhật chức vụ và phòng ban nếu cần
            dBContext1.PHONGBANs.Add(phongban);
            dBContext1.SaveChanges();


            return RedirectToAction("Index");
        }
        public ActionResult DeletePB(string idpb)
        {

            var phongban = dBContext1.PHONGBANs.Find(idpb);
            if (idpb != null)
            {
                dBContext1.PHONGBANs.Remove(phongban);
                dBContext1.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}