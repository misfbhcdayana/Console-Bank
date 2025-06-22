using System.Collections.Generic;
using System.IO;

namespace User_Account_System.Services
{
    public static class FileHandler
    {
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
