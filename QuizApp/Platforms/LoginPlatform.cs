using System;
using QuizApp.Entities;
using QuizApp.Exceptions;
using QuizApp.Services;

namespace QuizApp.Platforms
{
    public class LoginPlatform : BasePlatform
    {
        private readonly AuthorizationService _authenticationService;
        public LoginPlatform()
        {
            _authenticationService  = new AuthorizationService();
        }
        
        protected override bool ChooseCommand(int commandNumber)
        {
            bool correctCommand = true;
            switch (commandNumber)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Register();
                    break;
                case 3:
                    Exit();
                    break;
                default:
                    correctCommand = false;
                    break;
            }
            return correctCommand;
        }

        
        private void Login()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = ReadPassword();
            Account account;
            try
            {
                account = _authenticationService.Authorize(username, password);
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }

            if (account == null)
            {
                Console.WriteLine("Incorrect username or password");
                return;
            }

            if (account.Role == Role.Participant)
                new ParticipantPlatform().Start();
            else if (account.Role == Role.Admin)
                new AdminPlatform(account).Start();
            else
                Console.WriteLine("This account is damaged. Sorry, but you can't use it");
        }

        private void Register()
        {
            Console.WriteLine("Create new account");
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = ReadPassword();
            try
            {
                bool isSuccess = _authenticationService.Register(username, password);
                if (isSuccess)
                {
                    Console.WriteLine($"Account {username} was successfully created!");
                }
                else
                {
                    Console.WriteLine($"Account {username} wasn't created. This username already exists.");
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
        
        protected override void PrintUserMenu()
        {
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
        }

    }
}