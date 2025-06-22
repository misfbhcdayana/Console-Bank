using System;
using User_Account_System.Models;

namespace User_Account_System.Services
{
    public class App
    {
        private readonly System system;
        private const string AdminPassword = "open1234";

        //new instance of Sysytem() 
        public App()
        {
            system = new System();
        }

        public void Run()
        {
            //state of the app runnung
            bool running = true;
            while (running)
            {
                Console.Clear();
                //Bank app menu
                Console.WriteLine("=== Welcome to the Console Banking App ===\n");
                Console.WriteLine("1. Create new user" +
                    "\n2. Access the system " +
                    "\n3. View All Users (Admin) " +
                    "\n4. Exit");
                Console.Write("\nEnter an option: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        NewUser();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        AdminView();
                        break;
                    case "4":
                        //set running to false once 'exit' is selected
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
        private void NewUser()
        {
            //take in prefered username and password
            Console.Write("Enter a new username: ");
            string username = Console.ReadLine();
            Console.Write("Enter a password: ");
            string password = Console.ReadLine();
            //is system.CreateUser returns true, then the user doesn't exit
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
            //take username and password
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            //user is of type 'User'
            var user = system.Login(username, password);
            if (user == null) //if username not found
            {
                Console.WriteLine("Invalid login details.");
            }
            else
            {
                //if the username and password match, run the app under the user's details
                RunBankingMenu(user);
            }
        }

        private void RunBankingMenu(User user)
        {
            //user active state
            bool active = true;
            while (active)
            {
                Console.Clear();
                //app menu
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
                        //check for valid amount
                        if (decimal.TryParse(Console.ReadLine(), out decimal D_amt) && D_amt > 0)
                        {
                            user.Balance += D_amt;
                            //update the text file
                            system.SaveUsers();
                            //confirm deposit
                            Console.WriteLine($"Successfully deposited: R{D_amt}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Amount.");
                        }
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.Write("Amount to withdraw: R");
                        if (decimal.TryParse(Console.ReadLine(), out decimal W_amt) && W_amt > 0)
                        {
                            if (W_amt <= user.Balance)
                            {
                                user.Balance -= W_amt;
                                //update the text file
                                system.SaveUsers();
                                //confirm withdrawal
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
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    case "3":
                        //transfer funds
                        Console.Write("Enter the account holder's username: ");
                        string receiver = Console.ReadLine();
                        Console.Write("Enter the amount to send: R");
                        //check for valid amount
                        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                        {
                            //if system.Transfer returns true
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
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    case "4":
                        //view balance
                        Console.WriteLine($"Your balance: {user.Balance}");
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    case "5":
                        active = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        break;  
                }//switch case
            }//while loop
        }//RunBanking menu
        private void AdminView()
        {
            Console.Write("Enter the admin password: ");
            string adminpassword = Console.ReadLine();
            //check the admin password
            if (adminpassword != AdminPassword)
            {
                Console.WriteLine("Incorrect admin password.");
                return;
            }
            else
            {
                //users is a list of type 'User'
                var users = system.GetAllUsers();
                if (users.Count == 0)
                {
                    Console.WriteLine("No Users found");
                }
                else
                {
                    Console.WriteLine("\n=== All Users ===");
                    foreach (var user in users)
                    {
                        Console.WriteLine(user);
                    }
                }

            }
        }//AdminView

    }
}
