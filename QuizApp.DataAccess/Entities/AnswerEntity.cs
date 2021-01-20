namespace QuizApp.DataAccess.Entities
{
    public class AnswerEntity
    {
        public int ParticipantId { get; set; }
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int SecondsElapsed { get; set; }
    }
}