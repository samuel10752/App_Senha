using System;
using System.Diagnostics;

namespace App_Senha
{
    public static class PermissoesWindows
    {
        /// <summary>
        /// Concede permissão total (FullControl) para todos os usuários usando PowerShell.
        /// </summary>
        /// <param name="caminhoPrograma">Caminho completo do programa (arquivo) a ter as permissões ajustadas.</param>
        public static void ConcederPermissaoTotalViaPowerShell(string caminhoPrograma)
        {
            if (string.IsNullOrWhiteSpace(caminhoPrograma))
            {
                Console.WriteLine("Erro: Caminho do programa inválido.");
                return;
            }

            // 🔄 Restaurar permissões para os padrões do Windows
            ProcessStartInfo resetAcl = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c icacls \"{caminhoPrograma}\" /reset",
                Verb = "runas",
                UseShellExecute = true
            };

            Process processoResetAcl = Process.Start(resetAcl);
            processoResetAcl.WaitForExit();

            Console.WriteLine("Permissões restauradas com sucesso! Agora todos os usuários podem acessar o arquivo normalmente.");
        }


        public static void BloquearPrograma(string caminhoPrograma)
        {
            if (string.IsNullOrEmpty(caminhoPrograma))
            {
                Console.WriteLine("Erro: Caminho do programa inválido.");
                return;
            }

            // Bloqueia acesso ao programa via NTFS usando icacls.
            ProcessStartInfo icacls = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c icacls \"{caminhoPrograma}\" /deny Todos:F",
                Verb = "runas",
                UseShellExecute = true
            };

            Process.Start(icacls);
            Console.WriteLine("Programa bloqueado!");
        }
    }
}