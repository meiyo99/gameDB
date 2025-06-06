namespace GameDB.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int GameId { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        public string Headline { get; set; }
        public string Text { get; set; }
        public string GameTitle { get; set; }
    }
}