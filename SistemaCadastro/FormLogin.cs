using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaCadastro
{
    public partial class FormLogin : Form
    {
        //Construtor
        public FormLogin()
        {
            InitializeComponent();
        }

        //Botão de fechar 
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //confirmação de login
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            ConectaBanco conecta = new ConectaBanco();
            if (conecta.Verifica(txtUserLogin.Text, txtSenhaLogin.Text) == true)
            {
                Sistema sistema = new Sistema();
                this.Hide();
                sistema.ShowDialog();
                this.Close();
            }
            else
            { 
                MessageBox.Show("Usuário ou senha inválidos! "+ conecta.mensagem);

            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
