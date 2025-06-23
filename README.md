## Console-Bank
This is a simple yet functional **console-based banking simulator** written in C#. It uses a **class-based design**, supports **persistent data storage** using text files(substitute for a database), and allows for **basic banking operations** like deposits, withdrawals, and transfers.

### Features
- Create and manage user accounts
- Secure access by authenticating passwords 
- Deposit, Withdraw and Check balance
- Transfer funds between users
- Admin view for all user data
- Persistent data storage using file system (users.txt)

### How to run
#### Required
- .NET FrameWork(4.7.2+)
- VS19 or newer
#### Run
1. Clone the repo:<br>
```bash
    git clone https://github.com/misfbhcdayana/Console-Bank.git
```
2. Open the `.sln` file in VS
3. Build and Run

### Note
- For admin access, use password `open1234`
- All user data is saved in `users.txt` and each line contains `username|password|balance`

### To Do
- Transaction history
- Delete user acount
- Email-like notification (console simulated)
- Secure password hashing

### Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you'd like to change of improve.
<p align="center">Made with ❤️ and C#</p>
