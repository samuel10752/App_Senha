using Microsoft.Win32;

public static class RegistroSenha
{
    private const string ChaveRegistro = @"Software\AppSenha";

    public static void SalvarSenha(string senha)
    {
        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(ChaveRegistro))
        {
            key.SetValue("Senha", senha);
        }
    }

    public static string CarregarSenha()
    {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ChaveRegistro))
        {
            return key?.GetValue("Senha")?.ToString();
        }
    }
}