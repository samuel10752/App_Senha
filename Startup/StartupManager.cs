using System;
using Microsoft.Win32;
using System.Windows.Forms;

namespace App_Senha
{
    public class StartupManager
    {
        private const string RegistroPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string NomePrograma = "MeuPrograma";

        public static void AdicionarInicioAutomatico()
        {
            try
            {
                string caminhoExecutavel = Application.ExecutablePath;
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistroPath, true))
                {
                    key.SetValue(NomePrograma, caminhoExecutavel);
                }
                MessageBox.Show("Programa registrado para iniciar com o Windows.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao configurar inicialização automática: {ex.Message}");
            }
        }

        public static void RemoverInicioAutomatico()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistroPath, true))
                {
                    key.DeleteValue(NomePrograma, false);
                }
                MessageBox.Show("Programa removido da inicialização automática.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao remover da inicialização automática: {ex.Message}");
            }
        }
    }
}