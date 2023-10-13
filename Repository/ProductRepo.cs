using MercuryShopping.Models;
using MercuryShopping.Models.Tables;



namespace MercuryShopping.Repository.AdminRepository
{
    public interface IProductRepo
    {
        public bool addNewProduct(Product product, IFormFile productImg1, IFormFile productImg2, IFormFile productImg3);
        public List<Product> getAllProducts();

        public Product getProductByID(int Id);

        public bool EditProduct(Product product);

        public List<Product> getProductsByQuery(string searchQuery = "", int CatagID = 0);

        public bool RemoveProduct(int Id);

        public void ConfirmOrder(int OrderID);

    }
    public class ProductRepo: IProductRepo
    {
        public MyDBContext ctx;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductRepo(MyDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            ctx = context;
            _webHostEnvironment = webHostEnvironment;
        }



        public bool addNewProduct(Product product, IFormFile productImg1, IFormFile productImg2, IFormFile productImg3)
        {
            try
            {
                product.AvgRating = 0;
                product.NumOfReviews = 0;
                product.ProStatus = 1;
                ctx.Product.Add(product);
                ctx.SaveChanges();

                //----Uploading Images

                // Get the base directory path
                string baseDirPath = _webHostEnvironment.WebRootPath;

                // Upload Img1
                if (productImg1 != null && productImg1.Length > 0)
                {
                    string img1NameAndPath =Path.Combine("ProductImages/", product.ProID + "-1" + Path.GetExtension(productImg1.FileName));
                    using (var fileStream = new FileStream(Path.Combine(baseDirPath, img1NameAndPath), FileMode.Create))
                    {
                        productImg1.CopyTo(fileStream);
                    }
                    product.Img1 = img1NameAndPath;
                 }

                // Upload Img2
                if (productImg2 != null && productImg2.Length > 0)
                {
                    string img2NameAndPath = Path.Combine("ProductImages/", product.ProID + "-2" + Path.GetExtension(productImg2.FileName));
                    using (var fileStream = new FileStream(Path.Combine(baseDirPath, img2NameAndPath), FileMode.Create))
                    {
                        productImg2.CopyTo(fileStream);
                    }
                    product.Img2 = img2NameAndPath;
                }


                // Upload Img3
                if (productImg3 != null && productImg3.Length > 0)
                {
                    string img3NameAndPath = Path.Combine("ProductImages/", product.ProID + "-3" + Path.GetExtension(productImg3.FileName));
                    using (var fileStream = new FileStream(Path.Combine(baseDirPath, img3NameAndPath), FileMode.Create))
                    {
                        productImg3.CopyTo(fileStream);
                    }
                    product.Img3 = img3NameAndPath;
                }


                // Update the product entity and save changes
                ctx.Product.Update(product);
                ctx.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public void ConfirmOrder(int OrderID)
        {
            Orders order = ctx.Orders.Find(OrderID);
            if (order != null)
            {
                order.OrderStatus = 1;
                ctx.Orders.Update(order);
                ctx.SaveChanges();
            }

        }

        public List<Product> getAllProducts()
        {
            return ctx.Product.Where(p=>p.ProStatus==1).ToList();
        }

        public Product getProductByID(int Id)
        {
            return ctx.Product.Find(Id);
        }
        public List<Product> getProductsByQuery(string searchQuery="", int CatagID=0)
        {
            searchQuery = searchQuery.ToLower();
            if (CatagID == 0)
            {
                return ctx.Product
                                  .Where(p => p.ProStatus == 1 && p.ProName.ToLower().Contains(searchQuery)).ToList();
            }
            else
            {
                return ctx.Product
                                    .Where(p => p.ProStatus == 1
                                         && (p.ProName.ToLower().Contains(searchQuery))
                                         && (p.CatagID == CatagID)
                                     ).ToList();
            }
        }

        public bool RemoveProduct(int Id)
        {
            Product product = ctx.Product.Find(Id);
            if (product != null)
            {
                try
                {
                    product.ProStatus = 0;
                    ctx.Product.Update(product);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public bool EditProduct(Product product)
        {
            if (product != null)
            {
                try
                {
                    ctx.Product.Update(product);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

    }
}
