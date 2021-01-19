using System;
using System.Collections.Generic;
using System.Linq;
using QuizApp.Exceptions;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Platforms
{
    public class QuizEditingPlatform : BasePlatform
    {
        private readonly Quiz _currentQuiz;
        private readonly QuizEditingService _quizEditingService;
        
        public QuizEditingPlatform(Quiz currentQuiz)
        {
            _currentQuiz = currentQuiz ?? throw new ArgumentNullException(nameof(currentQuiz), "Quiz must be not null");
            _quizEditingService = new QuizEditingService();
        }
        private void AddQuestion()
        {
            Console.Write("Enter question: ");
            string question = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(question))
            {
                Console.WriteLine("You cannot create empty question");
                return;
            }
            
            Console.WriteLine("Now, enter the answer options. Each row must contain separate option.\nSend empty row when you are done");
            int optionId = 1;
            List<string> options = new List<string>();
            do
            {
                Console.Write($"Answer option #{optionId}: ");
                string answerOption = Console.ReadLine();
                if (string.IsNullOrEmpty(answerOption))
                    break;
                
                options.Add(answerOption);
                optionId++;

            } while (true);

            if (options.Count < 2)
            {
                Console.WriteLine("Unable to add a new question because less than 2 answer options were entered.");
                return;
            }

            Console.WriteLine($"Question: \"{question}\"");
            Console.WriteLine("Options");
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"#{i+1}. {options[i]}"); 
            }
            
            Console.WriteLine("Enter id of the correct answer option.\nOr type 'exit' to return to the menu");
            string input;
            int correctOptionId;
            do
            {
                Console.Write("Enter id of the correct answer option: ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                
                if (input.Trim().ToLower().Equals("exit"))
                {
                    return;
                }
            } while (!int.TryParse(input, out correctOptionId) || correctOptionId < 1 || correctOptionId >= optionId);

            try
            {
                bool isSuccess = _quizEditingService.AddQuestion(_currentQuiz, question, options, correctOptionId);
                if (isSuccess)
                {
                    Console.WriteLine($"A new question successfully added to the {_currentQuiz}");
                }
                else
                {
                    Console.WriteLine($"Unable to add a new question to the {_currentQuiz}\n" +
                                      $"");
                }

            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
            }
            
        }

        private void DeleteQuestion()
        {
            Console.WriteLine(_currentQuiz);
            if (_currentQuiz.Questions == null || _currentQuiz.Questions.Count == 0 )
            {
                Console.WriteLine("There are no questions added to this quiz.");
                return;
            }
            Console.WriteLine("Choose question number you want to delete from quiz.\nOr type 'exit' to return to the menu");
            _currentQuiz.Questions.ForEach(Console.WriteLine);
            int maxId = _currentQuiz.Questions.Max(q => q.Id);
            string input;
            int questionId;
            do
            {
                Console.Write("Enter the question number: ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                
                if (input.Trim().ToLower().Equals("exit"))
                {
                    return;
                }
            } while (!int.TryParse(input, out questionId) || questionId < 1 || questionId > maxId);

            Console.WriteLine($"Are you sure you want delete question #{questionId}? ");
            Console.Write("y/[n]: ");
            string answer = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(answer) || !answer.ToLower().Equals("y")) return;
            
            try
            {
                bool isSuccess = _quizEditingService.DeleteQuestion(_currentQuiz, questionId);
                if (isSuccess)
                {
                    Console.WriteLine($"Question #{questionId} was successfully deleted!");
                }
                else
                {
                    Console.WriteLine($"Question #{questionId} wasn't deleted.\n" +
                                      "Question with this id doesn't exist.");
                }
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void ShowAllQuestions()
        {
            Console.WriteLine(_currentQuiz);
            if (_currentQuiz.Questions == null || _currentQuiz.Questions.Count == 0 )
            {
                Console.WriteLine("There are no questions added to this quiz.");
                return;
            }
            _currentQuiz.Questions.ForEach(Console.WriteLine);
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
                    AddQuestion();
                    break;
                case 2:
                    DeleteQuestion();
                    break;
                case 3:
                    ShowAllQuestions();
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
            Console.WriteLine("1. Add question");
            Console.WriteLine("2. Delete question");
            Console.WriteLine("3. Show all questions");
            Console.WriteLine("0. Exit");
        }
    }
}