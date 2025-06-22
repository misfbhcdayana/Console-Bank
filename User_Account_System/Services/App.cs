using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User_Account_System.Models;

namespace User_Account_System.Services
{
    public class App
    {
        private readonly System system;
        private const string AdminPassword = "open1234";

        public App()
        {
            system = new System();
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== Welcoming to the Console Banking App ===\n");
                Console.WriteLine("1. Create new user" +
                    "\n2. Access the system " +
                    "\n3. View All Users (Admin) " +
                    "\n4. Exit");
                Console.Write("\nEnter an option: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        CreateUser();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        AdminView();
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Exiting the system...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }//Run
        private void CreateUser()
        {
            Console.Write("Enter a new username: ");
            string username = Console.ReadLine();
            Console.Write("Enter a password: ");
            string password = Console.ReadLine();
            if (system.CreateUser(username, password))
            {
                Console.WriteLine(" Account created successfully.");
            }
            else
            {
                Console.WriteLine("Username already exists");
            }
        }//create user

        private void Login()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            var user = system.Login(username, password);
            if (user == null)
            {
                Console.WriteLine("Invalid login details.");
            }
            else
            {
                Console.WriteLine($"Welcome {user.Username}");
                RunBankingMenu(user);
            }
        }

        private void RunBankingMenu(User user)
        {
            bool active = true;
            while (active)
            {
                Console.Clear();
                Console.WriteLine($"Logged in as: {user.Username}");
                Console.Write("1. Deposit" +
                    "\n2. Withdraw" +
                    "\n3. Transfer Funds" +
                    "\n4. Check balance" +
                    "\n5. Logout" +
                    "\nChoose an option: ");
                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        Console.Write("Amount to deposit: R");
                        if (decimal.TryParse(Console.ReadLine(), out decimal D_amt) && D_amt > 0)
                        {
                            user.Balance += D_amt;
                            system.SaveUsers();
                            Console.WriteLine($"Successfully deposited: {D_amt}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Amount.");
                        }
                        break;
                    case "2":
                        Console.Write("Amount to withdraw: R");
                        if (decimal.TryParse(Console.ReadLine(), out decimal W_amt) && W_amt > 0)
                        {
                            if (W_amt <= user.Balance)
                            {
                                user.Balance -= W_amt;
                                system.SaveUsers();
                                Console.WriteLine($"Successfully withdrawn: R{W_amt}");
                            }
                            else
                            {
                                Console.WriteLine("Insufficient funds.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Amount.");
                        }
                        break;
                    case "3":
                        Console.Write("Enter the account holder's username: ");
                        string receiver = Console.ReadLine();
                        Console.Write("Enter the amount to send: R");
                        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                        {
                            if (system.Transfer(user.Username, receiver, amount))
                            {
                                Console.Write($"Successfully transferred R{amount} to {receiver}.");
                            }
                            else
                            {
                                Console.WriteLine("Transaction not successful.");
                                Console.WriteLine("Insufficient funds or invalid receiver username.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Amount.");
                        }
                        break;
                    case "4":
                        Console.WriteLine($"Your balance: {user.Balance}");
                        break;
                    case "5":
                        active = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;  
                }//switch case
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }//while loop
        }//RunBanking menu
        private void AdminView()
        {
            Console.Write("Enter the admin password: ");
            string adminpassword = Console.ReadLine();
            if (adminpassword != AdminPassword)
            {
                Console.WriteLine("Incorrect admin password.");
                return;
            }
            else
            {
                var users = system.GetAllUsers();
                if (users.Count == 0)
                {
                    Console.WriteLine("No Users found");
                }
                else
                {
                    Console.WriteLine("=== All Users ===");
                    foreach (var user in users)
                    {
                        Console.WriteLine(user);
                    }
                }

            }
        }//AdminView

    }
}
