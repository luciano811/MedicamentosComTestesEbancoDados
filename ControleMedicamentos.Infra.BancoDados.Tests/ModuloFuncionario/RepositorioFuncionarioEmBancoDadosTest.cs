using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        private Funcionario funcionario;
        private RepositorioFuncionarioEmBancoDados repositorio;

        public RepositorioFuncionarioEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");

            funcionario = new Funcionario("José da SILVA","LOGIN123", "SENHA321654987");
            repositorio = new RepositorioFuncionarioEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_funcionario()
        {
            //action
            repositorio.Inserir(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorId(funcionario.Id);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario.Id, funcionarioEncontrado.Id);
            Assert.AreEqual(funcionario.Nome, funcionarioEncontrado.Nome);
            Assert.AreEqual(funcionario.Login, funcionarioEncontrado.Login);
            Assert.AreEqual(funcionario.Senha, funcionarioEncontrado.Senha);
        }

        [TestMethod]
        public void Deve_editar_informacoes_funcionario()
        {
            //arrange                      
            repositorio.Inserir(funcionario);

            //action
            funcionario.Nome = "João de Moraes";
            funcionario.Login = "log987654321";
            funcionario.Senha = "sen987654321";
            repositorio.Editar(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorId(funcionario.Id);
                        
            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario.Id, funcionarioEncontrado.Id);
            Assert.AreEqual(funcionario.Nome, funcionarioEncontrado.Nome);
            Assert.AreEqual(funcionario.Login, funcionarioEncontrado.Login);
            Assert.AreEqual(funcionario.Senha, funcionarioEncontrado.Senha);
        }

        [TestMethod]
        public void Deve_excluir_funcionario()
        {
            //arrange           
            repositorio.Inserir(funcionario);

            //action           
            repositorio.Excluir(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorId(funcionario.Id);
            Assert.IsNull(funcionarioEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_funcionario()
        {
            //arrange          
            repositorio.Inserir(funcionario);

            //action
            var pacienteEncontrado = repositorio.SelecionarPorId(funcionario.Id);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorId(funcionario.Id);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario.Id, funcionarioEncontrado.Id);
            Assert.AreEqual(funcionario.Nome, funcionarioEncontrado.Nome);
            Assert.AreEqual(funcionario.Login, funcionarioEncontrado.Login);
            Assert.AreEqual(funcionario.Senha, funcionarioEncontrado.Senha);
        }

        [TestMethod]
        public void Deve_selecionar_todos_os_funcionarios()
        {
            //arrange
            var p01 = new Funcionario("Alberto da Silva","login123", "321654987");
            var p02 = new Funcionario("Maria do Carmo", "login123", "321654987");
            var p03 = new Funcionario("Patricia Amorim", "login123", "321654987");

            var repositorio = new RepositorioFuncionarioEmBancoDados();
            repositorio.Inserir(p01);
            repositorio.Inserir(p02);
            repositorio.Inserir(p03);

            //action
            var funcionarios = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, funcionarios.Count);

            Assert.AreEqual(p01.Nome, funcionarios[0].Nome);
            Assert.AreEqual(p02.Nome, funcionarios[1].Nome);
            Assert.AreEqual(p03.Nome, funcionarios[2].Nome);
        }
    }
}
