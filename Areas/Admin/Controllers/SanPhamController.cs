using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using doAnChuyenNghanh02.Models;
using System.Drawing;
using System.IO;
namespace doAnChuyenNghanh02.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        MotoDBContext dBContext = new MotoDBContext();
        string idtknv = "idtknv";

        // GET: Admin/SanPham
        public ActionResult Index(IEnumerable<SANPHAM> listsanpham)
        {
            if (Session[idtknv] != null)
            {
                if (listsanpham == null)
                {
                    listsanpham = dBContext.SANPHAMs.Include(x => x.DANHMUC).ToList();
                    return View(listsanpham);
                }
                else
                {
                    return View(listsanpham);
                }
            }
            else
            {
                return RedirectToAction("Index", "DangNhapAdmin");
            }
            
            
        }

        public ActionResult SanPham(int iddm) 
        {
            List<SANPHAM> listsanpham;
            if (iddm == null)
            {
                listsanpham = dBContext.SANPHAMs.Include(x => x.DANHMUC).ToList();
            }
            else
            {
                listsanpham = dBContext.SANPHAMs.Include(x => x.DANHMUC).Where(x => x.IDDM == iddm).ToList();
            }
            
            return View("Index", listsanpham);
        }
        public ActionResult FillerDM(FormCollection field)
        {
            int iddm = int.Parse(field["danhmuc"]);
            if(iddm == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("SanPham", new { iddm = iddm });
            }
        }

        [HttpPost]
        public ActionResult CreateSP(FormCollection field)
        {

            string tensp = field["tensp"];
            int gia = int.Parse(field["gia"]);
            int soluong = int.Parse(field["soluong"]);
            string ncc = field["ncc"];
            
            int danhmuc = int.Parse(field["danhmuc"]);
           
            var sanpham = new doAnChuyenNghanh02.Models.SANPHAM()
            {
                TENSP = tensp,
                GIA = gia,
                SOLUONG = soluong,
                IDDM = danhmuc,
                IDNCC = ncc,
                
            };
            

            // Cập nhật chức vụ và phòng ban nếu cần
            dBContext.SANPHAMs.Add(sanpham);
            dBContext.SaveChanges();


            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateSP(FormCollection field)
        {
            int idsp = int.Parse(field["idsp1"]);
            string tensp = field["tensp1"];
            int gia = int.Parse(field["gia1"]);
            int soluong = int.Parse(field["soluong1"]);
            string ncc = field["ncc1"];

            int danhmuc = int.Parse(field["danhmuc1"]);

            var sanpham = dBContext.SANPHAMs.Include(nv => nv.DANHMUC).Include(nv => nv.NHACUNGCAP).FirstOrDefault(nv => nv.IDSP == idsp);

            if (sanpham != null)
            {
                sanpham.TENSP = tensp;
                sanpham.GIA = gia;
                sanpham.SOLUONG = soluong;
                sanpham.IDDM = danhmuc;
                sanpham.IDNCC = ncc;

                // Cập nhật chức vụ và phòng ban nếu cần
                

                dBContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteSP(int idsp)
        {
            var remove = dBContext.SANPHAMs.Find(idsp);
            dBContext.SANPHAMs.Remove(remove);
            dBContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateAnh(int idsp)
        {
            Session["idsp"] = idsp;   
            return View();
        }
        [HttpPost]
        public ActionResult UpdateAnh( HttpPostedFileBase fileAnh)
        {
            //tham số đầu vào
            int idsp = int.Parse(Session["idsp"].ToString());
            //kiểm tra có dữ liệu không
            if(fileAnh == null)
            {
                
                ViewBag.Erorr = "Chưa chọn file";
                return View();
            }
            if(fileAnh.ContentLength == 0)
            {
                
                ViewBag.Erorr = "file không có nội dung";
                return View();
            }

            // xác định đường dẫn
            var urlTuongDoi = "~/assets/img/"; // lấy đường dẫn lưu database
            var urlTuyetDoi = Server.MapPath(urlTuongDoi); // lấy đường dẫn lưu file trên server
            //lưu file
           

            string fullPath = urlTuyetDoi+ fileAnh.FileName;
            int i = 1;
            while(System.IO.File.Exists(fullPath))
            {
                string ten = Path.GetFileNameWithoutExtension(fileAnh.FileName);
                string duoi  = Path.GetExtension(fileAnh.FileName); 
                fullPath = urlTuongDoi+ ten + "-"+1+duoi;
                i++;
            }
            fileAnh.SaveAs(fullPath);
            var checksp = dBContext.SANPHAMs.Find(idsp);
            checksp.ANHSP = Path.GetFileName(fullPath);
            dBContext.SaveChanges();

            return RedirectToAction("UpdateAnh", new {idsp = idsp});
        }
    }
}