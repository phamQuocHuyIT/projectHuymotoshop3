using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using doAnChuyenNghanh02.Models;
using System.ComponentModel;
namespace doAnChuyenNghanh02.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        MotoDBContext dBContext = new MotoDBContext();
        string idtknv="idtknv";
        // GET: Admin/DonHang
        public ActionResult Index(IEnumerable<DONHANG> listdonhang)
        {
            if (Session[idtknv] != null)
            {
                if (listdonhang == null)
                {
                    listdonhang = dBContext.DONHANGs.Include(x => x.TAIKHOANKHACHHANG.KHACHHANG).Include(x => x.TAIKHOANNHANVIEN.NHANVIEN).ToList();
                }


                return View(listdonhang);
            }
            else
            {
                return RedirectToAction("Index", "DangNhapAdmin");
            }
            
        }

        public ActionResult DonHang(int iddh)
        {
            List<DONHANG> listdonhang;
            if(iddh != null)
            {
                listdonhang = dBContext.DONHANGs.Include(x => x.TAIKHOANKHACHHANG.KHACHHANG).Include(x => x.TAIKHOANNHANVIEN.NHANVIEN).Where(x => x.IDDONHANG == iddh).ToList();
            }
            else
            {
                listdonhang = dBContext.DONHANGs.Include(x => x.TAIKHOANKHACHHANG.KHACHHANG).Include(x => x.TAIKHOANNHANVIEN.NHANVIEN).ToList();
            }
            return RedirectToAction("Index", "DonHang", listdonhang);
        }

        public ActionResult UpdateDH(FormCollection field) {
            int iddh = int.Parse(field["iddh1"]);
            string trangthai = field["trangthaii"];
            //int idtkkh = int.Parse(field["idtkkh1"]);
            //int idtknv = int.Parse(field["idtknv1"]);
            string tenkh = field["idtkkh1"];
            int tennv =int.Parse( Session[idtknv].ToString());
            var donhang = dBContext.DONHANGs.Find(iddh);
            if(donhang != null)
            {
                //donhang.IDTKNV = idtkkh;
               // donhang.IDTKNV = idtknv;
                donhang.TRANGTHAI = trangthai;
                var chucVuEntity = dBContext.TAIKHOANKHACHHANGs.FirstOrDefault(cv => cv.KHACHHANG.TENKH == tenkh);
                if (chucVuEntity != null)
                {
                    donhang.TAIKHOANKHACHHANG = chucVuEntity;
                }

                var nhanvienEntity = dBContext.TAIKHOANNHANVIENs.FirstOrDefault(cv => cv.IDTKNV == tennv);
                if (nhanvienEntity != null)
                {
                    donhang.TAIKHOANNHANVIEN = nhanvienEntity;
                }
                dBContext.SaveChanges();
            }

            return RedirectToAction("Index"); 
        }

        public ActionResult DeleteDH(int iddh)
        {
            var donhang = dBContext.DONHANGs.Find(iddh);
            if(donhang != null)
            {
                dBContext.DONHANGs.Remove(donhang);
                dBContext.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult CreateDH(FormCollection field)
        {
            DateTime ngaydat = DateTime.Parse(field["ngaydat"]);
            string trangthai = "Đang xử lý";
            int idtkkh = int.Parse(field["idtkkh"]);
            int tknv = int.Parse(Session["idtknv"].ToString());
            var checkidtkkh = dBContext.TAIKHOANKHACHHANGs.Find(idtkkh);
            if(checkidtkkh != null)
            {
                //thêm đơn hàng
                var addDonhang = new doAnChuyenNghanh02.Models.DONHANG();
                addDonhang.NGAYDAT = ngaydat;
                addDonhang.TRANGTHAI = trangthai;

                addDonhang.IDTKKH = checkidtkkh.IDTKKH;
                addDonhang.IDTKNV = tknv;
                dBContext.DONHANGs.Add(addDonhang);
                dBContext.SaveChanges();
                // thêm chi tiết đơn hàng
                int idsp = int.Parse(field["idsp"]);
                int soluong = int.Parse(field["soluong"]);
                var checksp = dBContext.SANPHAMs.Find(idsp);
                int gia = int.Parse(checksp.GIA.ToString());
                int tongtien = soluong * gia;

                var addchitiet = new doAnChuyenNghanh02.Models.CHITIET_DONTHANG();
                addchitiet.IDSP = idsp;
                addchitiet.SOLUONG = soluong;
                addchitiet.TONGTIEN = tongtien;
                addchitiet.IDDONHANG = addDonhang.IDDONHANG;
                dBContext.CHITIET_DONTHANG.Add(addchitiet);
                dBContext.SaveChanges();

                // 1. Tìm số lượng sản phẩm đã đặt trong đơn hàng
                var chiTietDonHang = dBContext.CHITIET_DONTHANG.FirstOrDefault(ct => ct.IDSP == idsp);
                int soLuongDat = chiTietDonHang != null ? chiTietDonHang.IDSP : 0;
                    //chiTietDonHang != null ? chiTietDonHang.SOLUONG : 0;

                // 2. Tìm số lượng sản phẩm trong hóa đơn nhập
                var chiTietHDNhap = dBContext.CHITIET_HDNHAP.FirstOrDefault(ct => ct.IDSP == idsp);
                int soLuongNhap = chiTietHDNhap != null ? chiTietHDNhap.SOLUONG : 0;

                // 3. Cập nhật lại số lượng sản phẩm trong bảng SANPHAM
                var sanPham = dBContext.SANPHAMs.Find(idsp);
                if (sanPham != null)
                {
                    sanPham.SOLUONG = sanPham.SOLUONG + soLuongNhap - soLuongDat;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    dBContext.SaveChanges();
                }


                return RedirectToAction("Index");


            }
            else
            {
                ViewBag.erorrKH = "Không tìm thấy khách hàng";
                return RedirectToAction("Index");
            }
            
            
        }


        public ActionResult DetailDH(int iddh)
        {
            CHITIET_DONTHANG chitiet = dBContext.CHITIET_DONTHANG.FirstOrDefault(x => x.IDDONHANG == iddh);
            ViewBag.iddonhang = iddh;
            
            return View(chitiet);
        }

        public ActionResult UpdateDetail(FormCollection field)
        {
            int idhdct = int.Parse(field["idctdh1"]);
            int iddh = int.Parse(field["iddh1"]);
            
            int gia = int.Parse(field["gia1"]);
            int soluong = int.Parse(field["soluong1"]);
            int tongtien = gia * soluong;
            var chitiet = dBContext.CHITIET_DONTHANG.Find(idhdct);
            if (chitiet != null)
            {
                //donhang.IDTKNV = idtkkh;
                // donhang.IDTKNV = idtknv;
                chitiet.SOLUONG = soluong;
                chitiet.TONGTIEN = tongtien;
               
                dBContext.SaveChanges();
            }

            return RedirectToAction("DetailDH", new { iddh = iddh });
        }

        public ActionResult DeleteDetail(int iddh)
        {
            var donhang = dBContext.CHITIET_DONTHANG.Find(iddh);
            if (donhang != null)
            {
                dBContext.CHITIET_DONTHANG.Remove(donhang);
                dBContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}