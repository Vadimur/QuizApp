using System;
using QuizApp.Entities;

namespace QuizApp.Platforms
{
    public class AdminPlatform : BasePlatform
    {
        private readonly Account CurrentUser;

        public AdminPlatform(Account user)
        {
            CurrentUser = user;
        }

        private void CreateQuiz()
        {
            Console.WriteLine("Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Category:");
            string category = Console.ReadLine();
            
            

        }
        protected override bool ChooseCommand(int commandNumber)
        {
            throw new System.NotImplementedException();
        }
        
        protected override void PrintUserMenu()
        {
            Console.WriteLine("1. Create quiz");
            Console.WriteLine("2. Delete quiz");
            //Console.WriteLine("Edit quiz??");
            Console.WriteLine("3. Watch quiz statistics");
            Console.WriteLine("4. Create administrator account");
            Console.WriteLine("5. Exit");
        }

       
    }
}