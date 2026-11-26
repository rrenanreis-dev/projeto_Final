using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCadastro
{
    class Bovino
    {
        //Atributos
        int idBovino;
        string nome;
        int idade;
        decimal preco;
        int fkCategoria;
        int fkSexo;

        //Propriedades
        public int IdBovino { get => idBovino; set => idBovino = value; }
        public string Nome { get => nome; set => nome = value; }
        public int Idade { get => idade; set => idade = value; }
        public decimal Preco { get => preco; set => preco = value; }
        public int FkCategoria { get => fkCategoria; set => fkCategoria = value; }
        public int FkSexo { get => fkSexo; set => fkSexo = value; }
    }
}