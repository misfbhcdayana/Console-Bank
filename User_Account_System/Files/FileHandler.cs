using System;
using System.Collections.Generic;
using System.IO;
using User_Account_System.Models;

namespace User_Account_System.Services
{
    public static class FileHandler
    {
        public static void LogTransaction(User user, string details)
        {
            // user transactions file
            string filePath = $@"Transactions\transactions_{user.Username}.txt";
            //details
            string newEntry = $"{DateTime.Now}: {details}";
            //list of existing transactions
            List<string> ExistingLines = ReadFromFile(filePath);
            //add most recent transaction at the top
            ExistingLines.Insert(0, newEntry);
            //save all transaction to the file
            WriteToFile(filePath, ExistingLines);
        }
        public static void DeleteTransactionFile(User user)
        {
            // user transactions file
            string filePath = $@"Transactions\transactions_{user.Username}.txt";
            //delete the file if it exists
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        public static List<string> ReadTransactions(User user)
        {
            //display all transactions
            string filePath = $@"Transactions\transactions_{user.Username}.txt";
            return ReadFromFile(filePath);
        }
        public static List<string> ReadFromFile(string filePath)
        {
            //if the file doesn't exit, return an empty file
            if (!File.Exists(filePath))
                return new List<string>();
            //else, return a string list of all lines of the file
            return new List<string>(File.ReadAllLines(filePath));
        }
        public static void WriteToFile(string filePath, IEnumerable<string> lines)
        {
            //write all lines of text to the file
            File.WriteAllLines(filePath, lines);
        }
    }
}
