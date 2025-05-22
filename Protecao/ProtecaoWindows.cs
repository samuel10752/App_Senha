using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace App_Senha
{
    public static class ProtecaoWindows
    {
        // Restaura as permissões do programa (libera o bloqueio) e, em seguida, remove a marca visual.
        public static void LiberarPrograma()
        {
            string programaSelecionado = CarregarProgramaBloqueado();

            if (!string.IsNullOrEmpty(programaSelecionado))
            {
                MessageBox.Show($"Liberando: {programaSelecionado}");

                // Primeiro, restaura as permissões para conceder controle total
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    // O comando abaixo concede controle total ao grupo "Todos" com a flag /inheritance:r para aplicar a herança
                    Arguments = $"/c icacls \"{programaSelecionado}\" /grant Todos:F /inheritance:r",
                    Verb = "runas",
                    UseShellExecute = true
                };

                Process.Start(psi);

                // Em seguida, reseta as permissões para os padrões do Windows
                ProcessStartInfo resetPerms = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c icacls \"{programaSelecionado}\" /reset",
                    Verb = "runas",
                    UseShellExecute = true
                };

                Process.Start(resetPerms);

                MessageBox.Show("Programa desbloqueado com sucesso! Agora ele pode ser aberto normalmente.");
            }
        }

        public static void CriptografarPasta(string caminhoPasta)
        {
            if (string.IsNullOrWhiteSpace(caminhoPasta) || !Directory.Exists(caminhoPasta))
            {
                MessageBox.Show("Erro: Caminho da pasta inválido ou a pasta não existe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c cipher /E /S:\"{caminhoPasta}\"",
                Verb = "runas",
                UseShellExecute = true,
                CreateNoWindow = true
            };

            try
            {
                Process processo = Process.Start(psi);
                processo.WaitForExit();
                if (processo.ExitCode == 0)
                {
                    MessageBox.Show("Pasta criptografada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Falha na criptografia da pasta. Código de saída: {processo.ExitCode}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criptografar a pasta: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Descriptografa a pasta removendo o bloqueio, sem exigir senha.
        /// </summary>
        public static void DescriptografarPasta(string caminhoPasta)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c cipher /D /S:\"{caminhoPasta}\"",
                Verb = "runas",
                UseShellExecute = true
            };

            Process processo = Process.Start(psi);
            processo.WaitForExit();

            Console.WriteLine("Pasta descriptografada! Agora está acessível.");
        }

        // Lê o caminho do programa a partir do arquivo de configuração (config.txt)
        private static string CarregarProgramaBloqueado()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");

            if (File.Exists(configPath))
            {
                string[] linhas = File.ReadAllLines(configPath);
                foreach (string linha in linhas)
                {
                    if (linha.StartsWith("Programa="))
                    {
                        return linha.Replace("Programa=", "").Trim();
                    }
                }
            }

            return null;
        }

        public static void RestaurarPermissoes(string caminhoPasta)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                // O comando abaixo faz um reset nas permissões da pasta
                Arguments = $"/c icacls \"{caminhoPasta}\" /reset",
                Verb = "runas", // Necessário para executar com privilégios de administrador
                UseShellExecute = true,
                CreateNoWindow = true
            };

            try
            {
                Process processo = Process.Start(psi);
                processo.WaitForExit();

                if (processo.ExitCode == 0)
                {
                    MessageBox.Show("Permissões restauradas com sucesso! Agora a pasta está acessível.",
                                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Falha ao restaurar as permissões. Código de saída: {processo.ExitCode}",
                                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao restaurar as permissões: {ex.Message}",
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static void BloquearPrograma(string caminhoPrograma)
        {
            if (string.IsNullOrEmpty(caminhoPrograma))
            {
                Console.WriteLine("Erro: Caminho do programa inválido.");
                return;
            }

            // 🔒 Bloquear acesso ao programa via NTFS
            ProcessStartInfo icacls = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c icacls \"{caminhoPrograma}\" /deny Todos:F",
                Verb = "runas",
                UseShellExecute = true
            };

            Process.Start(icacls);
            Console.WriteLine("Programa bloqueado! Todos os usuários estão impedidos de executá-lo.");
        }
    }
}