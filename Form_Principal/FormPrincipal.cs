using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace App_Senha
{
    public partial class FormPrincipal : Form
    {
        // Controles para configurar o programa
        private TextBox txtCaminhoPrograma;
        private Button btnSelecionarPrograma;
        private Button btnSalvarConfiguracoes;
        private Button btnConcederPermissao;
        private Button btnRestaurarPermissoes;  // Botão para restaurar permissões

        // Controles para configurar a pasta
        private TextBox txtCaminhoPasta;
        private Button btnSelecionarPasta;
        private Button btnSalvarPasta;     // Botão para salvar e criptografar a pasta

        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtCaminhoPrograma = new System.Windows.Forms.TextBox();
            this.btnSelecionarPrograma = new System.Windows.Forms.Button();
            this.btnSalvarConfiguracoes = new System.Windows.Forms.Button();
            this.btnConcederPermissao = new System.Windows.Forms.Button();
            this.btnRestaurarPermissoes = new System.Windows.Forms.Button();
            this.txtCaminhoPasta = new System.Windows.Forms.TextBox();
            this.btnSelecionarPasta = new System.Windows.Forms.Button();
            this.btnSalvarPasta = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtCaminhoPrograma
            // 
            this.txtCaminhoPrograma.Location = new System.Drawing.Point(20, 20);
            this.txtCaminhoPrograma.Name = "txtCaminhoPrograma";
            this.txtCaminhoPrograma.Size = new System.Drawing.Size(300, 20);
            this.txtCaminhoPrograma.TabIndex = 0;
            this.txtCaminhoPrograma.Text = "Ex: C:\\Programa\\exemplo.exe";
            // 
            // btnSelecionarPrograma
            // 
            this.btnSelecionarPrograma.Location = new System.Drawing.Point(326, 11);
            this.btnSelecionarPrograma.Name = "btnSelecionarPrograma";
            this.btnSelecionarPrograma.Size = new System.Drawing.Size(139, 29);
            this.btnSelecionarPrograma.TabIndex = 1;
            this.btnSelecionarPrograma.Text = "Selecionar Programa";
            this.btnSelecionarPrograma.Click += new System.EventHandler(this.BtnSelecionarPrograma_Click);
            // 
            // btnSalvarConfiguracoes
            // 
            this.btnSalvarConfiguracoes.Location = new System.Drawing.Point(20, 46);
            this.btnSalvarConfiguracoes.Name = "btnSalvarConfiguracoes";
            this.btnSalvarConfiguracoes.Size = new System.Drawing.Size(131, 40);
            this.btnSalvarConfiguracoes.TabIndex = 3;
            this.btnSalvarConfiguracoes.Text = "Salvar Programa";
            this.btnSalvarConfiguracoes.Click += new System.EventHandler(this.BtnSalvarConfiguracoes_Click);
            // 
            // btnConcederPermissao
            // 
            this.btnConcederPermissao.Location = new System.Drawing.Point(157, 49);
            this.btnConcederPermissao.Name = "btnConcederPermissao";
            this.btnConcederPermissao.Size = new System.Drawing.Size(163, 37);
            this.btnConcederPermissao.TabIndex = 4;
            this.btnConcederPermissao.Text = "Acesso Total Programa";
            this.btnConcederPermissao.Click += new System.EventHandler(this.BtnConcederPermissao_Click);
            // 
            // btnRestaurarPermissoes
            // 
            this.btnRestaurarPermissoes.Location = new System.Drawing.Point(157, 156);
            this.btnRestaurarPermissoes.Name = "btnRestaurarPermissoes";
            this.btnRestaurarPermissoes.Size = new System.Drawing.Size(163, 36);
            this.btnRestaurarPermissoes.TabIndex = 8;
            this.btnRestaurarPermissoes.Text = "Restaurar Permissões";
            this.btnRestaurarPermissoes.Click += new System.EventHandler(this.BtnRestaurarPermissoes_Click);
            // 
            // txtCaminhoPasta
            // 
            this.txtCaminhoPasta.Location = new System.Drawing.Point(20, 120);
            this.txtCaminhoPasta.Name = "txtCaminhoPasta";
            this.txtCaminhoPasta.Size = new System.Drawing.Size(300, 20);
            this.txtCaminhoPasta.TabIndex = 5;
            this.txtCaminhoPasta.Text = "Ex: C:\\MinhaPasta";
            // 
            // btnSelecionarPasta
            // 
            this.btnSelecionarPasta.Location = new System.Drawing.Point(326, 108);
            this.btnSelecionarPasta.Name = "btnSelecionarPasta";
            this.btnSelecionarPasta.Size = new System.Drawing.Size(135, 32);
            this.btnSelecionarPasta.TabIndex = 6;
            this.btnSelecionarPasta.Text = "Selecionar Pasta";
            this.btnSelecionarPasta.Click += new System.EventHandler(this.BtnSelecionarPasta_Click);
            // 
            // btnSalvarPasta
            // 
            this.btnSalvarPasta.Location = new System.Drawing.Point(20, 156);
            this.btnSalvarPasta.Name = "btnSalvarPasta";
            this.btnSalvarPasta.Size = new System.Drawing.Size(131, 36);
            this.btnSalvarPasta.TabIndex = 7;
            this.btnSalvarPasta.Text = "Salvar e Criptografar Pasta";
            this.btnSalvarPasta.Click += new System.EventHandler(this.BtnSalvarPasta_Click);
            // 
            // FormPrincipal
            // 
            this.ClientSize = new System.Drawing.Size(500, 209);
            this.Controls.Add(this.txtCaminhoPrograma);
            this.Controls.Add(this.btnSelecionarPrograma);
            this.Controls.Add(this.btnSalvarConfiguracoes);
            this.Controls.Add(this.btnConcederPermissao);
            this.Controls.Add(this.txtCaminhoPasta);
            this.Controls.Add(this.btnSelecionarPasta);
            this.Controls.Add(this.btnSalvarPasta);
            this.Controls.Add(this.btnRestaurarPermissoes);
            this.Name = "FormPrincipal";
            this.Text = "Configuração do Programa e Pasta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        // Eventos para o txtSenhaPrograma

        // Método para salvar configurações do programa
        private void BtnSalvarConfiguracoes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCaminhoPrograma.Text))
            {
                MessageBox.Show("Por favor, insira o caminho do programa.");
                return;
            }
            SalvarProgramaSelecionado(txtCaminhoPrograma.Text);
            ProtecaoWindows.BloquearPrograma(txtCaminhoPrograma.Text);
            MessageBox.Show("Programa salvo e protegido com sucesso!");
        }

        // Método para conceder acesso total ao programa (exemplo)
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

        // Método para selecionar o programa (utilizando OpenFileDialog)
        private void BtnSelecionarPrograma_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Selecione um programa",
                Filter = "Executáveis (*.exe)|*.exe|Todos os arquivos (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtCaminhoPrograma.Text = openFileDialog.FileName;
                SalvarProgramaSelecionado(txtCaminhoPrograma.Text);
                MessageBox.Show("Programa selecionado! Agora você pode salvar ou conceder acesso total ao programa.");
            }
        }

        // Método para salvar o caminho do programa (configurado em config.txt)
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

        // Método para selecionar a pasta (usando FolderBrowserDialog)
        private void BtnSelecionarPasta_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Selecione a pasta para salvar e criptografar";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtCaminhoPasta.Text = folderDialog.SelectedPath;
                    MessageBox.Show("Pasta selecionada!");
                }
            }
        }

        // Método para salvar e criptografar a pasta (apenas o caminho é salvo)
        private void BtnSalvarPasta_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCaminhoPasta.Text))
            {
                MessageBox.Show("Por favor, selecione a pasta.");
                return;
            }
            SalvarPastaSelecionada(txtCaminhoPasta.Text);
            ProtecaoWindows.CriptografarPasta(txtCaminhoPasta.Text);
        }

        // Salva apenas o caminho da pasta no arquivo de configuração
        private void SalvarPastaSelecionada(string caminho)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
            try
            {
                File.WriteAllText(configPath, $"Pasta={caminho}");
                MessageBox.Show("Pasta salva com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar a pasta: {ex.Message}");
            }
        }

        // Método para restaurar permissões da pasta sem validar senha
        private void BtnRestaurarPermissoes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCaminhoPasta.Text))
            {
                MessageBox.Show("Por favor, selecione a pasta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ProtecaoWindows.RestaurarPermissoes(txtCaminhoPasta.Text);
        }

    }

}