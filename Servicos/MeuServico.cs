using System.Diagnostics;
using System.ServiceProcess;

namespace App_Senha
{
    public class MeuServico : ServiceBase
    {
        public MeuServico()
        {
            ServiceName = "MeuServicoProtecao";
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("MeuServicoProtecao", "Monitorando possíveis tentativas de desinstalação.");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("MeuServicoProtecao", "Serviço encerrado.");
        }
    }
}