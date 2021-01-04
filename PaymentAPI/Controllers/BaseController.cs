using Microsoft.AspNetCore.Mvc;
using Payment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers
{
    public class BaseController: Controller
    {

        public BaseController()
        { }
        public IUserDomain _domain { get; set; }

        public ICityDomain city { get; set; }

        public ICategoryDomain _Category { get; set; }
        public IProductDomain _Product { get; set; }

        public ICartDomain _Cart { get; set; }

        public IWishlistDomain _Wishlist { get; set; }

        public IOrderDomain _Order { get; set; }

        public ICommonDomain _common { get; set; }
    }
}
