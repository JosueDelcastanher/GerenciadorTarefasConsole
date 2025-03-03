using GerenciadorTarefas.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GerenciadorTarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public Situacao Situacao { get; set; }
        public string DataCriacao { get; set; }
        public string DataConclusao { get; set; }
    }
}
