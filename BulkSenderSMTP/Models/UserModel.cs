namespace BulkSenderSMTP.Models
{
    public class UserModel
    {
        public string UserName { get; }
        public string UserLogin { get; }
        public string Password { get; }

        public UserModel(string name, string login, string pass) 
        { 
            UserName = name;
            UserLogin = login;
            Password = pass;
        }
    }
}
