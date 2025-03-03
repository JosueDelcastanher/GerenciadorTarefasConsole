using GerenciadorTarefas.Enums;
using GerenciadorTarefas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GerenciadorTarefas
{
    public class ManipularXML
    {
        private readonly string PastaXml = "tarefas.xml";
        private readonly string Tarefas = "Tarefas";

        public ManipularXML()
        { }

        public List<Tarefa> ListarTarefas()
        {
            if (!File.Exists(PastaXml))
            {
                return new List<Tarefa>();
            }

            XDocument xmlDoc = XDocument.Load(PastaXml);
            return xmlDoc.Descendants(Tarefas)
                 .Select(ConverterParaTarefa)
                .ToList();
        }

        public void AdicionarTarefaXml(Tarefa tarefa)
        {
            XDocument xml = CriarBuscarXml();
            var ultimoId = ObterUltimoId();
            ultimoId++;

            XElement produtoElemento = new XElement(Tarefas,
                new XElement(nameof(tarefa.Id), ultimoId),
                new XElement(nameof(tarefa.Titulo), tarefa.Titulo),
                new XElement(nameof(tarefa.Descricao), tarefa.Descricao),
                new XElement(nameof(tarefa.DataCriacao), tarefa.DataCriacao),
                new XElement(nameof(tarefa.Situacao), tarefa.Situacao),
                new XElement(nameof(tarefa.DataConclusao), tarefa.DataConclusao)
            );

            xml.Root?.Add(produtoElemento);
            xml.Save(PastaXml);
        }

        public int ObterUltimoId()
        {
            if (!File.Exists(PastaXml))
            {
                return 0;
            }

            XDocument xml = CriarBuscarXml();
            return xml.Descendants(Tarefas)
                         .Select(p => int.Parse(p.Element("Id")?.Value ?? "0"))
                         .DefaultIfEmpty(0)
                         .Max();
        }

        public void EditarTarefaXML(int id, Tarefa tarefaEditada)
        {
            var xml = CriarBuscarXml();
            var elementoTarefa = BuscarElementoId(id, xml);

            if (elementoTarefa != null)
            {
                elementoTarefa.Element(nameof(tarefaEditada.Titulo))!.Value = tarefaEditada.Titulo;
                elementoTarefa.Element(nameof(tarefaEditada.Descricao))!.Value = tarefaEditada.Descricao;
                xml.Save(PastaXml);
            }
        }

        public Tarefa? BuscarTarefaXML(int id)
        {
            var xml = CriarBuscarXml();
            var tarefaEncontrada = BuscarElementoId(id, xml);

            return tarefaEncontrada != null ? ConverterParaTarefa(tarefaEncontrada) : null;
        }

        public List<Tarefa> FiltrarTarefaSituacaoXML(Situacao filtro)
        {
            XDocument xml = CriarBuscarXml();
            return xml.Descendants(Tarefas).Where(w => w.Element("Situacao")?.Value.Trim() == filtro.ToString().Trim()).
                    Select(ConverterParaTarefa).ToList();
        }

        public bool SituacaoTarefaXML(int id)
        {
            try
            {
                var xml = CriarBuscarXml();
                var elementoTarefa = BuscarElementoId(id, xml);
                if (elementoTarefa != null)
                {
                    Enum.TryParse(elementoTarefa.Element("Situacao")?.Value?.Trim(), true, out Situacao situacaoAtual);

                    if (situacaoAtual != Situacao.Concluido)
                    {
                        situacaoAtual = Situacao.Concluido;
                        elementoTarefa.Element("DataConclusao")!.Value = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        situacaoAtual = Situacao.Pendente;
                        elementoTarefa.Element("DataConclusao")!.Value = DateTime.MinValue.ToString("dd/MM/yyyy");
                    }

                    if (elementoTarefa != null)
                    {
                        elementoTarefa.Element("Situacao")!.Value = situacaoAtual.ToString();
                        xml.Save(PastaXml);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro: {ex}");
            }

            return false;
        }

        private Tarefa ConverterParaTarefa(XElement tarefaXml)
        {
            return new Tarefa
            {
                Id = int.TryParse(tarefaXml.Element("Id")?.Value, out int id) ? id : 0,
                Titulo = tarefaXml.Element("Titulo")?.Value ?? "Sem título",
                Descricao = tarefaXml.Element("Descricao")?.Value ?? "Sem descrição",
                DataCriacao = tarefaXml.Element("DataCriacao")?.Value ?? string.Empty,
                Situacao = Enum.TryParse(tarefaXml.Element("Situacao")?.Value?.Trim(), true, out Situacao situacao) ? situacao : Situacao.Pendente,
                DataConclusao = tarefaXml.Element("DataConclusao")?.Value ?? string.Empty
            };
        }

        private XDocument CriarBuscarXml()
        {
            if (File.Exists(PastaXml))
            {
                using (FileStream fileStream = new FileStream(PastaXml, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return XDocument.Load(fileStream);
                }
            }
            else
            {
                return new XDocument(new XElement(Tarefas));
            }
        }

        private XElement BuscarElementoId(int id, XDocument xml)
        {
            try
            {
                return xml.Descendants(Tarefas).
                    FirstOrDefault(p => (int?)p.Element("Id") == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao BuscarElementoID : {ex}");
            }

            return null;
        }
    }
}
