using Microsoft.AspNetCore.Mvc;
using GameDB.Models;

namespace GameDB.Controllers
{
    public class ReviewPageController : Controller
    {
        private readonly ReviewAPIController _reviewApi;
        private readonly GameAPIController _gameApi;

        public ReviewPageController(ReviewAPIController reviewApi, GameAPIController gameApi)
        {
            _reviewApi = reviewApi;
            _gameApi = gameApi;
        }

        // GET: /ReviewPage/List -> Shows a list of all reviews
        [HttpGet]
        public IActionResult List()
        {
            List<Review> reviews = _reviewApi.ListReviews();
            return View(reviews);
        }

        // GET: /ReviewPage/Show/{id} -> Shows details of a specific review
        [HttpGet]
        public IActionResult Show(int id)
        {
            Review selectedReview = _reviewApi.FindReview(id);
            return View(selectedReview);
        }

        // GET: /ReviewPage/New?gameId={gameId} -> Returns a form to add a new review
        [HttpGet]
        public IActionResult New(int? gameId)
        {
            ViewBag.Games = _gameApi.ListGames();
            ViewBag.SelectedGameId = gameId;
            return View();
        }

        // POST: /ReviewPage/Create -> Adds a new review to the database
        [HttpPost]
        public IActionResult Create(int GameId, int Rating, string Headline, string Text)
        {
            Review newReview = new Review()
            {
                GameId = GameId,
                Date = DateTime.Now,
                Rating = Rating,
                Headline = Headline,
                Text = Text
            };

            _reviewApi.AddReview(newReview);
            return RedirectToAction("List");
        }

        // GET: /ReviewPage/Edit/{id} -> Returns a form to edit a review
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Review selectedReview = _reviewApi.FindReview(id);
            ViewBag.Games = _gameApi.ListGames();
            return View(selectedReview);
        }

        // POST: /ReviewPage/Update/{id} -> Updates a review in the database
        [HttpPost]
        public IActionResult Update(int id, int GameId, int Rating, string Headline, string Text)
        {
            Review updatedReview = new Review()
            {
                GameId = GameId,
                Date = DateTime.Now,
                Rating = Rating,
                Headline = Headline,
                Text = Text
            };

            _reviewApi.UpdateReview(id, updatedReview);
            return RedirectToAction("List");
        }

        // GET: /ReviewPage/DeleteConfirm/{id} -> Shows confirmation page for deleting a review
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Review selectedReview = _reviewApi.FindReview(id);
            return View(selectedReview);
        }

        // POST: /ReviewPage/Delete/{id} -> Deletes a review from the database
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _reviewApi.DeleteReview(id);
            return RedirectToAction("List");
        }
    }
}