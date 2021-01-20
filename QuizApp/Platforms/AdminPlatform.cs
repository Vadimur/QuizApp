using System;
using System.Collections.Generic;
using System.Linq;
using QuizApp.Exceptions;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Platforms
{
    public class AdminPlatform : BasePlatform
    {
        private readonly Account _currentUser;
        private readonly QuizManagingService _quizManagingService;
        private readonly AuthorizationService _authenticationService;
        private readonly StatisticsService _statisticsService;
        
        public AdminPlatform(Account user)
        {
            if (user == null || user.Role != Role.Admin)
            {
                throw new PlatformAccessViolationException("Access only with admin rights");
            }
            _currentUser = user;
            _quizManagingService = new QuizManagingService();
            _authenticationService  = new AuthorizationService();
            _statisticsService = new StatisticsService();
        }

        private void CreateQuiz()
        {
            Console.Write("Quiz name: ");
            string name = Console.ReadLine();
            Console.Write("Quiz category: ");
            string category = Console.ReadLine();
            try
            {
                bool isSuccess = _quizManagingService.CreateQuiz(_currentUser.Id, name, category);
                if (isSuccess)
                {
                    Console.WriteLine($"Quiz {name} was successfully created!");
                }
                else
                {
                    Console.WriteLine($"Quiz {name} wasn't created. Quiz with the same name already exists.");
                }
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        
        private void ShowQuizzes()
        {
            Console.WriteLine("Your quizzes");
            try
            {
                var quizzes = _quizManagingService.GetQuizzesCreatedByCurrentUser(_currentUser.Id)?.ToList();
                if (quizzes == null || quizzes.Count == 0)
                {
                    Console.WriteLine("There are no created quizzes.");
                    return;
                }
                quizzes.ForEach(Console.WriteLine);
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        
        private void WorkWithQuizDetails()
        {
            List<Quiz> quizzes;
            try
            {
                quizzes = _quizManagingService.GetQuizzesCreatedByCurrentUser(_currentUser.Id)?.ToList();
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            if (quizzes == null || quizzes.Count == 0)
            {
                Console.WriteLine("There are no created quizzes.");
                return;
            }
            Console.WriteLine("Choose the number of quiz which you want to edit or watch its details.\nOr type 'exit' to return to the menu");
            quizzes.ForEach(Console.WriteLine);

            int[] availableIds = quizzes.Select(q => q.Id + 1).ToArray();
            string input;
            int id;
            do
            {
                Console.Write("Enter the quiz number: ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                
                if (input.Trim().ToLower().Equals("exit"))
                {
                    return;
                }
            } while (!int.TryParse(input, out id) || !availableIds.Contains(id));

            Quiz quiz;
            try
            {
                quiz = _quizManagingService.FindQuiz(id - 1);
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }

            Console.WriteLine($"\nHere you can edit or watch details of selected quiz\n{quiz}");
            new QuizEditingPlatform(quiz).Start();
            
        }

        private void DeleteQuiz()
        {
            List<Quiz> quizzes;
            try
            {
                quizzes = _quizManagingService.GetQuizzesCreatedByCurrentUser(_currentUser.Id)?.ToList();
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            
            if (quizzes == null || quizzes.Count == 0)
            {
                Console.WriteLine("There are no created quizzes.");
                return;
            }
            Console.WriteLine("Choose quiz number you want to delete.\nOr type 'exit' to return to the menu");
            quizzes.ForEach(Console.WriteLine);
            int[] availableIds = quizzes.Select(q => q.Id + 1).ToArray();
            string input;
            int id;
            do
            {
                Console.Write("Enter the quiz number: ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                
                if (input.Trim().ToLower().Equals("exit"))
                {
                    return;
                }
            } while (!int.TryParse(input, out id) || !availableIds.Contains(id));

            Console.WriteLine($"Are you sure you want delete quiz #{id}? ");
            Console.Write("y/[n]: ");
            string answer = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(answer) || !answer.ToLower().Equals("y")) return;
            
            try
            {
                bool isSuccess = _quizManagingService.DeleteQuiz(id - 1);
                if (isSuccess)
                {
                    Console.WriteLine($"Quiz #{id} was successfully deleted!");
                }
                else
                {
                    Console.WriteLine($"Quiz #{id} wasn't deleted.\n" +
                                      "Quiz with this id doesn't exist or it has other owner.");
                }
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
            }
            
        }
        
        private void WatchQuizStats()
        {
            List<Quiz> quizzes;
            try
            {
                quizzes = _quizManagingService.GetQuizzesCreatedByCurrentUser(_currentUser.Id)?.ToList();
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            
            if (quizzes == null || quizzes.Count == 0)
            {
                Console.WriteLine("There are no created quizzes.");
                return;
            }
            Console.WriteLine("Choose quiz number to watch its statistics.\nOr type 'exit' to return to the menu");
            quizzes.ForEach(Console.WriteLine);
            int[] availableIds = quizzes.Select(q => q.Id + 1).ToArray();
            string input;
            int quizId;
            do
            {
                Console.Write("Enter the quiz number: ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                
                if (input.Trim().ToLower().Equals("exit"))
                {
                    return;
                }
            } while (!int.TryParse(input, out quizId) || !availableIds.Contains(quizId));

            IEnumerable<Statistics<Question>> stats;
            try
            {
                stats = _statisticsService.GetQuestionStatistics(quizId - 1);
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            if (stats == null || !stats.Any())
            {
                Console.WriteLine("There are no answers for this quiz"); 
                return;
            }
            Console.WriteLine($"You chose quiz #{quizId}");
            
            foreach (var questionStat in stats)
            {
                double correctAnswersPercent = 0;
                if (questionStat.TotalAnswers != 0)
                {
                    correctAnswersPercent = Math.Round( (double) questionStat.CorrectAnswers / questionStat.TotalAnswers * 100, 3);
                }
                Console.WriteLine(questionStat.Item);
                Console.WriteLine($"Total answers: {questionStat.TotalAnswers}");
                Console.WriteLine($"Correct answers: {questionStat.CorrectAnswers} | {correctAnswersPercent}%");
                Console.WriteLine($"Average amount of seconds spent on each question: {questionStat.AverageElapsedSeconds}");
                Console.WriteLine();
            }

            Statistics<Quiz> quizStats;
            try
            {
                quizStats = _statisticsService.GetQuizStatistics(quizId - 1);
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            
            if (quizStats == null)
            {
                Console.WriteLine("Quiz statistics is unavailable"); 
                return;
            }
            
            Console.WriteLine($"Quiz #{quizId} statistics");
            double totalCorrectAnswersPercent = 0;
            if (quizStats.TotalAnswers != 0)
            {
                totalCorrectAnswersPercent = Math.Round( (double) quizStats.CorrectAnswers / quizStats.TotalAnswers * 100, 3);
            }
            Console.WriteLine($"Total answers: {quizStats.TotalAnswers}");
            Console.WriteLine($"Correct answers: {quizStats.CorrectAnswers} | {totalCorrectAnswersPercent}%");
            Console.WriteLine($"Average amount of seconds spent on each question: {quizStats.AverageElapsedSeconds}");
            Console.WriteLine();
            
        }
        
        private void RegisterAdminAccount()
        {
            Console.WriteLine("Create new administrator account");
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = ReadPassword();
            try
            {
                bool isSuccess = _authenticationService.Register(username, password, Role.Admin);
                if (isSuccess)
                {
                    Console.WriteLine($"Administrator account {username} was successfully created!");
                }
                else
                {
                    Console.WriteLine($"Administrator account {username} wasn't created. This username already exists.");
                }
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        
        private string ReadPassword()
        {
            var password = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
        
        private void Exit()
        {
            KeepProgramActive = false;
        }

        protected override bool ChooseCommand(int commandNumber)
        {
            bool correctCommand = true;
            switch (commandNumber)
            {
                case 1:
                    CreateQuiz();
                    break;
                case 2:
                    ShowQuizzes();
                    break;
                case 3:
                    WorkWithQuizDetails();
                    break;
                case 4:
                    DeleteQuiz();
                    break;
                case 5:
                    WatchQuizStats();
                    break;
                case 6:
                    RegisterAdminAccount();
                    break;
                case 0:
                    Exit();
                    break;
                default:
                    correctCommand = false;
                    break;
            }

            if (correctCommand)
                Console.WriteLine();
            
            return correctCommand;
        }
        
        protected override void PrintUserMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. Create a new quiz");
            Console.WriteLine("2. Show quizzes");
            Console.WriteLine("3. Quiz details");
            Console.WriteLine("4. Delete a quiz");
            Console.WriteLine("5. Watch quiz statistics");
            Console.WriteLine("6. Register administrator account");
            Console.WriteLine("0. Exit");
        }
        
    }
}