using MercuryShopping.Models;
using MercuryShopping.Models.Tables;

namespace MercuryShopping.Repository.AdminRepository
{


    public interface IReviewsRepo
    {
        public List<Reviews> getListOfReviewsForAProduct(int ProID);
        public bool addReview(int ProID, int CustID, int rating, String comment);
    }

    public class ReviewsRepo : IReviewsRepo
    {
        public MyDBContext ctx;
        public ReviewsRepo(MyDBContext context)
        {
            ctx = context;
        }


        public List<Reviews> getListOfReviewsForAProduct(int ProID)
        {
            List<Reviews> reviews = ctx.Reviews.Where(r => r.ProID == ProID).ToList();
	        reviews.ForEach(r => r.ReviewerFullName = ctx.Customer.Find(r.CustID).Fname+ " "+ ctx.Customer.Find(r.CustID).Lname);
            
            return reviews;
        }

        public bool addReview(int ProID, int CustID, int rating, String comment)
        {
            try
            {
                Product product = ctx.Product.Find(ProID);
                Reviews review = new Reviews();
                review.CustID = CustID;
                review.ProID = ProID;
                review.Rating = rating;
                review.Comment = comment.Trim();

                ctx.Reviews.Add(review);

                product.AvgRating = ((product.AvgRating * product.NumOfReviews) + rating) / (product.NumOfReviews + 1);
                product.NumOfReviews = product.NumOfReviews + 1;

                ctx.SaveChanges();

                return true;
            }catch
            {
                return false;
            }

        }

    }
}