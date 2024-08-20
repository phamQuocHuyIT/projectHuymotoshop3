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
    public class HDNhapController : Controller
    { 
        MotoDBContext dBContext = new MotoDBContext();
        string idtknv = "idtknv";
        // GET: Admin/HDNhap
        public ActionResult Index(IEnumerable<HOADONNHAP> listhdn)
        {
            if (Session[idtknv] != null)
            {
                if (listhdn == null)
                {
                    listhdn = dBContext.HOADONNHAPs.ToList();
                }
                return View(listhdn);
            }
            else
            {
                return RedirectToAction("Index", "DangNhapAdmin");
            }
           
        }

        public ActionResult UpdateHDN(FormCollection field)
        {
            int iddh = int.Parse(field["idhdn1"]);
            string trangthai = field["trangthaii"];
            string ghichu = field["ghichu1"];
            int tennv = int.Parse(Session[idtknv].ToString());
            var hoadonnhap = dBContext.HOADONNHAPs.Find(iddh);
            if (hoadonnhap != null)
            {
                //donhang.IDTKNV = idtkkh;
                // donhang.IDTKNV = idtknv;
                hoadonnhap.TRANGTHAI = trangthai;
                hoadonnhap.GHICHU = ghichu;

                var nhanvienEntity = dBContext.TAIKHOANNHANVIENs.FirstOrDefault(cv => cv.IDTKNV == tennv);
                if (nhanvienEntity != null)
                {
                    hoadonnhap.TAIKHOANNHANVIEN = nhanvienEntity;
                }
                dBContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult CreateHDN(FormCollection field)
        {
            DateTime ngaynhap = DateTime.Now;
            string trangthai = field["trangthai"];
            // int idtkkh = int.Parse(field["tenkh"]);
            // int idtknv = int.Parse(field["tennv"]);
            string ghichu = field["ghichu"];
            int tennv = int.Parse(Session[idtknv].ToString());


            int idsp = int.Parse(field["idsp"]);
            int soluong = int.Parse(field["soluong"]);
            var sanpham = dBContext.SANPHAMs.Find(idsp);
            

            var hoadonhap = new doAnChuyenNghanh02.Models.HOADONNHAP()
            {
                NGAYNHAP = ngaynhap,
                TRANGTHAI = trangthai,
                GHICHU = ghichu
               


            };
            

            var nhanvienEntity = dBContext.TAIKHOANNHANVIENs.FirstOrDefault(cv => cv.IDTKNV == tennv);
            if (nhanvienEntity != null)
            {
                hoadonhap.TAIKHOANNHANVIEN = nhanvienEntity;
            }
            dBContext.HOADONNHAPs.Add(hoadonhap);
            dBContext.SaveChanges();


            if (sanpham != null)
            {
                var add = new doAnChuyenNghanh02.Models.CHITIET_HDNHAP()
                {
                    IDHDNHAP = hoadonhap.IDHDNHAP,
                    SOLUONG = soluong,
                    SANPHAM = sanpham

                };
                dBContext.CHITIET_HDNHAP.Add(add);
                dBContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteHDN(int idhdn)
        {
            var hoadonnhap = dBContext.HOADONNHAPs.Find(idhdn);
            if (hoadonnhap != null)
            {
                dBContext.HOADONNHAPs.Remove(hoadonnhap);
                dBContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public ActionResult DetailHDN(int idhdn)
        {
            List<CHITIET_HDNHAP> cthdnhap = dBContext.CHITIET_HDNHAP.Include(x => x.SANPHAM).Where(x=> x.IDHDNHAP == idhdn).ToList();
            
            return View(cthdnhap);
        }

        public ActionResult UpdateDetailHDN(FormCollection field)
        {
            int idcthdn = int.Parse(field["idcthdn1"]);
            int iddh = int.Parse(field["idhdn1"]);

            int gia = int.Parse(field["gia1"]);
            int soluong = int.Parse(field["soluong1"]);
           
            var chitiet = dBContext.CHITIET_DONTHANG.Find(idcthdn);
            if (chitiet != null)
            {
                //donhang.IDTKNV = idtkkh;
                // donhang.IDTKNV = idtknv;
                chitiet.SOLUONG = soluong;
                

                dBContext.SaveChanges();
            }

            return RedirectToAction("DetailHDN", new { idhdn = iddh });
        }

        public ActionResult CreateDetailHDN(FormCollection field)
        {
            int idhdn = int.Parse(field["idhdn"]);
            int idsp = int.Parse(field["idsp"]);
            int soluong = int.Parse(field["soluong"]);
            var sanpham = dBContext.SANPHAMs.Find(idsp);

            if (sanpham != null)
            {
                var add = new doAnChuyenNghanh02.Models.CHITIET_HDNHAP()
                {
                    IDHDNHAP = idhdn,
                    SOLUONG = soluong,
                    SANPHAM = sanpham

                };
                dBContext.CHITIET_HDNHAP.Add(add);
                dBContext.SaveChanges();
            }
            return RedirectToAction("DetailHDN", new { idhdn = idhdn });
           
        }

        public ActionResult DeleteDetailHDN(int idcthdn)
        {
            var hoadonnhap = dBContext.CHITIET_HDNHAP.Find(idcthdn);
            var idhdn = hoadonnhap.IDHDNHAP;
            if (hoadonnhap != null)
            {
                dBContext.CHITIET_HDNHAP.Remove(hoadonnhap);
                dBContext.SaveChanges();
            }

            return RedirectToAction("DetailHDN", new { idhdn = idhdn});
        }
    }
}