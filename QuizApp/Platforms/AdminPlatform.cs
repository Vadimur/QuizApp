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


        public AdminPlatform(Account user)
        {
            if (user == null || user.Role != Role.Admin)
            {
                throw new PlatformAccessViolationException("Access only with admin rights");
            }
            _currentUser = user;
            _quizManagingService = new QuizManagingService();
            _authenticationService  = new AuthorizationService();
        }

        private void CreateQuiz()
        {
            Console.WriteLine("Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Category:");
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
            var quizzes = _quizManagingService.GetQuizzesCreatedByCurrentUser(_currentUser.Id)?.ToList();
            if (quizzes == null || quizzes.Count == 0)
            {
                Console.WriteLine("There are no created quizzes.");
                return;
            }
            quizzes.ForEach(Console.WriteLine);
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
            
            int maxId = quizzes.Max(q => q.Id);
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
            } while (!int.TryParse(input, out id) || id < 1 || id > maxId);

            Quiz quiz;
            try
            {
                quiz = _quizManagingService.FindQuiz(id);
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
            var quizzes = _quizManagingService.GetQuizzesCreatedByCurrentUser(_currentUser.Id)?.ToList();
            if (quizzes == null || quizzes.Count == 0)
            {
                Console.WriteLine("There are no created quizzes.");
                return;
            }
            Console.WriteLine("Choose quiz number you want to delete.\nOr type 'exit' to return to the menu");
            quizzes.ForEach(Console.WriteLine);
            int maxId = quizzes.Max(q => q.Id);
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
            } while (!int.TryParse(input, out id) || id < 1 || id > maxId);

            Console.WriteLine($"Are you sure you want delete quiz #{id}? ");
            Console.Write("y/[n]: ");
            string answer = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(answer) || !answer.ToLower().Equals("y")) return;
            
            try
            {
                bool isSuccess = _quizManagingService.DeleteQuiz(id);
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
        
        private void Exit()
        {
            KeepProgramActive = false;
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
            return correctCommand;
        }
        
        protected override void PrintUserMenu()
        {
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