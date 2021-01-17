using System;

namespace QuizApp.Platforms
{
    public abstract  class BasePlatform
    {
        protected bool KeepProgramActive = true;
        
        public void Start()
        {
            do
            {
                Console.WriteLine();
                PrintUserMenu();
                Console.Write("Enter command number: ");
                string userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Empty input. Try again\n");
                    continue;
                }

                if (int.TryParse(userInput, out int commandId) == false)
                {
                    Console.WriteLine("Unknown command. Try again\n");
                    continue;
                }

                bool isCommandValid = ChooseCommand(commandId);
                if (isCommandValid == false)
                {
                    Console.WriteLine("Unknown command. Try again\n");
                }
                
            } while (KeepProgramActive);  
        }

        protected abstract void PrintUserMenu();
        protected abstract bool ChooseCommand(int commandNumber);

    }
}