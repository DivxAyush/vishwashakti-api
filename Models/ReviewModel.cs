namespace vishwashakti.Models
{
    public class ReviewModel
    {
       
        public string PersonName { get; set; }
        public int Rating { get; set; }
       
        public string Comment { get; set; }
    }

    public class ReviewUserDetail
    {
        public string PersonName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class ReviewSummary
    {
        public int TotalReviews { get; set; }
        public int OverallRating { get; set; }
        public List<ReviewUserDetail> Users { get; set; }
    }

}
