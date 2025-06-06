﻿namespace GameDB.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public string Developer { get; set; }
    }
}