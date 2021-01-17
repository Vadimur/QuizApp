namespace QuizApp.Entities
{
    public class Account
    {
        public readonly int Id;
        public readonly string Username;
        public readonly Role Role;

        public Account(int id, string username,  Role role)
        {
            Id = id;
            Username = username;
            Role = role;
        }

    }
}