using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealer
{
    class Encryption
    {
        ushort encryptionKey = 0x00AA;
        public string StartEncryption(string originalText)
        {
            char[] temp = new char[originalText.Length];
            for (int i = 0; i < originalText.Length; i++)
            {
                temp[i] = (char)(originalText[i] ^ encryptionKey);
            }
            originalText = new string(temp);

            return originalText;
        }
    }
}
