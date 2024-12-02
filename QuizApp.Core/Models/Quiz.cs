namespace QuizApp.Core.Models
{
    public class Quiz
    {
        public string Title { get; set; }
        public string CreatorUsername { get; set; }
        public List<Question> Questions { get; set; } = new();
    }
}
