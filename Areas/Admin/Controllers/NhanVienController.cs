using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using doAnChuyenNghanh02.Models;
using System.Security.Principal;
using System.Net;
namespace doAnChuyenNghanh02.Areas.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        private MotoDBContext dBContext1 = new MotoDBContext();
        string idtknv = "idtknv";
        // GET: Admin/NhanVien
        public ActionResult Index(IEnumerable<NHANVIEN> listnhanvien)
        {
            if (Session[idtknv] != null)
            {
                if (listnhanvien == null)
                {
                    listnhanvien = dBContext1.NHANVIENs.Include(x => x.PHONGBAN).Include(x => x.CHUCVU).ToList();
                }
                return View(listnhanvien);
            }
            else
            {
                return RedirectToAction("Index", "DangNhapAdmin");
            }

        }
        public ActionResult NhanVien(string idpb, string chucvu)
        {
            List<NHANVIEN> listnhanvien;
            if (idpb == null && chucvu == null)
            {
                listnhanvien = dBContext1.NHANVIENs.Include(x => x.PHONGBAN).Include(x => x.CHUCVU).ToList();
            }
            else if (idpb == null && chucvu != null)
            {
                listnhanvien = dBContext1.NHANVIENs.Include(x => x.PHONGBAN).Include(x => x.CHUCVU).Where(x => x.IDCV == chucvu).ToList();
            }
            else if (idpb != null && chucvu != null)
            {
                listnhanvien = dBContext1.NHANVIENs.Include(x => x.PHONGBAN).Include(x => x.CHUCVU).Where(x => x.IDPB == idpb && x.CHUCVU.IDCV == chucvu).ToList();
            }
            else
            {
                listnhanvien = dBContext1.NHANVIENs.Include(x => x.PHONGBAN).Include(x => x.CHUCVU).Where(x => x.IDPB == idpb).ToList();
            }
            return View("Index", listnhanvien);
        }
        public ActionResult FillerNV(FormCollection field)
        {
            string idpb = field["phongban"];
            string tencv = field["chucvu"];
            return RedirectToAction("NhanVien", new { idpb = idpb, chucvu = tencv });
        }

        
        //Cập nhật nhân viên
        [HttpPost]
        public ActionResult UpdateNV(FormCollection field)
        {
            int idnv = int.Parse(field["idnv1"]);
            string tennv = field["tennv1"];
            DateTime ngaysinh = DateTime.Parse(field["ngaysinh1"]);
            string gioitinh = field["gioitinh1"];
            string diachi = field["diachi1"];
            int dienthoai = int.Parse(field["dienthoai1"]);
            string email = field["email1"];
            string chucvu = field["chucvu1"];
            string phongban = field["phongban1"];

            var nhanVien = dBContext1.NHANVIENs.Include(nv => nv.CHUCVU).Include(nv => nv.PHONGBAN).FirstOrDefault(nv => nv.IDNV == idnv);

            if (nhanVien != null)
            {
                nhanVien.TENNV = tennv;
                nhanVien.NGAYSINH = ngaysinh;
                nhanVien.GIOITINH = gioitinh;
                nhanVien.DIACHI = diachi;
                nhanVien.DIENTHOAI = dienthoai;
                nhanVien.EMAIL = email;

                // Cập nhật chức vụ và phòng ban nếu cần
                var chucVuEntity = dBContext1.CHUCVUs.FirstOrDefault(cv => cv.TENCHUCVU == chucvu);
                if (chucVuEntity != null)
                {
                    nhanVien.CHUCVU = chucVuEntity;
                }

                var phongBanEntity = dBContext1.PHONGBANs.FirstOrDefault(pb => pb.TENPB == phongban);
                if (phongBanEntity != null)
                {
                    nhanVien.PHONGBAN = phongBanEntity;
                }

                dBContext1.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        //thêm nhân viên mới
        [HttpPost]
        public ActionResult CreateNV(FormCollection field)
        {
            
            string tennv = field["tennv"];
            DateTime ngaysinh = DateTime.Parse(field["ngaysinh"]);
            string gioitinh = field["gioitinh"];
            string diachi = field["diachi"];
            int dienthoai = int.Parse(field["dienthoai"]);
            string email = field["email"];
            string chucvu = field["chucvu"];
            string phongban = field["phongban"];

            var nhanVien = new doAnChuyenNghanh02.Models.NHANVIEN()
            {

                TENNV = tennv,
                NGAYSINH = ngaysinh,
                GIOITINH = gioitinh,
                DIACHI = diachi,
                DIENTHOAI = dienthoai,
                EMAIL = email,
                IDCV = chucvu,
                IDPB = phongban
            };
            // Cập nhật chức vụ và phòng ban nếu cần
            dBContext1.NHANVIENs.Add(nhanVien);
           dBContext1.SaveChanges();
            

            return RedirectToAction("Index");
        }

        public ActionResult DeleteNV(int idnv)
        {
            
            var nhanvien = dBContext1.NHANVIENs.Find(idnv);
            if(idnv != null)
            {
                dBContext1.NHANVIENs.Remove(nhanvien);
                dBContext1.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }
    }

    
}