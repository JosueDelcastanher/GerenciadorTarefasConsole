// See https://aka.ms/new-console-template for more information
using GerenciadorTarefas;
using GerenciadorTarefas.Enums;
using GerenciadorTarefas.Models;
TelaInicio();

void TelaInicio(List<Tarefa> tarefaFiltrada = null)
{
    var listar = new ManipularXML();
    var tarefas = tarefaFiltrada == null ? listar.ListarTarefas() : tarefaFiltrada;

    Console.WriteLine("╔════╦══════════════════╦═════════════════════════════════╦═════════════╦══════════════╦══════════════════════╗");
    Console.WriteLine("║ ID ║ Título           ║ Descrição                       ║ Abertura    ║ Situação     ║   Data de Conclusão  ║");
    Console.WriteLine("╠════╬══════════════════╬═════════════════════════════════╬═════════════╬══════════════╬══════════════════════╣");

    foreach (var tarefa in tarefas)
    {
        if (tarefa.Id != 0)
        {
            string dataConclusao = (tarefa.DataConclusao != DateTime.MinValue.ToString("dd/MM/yyyy")) ? tarefa.DataConclusao.ToString() : "";
            
            Console.WriteLine($"║ {tarefa.Id,-2} ║ {tarefa.Titulo,-16} ║ {tarefa.Descricao,-32}║ {tarefa.DataCriacao:dd/MM/yyyy}  ║{tarefa.Situacao,-8}         {dataConclusao,8}");
        }
    }

    Console.WriteLine("╚════╩══════════════════╩═════════════════════════════════╩═════════════╩═════════════════════════════════════╝");

    Console.WriteLine("╔══════════════════════╦═══════════════════════════════╬═══════════════════════════════════╬═════════════════════╗");
    Console.WriteLine("║ Adicionar Tarefa: 1           Editar Tarefa: 2             Marcar/Desmarcar Tarefa: 3       Filtrar Tarefa: 4  ║");
    Console.WriteLine("╠══════════════════════╬═══════════════════════════════╬═══════════════════════════════════║═════════════════════╝");


    var decisao = Console.ReadLine();
    var gerenciador = new Gerenciador();

    switch (decisao)
    {
        case "1":
            Console.WriteLine("Adicione uma Tarefa");
            if (gerenciador.AdicionarTarefa())
            {
                Console.WriteLine("TAREFA CADASTRADA");
                Thread.Sleep(2000);
                Console.Clear();
                TelaInicio();
            }
            else
            {
                Console.Clear();
                TelaInicio();
            }

            break;
        case "2":
            Console.WriteLine("Edite uma Tarefa");
            if (gerenciador.EditarTarefa())
            {
                Console.WriteLine("TAREFA EDITADA");
                Thread.Sleep(2000);
                Console.Clear();
                TelaInicio();
            }
            else
            {
                Console.Clear();
                TelaInicio();
            }
            break;
        case "3":
            Console.WriteLine("Marcar/Desmarcar uma Tarefa");
            if (gerenciador.SituacaoTarefa())
            {
                Console.WriteLine("TAREFA EDITADA");
                Thread.Sleep(2000);
                Console.Clear();
                TelaInicio();
            }
            else
            {
                Console.Clear();
                TelaInicio();
            }
            break;
        case "4":
            Console.WriteLine("Filtrar Tarefa");
            TelaInicio(gerenciador.FiltrarTarefa());
            
            break;
        default:
            Console.WriteLine("Leitura errada, favor inserir corretamente a opção");
            Thread.Sleep(2000);
            Console.Clear();
            TelaInicio();
            break;
    }
}