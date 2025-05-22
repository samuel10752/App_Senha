using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class GerenciadorSenha
{
    // Arquivo de configuração que armazenará também outras opções, se necessário.
    private static string arquivoConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");

    // Lê o arquivo de configuração e retorna um dicionário com os pares chave=valor.
    private static Dictionary<string, string> LerConfiguracoes()
    {
        var configs = new Dictionary<string, string>();
        if (File.Exists(arquivoConfig))
        {
            string[] linhas = File.ReadAllLines(arquivoConfig);
            foreach (string linha in linhas)
            {
                int index = linha.IndexOf('=');
                if (index > 0 && index < linha.Length - 1)
                {
                    string chave = linha.Substring(0, index);
                    string valor = linha.Substring(index + 1);
                    configs[chave] = valor;
                }
            }
        }
        return configs;
    }

    // Salva as configurações (pares chave=valor) de volta ao arquivo.
    private static void SalvarConfiguracoes(Dictionary<string, string> configs)
    {
        var linhas = new List<string>();
        foreach (var kvp in configs)
        {
            linhas.Add($"{kvp.Key}={kvp.Value}");
        }
        File.WriteAllLines(arquivoConfig, linhas);
    }

    // Salva a senha do programa, criptografada, usando a chave "SenhaPrograma".
    public static void SalvarSenhaPrograma(string senha)
    {
        var configs = LerConfiguracoes();
        configs["SenhaPrograma"] = EncryptSenha(senha);
        SalvarConfiguracoes(configs);
    }

    // Carrega e descriptografa a senha do programa.
    public static string CarregarSenhaPrograma()
    {
        var configs = LerConfiguracoes();
        if (configs.ContainsKey("SenhaPrograma"))
        {
            return DecryptSenha(configs["SenhaPrograma"]);
        }
        return null;
    }

    // Salva a senha da pasta, criptografada, usando a chave "SenhaPasta".
    public static void SalvarSenhaPasta(string senha)
    {
        var configs = LerConfiguracoes();
        configs["SenhaPasta"] = EncryptSenha(senha);
        SalvarConfiguracoes(configs);
    }

    // Carrega e descriptografa a senha da pasta.
    public static string CarregarSenhaPasta()
    {
        var configs = LerConfiguracoes();
        if (configs.ContainsKey("SenhaPasta"))
        {
            return DecryptSenha(configs["SenhaPasta"]);
        }
        return null;
    }

    // Método interno para criptografar a senha, utilizando AES.
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

            return Convert.ToBase64String(resultado);
        }
    }

    // Método interno para descriptografar uma senha que esteja em formato Base64.
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