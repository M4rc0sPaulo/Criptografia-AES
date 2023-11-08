using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        string textoOriginal = "Textos legais. ";
        string chave = "Ajasklskeoessinmieosiusfoiweuwww"; // A chave deve ter 16, 24 ou 32 caracteres para AES-128, AES-192 ou AES-256, respectivamente


        // Criptografar
        byte[] textoCriptografado = CriptografarAES(textoOriginal, chave);

        // Descriptografar
        string textoDescriptografado = DescriptografarAES(textoCriptografado, chave);

        Console.WriteLine("Texto original: " + textoOriginal);
        Console.WriteLine("Texto criptografado (em Base64): " + Convert.ToBase64String(textoCriptografado));
        Console.WriteLine("Texto descriptografado: " + textoDescriptografado);
    }

    static byte[] CriptografarAES(string textoOriginal, string chave)
    {
        using (AesManaged aesAlg = new AesManaged())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(chave);
            aesAlg.Mode = CipherMode.CBC; // Use o modo CBC
            aesAlg.Padding = PaddingMode.PKCS7; 

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(textoOriginal);
                    }
                }

                return msEncrypt.ToArray();
            }
        }
    }

    static string DescriptografarAES(byte[] textoCriptografado, string chave)
    {
        using (AesManaged aesAlg = new AesManaged())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(chave);
            aesAlg.Mode = CipherMode.CBC; // Use o modo CBC
            aesAlg.Padding = PaddingMode.PKCS7; // Use PKCS7 padding

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(textoCriptografado))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
