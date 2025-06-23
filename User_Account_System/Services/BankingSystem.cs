using System.Collections.Generic;
using System.Linq;
using User_Account_System.Models;


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

        public bool CreateUser(string username, string password)
        {
            //if for any element present in the list 'Users', say u, if u.Username == username,
            //return false (account not created)
            if (Users.Any(u => u.Username.Equals(username)))
            {
                return false;
            }
            //else, add the user (account) and save the Users
            Users.Add(new User(username, password));
            SaveUsers();
            //return true (account created)
            return true;
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
                if (parts.Length==3 && decimal.TryParse(parts[2], out decimal balance))
                {
                    //populate the list with all existing users
                    Users.Add(new User(parts[0], parts[1], balance));
                }
            }
        }//LoadUsers

        public void SaveUsers()
        {
            IEnumerable<string> lines = Users.Select(u => $"{u.Username}|{u.Password}|{u.Balance}");
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
            FileHandler.LogTransaction(sender, $"Transfered R{amt} to {receiver.Username}");
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
