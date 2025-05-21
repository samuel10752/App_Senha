using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace App_Senha
{
    public class ProcessMonitor
    {
        private static string nomeProgramaBloqueado;
        private static bool senhaValida = false;

        public static void IniciarMonitoramento()
        {
            nomeProgramaBloqueado = CarregarProgramaBloqueado();
            Thread monitorThread = new Thread(VerificarExecucao);
            monitorThread.IsBackground = true;
            monitorThread.Start();
        }

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
                        return Path.GetFileNameWithoutExtension(linha.Replace("Programa=", "").Trim());
                    }
                }
            }
            return "";
        }

        private static void VerificarExecucao()
        {
            while (true)
            {
                Process[] processos = Process.GetProcesses();
                foreach (Process processo in processos)
                {
                    try
                    {
                        if (!senhaValida && !string.IsNullOrEmpty(nomeProgramaBloqueado) &&
                            processo.ProcessName.Equals(nomeProgramaBloqueado, StringComparison.OrdinalIgnoreCase))
                        {
                            processo.Kill(); // Bloqueia a execução do programa!
                            Console.WriteLine($"Programa bloqueado: {processo.ProcessName}");
                        }
                    }
                    catch { } // Evita erro de acesso a processos protegidos
                }

                Thread.Sleep(2000); // Aguarda antes de verificar novamente
            }
        }

        public static void LiberarExecucao()
        {
            senhaValida = true;
        }
    }
}