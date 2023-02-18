using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using GarmentStreet.Models.ViewModels;
using GarmentStreet.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Stripe.Checkout;
using System.Security.Claims;


namespace GarmentStreetWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ShoppingCartItemsViewModel ShoppingCartItemsVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartItemsVM = new ShoppingCartItemsViewModel()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Inventory.Product,Inventory.VariationOption"),
                OrderHeader = new()
            };

            foreach (var cart in ShoppingCartItemsVM.ListCart)
            {
                ShoppingCartItemsVM.OrderHeader.OrderTotal += (cart.Inventory.Product.Price * cart.Quatity);
            }
            return View(ShoppingCartItemsVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartItemsVM = new ShoppingCartItemsViewModel()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Inventory.Product,Inventory.VariationOption"),
                OrderHeader = new()
            };

            ShoppingCartItemsVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == claim.Value);

            ShoppingCartItemsVM.OrderHeader.Name = ShoppingCartItemsVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartItemsVM.OrderHeader.PhoneNumber = ShoppingCartItemsVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartItemsVM.OrderHeader.StreetAddress = ShoppingCartItemsVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartItemsVM.OrderHeader.City = ShoppingCartItemsVM.OrderHeader.ApplicationUser.City;
            ShoppingCartItemsVM.OrderHeader.State = ShoppingCartItemsVM.OrderHeader.ApplicationUser.State;
            ShoppingCartItemsVM.OrderHeader.PostalCode = ShoppingCartItemsVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartItemsVM.ListCart)
            {
                ShoppingCartItemsVM.OrderHeader.OrderTotal += (cart.Inventory.Product.Price * cart.Quatity);
            }
            return View(ShoppingCartItemsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(/*ShoppingCartItemsViewModel ShoppingCartItemsVM*/)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartItemsVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Inventory.Product,Inventory.VariationOption");

            ShoppingCartItemsVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartItemsVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartItemsVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartItemsVM.OrderHeader.ApplicationUserId = claim.Value;

            foreach (var cart in ShoppingCartItemsVM.ListCart)
            {
                ShoppingCartItemsVM.OrderHeader.OrderTotal += (cart.Inventory.Product.Price * cart.Quatity);
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartItemsVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartItemsVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    InventoryId = cart.InventoryId,
                    OrderId = ShoppingCartItemsVM.OrderHeader.Id,
                    Price = cart.Inventory.Product.Price,
                    Quantity = cart.Quatity
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();

            }
            //Stripe Settings
            var domainStatic =  "https://localhost:44374/";
            
            
            var domain = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartItemsVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in ShoppingCartItemsVM.ListCart)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Inventory.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Inventory.Product.Name,
                        },
                    },
                    Quantity = item.Quatity,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripeSessionId(ShoppingCartItemsVM.OrderHeader.Id, session.Id);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);


            //_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartItemsVM.ListCart);
            //_unitOfWork.Save();

            //return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == id, includeProperties: "ApplicationUser");
            var service = new SessionService();
            Session session = service.Get(orderHeaderFromDb.SessionId);
            //check the stripe status
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStripePaymentIntentId(id, session.PaymentIntentId);
                _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                _unitOfWork.Save();
            }


            var model = new OrderConfirmationEmailViewModel() { OrderHeader= orderHeaderFromDb , 
                OrderDetails = _unitOfWork.OrderDetail.GetAll(x=>x.OrderId == id,includeProperties:"Inventory.Product,Inventory.VariationOption")};
            var result = await RenderView.RenderPageToStringAsync("OrderConfirmationEmail", model, this.ControllerContext);

            _emailSender.SendEmailAsync(orderHeaderFromDb.ApplicationUser.Email, "New Order - GarmentStreet", result);
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == orderHeaderFromDb.ApplicationUserId).ToList();
            HttpContext.Session.Clear();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);

        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == cart.ApplicationUserId).Sum(x => x.Quatity));
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            if (cart.Quatity <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);

            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
                

            }
            _unitOfWork.Save();
            var count = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == cart.ApplicationUserId).Sum(x => x.Quatity);
            HttpContext.Session.SetInt32(SD.SessionCart, count);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            var count = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == cart.ApplicationUserId).Sum(x => x.Quatity);
            HttpContext.Session.SetInt32(SD.SessionCart, count);
            return RedirectToAction(nameof(Index));
        }


    }
}
