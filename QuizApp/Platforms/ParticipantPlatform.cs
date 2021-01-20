using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using QuizApp.Exceptions;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Platforms
{
    public class ParticipantPlatform : BasePlatform
    {
        private readonly Account _currentUser;
        private readonly QuizManagingService _quizManagingService;
        private readonly ParticipantService _participantService;
        private readonly StatisticsService _statisticsService;
        
        public ParticipantPlatform(Account user)
        {
            if (user == null || user.Role != Role.Participant)
            {
                throw new PlatformAccessViolationException("Access only with participant rights");
            }
            _currentUser = user;
            _quizManagingService = new QuizManagingService();
            _participantService = new ParticipantService();
            _statisticsService = new StatisticsService();
        }
        
        private void ShowAvailableQuizzes()
        {
            Console.WriteLine("Available quizzes");
            try
            {
                var quizzes = _quizManagingService.GetAllQuizzes()?.ToList();
                if (quizzes == null || quizzes.Count == 0)
                {
                    Console.WriteLine("There are no available quizzes.");
                    return;
                }
                quizzes.ForEach(Console.WriteLine);
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void StartQuiz()
        {
            List<Quiz> quizzes;
            try
            {
                quizzes = _quizManagingService.GetAllQuizzes()?.ToList();
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            if (quizzes == null || quizzes.Count == 0)
            {
                Console.WriteLine("There are no available quizzes.");
                return;
            }
            Console.WriteLine("Choose the number of quiz which you want to take.\nOr type 'exit' to return to the menu");
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

            if (quiz == null || quiz.Questions == null || quiz.Questions.Count == 0)
            {
                Console.WriteLine("There are no questions in chosen quiz.");
                return;
            }
            else
            {
                TakeQuiz(quiz);
            }
        }

        private void TakeQuiz(Quiz quiz)
        {
            if (quiz == null || quiz.Questions == null || quiz.Questions.Count == 0)
            {
                Console.WriteLine("There are no questions in chosen quiz.");
                return;
            }

            List<Question> questions;
            try
            {
                questions = _participantService.GetQuestionsForParticipant(quiz, _currentUser.Id).ToList();
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }

            if (!questions.Any())
            {
                Console.WriteLine("Selected quiz doesn't contain any questions or you have already answered them.");
                return;
            }

            Console.WriteLine($"You chose quiz #{quiz.Id + 1}");
            TimeSpan totalTimeElapsed = TimeSpan.Zero;
            int correctAnswers = 0;
            int questionCounter = 1;
            foreach (var question in questions)
            {    
                Console.WriteLine($"Question {questionCounter}/{questions.Count}");
                Console.WriteLine(question);
                Console.WriteLine("You can type 'exit' to return to the menu");
                
                string input;
                int maxAnswerId = question.Options.Count;
                int chosenAnswerId;
                
                var watch = Stopwatch.StartNew();
                do
                {
                    Console.Write("Enter id of your answer: ");
                    input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                        continue;
                
                    if (input.Trim().ToLower().Equals("exit"))
                    {
                        return;
                    }
                } while (!int.TryParse(input, out chosenAnswerId) || chosenAnswerId < 1 || chosenAnswerId > maxAnswerId);

                watch.Stop();
                TimeSpan elapsedTime = watch.Elapsed;
                totalTimeElapsed += elapsedTime;

                if (chosenAnswerId - 1 == question.CorrectOptionIndex)
                {
                    Console.WriteLine("You answered correctly! ");
                    correctAnswers++;
                }
                else
                {
                    Console.WriteLine($"Incorrect answer. The right answer has id {question.CorrectOptionIndex + 1}");
                }
                
                try
                {
                    _participantService.SaveAnswer(_currentUser.Id, question, chosenAnswerId - 1, elapsedTime);
                }
                catch (ServiceException exception)
                {
                    Console.WriteLine(exception.Message);
                    return;
                }

                questionCounter++;
            }

            Console.WriteLine("Well done. You have finished the quiz.\n" +
                              $"Your result: {correctAnswers}/{questions.Count}\n" +
                              $"Time spent: {totalTimeElapsed.ToString()}");
        }
        
        private void ShowQuizStatistics()
        {
            List<Quiz> quizzes;
            try
            {
                quizzes = _quizManagingService.GetAllQuizzes()?.ToList();
            }
            catch (ServiceException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            if (quizzes == null || quizzes.Count == 0)
            {
                Console.WriteLine("There are no available quizzes.");
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

            
            Statistics<Quiz> quizStats;
            try
            {
                quizStats = _statisticsService.GetQuizStatisticsForParticipant(quizId - 1 , _currentUser.Id);
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

            Console.WriteLine($"Total questions: {quizStats.Item.Questions.Count}");
            Console.WriteLine($"Total answers: {quizStats.TotalAnswers}");
            Console.WriteLine($"Correct answers: {quizStats.CorrectAnswers} | {totalCorrectAnswersPercent}%");
            Console.WriteLine($"Average amount of seconds spent on each question: {quizStats.AverageElapsedSeconds}");
            Console.WriteLine();

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
                    ShowAvailableQuizzes();
                    break;
                case 2:
                    StartQuiz();
                    break;
                case 3:
                    ShowQuizStatistics();
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
            Console.WriteLine("1. Available quizzes");
            Console.WriteLine("2. Start quiz");
            Console.WriteLine("3. Quiz statistics");
            Console.WriteLine("0. Exit");
        }
    }
}