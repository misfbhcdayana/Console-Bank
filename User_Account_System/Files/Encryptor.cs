using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_Account_System.Files
{
    public static class Encryptor
    {
        static readonly string EncryptionKey = "Aq7$Rz9&LdX@tM2p#Wv3!NhK0eFb^Cu";
        private const int Shift = 7;
        public static string En_crypt(string text)
        {
            int KeyLength = EncryptionKey.Length;
            string encrypted_text = "";
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                char shifted = (char)(c + Shift);
                char flipped = (char)(255 - shifted);
                char xorValue = EncryptionKey[i % KeyLength];
                char xorred = (char)(flipped ^ xorValue);
                encrypted_text += xorred;
            }
            return encrypted_text;
        }
    }
}
