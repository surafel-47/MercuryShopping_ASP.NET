using MercuryShopping.Models.Tables;
using MercuryShopping.Repository.AdminRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MercuryShopping.Controllers
{
    public class CustomersController : Controller
    {
        public IProductRepo productRepo;
        public ICustomerRepo customerRepo;
        public IOrdersRepo ordersRepo;
        public IReviewsRepo reviewsRepo;


        public CustomersController(IProductRepo IproductRepo, IOrdersRepo IordersRepo, ICustomerRepo IcustomerRepo,IReviewsRepo IreviewsRepo)
        {
            productRepo = IproductRepo;
            ordersRepo = IordersRepo;
            customerRepo = IcustomerRepo;
            reviewsRepo = IreviewsRepo;
        }

        [NonAction]
        public Customer getCustomerFromSession()
        {

            string customerJson = HttpContext.Session.GetString("customer");
            if (string.IsNullOrEmpty(customerJson))
                return null;
            else
                return JsonConvert.DeserializeObject<Customer>(customerJson);
        }

        [NonAction]
        public void setCustomerToSession(Customer customer)
        {
            string customerJson = JsonConvert.SerializeObject(customer);
            HttpContext.Session.SetString("customer", customerJson);
        }


        [NonAction]
        public List<Product> getCartItemsFromSession()
        {
            string cartJson = HttpContext.Session.GetString("cartItems");
            if (string.IsNullOrEmpty(cartJson))
                return new List<Product>();
            else
                return JsonConvert.DeserializeObject<List<Product>>(cartJson);
        }
        [NonAction]
        public void setCartItemsToSession(List<Product> cartItems)
        {
            string updatedCartJson = JsonConvert.SerializeObject(cartItems);
            HttpContext.Session.SetString("cartItems", updatedCartJson);
        }


        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignInSignUp()
        {
            return View("SignInSignUp");
        }


        [HttpGet]
        public IActionResult SignIn(String EmailOrPhoneNumber, String PassWord)
        {
            Customer customer = customerRepo.loginCustomer(EmailOrPhoneNumber, PassWord);
            if (customer != null)
            {
                setCustomerToSession(customer);
                return RedirectToAction("SearchProducts");

            }
            else
            {
                ViewBag.msg = "Sign In Failed";
                return View("SignInSignUp");
            }

        }

        [HttpPost]
        public IActionResult SignUp(Customer customer, IFormFile custImg)
        {
            customer.CustStatus = 1;
            Customer newlyAddedCustomer = customerRepo.signUpNewCustomer(customer, custImg);
            if (newlyAddedCustomer != null) //means they have been added
            {
                setCustomerToSession(newlyAddedCustomer);
                return RedirectToAction("SearchProducts");
            }
            ViewBag.msg = "Sign Up Failed";
            return View("SignInSignUp");
        }


        [HttpGet]
        public IActionResult SignOut()
        {
            HttpContext.Session.SetString("customer", "");
            HttpContext.Session.SetString("cartItems", "");
            return RedirectToAction("SearchProducts");
        }


        public IActionResult CheckOut(String location="")
        {
            Customer customer = getCustomerFromSession();

            List<Product> cartItems=getCartItemsFromSession();


            if (cartItems.Count>0)
            {
                bool sucess = ordersRepo.makeAnOrder(customer!.CustID, location, cartItems);

                if (sucess)
                {
                    return RedirectToAction("SearchProducts");
                }
                else
                {
                    return Content("Failed Sorry :)");
                }

            }

            HttpContext.Session.SetString("cartItems", "");
            return View("SearchProducts");
        }

        [HttpGet]
        public IActionResult SearchProducts(String searchQuery = "", int SelectedCatagID = 0)
        {

            if (searchQuery == null)
                searchQuery = "";
            List<Product> products = productRepo.getProductsByQuery(searchQuery, SelectedCatagID);
            ViewBag.searchQuery = searchQuery;
            ViewBag.selectedCatagID = SelectedCatagID;
            return View(products);
        }

        [HttpGet]
        public IActionResult ViewProductDetail(int ProID)
        {
            Product product = productRepo.getProductByID(ProID);
            List<Reviews> reviewsForProduct = reviewsRepo.getListOfReviewsForAProduct(product.ProID);
            ViewBag.reviews = reviewsForProduct;
            return View(product);
        }


        [HttpPost]
        public IActionResult AddReview(int ProID,int CustID,int rating,String comment)
        {
            reviewsRepo.addReview(ProID,CustID,rating,comment);
            return RedirectToAction("ViewProductDetail",new {ProID=ProID});
        }

    

        public IActionResult MyCart()
        {
            List<Product> cartItems = getCartItemsFromSession();
            return View(cartItems);
        }

        public IActionResult RemoveFromCart(int ProID, String RedirectToo = "SearchProducts")
        {
            List<Product> cartItems = getCartItemsFromSession();

            cartItems.Remove(cartItems.Find(ct=>ct.ProID==ProID));
            
            setCartItemsToSession(cartItems);
            
            return RedirectToAction(RedirectToo);
        }

        public IActionResult AddToCart(int ProID, String RedirectToo = "SearchProducts")
        {
            List<Product> cartItems = getCartItemsFromSession();

            Product newItem = productRepo.getProductByID(ProID);
            newItem.Qty = 1;


            if (cartItems.FirstOrDefault(c => c.ProID == newItem.ProID) == null)
                cartItems.Add(newItem);

            setCartItemsToSession(cartItems);

            return RedirectToAction(RedirectToo);

        }

        public IActionResult AddProductQtyByOne(int ProID)
        {
            List<Product> cartItems = getCartItemsFromSession();


            int index = cartItems.FindIndex(p => p.ProID == ProID);
            cartItems[index].Qty += 1;

            setCartItemsToSession(cartItems);


            return RedirectToAction("MyCart");

        }
        public IActionResult SubProductQtyByOne(int ProID)
        {
            List<Product> cartItems = getCartItemsFromSession();


            int index = cartItems.FindIndex(p => p.ProID == ProID);

            if (cartItems[index].Qty>1)
                  cartItems[index].Qty -= 1;

            setCartItemsToSession(cartItems);

            return RedirectToAction("MyCart");
        }

        public IActionResult MyOrders()
        {
            Customer customer = getCustomerFromSession();
            if (customer == null)
                return View("SignInSignUp");

            var x = customer.CustID;
            List<Orders> orders = ordersRepo.GetOrdersByCustID(customer.CustID);
            return View(orders);
        }

        [HttpGet]
        public IActionResult MyOrderDetails(int orderID)
        {
            Customer customer = getCustomerFromSession();
            if (customer == null)
                return View("SignInSignUp");

            List<OrderDetails> orderDetails = ordersRepo.GetOrderDetails(orderID);
            ViewBag.order = ordersRepo.getOrdersByID(orderID);
            return View(orderDetails);
        }
    }
}
