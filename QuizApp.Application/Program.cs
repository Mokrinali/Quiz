using QuizApp.Core.Models;
using QuizApp.Core.Services;
using QuizApp.Data.Repositories;

class Program
{
    static void Main(string[] args)
    {
        var userRepo = new JsonRepository<User>("C:\\Users\\kikia\\OneDrive\\Desktop\\Quiz 02.12.2024\\QuizApp\\QuizApp.Application\\users.json.json");
        var quizRepo = new JsonRepository<Quiz>("C:\\Users\\kikia\\OneDrive\\Desktop\\Quiz 02.12.2024\\QuizApp\\QuizApp.Application\\quizzes.json.json");

        var users = userRepo.LoadData();
        var quizzes = quizRepo.LoadData();

        var userService = new UserService(users);
        var quizService = new QuizService(quizzes);

        while (true)
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Leaderboard");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");
            var choice = Console.ReadLine();

            if (choice == "4") break;

            switch (choice)
            {
                case "1":
                    Console.Write("Enter username: ");
                    var username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    var password = Console.ReadLine();

                    var newUser = userService.Register(username, password);
                    if (newUser == null)
                    {
                        Console.WriteLine("Returning to Main Menu...");
                        break;
                    }

                    userRepo.SaveData(users);
                    Console.WriteLine("Registration successful!");
                    break;

                case "2":
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();

                    try
                    {
                        var loggedInUser = userService.Login(username, password);
                        Console.WriteLine($"Welcome, {loggedInUser.Username}!");

                        while (true)
                        {
                            Console.WriteLine("\nUser Menu:");
                            Console.WriteLine("1. Create Quiz");
                            Console.WriteLine("2. Solve Quiz");
                            Console.WriteLine("3. Delete Quiz");
                            Console.WriteLine("4. Logout");
                            Console.Write("Choose an option: ");

                            var userChoice = Console.ReadLine();
                            if (userChoice == "4") break;

                            switch (userChoice)
                            {
                                case "1":
                                    var quiz = new Quiz();
                                    Console.Write("Enter quiz title: ");
                                    quiz.Title = Console.ReadLine();

                                    for (int i = 0; i < 5; i++)
                                    {
                                        var question = new Question();
                                        Console.Write($"Question {i + 1}: ");
                                        question.Text = Console.ReadLine();

                                        for (int j = 0; j < 4; j++)
                                        {
                                            var answer = new Answer();
                                            Console.Write($"Answer {j + 1}: ");
                                            answer.Text = Console.ReadLine();
                                            Console.Write("Is this correct (yes/no)? ");
                                            answer.IsCorrect = Console.ReadLine()?.ToLower() == "yes";
                                            question.Answers.Add(answer);
                                        }
                                        quiz.Questions.Add(question);
                                    }

                                    quizService.CreateQuiz(loggedInUser, quiz);
                                    quizRepo.SaveData(quizzes);
                                    Console.WriteLine("Quiz created successfully!");
                                    break;

                                case "2":
                                    var availableQuizzes = quizService.GetAvailableQuizzes(loggedInUser);
                                    if (!availableQuizzes.Any())
                                    {
                                        Console.WriteLine("No quizzes available.");
                                        break;
                                    }

                                    Console.WriteLine("Available Quizzes:");
                                    foreach (var q in availableQuizzes)
                                    {
                                        Console.WriteLine($"- {q.Title} by {q.CreatorUsername}");
                                    }

                                    Console.Write("Enter quiz title: ");
                                    var quizTitle = Console.ReadLine();
                                    var quizToSolve = availableQuizzes.FirstOrDefault(q => q.Title == quizTitle);

                                    if (quizToSolve != null)
                                        quizService.SolveQuiz(loggedInUser, quizToSolve);
                                    else
                                        Console.WriteLine("Quiz not found.");
                                    break;

                                case "3":
                                    Console.Write("Enter quiz title to delete: ");
                                    var quizToDelete = Console.ReadLine();
                                    quizService.DeleteQuiz(loggedInUser, quizToDelete);
                                    quizRepo.SaveData(quizzes);
                                    break;

                                default:
                                    Console.WriteLine("Invalid option.");
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "3":
                    Console.WriteLine("Leaderboard:");
                    foreach (var user in users.OrderByDescending(u => u.Record).Take(10))
                    {
                        Console.WriteLine($"{user.Username}: {user.Record} points");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}
