namespace QuizApp.Models
{
    public class Statistics<T> where T : class
    {
        public readonly T Item;
        public readonly int TotalAnswers;
        public readonly int CorrectAnswers;
        public readonly int AverageElapsedSeconds;

        public Statistics(T item, int totalAnswers, int correctAnswers, int averageElapsedSeconds)
        {
            Item = item;
            TotalAnswers = totalAnswers;
            CorrectAnswers = correctAnswers;
            AverageElapsedSeconds = averageElapsedSeconds;
        }
    }
}