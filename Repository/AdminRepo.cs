using MercuryShopping.Models;

namespace MercuryShopping.Repository.AdminRepository
{

    public interface IAdminRepo
    {
        public bool isAdminLoggedIn(String userName, String password);
    }
    public class AdminRepo:IAdminRepo
    {
        public MyDBContext ctx;
        public AdminRepo(MyDBContext context)
        {
            ctx = context;
        }


        public bool isAdminLoggedIn(String userName,String password)
        {
            if (ctx.Admin.FirstOrDefault(a => a.UserName ==userName && a.PassWord == password) != null)
                return true;
            else
                return false;
        }
    }
}
