using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shopthethao.Models
{
    [Serializable]
    public class CartItem
    {
        public SanPham Product { set; get; }
        public int Quantity { set; get; }
    }
}