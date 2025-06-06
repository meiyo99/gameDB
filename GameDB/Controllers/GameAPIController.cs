using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameDB.Models;
using MySql.Data.MySqlClient;

namespace GameDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameAPIController : ControllerBase
    {
        /// <summary>
        /// Returns all games
        /// </summary>
        /// <returns>
        /// List of games
        /// </returns>
        /// <example>
        /// GET: api/GameAPI/ListGames
        /// </example>
        [HttpGet]
        [Route("ListGames")]
        public List<Game> ListGames()
        {
            List<Game> games = new List<Game>();
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = "SELECT * FROM games ORDER BY title";
                command.CommandText = query;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Game game = new Game()
                        {
                            GameId = Convert.ToInt32(reader["game_id"]),
                            Title = reader["title"].ToString(),
                            ReleaseDate = Convert.ToDateTime(reader["release_date"]),
                            Description = reader["description"].ToString(),
                            Developer = reader["developer"].ToString()
                        };

                        games.Add(game);
                    }
                }
            }

            return games;
        }

        /// <summary>
        /// Finds a specific game by ID
        /// </summary>
        /// <param name="id">Game ID</param>
        /// <returns>
        /// Game object
        /// </returns>
        /// <example>
        /// GET: api/GameAPI/FindGame/{id}
        /// </example>
        [HttpGet]
        [Route("FindGame/{id}")]
        public Game FindGame(int id)
        {
            Game game = new Game();
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = "SELECT * FROM games WHERE game_id = @id";
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        game.GameId = Convert.ToInt32(reader["game_id"]);
                        game.Title = reader["title"].ToString();
                        game.ReleaseDate = Convert.ToDateTime(reader["release_date"]);
                        game.Description = reader["description"].ToString();
                        game.Developer = reader["developer"].ToString();
                    }
                }
            }

            return game;
        }

        /// <summary>
        /// Adds a new game to the database
        /// </summary>
        /// <param name="newGame">Game object</param>
        /// <returns>
        /// Success message with game ID
        /// </returns>
        /// <example>
        /// POST: api/GameAPI/AddGame
        /// </example>
        [HttpPost]
        [Route("AddGame")]
        public string AddGame([FromBody] Game newGame)
        {
            if (string.IsNullOrEmpty(newGame.Title))
            {
                return "Game title is required.";
            }

            int gameId = 0;
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = @"
                    INSERT INTO games (title, release_date, description, developer, publisher) 
                    VALUES (@title, @releaseDate, @description, @developer, '')";

                command.CommandText = query;
                command.Parameters.AddWithValue("@title", newGame.Title);
                command.Parameters.AddWithValue("@releaseDate", newGame.ReleaseDate);
                command.Parameters.AddWithValue("@description", newGame.Description);
                command.Parameters.AddWithValue("@developer", newGame.Developer);

                command.ExecuteNonQuery();
                gameId = Convert.ToInt32(command.LastInsertedId);
            }

            return $"Game added successfully with ID: {gameId}";
        }

        /// <summary>
        /// Updates an existing game
        /// </summary>
        /// <param name="id">Game ID</param>
        /// <param name="updatedGame">Updated game object</param>
        /// <returns>
        /// Success message
        /// </returns>
        /// <example>
        /// PUT: api/GameAPI/UpdateGame/{id}
        /// </example>
        [HttpPut]
        [Route("UpdateGame/{id}")]
        public string UpdateGame(int id, [FromBody] Game updatedGame)
        {
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = @"
                    UPDATE games 
                    SET title = @title, release_date = @releaseDate, description = @description, 
                        developer = @developer, publisher = ''
                    WHERE game_id = @id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@title", updatedGame.Title);
                command.Parameters.AddWithValue("@releaseDate", updatedGame.ReleaseDate);
                command.Parameters.AddWithValue("@description", updatedGame.Description);
                command.Parameters.AddWithValue("@developer", updatedGame.Developer);

                command.ExecuteNonQuery();
            }

            return "Game updated successfully!";
        }

        /// <summary>
        /// Deletes a game and all associated reviews
        /// </summary>
        /// <param name="id">Game ID</param>
        /// <returns>
        /// Success message
        /// </returns>
        /// <example>
        /// DELERE: api/GameAPI/DeleteGame/{id}
        /// </example>
        [HttpDelete]
        [Route("DeleteGame/{id}")]
        public string DeleteGame(int id)
        {
            GameDBContext gameDB = new GameDBContext();

            using (MySqlConnection connection = gameDB.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                // The CASCADE DELETE in your schema will handle deleting reviews and game_platforms
                string query = "DELETE FROM games WHERE game_id = @id";
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }

            return "Game deleted successfully!";
        }
    }
}