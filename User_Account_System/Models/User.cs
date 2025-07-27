
namespace User_Account_System.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public string Key { get; set; }
        public User() { }
        public User(string username, string password, decimal balance = 0, string key = "------")
        {
            Username = username;
            Password = password;
            Balance = balance;
            Key = key;
        }
        //Displays the users
        //if omitted, 'then User_Account_System.Models' is displayed instead of the user details
        //when Console.WriteLine(user) is called
        public override string ToString()
        {
            string text = (Key == "------") ? "No" : "Yes";
            return $"Username: {Username}, Balance: R{Balance}, Admin Rights: {text}";
        }

    }
}