using System;
using System.Collections.Generic;
using User_Account_System.Models;

namespace User_Account_System.Services
{
    public class App
    {
        private readonly BankingSystem _bank;
        private const string AdminPassword = "open1234";

        //new instance of Sysytem() 
        public App()
        {
            _bank = new BankingSystem();
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
            if (_bank.CreateUser(username, password))
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
            User user = _bank.Login(username, password);
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
                    "\n5. View Transaction History" +
                    "\n6. Delete account" +
                    "\n7. Logout" +
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
                            //save the transaction
                            FileHandler.LogTransaction(user, $"Deposited R{D_amt}");
                            //update the text file
                            _bank.SaveUsers();
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
                                _bank.SaveUsers();
                                //save the transaction
                                FileHandler.LogTransaction(user, $"Withdrawn R{W_amt}");
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
                            //prompt the user to confirm the transfer
                            Console.WriteLine("\n==Confirm Transfer==");
                            Console.WriteLine($"Sender: {user.Username}");
                            Console.WriteLine($"Recipient: {receiver}");
                            Console.WriteLine($"Amount: R{amount}");
                            Console.Write("Do you want to proceed (1.Yes / 2.No) ? ");
                            string confirm = Console.ReadLine().Trim().ToLower();
                            //if they agree to send
                            if (confirm == "1")
                            {
                                //if system.Transfer returns true
                                if (_bank.Transfer(user.Username, receiver, amount))
                                {
                                    Console.Write($"\nSuccessfully transferred R{amount} to {receiver}.");

                                }
                                else //if the transfer didn't go through
                                {
                                    Console.WriteLine("\nTransaction not successful.");
                                    Console.WriteLine("Insufficient funds or invalid receiver username.");
                                }
                            }
                            else//if they cancelled the transaction
                            {
                                Console.WriteLine("\nTransaction Cancelled.");
                            }
                        }
                        else //if the amount entered was invalid.
                        {
                            Console.WriteLine("Invalid Amount.");
                        }
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    case "4":
                        //view balance
                        Console.WriteLine($"Your balance: R{user.Balance}");
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    case "5"://transaction history
                        //List containing transaction history
                        List<string> history = FileHandler.ReadTransactions(user);
                        Console.WriteLine("\n=== Transaction history ===\n");
                        //display
                        if (history.Count == 0)
                        {
                            Console.WriteLine("No transactions found.");
                        }
                        else
                        {
                            foreach (string line in history)
                            {
                                Console.WriteLine(line);
                            }
                        }
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    case "6"://delete account
                        Console.WriteLine("To Delete Your Account,");
                        Console.Write("Enter your password to confirm: ");
                        string pass = Console.ReadLine();
                        //if the password matches
                        if (pass == user.Password)
                        {
                            //delete the account
                            _bank.DeleteAccount(user);
                            Console.WriteLine("Account Deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid password. Action not completed");
                        }
                        return;
                    case "7"://logout
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
                List<User> users = _bank.GetAllUsers();
                if (users.Count == 0)
                {
                    Console.WriteLine("No Users found");
                }
                else
                {
                    Console.WriteLine("\n=== All Users ===");
                    foreach (User user in users)
                    {
                        Console.WriteLine(user);
                    }
                }

            }
        }//AdminView

    }
}
