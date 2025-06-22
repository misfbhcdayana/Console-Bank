using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_Account_System.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public User() { }
        //balance is optional in the method all
        //if ommitted, it is automatically set to zero
        public User(string username, string password, decimal balance = 0)
        {
            Username = username;
            Password = password;
            Balance = balance;
        }
        //Displays the users
        //if omitted, 'then User_Account_System.Models' is displayed instead of the user details
        //when Console.WriteLine(user) is called
        public override string ToString()
        {
            return $"Username: {Username}, Balance: R{Balance}";
        }

    }
}