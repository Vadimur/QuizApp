namespace QuizApp.DataAccess.Entities
{
    public class AccountEntity
    {
        public int Id { get; set; }
        public string Username  { get; set; }
        public int PasswordHash  { get; set; }
        public RoleEntity Role  { get; set; }

        public AccountEntity(int id, string username, string password, RoleEntity role)
        {
            Id = id;
            Username = username;
            PasswordHash = password.GetHashCode();
            Role = role;
        }

        public AccountEntity()
        {
            
        }
        
        public bool IsPasswordCorrect(string password)
        {
            return password.GetHashCode() == PasswordHash;
        }
    }
}