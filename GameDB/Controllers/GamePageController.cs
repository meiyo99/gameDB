using Microsoft.AspNetCore.Mvc;
using GameDB.Models;

namespace GameDB.Controllers
{
    public class GamePageController : Controller
    {
        private readonly GameAPIController _gameApi;

        public GamePageController(GameAPIController gameApi)
        {
            _gameApi = gameApi;
        }

        // GET: /GamePage/List -> Shows a list of all games
        [HttpGet]
        public IActionResult List()
        {
            List<Game> games = _gameApi.ListGames();
            return View(games);
        }

        // GET: /GamePage/Show/{id} -> Shows details of a specific game
        [HttpGet]
        public IActionResult Show(int id)
        {
            Game selectedGame = _gameApi.FindGame(id);
            return View(selectedGame);
        }

        // GET: /GamePage/New -> Returns a form to add a new game
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        // POST: /GamePage/Create -> Adds a new game to the database
        [HttpPost]
        public IActionResult Create(string Title, DateTime ReleaseDate, string Description, string Developer)
        {
            Game newGame = new Game()
            {
                Title = Title,
                ReleaseDate = ReleaseDate,
                Description = Description,
                Developer = Developer
            };

            _gameApi.AddGame(newGame);
            return RedirectToAction("List");
        }

        // GET: /GamePage/Edit/{id} -> Returns a form to edit a game
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Game selectedGame = _gameApi.FindGame(id);
            return View(selectedGame);
        }

        // POST: /GamePage/Update/{id} -> Updates a game in the database
        [HttpPost]
        public IActionResult Update(int id, string Title, DateTime ReleaseDate, string Description, string Developer)
        {
            Game updatedGame = new Game()
            {
                Title = Title,
                ReleaseDate = ReleaseDate,
                Description = Description,
                Developer = Developer
            };

            _gameApi.UpdateGame(id, updatedGame);
            return RedirectToAction("List");
        }

        // GET: /GamePage/DeleteConfirm/{id} -> Shows confirmation page for deleting a game
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Game selectedGame = _gameApi.FindGame(id);
            return View(selectedGame);
        }

        // POST: /GamePage/Delete/{id} -> Deletes a game from the database
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _gameApi.DeleteGame(id);
            return RedirectToAction("List");
        }
    }
}