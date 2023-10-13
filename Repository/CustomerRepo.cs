using MercuryShopping.Models;
using MercuryShopping.Models.Tables;

namespace MercuryShopping.Repository.AdminRepository
{
    public interface ICustomerRepo
    {
        public Customer getCustomerInfo(int CustID);

        public bool isCustomerValiedForAPIRequest(int CustID, string PassWord);

        public Customer loginCustomer(String EmailOrPhoneNumber, String PassWord);

        public Customer signUpNewCustomer(Customer customer,IFormFile custImg);

    }

    public class CustomerRepo : ICustomerRepo
    {
        public MyDBContext ctx;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerRepo(MyDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            ctx = context;
            _webHostEnvironment = webHostEnvironment;

        }

        public Customer getCustomerInfo(int CustID)
        {
            return ctx.Customer.FirstOrDefault(Cust => Cust.CustID == CustID && Cust.CustStatus == 1);
        }


 

        public bool isCustomerValiedForAPIRequest(int CustID, string PassWord)
        {
            if (ctx.Customer.FirstOrDefault(Cust => Cust.CustID == CustID && Cust.PassWord == PassWord)!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Customer loginCustomer(String EmailOrPhoneNumber, String PassWord)
        {
            if (EmailOrPhoneNumber == null || PassWord == null)
                return null;



            Customer customer = ctx.Customer.FirstOrDefault(Cust => (Cust.CustStatus == 1)
                                                                && (
                                                                        Cust.Email.ToLower() == EmailOrPhoneNumber.ToLower()
                                                                      || Cust.PhoneNumber == EmailOrPhoneNumber
                                                                     )
                                                                && Cust.PassWord == PassWord);
            return customer;
        }

        public Customer signUpNewCustomer(Customer newCustomer, IFormFile custImg)
        {
            newCustomer.CustStatus = 1;

            try
            {
                ctx.Customer.Add(newCustomer);
                ctx.SaveChanges();

                //----Uploading Images

                // Get the base directory path
                string baseDirPath = _webHostEnvironment.WebRootPath;

                // Upload Img1
                if (custImg != null && custImg.Length > 0)
                {
                    string custImgNameAndPath = Path.Combine("CustomerImages/", newCustomer.CustID + Path.GetExtension(custImg.FileName));
                    using (var fileStream = new FileStream(Path.Combine(baseDirPath, custImgNameAndPath), FileMode.Create))
                    {
                        custImg.CopyTo(fileStream);
                    }
                    newCustomer.CustImg = custImgNameAndPath;
                }
                ctx.SaveChanges();
                return newCustomer;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}