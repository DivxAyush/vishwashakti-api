using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vishwashakti.Models;
using System.Data.SqlClient;

namespace vishwashakti.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IConfiguration _configuration;

       
        public ReviewController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API is working!");
        }

        [HttpPost]
        public IActionResult PostReview([FromBody] ReviewModel review)
        {
            string connstr = _configuration.GetConnectionString("VishwaShaktiConstr");
            using (SqlConnection con = new SqlConnection(connstr))
            {
                using (SqlCommand cmd = new SqlCommand("PostReview", con)) {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    /*cmd.Parameters.AddWithValue("@PersonId", review.PersonId);*/
                    cmd.Parameters.AddWithValue("@PersonName", review.PersonName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Rating", review.Rating);
                    cmd.Parameters.AddWithValue("@ReviewDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Comment", review.Comment);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            var response = new
            {
                status = "success",
                message = "Data saved successfully"
            };

            return Ok(response);
        }


        [HttpGet("summary")]
        public IActionResult GetReviewSummary()
        {
            string connStr = _configuration.GetConnectionString("VishwaShaktiConstr");
            var summary = new ReviewSummary
            {
                Users = new List<ReviewUserDetail>()
            };

            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("GetReviews", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // First result set: summary
                        if (reader.Read())
                        {
                            summary.TotalReviews = Convert.ToInt32(reader["TotalReviews"]);
                            summary.OverallRating = Convert.ToInt32(reader["OverallRating"]);
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                summary.Users.Add(new ReviewUserDetail
                                {
                                    PersonName = reader["PersonName"].ToString(),
                                    Rating = Convert.ToInt32(reader["Rating"]),
                                    Comment = reader["Comment"].ToString(),
                                    ReviewDate = Convert.ToDateTime(reader["ReviewDate"])
                                });
                            }
                        }
                    }
                }
            }

            return Ok(new
            {
                status = "success",
                data = summary
            });
        }


    }
}
