using System;
using System.Linq;
using System.Collections.Generic;
using User_Account_System.Models;
using User_Account_System.Files;

namespace User_Account_System.Services
{
    public class App
    {
        private readonly BankingSystem _bank;
        private const int CurrentKey = 796852;

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
                Console.WriteLine("1. Create new account" +
                    "\n2. Login " +
                    "\n3. Exit");
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
                    //case "3":
                    //    AdminView();
                    //    break;
                    case "3":
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
            Console.WriteLine("\nUsername criteria:" +
                "\n\t- Must be atleast between 6 and 20 characters" + 
                "\n\t- Must not contain any special characters except an underscore (_)");
            Console.Write("\nEnter a new username: ");
            string username = Console.ReadLine().Trim();
            Console.WriteLine("\nPassword criteria:" +
                "\n\t- Must be between 8 and 35 characters" +
                "\n\t- Must contain atleast 1 uppercase letter" +
                "\n\t- Must contain atleast 1 lowercase letter" +
                "\n\t- Must contain at least 1 number");
            Console.Write("\nEnter a password: ");
            string password = Console.ReadLine();
            (bool Valid_Details, bool Valid_Acc) = _bank.CreateUser(username, password);
            //is system.CreateUser returns true, then the user doesn't exit
            if (Valid_Acc)
            {
                if (Valid_Details)
                {
                    Console.WriteLine("\n\tAccount created successfully.");
                }
                else
                {
                    Console.WriteLine("Username or Password do not meet the criteria.");
                }
                
            }
            else
            {
                Console.WriteLine("Username already exists!");
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
            if (user == null) //if username not found or password doesn't match
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
                string AdminOption = (user.Key == -11111) ? "Activate admin rights" : "Admin view";
                Console.Clear();
                //app menu
                Console.WriteLine($"Logged in as: {user.Username}");
                Console.Write("1. Deposit" +
                    "\n2. Withdraw" +
                    "\n3. Transfer Funds" +
                    "\n4. Check balance" +
                    "\n5. View Transaction History" +
                    "\n6. Delete account" +
                    "\n7. " + AdminOption +
                    "\n8. Logout" +
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
                    case "7":
                        if (user.Key == -11111)
                        {
                            //max tries to activate admin rights
                            int tries = 3;
                            while (tries > 0)
                            {
                                Console.Write("Enter the Admin key: ");
                                //if the key is valid, save the admin key to the user account and update the text file
                                if (int.TryParse(Console.ReadLine(), out int key) && key == CurrentKey)
                                {
                                    user.Key = key;
                                    _bank.SaveUsers();
                                    Console.WriteLine("Admin Rights Activated.");
                                    Console.ReadKey();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine($"You have {--tries} more tries.");
                                    if (tries == 0)
                                        Console.ReadKey();
                                }
                            }
                        }
                        else
                        {
                            AdminView(user);
                        }
                        break;
                    case "8"://logout
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
        private void AdminView(User user)
        {
            //running state
            bool AdminActive = true;
            while (AdminActive) 
            {
                Console.Clear();
                Console.WriteLine($"Logged in as: {user.Username}. \nState: Account & Admin rights active.\n");
                Console.WriteLine("1. View all accounts" +
                        "\n2. View a user's Transaction History " +
                        "\n3. Delete a user account" +
                        "\n4. Exit Admin View");
                Console.Write("\nEnter an option: ");
                string opt = Console.ReadLine().Trim();
                //copy of the list of users
                List<User> _Users = _bank.GetAllUsers();
                if (opt == "1")
                {
                    Console.WriteLine("\n=== All Users ===\n");
                    foreach (User u in _Users)
                    {
                        if (u == user) // highlight the active account
                            Console.WriteLine($"(On account) {u}");
                        else
                            Console.WriteLine(u);
                    }
                }
                else if (opt == "2")
                {
                    Console.Write("Enter the account holder's username: ");
                    string _readUsername = Console.ReadLine();
                    //get the user from the list
                    User _HandlingAcc = _Users.FirstOrDefault(u => u.Username == _readUsername);
                    if (_HandlingAcc == null)
                        Console.WriteLine("\nUser not found.");
                    else if (_HandlingAcc == user) //perform self actions out of admin view
                        Console.WriteLine("\nTo view your transactions, log out of Admin View");
                    else
                    {
                        Console.WriteLine($"\n=== Transaction history ({_HandlingAcc.Username}) === \n");
                        List<string> _HandlingAccHistory = FileHandler.ReadTransactions(_HandlingAcc);
                        //display
                        if (_HandlingAccHistory.Count == 0)
                        {
                            Console.WriteLine("No transactions found.");
                        }
                        else
                        {
                            foreach (string line in _HandlingAccHistory)
                            {
                                Console.WriteLine(line);
                            }
                        }
                    }
                }
                else if (opt == "3")
                {
                    Console.Write("Enter the account holder's username: ");
                    string _deleteUsername = Console.ReadLine();
                    User _delAcc = _Users.FirstOrDefault(u => u.Username == _deleteUsername);
                    if (_delAcc == null)
                        Console.WriteLine("\nUser not found.");
                    else if (_delAcc == user)
                        Console.WriteLine("\nTo delete your account, log out of Admin View");//perform self actions out of admin view
                    else
                    {
                        Console.WriteLine(_delAcc);
                        Console.WriteLine("Are you sure you want to delete this account (y/n) ? ");
                        string confirm = Console.ReadLine().Trim().ToLower();
                        if (confirm == "y")
                        {
                            _bank.DeleteAccount(_delAcc);
                            Console.WriteLine("\nAccount Deleted.");
                        }
                        else 
                        {
                            Console.WriteLine("\nAction cancelled.");
                        }
                    }
                }
                else if (opt == "4")
                {
                    AdminActive = false;
                }
                else
                {
                    Console.WriteLine("\nInvalid option.");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }//while lopp
        }//AdminView

    }
}
