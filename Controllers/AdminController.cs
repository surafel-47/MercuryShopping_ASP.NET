using MercuryShopping.Models.Tables;
using MercuryShopping.Repository.AdminRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MercuryShopping.Controllers
{
    public class AdminController : Controller
    {
        public IProductRepo productRepo;
        public IOrdersRepo ordersRepo;
        public IAdminRepo adminRepo;
        public ICustomerRepo customerRepo;

        public AdminController(IProductRepo IproductRepo, IOrdersRepo IordersRepo, IAdminRepo IadminRepo, ICustomerRepo IcustomerRepo)
        {
            productRepo = IproductRepo;
            ordersRepo = IordersRepo;
            adminRepo = IadminRepo;
            customerRepo = IcustomerRepo;
        }


        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View("AdminLogin");
        }

        [HttpPost]
        public IActionResult AdminLogin(String UserName, String Password)
        {
            if (adminRepo.isAdminLoggedIn(UserName, Password))
            {
                HttpContext!.Session.SetString("isAdminLoggedIn", "true");
                return RedirectToAction("SearchProducts");
            }
            else
            {
                return View("AdminLogin");
            }
        }

        public IActionResult AdminLogOut()
        {
            HttpContext!.Session.SetString("isAdminLoggedIn", "false");
            return RedirectToAction("AdminLogin");
        }


        [HttpGet]
        public IActionResult AddNewProduct()
        {
            if (HttpContext!.Session.GetString("isAdminLoggedIn") != "true")
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddNewProduct(Product product, IFormFile productImg1, IFormFile productImg2, IFormFile productImg3)
        {
            if (HttpContext!.Session.GetString("isAdminLoggedIn") != "true")
            {
                return RedirectToAction("AdminLogin");
            }
            try
            {
                productRepo.addNewProduct(product, productImg1, productImg2, productImg3);
                ViewBag.msg = "Item Added Succesfully";
            }
            catch (Exception e)
            {
                ViewBag.msg = e.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult SearchProducts(string searchQuery = "")
        {
            if (HttpContext!.Session.GetString("isAdminLoggedIn") != "true")
            {
                return RedirectToAction("AdminLogin");
            }
            List<Product> productList;
            if (searchQuery == "" || searchQuery == null)
            {
                productList = productRepo.getAllProducts();
            }
            else
            {
                productList = productRepo.getProductsByQuery(searchQuery);
            }
            return View("SearchProducts", productList);
        }



        public IActionResult EditProduct(int ProID)
        {
            if (HttpContext!.Session.GetString("isAdminLoggedIn") != "true")
            {
                return RedirectToAction("AdminLogin");
            }
            Product product = productRepo.getProductByID(ProID);
            if (product != null)
            {
                return View("EditProduct", product);
            }
            else
            {
                return View("SearchProducts");
            }
        }

        public IActionResult SaveChangesToEditedProduct(Product product)
        {
            if (HttpContext!.Session.GetString("isAdminLoggedIn") != "true")
            {
                return RedirectToAction("AdminLogin");
            }
            if (product == null)
            {
                return RedirectToAction("SearchProducts");
            }
            else
            {
                productRepo.EditProduct(product);
                return RedirectToAction("SearchProducts");
            }
        }

        public IActionResult RemoveProduct(int ProID = 0)
        {
            if (HttpContext!.Session.GetString("isAdminLoggedIn") != "true")
            {
                return RedirectToAction("AdminLogin");
            }
            productRepo.RemoveProduct(ProID);
            return RedirectToAction("SearchProducts");
        }

        [HttpGet]
        public IActionResult ViewOrders(DateTime datePicker)
        {
            if (HttpContext!.Session.GetString("isAdminLoggedIn") != "true")
            {
                return RedirectToAction("AdminLogin");
            }
            if (datePicker == null || datePicker.Equals(DateTime.MinValue))
                datePicker = DateTime.Today;
            ViewBag.dateSelected = datePicker.ToString("yyyy-MM-dd");
            List<Orders> orders = ordersRepo.getListOfOrdersForThisDate(datePicker);
            return View(orders);
        }


        [HttpGet]
        public IActionResult ViewOrderDetails(int OrderID)
        {
            if (HttpContext!.Session.GetString("isAdminLoggedIn") != "true")
            {
                return RedirectToAction("AdminLogin");
            }

            Orders order = ordersRepo.getOrdersByID(OrderID);
            List<OrderDetails> orderDetails = ordersRepo.GetOrderDetails(OrderID);
            Customer customer = customerRepo.getCustomerInfo(order.CustID);


            ViewBag.order = order;
            ViewBag.orderDetails = orderDetails;
            ViewBag.customer = customer;

            return View(); ;
        }

        public IActionResult ConfirmOrder(int OrderID)
        {
            if (HttpContext!.Session.GetString("isAdminLoggedIn") != "true")
            {
                return RedirectToAction("AdminLogin");
            }
            productRepo.ConfirmOrder(OrderID);
            return RedirectToAction("ViewOrders");
        }
    }
}
