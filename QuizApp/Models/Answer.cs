namespace QuizApp.Models
{
    public class Answer
    {
        public int ParticipantId { get; set; }
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string Time { get; set; } //TODO DateTime.ToString();
    }
}