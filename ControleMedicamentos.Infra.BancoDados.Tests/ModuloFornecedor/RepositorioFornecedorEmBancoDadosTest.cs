using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]

    public class RepositorioFornecedorEmBancoDadosTest
    {
        private Fornecedor fornecedor;
        private RepositorioFornecedorEmBancoDados repositorio;

        public RepositorioFornecedorEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT(TBMEDICAMENTO, RESEED, 0) " +
                "DELETE FROM TBFORNECEDOR;DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");

            fornecedor = new Fornecedor("nomefornecedor1",  "telefonefornecedor1",  "emailfornecedor1",  "cidadefornecedor1", "estadofornecedor1");
            repositorio = new RepositorioFornecedorEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_fornecedor()
        {
            //action
            repositorio.Inserir(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor.Id, fornecedorEncontrado.Id);
            Assert.AreEqual(fornecedor.Nome, fornecedorEncontrado.Nome);
            Assert.AreEqual(fornecedor.Telefone, fornecedorEncontrado.Telefone);
            Assert.AreEqual(fornecedor.Email, fornecedorEncontrado.Email);
            Assert.AreEqual(fornecedor.Cidade, fornecedorEncontrado.Cidade);
            Assert.AreEqual(fornecedor.Estado, fornecedorEncontrado.Estado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_fornecedor()
        {
            //arrange                      
            repositorio.Inserir(fornecedor);
            //action
            fornecedor.Nome = "NomeJoão de Moraes";
            fornecedor.Telefone = "Telefong987654321";
            fornecedor.Email = "Email987654321";
            fornecedor.Cidade = "Cidade987654321";
            fornecedor.Estado = "Estado87654321";
            repositorio.Editar(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor.Id, fornecedorEncontrado.Id);
            Assert.AreEqual(fornecedor.Nome, fornecedorEncontrado.Nome);
            Assert.AreEqual(fornecedor.Telefone, fornecedorEncontrado.Telefone);
            Assert.AreEqual(fornecedor.Email, fornecedorEncontrado.Email);
            Assert.AreEqual(fornecedor.Cidade, fornecedorEncontrado.Cidade);
            Assert.AreEqual(fornecedor.Estado, fornecedorEncontrado.Estado);
        }

        [TestMethod]
        public void Deve_excluir_fornecedor()
        {
            //arrange           
            repositorio.Inserir(fornecedor);

            //action           
            repositorio.Excluir(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);
            Assert.IsNull(fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_fornecedor()
        {
            //arrange          
            repositorio.Inserir(fornecedor);

            //action
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            //assert
            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor.Id, fornecedorEncontrado.Id);
            Assert.AreEqual(fornecedor.Nome, fornecedorEncontrado.Nome);
            Assert.AreEqual(fornecedor.Telefone, fornecedorEncontrado.Telefone);
            Assert.AreEqual(fornecedor.Email, fornecedorEncontrado.Email);
            Assert.AreEqual(fornecedor.Cidade, fornecedorEncontrado.Cidade);
            Assert.AreEqual(fornecedor.Estado, fornecedorEncontrado.Estado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_os_fornecedores()
        {
            //arrange
            var p01 = new Fornecedor("Alberto da Silva", "telefonefornecedor2", "emailfornecedor2", "cidadefornecedor2", "estadofornecedor2");
            var p02 = new Fornecedor("Maria do Carmo", "telefonefornecedor3", "emailfornecedor3", "cidadefornecedor3", "estadofornecedor3");
            var p03 = new Fornecedor("Patricia Amorim", "telefonefornecedor4", "emailfornecedor4", "cidadefornecedor4", "estadofornecedor4");

            var repositorio = new RepositorioFornecedorEmBancoDados();
            repositorio.Inserir(p01);
            repositorio.Inserir(p02);
            repositorio.Inserir(p03);

            //action
            var fornecedores = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, fornecedores.Count);

            Assert.AreEqual(p01.Nome, fornecedores[0].Nome);
            Assert.AreEqual(p02.Nome, fornecedores[1].Nome);
            Assert.AreEqual(p03.Nome, fornecedores[2].Nome);
        }
    }
}
