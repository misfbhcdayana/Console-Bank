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
        public User(string username, string password, decimal balance = 0)
        {
            Username = username;
            Password = password;
            Balance = balance;
        }

        public override string ToString()
        {
            return $"Username: {Username}, Balance: R{Balance}";
        }

        public string DisplayWithPassword()
        {
            return $"Username: {Username}, Password: {Password}, Balance: R{Balance}";
        }
    }
}
