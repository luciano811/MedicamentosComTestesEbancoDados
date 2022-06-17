using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {
        private Medicamento medicamento;
        private Fornecedor fornecedor;
        private RepositorioMedicamentoEmBancoDados repositorio;
        private RepositorioFornecedorEmBancoDados repositorioFornecedor;


        public RepositorioMedicamentoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0) " +
                "DELETE FROM TBMEDICAMENTO;DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0) " +
                "DELETE FROM TBFORNECEDOR;DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");
            
            fornecedor = new Fornecedor("nomefornecedor1", "telefonefornecedor1", "emailfornecedor1", "cidadefornecedor1", "estadofornecedor1");
            medicamento = new Medicamento("Nome teste 1", "Descricao teste 1", "Lote teste 1", DateTime.Now.Date, 60, fornecedor);

            repositorio = new RepositorioMedicamentoEmBancoDados();
            repositorioFornecedor = new RepositorioFornecedorEmBancoDados();

        }

        [TestMethod]
        public void Deve_inserir_novo_medicamento()
        {
            //arrange                               
            repositorioFornecedor.Inserir(fornecedor);

            //action
            repositorio.Inserir(medicamento);

            //assert
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento.Id, medicamentoEncontrado.Id);
            Assert.AreEqual(medicamento.Nome, medicamentoEncontrado.Nome);
            Assert.AreEqual(medicamento.Descricao, medicamentoEncontrado.Descricao);
            Assert.AreEqual(medicamento.Lote, medicamentoEncontrado.Lote);
            Assert.AreEqual(medicamento.Validade, medicamentoEncontrado.Validade);
            Assert.AreEqual(medicamento.Fornecedor.Id, medicamentoEncontrado.Fornecedor.Id);
        }

        [TestMethod]
        public void Deve_editar_informacoes_medicamento()
        {
            //arrange                      
            repositorioFornecedor.Inserir(fornecedor);
            repositorio.Inserir(medicamento);

            //action
            medicamento.Nome = "Nome87654321";
            medicamento.Descricao = "Descricao987654321";
            medicamento.Lote = "Lote novo";
            medicamento.Validade = DateTime.Now.AddDays(30).Date;
            medicamento.QuantidadeDisponivel = 11;
            medicamento.Fornecedor = repositorioFornecedor.SelecionarPorId(1);
            repositorio.Editar(medicamento);

            //assert
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento.Id, medicamentoEncontrado.Id);
            Assert.AreEqual(medicamento.Nome, medicamentoEncontrado.Nome);
            Assert.AreEqual(medicamento.Validade, medicamentoEncontrado.Validade);
            Assert.AreEqual(medicamento.QuantidadeDisponivel, medicamentoEncontrado.QuantidadeDisponivel);
            Assert.AreEqual(medicamento.Fornecedor.Id, medicamentoEncontrado.Fornecedor.Id);
        }

        [TestMethod]
        public void Deve_excluir_medicamento()
        {
            //arrange           
            repositorioFornecedor.Inserir(fornecedor);
            repositorio.Inserir(medicamento);

            //action           
            repositorio.Excluir(medicamento);

            //assert
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);
            Assert.IsNull(medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_medicamento()
        {
            //arrange          
            repositorioFornecedor.Inserir(fornecedor);
            repositorio.Inserir(medicamento);

            //action
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);

            //assert
            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento.Id, medicamentoEncontrado.Id);
            Assert.AreEqual(medicamento.Nome, medicamentoEncontrado.Nome);
            Assert.AreEqual(medicamento.Validade, medicamentoEncontrado.Validade);
            Assert.AreEqual(medicamento.QuantidadeDisponivel, medicamentoEncontrado.QuantidadeDisponivel);
            Assert.AreEqual(medicamento.Fornecedor.Id, medicamentoEncontrado.Fornecedor.Id);
        }

        [TestMethod]
        public void Deve_selecionar_todos_os_medicamentoes()
        {
            //arrange
            repositorioFornecedor.Inserir(fornecedor);
            var m01 = new Medicamento("Nome teste 2", "Descricao teste 2", "Lote teste 2", DateTime.Now.Date, 30, fornecedor);
            var m02 = new Medicamento("Nome teste 3", "Descricao teste 3", "Lote teste 3", DateTime.Now.Date, 30, fornecedor);
            var m03 = new Medicamento("Nome teste 4", "Descricao teste 4", "Lote teste 4", DateTime.Now.Date, 30, fornecedor);

            var repositorio = new RepositorioMedicamentoEmBancoDados();
            repositorio.Inserir(m01);
            repositorio.Inserir(m02);
            repositorio.Inserir(m03);

            //action
            var medicamentoes = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, medicamentoes.Count);

            Assert.AreEqual(m01.Nome, medicamentoes[0].Nome);
            Assert.AreEqual(m02.Nome, medicamentoes[1].Nome);
            Assert.AreEqual(m03.Nome, medicamentoes[2].Nome);
        }
    }
}