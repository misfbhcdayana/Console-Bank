using System;
using System.Linq;
using System.Collections.Generic;
using User_Account_System.Models;
using User_Account_System.Files;


namespace User_Account_System.Services
{
    public class BankingSystem
    {
        //an list instance of the users class
        private List<User> Users = new List<User>();
        //text file named 'users' stored in Console-Bank\User_Account_System\bin\Debug
        private readonly string DataFile = "users.txt";

        public BankingSystem()
        {
            //Load the list of users
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
                //if an existing username matches, then it's not valid
                valid_acc = false;
            }
            //if the username and password lengths meet the length criteria, check for special characters, upper and Lowercase character 
            if (username.Length >= 6 && username.Length<=20 && password.Length >= 8 && password.Length <= 35)
            {
                foreach (char c in username)
                {
                    //if the character is not a letter or digit or an underscore, then it's a special character and the username is invalid.
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
                //if the account doesn't exist, and the username and password meet the criteria, create the account
                Users.Add(new User(username, password));
                SaveUsers();
            }
            return (valid_details, valid_acc);
        }//CreateUsers
        public User Login(string username, string password)
        {
            //return the elt whose username and passowrd match or return 'null' if none found
            return Users.FirstOrDefault(u => u.Username.Equals(username) && u.Password == password);
        }//Login

        public List<User> GetAllUsers()
        {
            //the list of all users and their balance
            return Users;
        }//GetAllUsers
        public void LoadUsers()
        {
            //Read all lines from the data file
            //'lines' is of type IEnumerable, which includes lines of text
            IEnumerable<string> lines = FileHandler.ReadFromFile(DataFile);
            foreach(string line in lines)
            {
                //parts is of type string[]
                string[] parts = line.Split('|');
                if (parts.Length==4 && decimal.TryParse(parts[2], out decimal balance))
                {
                    //populate the list with all existing users
                    Users.Add(new User(parts[0], parts[1], balance, Convert.ToInt32(parts[3])));
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
            //sender and receiver are of type 'User'
            User sender = Users.FirstOrDefault(u => u.Username == fromUsername);
            User receiver = Users.FirstOrDefault(u => u.Username == toUsername);
            //if the sender not found or receiver not found or sender has insufficient funds,
            //return false (transaction not succesful)
            if (sender == null || receiver == null || sender.Balance < amt)
                return false;
            //else, update the sender's balance
            sender.Balance -= amt;
            //log the transaction
            FileHandler.LogTransaction(sender, $"Transferred R{amt} to {receiver.Username}");
            //update the receiver's balance
            receiver.Balance += amt;
            //log the transaction
            FileHandler.LogTransaction(receiver, $"Received R{amt} from {sender.Username}");
            //update the text file
            SaveUsers();
            //(transaction succesful)
            return true;
        }//transfer
        public void DeleteAccount(User user)
        {
            //Remove the user from the list
            Users.Remove(user);
            //Update the text file
            SaveUsers();
            //Delete the transactions file
            FileHandler.DeleteTransactionFile(user);
        }//DeleteAccount
    }//class System
}//namespace
