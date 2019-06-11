
namespace Dealer
{
    class Decryption
    {
        ushort decryptionKey = 0x00AA;
        public string StartDecryption(string originalText)
        {
            char[] temp = new char[originalText.Length];
            for (int i = 0; i < originalText.Length; i++)
            {
                temp[i] = (char)(originalText[i] ^ decryptionKey);
            }
            originalText = new string(temp);

            return originalText;
        }
    }
}
