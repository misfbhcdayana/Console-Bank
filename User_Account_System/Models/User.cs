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
        public int? Key { get; set; }
        public User() { }
        //balance is optional in the method all
        //if ommitted, it is automatically set to zero
        public User(string username, string password, decimal balance = 0, int? key = null)
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
            string text = (Key == null) ? "No" : "Yes";
            return $"Username: {Username}, Balance: R{Balance}, Admin Rights: {text}";
        }

    }
}