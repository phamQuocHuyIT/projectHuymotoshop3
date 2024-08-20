using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doAnChuyenNghanh02.Models
{
    public class Cart
    {
        public SANPHAM Product { get; set; }
        public int Quantity { get; set; }

        public Cart(SANPHAM product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}