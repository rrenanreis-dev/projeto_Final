using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace SistemaCadastro
{
    class ConectaBanco
    {
        MySqlConnection conexao = new MySqlConnection("server=localhost;user id=root ;port=3306;password=1234;database=banco_leilao");
        public string mensagem = "";

        
        public DataTable listaCategorias()
        {
            MySqlCommand cmd = new MySqlCommand("sp_listaCategorias", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            conexao.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable tabela = new DataTable();
            da.Fill(tabela);
            conexao.Close();
            return tabela;
        }

        
        public DataTable listaSexos()
        {
            MySqlCommand cmd = new MySqlCommand("sp_listaSexos", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            conexao.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable tabela = new DataTable();
            da.Fill(tabela);
            conexao.Close();
            return tabela;
        }

        public DataTable listaBovinos()
        {
            MySqlCommand cmd = new MySqlCommand("sp_listaBovinos", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            conexao.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable tabela = new DataTable();
            da.Fill(tabela);
            conexao.Close();
            return tabela;
        }

        public bool insereCategoria(string nomeCategoria)
        {
            MySqlCommand cmd = new MySqlCommand("sp_insereCategoria", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("p_categoria", nomeCategoria);

            conexao.Open();
            cmd.ExecuteNonQuery();
            conexao.Close();
            return true;
        }

        public bool insereBovino(Bovino novoBovino)
        {
            MySqlCommand cmd = new MySqlCommand("sp_insereBovino", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("p_nome", novoBovino.Nome);
            cmd.Parameters.AddWithValue("p_idade", novoBovino.Idade);
            cmd.Parameters.AddWithValue("p_preco", novoBovino.Preco);
            cmd.Parameters.AddWithValue("p_categoria", novoBovino.FkCategoria);
            cmd.Parameters.AddWithValue("p_sexo", novoBovino.FkSexo);

            conexao.Open();
            cmd.ExecuteNonQuery();
            conexao.Close();
            return true;
        }

        public bool alteraBovino(Bovino bovino)
        {
            MySqlCommand cmd = new MySqlCommand("sp_alteraBovino", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("p_idbovino", bovino.IdBovino);
            cmd.Parameters.AddWithValue("p_nome", bovino.Nome);
            cmd.Parameters.AddWithValue("p_idade", bovino.Idade);
            cmd.Parameters.AddWithValue("p_preco", bovino.Preco);
            cmd.Parameters.AddWithValue("p_categoria", bovino.FkCategoria);
            cmd.Parameters.AddWithValue("p_sexo", bovino.FkSexo);

            conexao.Open();
            cmd.ExecuteNonQuery();
            conexao.Close();
            return true;
        }

        public bool deletaBovino(int idbovino)
        {
            MySqlCommand cmd = new MySqlCommand("sp_removeBovino", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("p_idbovino", idbovino);

            conexao.Open();
            cmd.ExecuteNonQuery();
            conexao.Close();
            return true;
        }

        public bool Verifica(string user, string pass)
        {
            mensagem = "";
            string senhaHash = Biblioteca.makeHash(pass);
            MySqlCommand cmd = new MySqlCommand("consultaLogin", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("usuario", user);
            cmd.Parameters.AddWithValue("senha", senhaHash);

            try
            {
                conexao.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (MySqlException er)
            {
                mensagem = "Erro: " + er.Message;
            }
            finally
            {
                if (conexao.State == ConnectionState.Open)
                {
                    conexao.Close();
                }
            }

            return false;
        }
    }
}