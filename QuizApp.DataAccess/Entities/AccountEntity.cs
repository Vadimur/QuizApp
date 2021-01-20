namespace QuizApp.DataAccess.Entities
{
    public class AccountEntity
    {
        public int Id { get; set; }
        public string Username  { get; set; }
        public int PasswordHash  { get; set; }
        public RoleEntity Role  { get; set; }

        public AccountEntity(int id, string username, int passwordHash, RoleEntity role)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }

        public AccountEntity()
        {
            
        }
    }
}