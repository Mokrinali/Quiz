using QuizApp.Core.Models;

namespace QuizApp.Core.Services
{
    public class QuizService
    {
        private readonly List<Quiz> _quizzes;

        public QuizService(List<Quiz> quizzes)
        {
            _quizzes = quizzes;
        }

        public void CreateQuiz(User user, Quiz quiz)
        {
            quiz.CreatorUsername = user.Username;
            _quizzes.Add(quiz);
            user.CreatedQuizzes.Add(quiz);
        }

        public void DeleteQuiz(User user, string quizTitle)
        {
            var quizToDelete = _quizzes.FirstOrDefault(q => q.Title == quizTitle && q.CreatorUsername == user.Username);
            if (quizToDelete != null)
            {
                _quizzes.Remove(quizToDelete);
                user.CreatedQuizzes.Remove(quizToDelete);
                Console.WriteLine("Quiz deleted successfully.");
            }
            else
            {
                Console.WriteLine("You can only delete quizzes you created.");
            }
        }

        public List<Quiz> GetAvailableQuizzes(User user)
        {
            return _quizzes.Where(q => q.CreatorUsername != user.Username).ToList();
        }

        public void SolveQuiz(User user, Quiz quiz)
        {
            if (quiz.CreatorUsername == user.Username)
            {
                Console.WriteLine("You cannot solve your own quiz.");
                return;
            }

            Console.WriteLine($"Starting quiz: {quiz.Title}");
            var score = 0;
            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            foreach (var question in quiz.Questions)
            {
                if (timer.Elapsed.TotalMinutes >= 2)
                {
                    Console.WriteLine("Time's up! You failed the quiz.");
                    return;
                }

                Console.WriteLine(question.Text);
                for (int i = 0; i < question.Answers.Count; i++)
                    Console.WriteLine($"{i + 1}. {question.Answers[i].Text}");

                if (int.TryParse(Console.ReadLine(), out int choice) &&
                    choice > 0 && choice <= question.Answers.Count &&
                    question.Answers[choice - 1].IsCorrect)
                {
                    score += 20;
                }
                else
                {
                    score -= 20;
                }
            }

            if (score > user.Record)
            {
                user.Record = score;
            }

            Console.WriteLine($"Quiz complete! Your score: {score}");
        }
    }
}
