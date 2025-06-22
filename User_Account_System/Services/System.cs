using System.Collections.Generic;
using System.Linq;
using User_Account_System.Models;


namespace User_Account_System.Services
{
    public class System
    {
        private List<User> Users = new List<User>();
        private readonly string DataFile = "users.txt";

        public System()
        {
            LoadUsers();
        }//System

        public bool CreateUser(string username, string password)
        {
            if (Users.Any(u => u.Username.Equals(username)))
            {
                return false;
            }
            Users.Add(new User(username, password));
            SaveUsers();
            return true;
        }//CreateUsers
        public User Login(string username, string password)
        {
            return Users.FirstOrDefault(u => u.Username.Equals(username) && u.Password == password);
        }//Login

        public List<User> GetAllUsers()
        {
            return Users;
        }//GetAllUsers
        public void LoadUsers()
        {
            var lines = FileHandler.ReadFromFile(DataFile);
            foreach(var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length==3 && decimal.TryParse(parts[2], out decimal balance))
                {
                    Users.Add(new User(parts[0], parts[1], balance));
                }
            }
        }//LoadUsers

        public void SaveUsers()
        {
            var lines = Users.Select(u => $"{u.Username}|{u.Password}|{u.Balance}");
            FileHandler.WriteToFile(DataFile, lines);
        }//SaveUsers

        public bool Transfer(string fromUsername, string toUsername, decimal amt)
        {
            var sender = Users.FirstOrDefault(u => u.Username == fromUsername);
            var receiver = Users.FirstOrDefault(u => u.Username == toUsername);
            if (sender == null || receiver == null || sender.Balance < amt)
                return false;
            sender.Balance -= amt;
            receiver.Balance += amt;
            SaveUsers();
            return true;
        }
    }//class System
}//namespace
