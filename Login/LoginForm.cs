using System;
using System.Windows.Forms;

namespace App_Senha
{
    public partial class LoginForm : Form
    {
        private TextBox txtSenha;
        private Button btnEntrar;
        private string senhaCorreta;
        private TextBox txtCaminhoPrograma;

        public LoginForm()
        {
            InitializeComponent();
            CarregarSenha();
        }

        private void InitializeComponent()
        {
            this.txtSenha = new TextBox();
            this.btnEntrar = new Button();

            this.SuspendLayout();

            // txtSenha
            this.txtSenha.Location = new System.Drawing.Point(50, 30);
            this.txtSenha.Size = new System.Drawing.Size(200, 20);
            this.txtSenha.PasswordChar = '*';

            // btnEntrar
            this.btnEntrar.Location = new System.Drawing.Point(50, 60);
            this.btnEntrar.Text = "Entrar";
            this.btnEntrar.Click += new EventHandler(btnEntrar_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(300, 150);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.btnEntrar);
            this.Text = "Autenticação";

            this.ResumeLayout(false);
        }

        private void CarregarSenha()
        {
            senhaCorreta = GerenciadorSenha.CarregarSenha();
            if (string.IsNullOrEmpty(senhaCorreta))
            {
                senhaCorreta = "123"; // Caso não haja senha salva
            }
        }
        private void BtnSalvarConfiguracoes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                MessageBox.Show("Por favor, defina uma senha.");
                return;
            }

            if (string.IsNullOrEmpty(txtCaminhoPrograma.Text))
            {
                MessageBox.Show("Por favor, insira o caminho do programa.");
                return;
            }

            GerenciadorSenha.SalvarSenha(txtSenha.Text); // Salva a senha criptografada
            ProtecaoWindows.BloquearPrograma(txtCaminhoPrograma.Text); // Bloqueia o programa passando o caminho como parâmetro

            MessageBox.Show("Senha salva e programa protegido com sucesso!");
        }
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string senhaSalva = GerenciadorSenha.CarregarSenha();

            if (txtSenha.Text == senhaSalva)
            {
                MessageBox.Show("Acesso permitido!");
                ProtecaoWindows.LiberarPrograma(); // Remove bloqueio e libera acesso
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Senha incorreta! O programa continuará bloqueado.");
            }
        }
    }
}