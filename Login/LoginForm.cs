using System;
using System.Windows.Forms;

namespace App_Senha
{
    public partial class LoginForm : Form
    {
        private TextBox txtSenha;
        private Button btnEntrar;
        private Button btnCancelar;  // Botão para cancelar
        private string senhaCorreta;

        public LoginForm()
        {
            InitializeComponent();
            CarregarSenha();
            // Registra o evento de fechamento para diferenciar a saída
            this.FormClosing += new FormClosingEventHandler(LoginForm_FormClosing);
        }

        private void InitializeComponent()
        {
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.btnEntrar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button(); // Instanciando o botão "Cancelar"
            this.SuspendLayout();
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(50, 30);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(200, 20);
            this.txtSenha.TabIndex = 0;
            // 
            // btnEntrar
            // 
            this.btnEntrar.Location = new System.Drawing.Point(50, 60);
            this.btnEntrar.Name = "btnEntrar";
            this.btnEntrar.Size = new System.Drawing.Size(95, 25);
            this.btnEntrar.TabIndex = 1;
            this.btnEntrar.Text = "Entrar";
            this.btnEntrar.Click += new System.EventHandler(this.btnEntrar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(151, 60);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(99, 25);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(295, 100);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.btnEntrar);
            this.Controls.Add(this.btnCancelar);
            this.Name = "LoginForm";
            this.Text = "Autenticação";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CarregarSenha()
        {
            // Carrega a senha salva para o programa.
            // Se não houver senha salva via GerenciadorSenha, usa "123" como padrão.
            senhaCorreta = GerenciadorSenha.CarregarSenhaPrograma();
            if (string.IsNullOrEmpty(senhaCorreta))
            {
                senhaCorreta = "123";
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            // Compara o que foi digitado com a senha carregada
            if (txtSenha.Text == senhaCorreta)
            {
                // Libera o programa removendo o bloqueio.
                // Certifique-se de que o método LiberarPrograma está implementado em ProtecaoWindows.
                ProtecaoWindows.LiberarPrograma();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Senha incorreta! O programa continuará bloqueado.");
            }
        }

        // Ao clicar em "Cancelar", define o DialogResult como Cancel e fecha o formulário.
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Se o usuário fechar a janela pelo ícone "X" sem confirmar o login, define o DialogResult como Cancel.
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Caso o DialogResult não esteja OK, garantimos que seja Cancel.
            if (this.DialogResult != DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}