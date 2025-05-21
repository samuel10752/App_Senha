using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class GerenciadorSenha
{
    private static string arquivoConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");

    public static void SalvarSenha(string senha)
    {
        string senhaCriptografada = EncryptSenha(senha);
        File.WriteAllText(arquivoConfig, senhaCriptografada);
    }

    public static string CarregarSenha()
    {
        if (File.Exists(arquivoConfig))
        {
            string senhaCriptografada = File.ReadAllText(arquivoConfig);
            return DecryptSenha(senhaCriptografada);
        }
        return null;
    }

    private static string EncryptSenha(string senha)
    {
        byte[] data = Encoding.UTF8.GetBytes(senha);
        byte[] chave = Encoding.UTF8.GetBytes("1234567890123456");

        using (Aes aes = Aes.Create())
        {
            aes.Key = chave;
            aes.IV = chave;
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] resultado = encryptor.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(resultado); // Codificação Base64 após criptografia
        }
    }

    private static string DecryptSenha(string senhaCriptografada)
    {
        try
        {
            byte[] data = Convert.FromBase64String(senhaCriptografada);
            byte[] chave = Encoding.UTF8.GetBytes("1234567890123456");

            using (Aes aes = Aes.Create())
            {
                aes.Key = chave;
                aes.IV = chave;
                ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] resultado = decryptor.TransformFinalBlock(data, 0, data.Length);

                return Encoding.UTF8.GetString(resultado);
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Erro: A senha criptografada não está em um formato válido de Base64.");
            return null;
        }
    }
}