using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace App_Senha
{
    public partial class FormPrincipal : Form
    {
        private TextBox txtCaminhoPrograma;
        private Button btnSelecionarPrograma;
        private TextBox txtSenha;
        private Button btnSalvarConfiguracoes;
        private Button btnConcederPermissao; // Novo botão para conceder acesso total

        public FormPrincipal()
        {
            InitializeComponent();
            CarregarSenha(); // Carrega a senha ao iniciar

        }

        private void InitializeComponent()
        {
            this.txtCaminhoPrograma = new TextBox();
            this.btnSelecionarPrograma = new Button();
            this.txtSenha = new TextBox();
            this.btnSalvarConfiguracoes = new Button();
            this.btnConcederPermissao = new Button(); // Inicializando o novo botão

            this.SuspendLayout();

            // txtCaminhoPrograma
            this.txtCaminhoPrograma.Location = new Point(50, 30);
            this.txtCaminhoPrograma.Size = new Size(300, 20);
            this.txtCaminhoPrograma.ReadOnly = false; // Permite que o usuário digite o caminho

            // btnSelecionarPrograma
            this.btnSelecionarPrograma.Location = new Point(360, 30);
            this.btnSelecionarPrograma.Size = new Size(100, 23);
            this.btnSelecionarPrograma.Text = "Selecionar...";
            this.btnSelecionarPrograma.Click += new EventHandler(this.BtnSelecionarPrograma_Click);

            // txtSenha
            this.txtSenha.Location = new Point(50, 70);
            this.txtSenha.Size = new Size(200, 20);
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Text = "Digite a senha";
            this.txtSenha.ForeColor = Color.Gray;
            this.txtSenha.Enter += new EventHandler(txtSenha_Enter);
            this.txtSenha.Leave += new EventHandler(txtSenha_Leave);

            // btnSalvarConfiguracoes
            this.btnSalvarConfiguracoes.Location = new Point(260, 70);
            this.btnSalvarConfiguracoes.Size = new Size(100, 23);
            this.btnSalvarConfiguracoes.Text = "Salvar";
            this.btnSalvarConfiguracoes.Click += new EventHandler(this.BtnSalvarConfiguracoes_Click);

            // btnConcederPermissao (Novo botão)
            this.btnConcederPermissao.Location = new Point(50, 100);
            this.btnConcederPermissao.Size = new Size(150, 23);
            this.btnConcederPermissao.Text = "Conceder Acesso Total";
            this.btnConcederPermissao.Click += new EventHandler(this.BtnConcederPermissao_Click);

            // Form
            this.ClientSize = new Size(500, 180);
            this.Controls.Add(this.txtCaminhoPrograma);
            this.Controls.Add(this.btnSelecionarPrograma);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.btnSalvarConfiguracoes);
            this.Controls.Add(this.btnConcederPermissao); // Adiciona o novo botão ao formulário

            this.Text = "Configuração do Programa";

            this.ResumeLayout(false);
        }
        private void txtSenha_Enter(object sender, EventArgs e)
        {
            if (txtSenha.Text == "Digite a senha")
            {
                txtSenha.Text = "";
                txtSenha.ForeColor = Color.Black;
                txtSenha.PasswordChar = '*';
            }
        }

        private void txtSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                txtSenha.Text = "Digite a senha";
                txtSenha.ForeColor = Color.Gray;
                txtSenha.PasswordChar = '\0';
            }
        }

        private void BtnSalvarConfiguracoes_Click(object sender, EventArgs e)
        {
            // Coloque aqui o código que deve ser executado ao clicar no botão "Salvar"
            if (string.IsNullOrEmpty(txtCaminhoPrograma.Text) || string.IsNullOrEmpty(txtSenha.Text))
            {
                MessageBox.Show("Por favor, insira o caminho do programa e defina uma senha.");
                return;
            }

            SalvarProgramaSelecionado(txtCaminhoPrograma.Text);
            GerenciadorSenha.SalvarSenha(txtSenha.Text);
            ProtecaoWindows.BloquearPrograma(txtCaminhoPrograma.Text);

            MessageBox.Show("Senha salva e programa protegido com sucesso!");
        }
        private void BtnSelecionarPrograma_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selecione um programa",
                Filter = "Executáveis (*.exe)|*.exe|Todos os arquivos (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtCaminhoPrograma.Text = openFileDialog.FileName;
                SalvarProgramaSelecionado(txtCaminhoPrograma.Text);
                MessageBox.Show("Programa selecionado! Agora você pode bloquear, desbloquear ou conceder acesso total.");
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

        private void CarregarSenha()
        {
            string senhaSalva = GerenciadorSenha.CarregarSenha();
            if (!string.IsNullOrEmpty(senhaSalva))
            {
                txtSenha.Text = senhaSalva;
                txtSenha.ForeColor = Color.Black;
                txtSenha.PasswordChar = '*';
            }
        }
        public static void BloquearProgramaSistema(string caminhoPrograma)
        {
            if (string.IsNullOrEmpty(caminhoPrograma))
            {
                Console.WriteLine("Erro: Caminho do programa inválido.");
                return;
            }

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

        // Evento do novo botão de conceder acesso total
        private void BtnConcederPermissao_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCaminhoPrograma.Text))
            {
                MessageBox.Show("Por favor, insira o caminho do programa.");
                return;
            }

            PermissoesWindows.ConcederPermissaoTotalViaPowerShell(txtCaminhoPrograma.Text);
            MessageBox.Show("Permissões concedidas! Agora todos os usuários têm acesso total.");
        }

        private void BtnBloquearPrograma_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCaminhoPrograma.Text))
            {
                MessageBox.Show("Por favor, insira o caminho do programa.");
                return;
            }

            // Corrigir chamada:
            PermissoesWindows.BloquearPrograma(txtCaminhoPrograma.Text);

            MessageBox.Show("Programa bloqueado! Acesso negado para todos.");
        }

        private void BtnBloquearPrograma_Click_Novo(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCaminhoPrograma.Text))
            {
                MessageBox.Show("Por favor, insira o caminho do programa.");
                return;
            }

            PermissoesWindows.BloquearPrograma(txtCaminhoPrograma.Text);
            MessageBox.Show("Programa bloqueado! Acesso negado para todos.");
        }

        private void SalvarProgramaSelecionado(string caminho)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");

            try
            {
                File.WriteAllText(configPath, $"Programa={caminho}");
                MessageBox.Show("Programa salvo com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar programa: {ex.Message}");
            }
        }
    }

}