using MercuryShopping.Models.Tables;
using MercuryShopping.Repository.AdminRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MercuryShopping.Controllers
{

    public class ApiController : Controller
    {

        public IProductRepo productRepo;
        public IOrdersRepo ordersRepo;
        public ICustomerRepo customerRepo;
        public IReviewsRepo reviewsRepo;

        public ApiController(IProductRepo IproductRepo, IOrdersRepo IordersRepo, ICustomerRepo IcustomerRepo,IReviewsRepo IreviewsRepo)
        {
            productRepo = IproductRepo;
            ordersRepo = IordersRepo;
            customerRepo = IcustomerRepo;
            reviewsRepo = IreviewsRepo;
        }
        [HttpGet]
        public string loginCustomer(String EmailOrPhoneNumber="",String PassWord="")
        {
            Customer customer = customerRepo.loginCustomer(EmailOrPhoneNumber, PassWord);
            
            if(customer != null)
            {
                return JsonConvert.SerializeObject(customer); ;
            }
            else
            {
                return "Login Failed";
            }
        }

        [HttpGet]
        public string getCustomerOrdersHistory(int CustID,String PassWord)
        {
            if (!customerRepo.isCustomerValiedForAPIRequest(CustID, PassWord))
                return null;
             
         
            List<Orders> orders = ordersRepo.GetOrdersByCustID(CustID);
            return JsonConvert.SerializeObject(orders); ;
        }

        [HttpGet]
        public string getCustomerOrdersDetails(int OrderID,int CustID,String PassWord)
        {
            if (!customerRepo.isCustomerValiedForAPIRequest(CustID, PassWord))
                return null;


            List<OrderDetails> orderDetails = ordersRepo.GetOrderDetails(OrderID);
            return JsonConvert.SerializeObject(orderDetails);
        }


        [HttpGet]
        public string getProductList(String searchQuery="",int CatagoryID = 0)
        {
            List<Product> products = productRepo.getProductsByQuery(searchQuery, CatagoryID);
            return JsonConvert.SerializeObject(products);
        }


        public string getReviewsForAProduct(int ProID=0)
        {
            List<Reviews> reviews = reviewsRepo.getListOfReviewsForAProduct(ProID);
            return JsonConvert.SerializeObject(reviews);
        }


        public string CheckOutAPI(int custID,string password,string address ,string productListJson)
        {
            if (!customerRepo.isCustomerValiedForAPIRequest(custID, password))
                return "Failed"; 

            List<Product> productsToBuy=new List<Product>();
            var productList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(productListJson);
            
            if (productList == null)
                return "Failed";
            
            foreach (var productJson in productList)
            {
                Product item = new Product();
                item.ProID = Convert.ToInt32(productJson["proID"]);
                item.Qty = Convert.ToInt32(productJson["qty"]);
                item.UPrice = Convert.ToInt32(productJson["uPrice"]);
                productsToBuy.Add(item);
            }

            bool isOrderSuccessfull = ordersRepo.makeAnOrder(custID, address, productsToBuy);

            if (isOrderSuccessfull)
            {
                return "Successfull";

            }
            else 
            {
                return "Failed";
            }
        }
    }
}
