using System.Collections.Generic;
using System.IO;

namespace User_Account_System.Services
{
    public static class FileHandler
    {
        public static List<string> ReadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<string>();
            return new List<string>(File.ReadAllLines(filePath));
        }
        public static void WriteToFile(string filePath, IEnumerable<string> lines)
        {
            File.WriteAllLines(filePath, lines);
        }
    }
}
