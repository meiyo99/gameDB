using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameDB.Models;
using MySql.Data.MySqlClient;

namespace GameDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewAPIController : ControllerBase
    {
        /// <summary>
        /// Returns all reviews with game titles
        /// </summary>
        /// <returns>
        /// List of reviews
        /// </returns>
        /// <example>
        /// GET: api/ReviewAPI/ListReviews
        /// </example>
        [HttpGet]
        [Route("ListReviews")]
        public List<Review> ListReviews()
        {
            List<Review> reviews = new List<Review>();
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = @"
                    SELECT r.*, g.title as game_title 
                    FROM reviews r 
                    JOIN games g ON r.game_id = g.game_id 
                    ORDER BY r.date DESC";

                command.CommandText = query;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reviews.Add(new Review
                        {
                            ReviewId = Convert.ToInt32(reader["review_id"]),
                            GameId = Convert.ToInt32(reader["game_id"]),
                            Date = Convert.ToDateTime(reader["date"]),
                            Rating = Convert.ToInt32(reader["rating"]),
                            Headline = reader["headline"].ToString(),
                            Text = reader["text"].ToString(),
                            GameTitle = reader["game_title"].ToString()
                        });
                    }
                }
            }

            return reviews;
        }

        /// <summary>
        /// Finds a specific review by ID
        /// </summary>
        /// <param name="id">Review ID</param>
        /// <returns>
        /// Review object
        /// </returns>
        /// <example>
        /// GET: api/ReviewAPI/FindReview/{id}
        /// </example>
        [HttpGet]
        [Route("FindReview/{id}")]
        public Review FindReview(int id)
        {
            Review review = new Review();
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = @"
                    SELECT r.*, g.title as game_title 
                    FROM reviews r 
                    JOIN games g ON r.game_id = g.game_id 
                    WHERE r.review_id = @id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        review.ReviewId = Convert.ToInt32(reader["review_id"]);
                        review.GameId = Convert.ToInt32(reader["game_id"]);
                        review.Date = Convert.ToDateTime(reader["date"]);
                        review.Rating = Convert.ToInt32(reader["rating"]);
                        review.Headline = reader["headline"].ToString();
                        review.Text = reader["text"].ToString();
                        review.GameTitle = reader["game_title"].ToString();
                    }
                }
            }

            return review;
        }

        /// <summary>
        /// Adds a new review to the database
        /// </summary>
        /// <param name="newReview">Review object</param>
        /// <returns>
        /// Success message with review ID
        /// </returns>
        /// <example>
        /// POST: api/ReviewAPI/AddReview
        /// </example>
        [HttpPost]
        [Route("AddReview")]
        public string AddReview([FromBody] Review newReview)
        {
            if (string.IsNullOrEmpty(newReview.Headline) || newReview.Rating < 1 || newReview.Rating > 10)
            {
                return "Review headline is required and rating must be between 1-10.";
            }

            int reviewId = 0;
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = @"
                    INSERT INTO reviews (game_id, date, rating, headline, text, status) 
                    VALUES (@gameId, @date, @rating, @headline, @text, 'Approved')";

                command.CommandText = query;
                command.Parameters.AddWithValue("@gameId", newReview.GameId);
                command.Parameters.AddWithValue("@date", newReview.Date);
                command.Parameters.AddWithValue("@rating", newReview.Rating);
                command.Parameters.AddWithValue("@headline", newReview.Headline);
                command.Parameters.AddWithValue("@text", newReview.Text);

                command.ExecuteNonQuery();
                reviewId = Convert.ToInt32(command.LastInsertedId);
            }

            return $"Review added successfully with ID: {reviewId}";
        }

        /// <summary>
        /// Updates an existing review
        /// </summary>
        /// <param name="id">Review ID</param>
        /// <param name="updatedReview">Updated review object</param>
        /// <returns>
        /// Success message
        /// </returns>
        /// <example>
        /// PUT: api/ReviewAPI/UpdateReview/{id}
        /// </example>
        [HttpPut]
        [Route("UpdateReview/{id}")]
        public string UpdateReview(int id, [FromBody] Review updatedReview)
        {
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = @"
                    UPDATE reviews 
                    SET game_id = @gameId, date = @date, rating = @rating, 
                        headline = @headline, text = @text, status = 'Approved'
                    WHERE review_id = @id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@gameId", updatedReview.GameId);
                command.Parameters.AddWithValue("@date", updatedReview.Date);
                command.Parameters.AddWithValue("@rating", updatedReview.Rating);
                command.Parameters.AddWithValue("@headline", updatedReview.Headline);
                command.Parameters.AddWithValue("@text", updatedReview.Text);

                command.ExecuteNonQuery();
            }

            return "Review updated successfully!";
        }

        /// <summary>
        /// Deletes a review
        /// </summary>
        /// <param name="id">Review ID</param>
        /// <returns>
        /// Success message
        /// </returns>
        /// <example>
        /// DELETE: api/ReviewAPI/DeleteReview/{id}
        /// </example>
        [HttpDelete]
        [Route("DeleteReview/{id}")]
        public string DeleteReview(int id)
        {
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = "DELETE FROM reviews WHERE review_id = @id";
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }

            return "Review deleted successfully!";
        }
    }
}