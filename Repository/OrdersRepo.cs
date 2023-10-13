using MercuryShopping.Models;
using MercuryShopping.Models.Tables;

namespace MercuryShopping.Repository.AdminRepository
{
    public interface IOrdersRepo
    {
        public List<Orders> getListOfOrdersForThisDate(DateTime dateTime);
        public Orders getOrdersByID(int OrderID);
        public List<Orders> GetOrdersByCustID(int CustID);

        public List<OrderDetails> GetOrderDetails(int OrderID);

        public bool makeAnOrder(int CustID, string Address, List<Product> cartItems);
    }

    public class OrdersRepo: IOrdersRepo
    {
        public MyDBContext ctx;
        public OrdersRepo(MyDBContext context)
        {
            ctx = context;
        }
        public List<Orders> getListOfOrdersForThisDate(DateTime dateTime)
        {
            return ctx.Orders.Where(o => o.Date_.Date == dateTime.Date).OrderByDescending(o => o.OrderStatus).ToList();
        }

        public Orders getOrdersByID(int OrderID)
        {
            return ctx.Orders.Find(OrderID);
        }


        public List<OrderDetails> GetOrderDetails(int OrderID)
        {
            List<OrderDetails> orderDetails= ctx.OrderDetails.Where(od => od.OrderID == OrderID).ToList();

            orderDetails.ForEach(od => { od.ProName = ctx.Product.Find(od.ProID).ProName; }); //adding the Product name with Order Details

            return orderDetails;
           
        }

        public List<Orders> GetOrdersByCustID(int CustID)
        {
            return ctx.Orders.Where(o => o.CustID == CustID).OrderByDescending(o => o.OrderStatus).ToList();

        }

        public bool makeAnOrder(int CustID, string Address, List<Product> cartItems)
        {
            try
            {
                Orders newOrder = new Orders();
                newOrder.CustID = CustID;
                newOrder.Date_ = DateTime.Now;
                newOrder.OrderStatus = 0;
                newOrder.Address = Address;


                ctx.Orders.Add(newOrder);
                ctx.SaveChanges();

                int OrderId = newOrder.OrderID;

                foreach (Product productFromCart in cartItems)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.OrderID = OrderId;
                    orderDetails.ProID = productFromCart.ProID;

                    Product productFromDB = ctx.Product.Find(productFromCart.ProID);

                    if (productFromCart.Qty > productFromDB.Qty)
                    {
                        productFromCart.Qty = productFromDB.Qty;
                        productFromDB.Qty = 0;
                    }
                    else
                    {
                        productFromDB.Qty = productFromDB.Qty - productFromCart.Qty;
                    }
                    orderDetails.Qty = productFromCart.Qty;
                    orderDetails.ProTotal = productFromCart.UPrice * productFromCart.Qty;

                    ctx.OrderDetails.Add(orderDetails);

                    ctx.SaveChanges();

                }

                newOrder.Total = ctx.OrderDetails.Where(od => od.OrderID == newOrder.OrderID).Sum(od => od.ProTotal);
                ctx.Orders.Update(newOrder);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


    }
}
