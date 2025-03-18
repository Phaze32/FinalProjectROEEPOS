using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EncryptionClass
/// </summary>
public class EncryptionClass
{
    public string Encrypt(string text, int shift)
    {
        string result = "";
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char offset = char.IsUpper(c) ? 'A' : 'a';
                result += (char)(((c + shift - offset) % 26) + offset);
            }
            else
            {
                result += c;
            }
        }
        return result;
    }

    public string Decrypt(string text, int shift)
    {
        return Encrypt(text, -shift);
    }
}
/* Usage Example Through Code 
 class Program
{
    static void Main()
    {
        EncryptionClass encryption = new EncryptionClass();

        string text = "A Quick Brown Fox Jumps over the Lazy Dog";
        int shift = 3;

        string encryptedText = encryption.Encrypt(text, shift);
        string decryptedText = encryption.Decrypt(encryptedText, shift);

        Console.WriteLine($"Original Text: {text}");
        Console.WriteLine($"Encrypted Text: {encryptedText}");
        Console.WriteLine($"Decrypted Text: {decryptedText}");
    }
}
*/