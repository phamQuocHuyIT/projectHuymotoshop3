using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using doAnChuyenNghanh02.Models;
using System.Net;
namespace doAnChuyenNghanh02.Areas.User.Controllers
{
    public class ShoppingCartController : Controller
    {
        private MotoDBContext dbContext = new MotoDBContext();
        private string strCart = "Cart";
        // GET: User/ShoppingCart

        //TRANG CHỦ
        public ActionResult Index()
        {
            return View();
        }


        //THÊM SẢN PHẨM VÀO GIỎ HÀNG
        public ActionResult OrderNow(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session[strCart] == null)
            {
                List<Cart> ListCart = new List<Cart>
                {
                    new Cart(dbContext.SANPHAMs.Find(Id),1)
                };
                Session[strCart] = ListCart;

            }
            else
            {
                List<Cart> ListCart = (List<Cart>)Session[strCart];
                int check = IsExistingCheck(Id);
                if (check == -1)
                {
                    ListCart.Add(new Cart(dbContext.SANPHAMs.Find(Id), 1));
                }
                else
                {
                    ListCart[check].Quantity++;
                }
                Session[strCart] = ListCart;
            }
            return RedirectToAction("Index");
        }
        private int IsExistingCheck(int? Id)
        {
            List<Cart> ListCart = (List<Cart>)Session[strCart];
            for (int i = 0; i < ListCart.Count; i++)
            {
                if (ListCart[i].Product.IDSP == Id) { return i; }
            }
            return -1;
        }

        //XÓA SẢN PHẨM
        public ActionResult RemoveItem(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int check = IsExistingCheck(Id);
            List<Cart> ListCart = (List<Cart>)Session[strCart];
            ListCart.RemoveAt(check);
            if (ListCart.Count == 0)
            {
                Session[strCart] = null;
            }
            else
            {
                Session[strCart] = ListCart;
            }
            return RedirectToAction("Index");
        }
        [HttpPost]

        public ActionResult UpdateCart(FormCollection field)
        {
            string[] quantities = field.GetValues("quantity");
            List<Cart> ListCart = (List<Cart>)Session[strCart];
            for (int i = 0; i < ListCart.Count; i++)
            {
                ListCart[i].Quantity = Convert.ToInt32(quantities[i]);
            }
            Session[strCart] = ListCart;
            return RedirectToAction("Index");
        }

        public ActionResult ClearCart()
        {
            Session[strCart] = null;
            return RedirectToAction("Index");
        }

        public ActionResult CheckOut()
        {
            if (Session["idtkkh"] == null)
            {
                return RedirectToAction("DangNhap");
            }
            else
            {
                ViewBag.Idtkkh = Session["idtkkh"];
                return View();
            }
           
        }

        [HttpPost]
        public ActionResult ProcessOrder(FormCollection field)
        {
            int idtkkh = int.Parse(Session["idtkkh"].ToString());
            List<Cart> ListCart = (List<Cart>)Session[strCart];

            //1. luu dat hang va bang dat hang
            var chitiet = new doAnChuyenNghanh02.Models.DONHANG()
            {
                NGAYDAT = DateTime.Now,
               
                TRANGTHAI = "Đang xử lý"
               
            };
            var checktkkh = dbContext.TAIKHOANKHACHHANGs.Find(idtkkh);
            chitiet.IDTKKH = checktkkh.IDTKKH;
            dbContext.DONHANGs.Add(chitiet);
            dbContext.SaveChanges();

            //2 luu chi tiet dat hang vao bang chi tiet dat hang
            foreach (Cart cart in ListCart)
            {
                CHITIET_DONTHANG orderDetail = new CHITIET_DONTHANG()
                {
                    IDDONHANG = chitiet.IDDONHANG,
                    IDSP = cart.Product.IDSP,
                    SOLUONG = Convert.ToInt32(cart.Quantity),
                    TONGTIEN = Convert.ToInt32(cart.Product.GIA)
                };
                dbContext.CHITIET_DONTHANG.Add(orderDetail);
                dbContext.SaveChanges();
            }

            //3 tro lai shopping cart session
            Session.Remove(strCart);
            return View("OrderSeccess");
        }

       public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckDangNhap(FormCollection field)
        {
            string tendn = field["tendn"];
            string pass = field["mkdn"];

            var userAccount = dbContext.TAIKHOANKHACHHANGs
                .FirstOrDefault(x => x.USERNAME == tendn && x.PASSWORD == pass);

            if (userAccount != null)
            {
                Session["idtkkh"] = userAccount.IDTKKH; // Lưu ID khách hàng vào Session
                Session["tenkh"] = userAccount.KHACHHANG.TENKH;
                return RedirectToAction("Index", "User"); // Điều hướng về trang chủ
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
        }


       
    }
}