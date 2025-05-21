using System;
using System.Windows.Forms;

namespace App_Senha
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ProcessMonitor.IniciarMonitoramento(); // Inicia bloqueio automático

            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FormPrincipal());
            }
            else
            {
                MessageBox.Show("Acesso negado!");
            }
        }
    }
}