using System;
using QuizApp.Platforms;

namespace QuizApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginPlatform loginPlatform = new LoginPlatform();
            loginPlatform.Start();
        }
    }
}
