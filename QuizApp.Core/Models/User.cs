namespace QuizApp.Core.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Record { get; set; }
        public List<Quiz> CreatedQuizzes { get; set; } = new();
    }
}
