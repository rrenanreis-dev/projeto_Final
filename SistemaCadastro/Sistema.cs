using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SistemaCadastro
{
    public partial class Sistema : Form
    {
        private int idBovinoSelecionado = 0;

        //Inicializador
        public Sistema()
        {
            InitializeComponent();
        }

        //fechar
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Aba cadastrar
        private void btnCadastra_Click(object sender, EventArgs e)
        {
            marcador.Height = btnCadastra.Height;
            marcador.Top = btnCadastra.Top;
            tabControl1.SelectedTab = tabControl1.TabPages[0];
        }

        //Aba buscar
        private void btnBusca_Click(object sender, EventArgs e)
        {
            marcador.Height = btnBusca.Height;
            marcador.Top = btnBusca.Top;
            tabControl1.SelectedTab = tabControl1.TabPages[1];
        }

        //Carrega banco
        void lista_gridBovinos()
        {
            ConectaBanco con = new ConectaBanco();
            dgBovinos.DataSource = con.listaBovinos();
            dgBovinos.Columns["idbovino"].Visible = false;
        }

        //Preenche combobox
        private void Sistema_Load(object sender, EventArgs e)
        {
            listaCBCategorias();
            listaCBSexos();
            listaCBAlterar();
            lista_gridBovinos();
        }

        //Preenche combobox raça
        public void listaCBCategorias()
        {
            ConectaBanco con = new ConectaBanco();
            DataTable tabelaDados = con.listaCategorias();
            cbcategoria.DataSource = tabelaDados;
            cbcategoria.DisplayMember = "categoria";
            cbcategoria.ValueMember = "idcategoria";
        }

        //Preenche combobox sexo
        public void listaCBSexos()
        {
            ConectaBanco con = new ConectaBanco();
            DataTable tabelaDados = con.listaSexos();
            cbsexo.DataSource = tabelaDados;
            cbsexo.DisplayMember = "sexo";
            cbsexo.ValueMember = "idsexo";
        }

        //Preenche combobox alterar
        public void listaCBAlterar()
        {
            ConectaBanco con = new ConectaBanco();

            DataTable tabCat = con.listaCategorias();
            cbAlterarRaca.DataSource = tabCat.Copy();
            cbAlterarRaca.DisplayMember = "categoria";
            cbAlterarRaca.ValueMember = "idcategoria";

            DataTable tabSex = con.listaSexos();
            cbAlterarSexo.DataSource = tabSex.Copy();
            cbAlterarSexo.DisplayMember = "sexo";
            cbAlterarSexo.ValueMember = "idsexo";
        }

        //Limpa campos
        void LimpaCampos()
        {
            txtnome.Clear();
            cbcategoria.SelectedIndex = -1;
            cbsexo.SelectedIndex = -1;
            txtidade.Clear();
            txtpreco.Clear();
            txtnome.Focus();
        }

        //Limpa campos
        void LimpaCamposAlterar()
        {
            txtAlterarNome.Clear();
            cbAlterarRaca.SelectedIndex = -1;
            cbAlterarSexo.SelectedIndex = -1;
            txtAlterarIdade.Clear();
            txtAlterarPreco.Clear();
            idBovinoSelecionado = 0;
        }

        //janela nova raça
        private string PedirNovaRaca()
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 340;
                prompt.Height = 140;
                prompt.Text = "Nova Raça";
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;

                Label lbl = new Label() { Left = 15, Top = 15, Width = 300, Text = "Digite o nome da nova raça:" };
                TextBox txt = new TextBox() { Left = 15, Top = 40, Width = 295 };
                Button btnOk = new Button() { Text = "OK", Left = 230, Top = 70, Width = 80, DialogResult = DialogResult.OK };

                prompt.Controls.Add(lbl);
                prompt.Controls.Add(txt);
                prompt.Controls.Add(btnOk);
                prompt.AcceptButton = btnOk;

                return prompt.ShowDialog(this) == DialogResult.OK ? txt.Text.Trim() : string.Empty;
            }
        }

        //Botão adicionar raça
        private void bntAddRaca_Click(object sender, EventArgs e)
        {
            string novaRaca = PedirNovaRaca();

            if (string.IsNullOrWhiteSpace(novaRaca))
            {
                return;
            }

            ConectaBanco con = new ConectaBanco();
            bool retorno = con.insereCategoria(novaRaca);

            if (retorno)
            {
                MessageBox.Show("Raça adicionada com sucesso!");
                listaCBCategorias();
                listaCBAlterar();

                int indice = cbcategoria.FindStringExact(novaRaca);
                if (indice >= 0)
                {
                    cbcategoria.SelectedIndex = indice;
                }
            }
            else
            {
                MessageBox.Show(con.mensagem);
            }
        }

        //insere bovino
        private void BtnConfirmaCadastro_Click(object sender, EventArgs e)
        {
            if (cbcategoria.SelectedValue == null || cbsexo.SelectedValue == null)
            {
                MessageBox.Show("Selecione uma raça e um sexo válidos.");
                return;
            }

            ConectaBanco con = new ConectaBanco();
            Bovino novoBovino = new Bovino();

            novoBovino.Nome = txtnome.Text;
            novoBovino.Idade = Convert.ToInt32(txtidade.Text);
            novoBovino.Preco = Convert.ToDecimal(txtpreco.Text);
            novoBovino.FkCategoria = Convert.ToInt32(cbcategoria.SelectedValue);
            novoBovino.FkSexo = Convert.ToInt32(cbsexo.SelectedValue);

            bool retorno = con.insereBovino(novoBovino);
            if (retorno == false)
            {
                MessageBox.Show(con.mensagem);
            }
            else
            {
                MessageBox.Show("Bovino cadastrado com sucesso!");
            }

            LimpaCampos();
            lista_gridBovinos();
        }
        //mostra busca
        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            (dgBovinos.DataSource as DataTable).DefaultView.RowFilter =
                string.Format("nome like '{0}%'", txtBusca.Text);
        }

        private void btnRemoveBanda_Click(object sender, EventArgs e)
        {
            int linha = dgBovinos.CurrentRow.Index;
            int id = Convert.ToInt32(
                dgBovinos.Rows[linha].Cells["idbovino"].Value.ToString());
            DialogResult resp = MessageBox.Show("Tem certeza que deseja excluir?",
                "Remove Bovino", MessageBoxButtons.OKCancel);
            if (resp == DialogResult.OK)
            {
                ConectaBanco con = new ConectaBanco();
                bool retorno = con.deletaBovino(id);
                if (retorno == true)
                {
                    MessageBox.Show("Bovino excluído com sucesso!");
                    lista_gridBovinos();
                }
                else
                    MessageBox.Show(con.mensagem);
            }
            else
                MessageBox.Show("Exclusão cancelada");
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (dgBovinos.CurrentRow == null)
            {
                return;
            }

            int linha = dgBovinos.CurrentRow.Index;
            idBovinoSelecionado = Convert.ToInt32(dgBovinos.Rows[linha].Cells["idbovino"].Value);

            txtAlterarNome.Text = dgBovinos.Rows[linha].Cells["nome"].Value.ToString();
            txtAlterarIdade.Text = dgBovinos.Rows[linha].Cells["idade"].Value.ToString();
            txtAlterarPreco.Text = dgBovinos.Rows[linha].Cells["preco"].Value.ToString();
            cbAlterarRaca.Text = dgBovinos.Rows[linha].Cells["categoria"].Value.ToString();
            cbAlterarSexo.Text = dgBovinos.Rows[linha].Cells["sexo"].Value.ToString();

            tabControl1.SelectedTab = tabControl1.TabPages[2];
        }

        private void btnConfirmaAlteracao_Click(object sender, EventArgs e)
        {
            if (idBovinoSelecionado == 0)
            {
                MessageBox.Show("Selecione um bovino na aba Buscar para alterar.");
                return;
            }

            if (cbAlterarRaca.SelectedValue == null || cbAlterarSexo.SelectedValue == null)
            {
                MessageBox.Show("Selecione uma raça e um sexo válidos.");
                return;
            }

            ConectaBanco con = new ConectaBanco();
            Bovino bovinoAlterado = new Bovino();

            bovinoAlterado.IdBovino = idBovinoSelecionado;
            bovinoAlterado.Nome = txtAlterarNome.Text;
            bovinoAlterado.Idade = Convert.ToInt32(txtAlterarIdade.Text);
            bovinoAlterado.Preco = Convert.ToDecimal(txtAlterarPreco.Text);
            bovinoAlterado.FkCategoria = Convert.ToInt32(cbAlterarRaca.SelectedValue);
            bovinoAlterado.FkSexo = Convert.ToInt32(cbAlterarSexo.SelectedValue);

            bool retorno = con.alteraBovino(bovinoAlterado);

            if (retorno)
            {
                MessageBox.Show("Bovino alterado com sucesso!");
                LimpaCamposAlterar();
                lista_gridBovinos();
                tabControl1.SelectedTab = tabControl1.TabPages[1];
            }
            else
            {
                MessageBox.Show(con.mensagem);
            }
        }

        private void cbGenero_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label11_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void txtpreco_TextChanged(object sender, EventArgs e)
        {
        }

        private void cbsexo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}