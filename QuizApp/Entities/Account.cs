namespace QuizApp.Entities
{
    public class Account
    {
        public readonly int Id;
        public readonly string Username;
        public readonly int PasswordHash;
        public readonly Role Role;

        public Account(int id, string username, int passwordHash,  Role role)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }

    }
}