using System;
using System.Linq;
using System.Collections.Generic;
using User_Account_System.Models;
using User_Account_System.Files;


namespace User_Account_System.Services
{
    public class BankingSystem
    {
        private List<User> Users = new List<User>();
        private readonly string DataFile = "users.txt";

        public BankingSystem()
        {
            LoadUsers();
        }//System

        public (bool, bool) CreateUser(string username, string password)
        {
            //the username and password meet the validation criteria
            bool valid_details = true;
            //the account doesn't exist yet
            bool valid_acc = true;
            if (Users.Any(u => u.Username.Equals(username)))
            {
                valid_acc = false;
            }
            if (username.Length >= 6 && username.Length<=20 && password.Length >= 8 && password.Length <= 35)
            {
                foreach (char c in username)
                {
                    if (!char.IsLetterOrDigit(c) && c != '_')
                        valid_details = false;
                }
                int SpecialCharCount = 0;
                int UpperCharCount = 0;
                int LowerCharCount = 0;
                int DigitCount = 0;
                foreach (char c in password)
                {
                    if (char.IsDigit(c))
                        DigitCount++;
                    if (char.IsUpper(c))
                        UpperCharCount++;
                    if (char.IsLower(c))
                        LowerCharCount++;
                    if (!char.IsLetterOrDigit(c)) //if it's not a letter or digit, then it's a special character
                        SpecialCharCount++;
                }
                //if there is not atleast one of each type of character, valid_details=false
                if (SpecialCharCount == 0 || UpperCharCount == 0 || LowerCharCount == 0 || DigitCount == 0)
                    valid_details = false;
            }
            else //if the username and password lengths do not meet the length criteria, automaticatically disqualify them
            {
                valid_details = false;
            }
            if (valid_details && valid_acc)
            {
                Users.Add(new User(username, password));
                SaveUsers();
            }
            return (valid_details, valid_acc);
        }//CreateUsers
        public User Login(string username, string password)
        {
            //return 'null' if none found
            return Users.FirstOrDefault(u => u.Username.Equals(username) && u.Password == password);
        }//Login

        public List<User> GetAllUsers()
        {
            return Users;
        }//GetAllUsers
        public void LoadUsers()
        {
            IEnumerable<string> lines = FileHandler.ReadFromFile(DataFile);
            foreach(string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length==4 && decimal.TryParse(parts[2], out decimal balance))
                {
                    Users.Add(new User(parts[0], parts[1], balance, parts[3]));
                }
            }
        }//LoadUsers

        public void SaveUsers()
        {
            IEnumerable<string> lines = Users.Select(u => $"{u.Username}|{u.Password}|{u.Balance}|{u.Key}");
            FileHandler.WriteToFile(DataFile, lines);
        }//SaveUsers

        public bool Transfer(string fromUsername, string toUsername, decimal amt)
        {
            User sender = Users.FirstOrDefault(u => u.Username == fromUsername);
            User receiver = Users.FirstOrDefault(u => u.Username == toUsername);
            if (sender == null || receiver == null || sender.Balance < amt)
                return false;
            sender.Balance -= amt;
            FileHandler.LogTransaction(sender, $"Transferred R{amt} to {receiver.Username}");
            receiver.Balance += amt;
            FileHandler.LogTransaction(receiver, $"Received R{amt} from {sender.Username}");
            SaveUsers();
            return true;
        }//transfer
        public void DeleteAccount(User user)
        {
            Users.Remove(user);
            SaveUsers();
            FileHandler.DeleteTransactionFile(user);
        }//DeleteAccount
    }//class System
}//namespace
