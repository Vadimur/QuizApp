namespace QuizApp.Entities
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Username  { get; set; }
        public int PasswordHash  { get; set; }
        public Role Role  { get; set; }

        public AccountModel(int id, string username, string password, Role role)
        {
            Id = id;
            Username = username;
            PasswordHash = password.GetHashCode();
            Role = role;
        }

        public AccountModel()
        {
            
        }
        
        public bool IsPasswordCorrect(string password)
        {
            return password.GetHashCode() == PasswordHash;
        }
    }
}