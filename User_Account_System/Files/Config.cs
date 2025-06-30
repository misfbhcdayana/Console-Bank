using DotNetEnv;

namespace User_Account_System.Files
{
    public static class Config
    {
        static Config()
        {
            Env.Load();
        }
        public static string EncryptionKey => Env.GetString("Encryption_Key");
        public static string AdminKey => Env.GetString("Admin_Key");
    }
}
