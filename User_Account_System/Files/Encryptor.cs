using System;
using System.Collections.Generic;
using System.Text;

namespace User_Account_System.Files
{
    public static class Encryptor
    {
        //32 byte long key
        static readonly string EncryptionKey = Config.EncryptionKey;
        private const int Shift = 7;
        public static string En_crypt(string _text)
        {
            int KeyLength = EncryptionKey.Length;
            List<byte> _encrypted_text_list = new List<byte>();

            for (int i = 0; i < _text.Length; i++)
            {
                char c = _text[i];
                char shifted = (char)(c + Shift);
                char flipped = (char)(255 - shifted);
                char xorValue = EncryptionKey[i % KeyLength];
                char xorred = (char)(flipped ^ xorValue);
                _encrypted_text_list.Add((byte)xorred);
            }
            //to avoid characters like \n, \r, \t or even \0
            return Convert.ToBase64String(_encrypted_text_list.ToArray());
        }
        public static string De_crypt(string _text)
        {
            int KeyLength = EncryptionKey.Length;
            List<byte> _decrypted_text = new List<byte>();
            byte[] retrieved = Convert.FromBase64String(_text);
            for (int i = 0; i < retrieved.Length; i++)
            {
                char c = (char)retrieved[i];
                char xorValue = EncryptionKey[i % KeyLength];
                char xorred = (char)(c ^ xorValue);
                char flipped = (char)(255 - xorred);
                char shifted = (char)(flipped - Shift);
                _decrypted_text.Add((byte)shifted);
            }
            //convert to readable string
            return Encoding.UTF8.GetString(_decrypted_text.ToArray());
        }
    }
}
