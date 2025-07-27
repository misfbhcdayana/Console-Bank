using System;
using System.Collections.Generic;
using System.IO;
using User_Account_System.Models;

namespace User_Account_System.Files
{
    public static class FileHandler
    {
        public static void LogTransaction(User user, string details)
        {
            string filePath = $@"Transactions\transactions_{user.Username}.txt";
            //details
            string newEntry = $"{DateTime.Now}: {details}";
            List<string> ExistingLines = ReadFromFile(filePath);
            ExistingLines.Insert(0, newEntry);
            WriteToFile(filePath, ExistingLines);
        }
        public static void DeleteTransactionFile(User user)
        {
            string filePath = $@"Transactions\transactions_{user.Username}.txt";
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        public static List<string> ReadTransactions(User user)
        {
            string filePath = $@"Transactions\transactions_{user.Username}.txt";
            return ReadFromFile(filePath);
        }
        public static List<string> ReadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<string>();
            List<string> FileContent = new List<string>();
            IEnumerable<string> Lines = File.ReadAllLines(filePath);
            foreach (string line in Lines)
                FileContent.Add(Encryptor.De_crypt(line));
            return FileContent;
        }
        public static void WriteToFile(string filePath, IEnumerable<string> Lines)
        {
            List<string> AddingLines = new List<string>();
            foreach (string line in Lines)
                AddingLines.Add(Encryptor.En_crypt(line));
            File.WriteAllLines(filePath, AddingLines);
        }
    }
}
