using System;

namespace QuizApp.Platforms
{
    public class AdminPlatform : BasePlatform
    {
        protected override bool ChooseCommand(int commandNumber)
        {
            throw new System.NotImplementedException();
        }
        protected override void PrintUserMenu()
        {
            Console.WriteLine("1. Create quiz");
            Console.WriteLine("2. Delete quiz");
            Console.WriteLine("3. Watch quiz statistics");
        }

       
    }
}