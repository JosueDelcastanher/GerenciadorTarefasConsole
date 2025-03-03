using GerenciadorTarefas.Enums;
using GerenciadorTarefas.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorTarefas
{
    public class Gerenciador
    {
        public Gerenciador() { }

        public bool AdicionarTarefa()
        {
            try
            {
                var tarefa = new Tarefa();

                Console.WriteLine("\nDigite um título para sua tarefa?");
                tarefa.Titulo = Console.ReadLine();

                Console.WriteLine("\n Digite uma descrição para sua tarefa?");
                tarefa.Descricao = Console.ReadLine();
                tarefa.DataCriacao = DateTime.Now.ToString("dd/MM/yyyy");
                tarefa.DataConclusao = DateTime.MinValue.ToString("dd/MM/yyyy");
                tarefa.Situacao = Situacao.Pendente;

                var manipularXML = new ManipularXML();
                manipularXML.AdicionarTarefaXml(tarefa);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro: {ex}");
            }
        }

        public bool EditarTarefa()
        {
            try
            {
                var tarefa = new Tarefa();
                Console.WriteLine("╔═════════════════════════════════════════╗");
                Console.WriteLine("║   Digite qual ID da tarefa irá EDITAR   ║");
                Console.WriteLine("╠═════════════════════════════════════════╬");
                var id = Convert.ToInt32(Console.ReadLine());

                var manipularXML = new ManipularXML();
                var tarefaEncontrada = manipularXML.BuscarTarefaXML(Convert.ToInt32(id));

                if (tarefaEncontrada != null)
                {
                    Console.Clear();
                    Console.WriteLine("╔═════════════╦═════════════╗");
                    Console.WriteLine("║ Título        Descrição   ║");
                    Console.WriteLine("╠═════════════╬═════════════╬");
                    Console.WriteLine($"║ {tarefaEncontrada.Titulo,+8}  {tarefaEncontrada.Descricao,-10}║");


                    Console.WriteLine("\n Edite o titulo da sua tarefa? ");
                    tarefa.Titulo = Console.ReadLine();

                    Console.WriteLine("\n Edite a descrição da sua tarefa? ");
                    tarefa.Descricao = Console.ReadLine();

                    manipularXML.EditarTarefaXML(id, tarefa);

                    return true;
                }
                else
                {
                    Console.WriteLine($"Não foi possível encontrar a tarefa com o ID: {id}");
                    Console.WriteLine($"Tente Novamente");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro: {ex}");
            }
            return false;
        }

        public bool SituacaoTarefa()
        {
            try
            {
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║  Digite qual ID da tarefa deseja alterar a situação    ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╬");
                var id = Convert.ToInt32(Console.ReadLine());
                var manipularXML = new ManipularXML();

                if (manipularXML.SituacaoTarefaXML(id))
                {
                    return true;
                }

                Console.WriteLine($"Não foi possível encontrar a tarefa com o ID: {id}");
                Console.WriteLine($"Tente Novamente");
                Thread.Sleep(1000);
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro: {ex}");
            }
        }

        public List<Tarefa> FiltrarTarefa()
        {
            var manipularXML = new ManipularXML();

            Console.WriteLine("╔════════════════╦═════════════╗");
            Console.WriteLine("║ FILTRO             VALOR     ║");
            Console.WriteLine("╠════════════════╬═════════════╬");
            Console.WriteLine("║ Remover Filtro       0       ║");
            Console.WriteLine("╠════════════════╬═════════════╬");
            Console.WriteLine("║ Concluído            1       ║");
            Console.WriteLine("╠════════════════╬═════════════╬");
            Console.WriteLine("║ Pendente             2       ║");
            Console.WriteLine("╠════════════════╬═════════════╬");

            Enum.TryParse(Console.ReadLine(), true, out Situacao situacaoReal);

            if (Enum.IsDefined(typeof(Situacao), situacaoReal))
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════╦═════════════════╗");
                Console.WriteLine($"║ FILTRO APLICADO    {situacaoReal} ║");
                Console.WriteLine("╠══════════════════╬═════════════════╬");

                return manipularXML.FiltrarTarefaSituacaoXML(situacaoReal);
            }

            Console.Clear();
            return manipularXML.ListarTarefas();
        }
    }
}
