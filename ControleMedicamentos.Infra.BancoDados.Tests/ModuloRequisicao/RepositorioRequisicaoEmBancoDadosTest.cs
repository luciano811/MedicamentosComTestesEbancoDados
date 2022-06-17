using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisicao
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDadosTest
    {
        private Medicamento medicamento;
        private Funcionario funcionario;
        private Fornecedor fornecedor;
        private Paciente paciente;
        private Requisicao requisicao;
        private RepositorioMedicamentoEmBancoDados repositorioMedicamento;
        private RepositorioFornecedorEmBancoDados repositorioFornecedor;
        private RepositorioFuncionarioEmBancoDados repositorioFuncionario;
        private RepositorioPacienteEmBancoDados repositorioPaciente;
        private RepositorioRequisicaoEmBancoDados repositorioRequisicao;


        public RepositorioRequisicaoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0) " +
                "DELETE FROM TBMEDICAMENTO;DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0) " +
                "DELETE FROM TBPACIENTE;DBCC CHECKIDENT (TBPACIENTE, RESEED, 0) " +
                "DELETE FROM TBFUNCIONARIO;DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");
            fornecedor = new Fornecedor("nomefornecedor1", "telefonefornecedor1", "emailfornecedor1", "cidadefornecedor1", "estadofornecedor1");
            paciente = new Paciente("José da Silva", "321654987");
            funcionario = new Funcionario("José da SILVA", "LOGIN123", "SENHA321654987");
            medicamento = new Medicamento("Nome teste 1", "Descricao teste 1", "Lote teste 1", DateTime.Now.Date, 60, fornecedor);
            requisicao = new Requisicao(medicamento, paciente, 5, funcionario);
            repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
            repositorioFornecedor = new RepositorioFornecedorEmBancoDados();
            repositorioFuncionario = new RepositorioFuncionarioEmBancoDados();
            repositorioPaciente = new RepositorioPacienteEmBancoDados();
            repositorioRequisicao = new RepositorioRequisicaoEmBancoDados();

        }

        [TestMethod]
        public void Deve_inserir_nova_requisicao()
        {
            //arrange
            repositorioFuncionario.Inserir(funcionario);
            repositorioPaciente.Inserir(paciente);
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);

            //action
            repositorioRequisicao.Inserir(requisicao);

            //assert
            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorId(requisicao.Id);

            Assert.IsNotNull(requisicaoEncontrada);
            Assert.AreEqual(requisicao.Id, requisicaoEncontrada.Id);
            Assert.AreEqual(requisicao.Funcionario.Id, requisicaoEncontrada.Funcionario.Id);
            Assert.AreEqual(requisicao.Paciente.Id, requisicaoEncontrada.Paciente.Id);
            Assert.AreEqual(requisicao.Medicamento.Id, requisicaoEncontrada.Medicamento.Id);
            Assert.AreEqual(requisicao.QtdMedicamento, requisicaoEncontrada.QtdMedicamento);
            Assert.AreEqual(requisicao.Data.Date, requisicaoEncontrada.Data.Date);
        }

        [TestMethod]
        public void Deve_editar_informacoes_requisicao()
        {
            //arrange                      
            repositorioFuncionario.Inserir(funcionario);
            repositorioPaciente.Inserir(paciente);
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);

            repositorioRequisicao.Inserir(requisicao);

            //action
            requisicao.QtdMedicamento = 99;
            requisicao.Data = DateTime.Now.AddDays(30).Date;
            repositorioRequisicao.Editar(requisicao);

            //assert
            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorId(requisicao.Id);

            Assert.IsNotNull(requisicaoEncontrada);
            Assert.AreEqual(requisicao.QtdMedicamento, requisicaoEncontrada.QtdMedicamento);
            Assert.AreEqual(requisicao.Data, requisicaoEncontrada.Data);
        }

        [TestMethod]
        public void Deve_excluir_requisicao()
        {
            //arrange                      
            repositorioFuncionario.Inserir(funcionario);
            repositorioPaciente.Inserir(paciente);
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);
            repositorioRequisicao.Inserir(requisicao);

            //action           
            repositorioRequisicao.Excluir(requisicao);

            //assert
            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorId(requisicao.Id);
            Assert.IsNull(requisicaoEncontrada);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_uma_requisicao()
        {
            //arrange                      
            repositorioFuncionario.Inserir(funcionario);
            repositorioPaciente.Inserir(paciente);
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);
            repositorioRequisicao.Inserir(requisicao);

            //action
            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorId(requisicao.Id);

            //assert
            Assert.IsNotNull(requisicaoEncontrada);
            Assert.AreEqual(requisicao.Id, requisicaoEncontrada.Id);
            Assert.AreEqual(requisicao.Funcionario.Id, requisicaoEncontrada.Funcionario.Id);
            Assert.AreEqual(requisicao.Paciente.Id, requisicaoEncontrada.Paciente.Id);
            Assert.AreEqual(requisicao.Medicamento.Id, requisicaoEncontrada.Medicamento.Id);
            Assert.AreEqual(requisicao.QtdMedicamento, requisicaoEncontrada.QtdMedicamento);
            Assert.AreEqual(requisicao.Data.Date, requisicaoEncontrada.Data.Date);
        }
    

        [TestMethod]
        public void Deve_selecionar_todas_as_requisicoes()
        {
            //arrange                      
            repositorioFuncionario.Inserir(funcionario);
            repositorioPaciente.Inserir(paciente);
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);
            repositorioRequisicao.Inserir(requisicao);

            Requisicao r1 = new Requisicao(medicamento, paciente, 1, funcionario);
            repositorioRequisicao.Inserir(r1);

            Requisicao r2 = new Requisicao(medicamento, paciente, 2, funcionario);
            repositorioRequisicao.Inserir(r2);

            Requisicao r3 = new Requisicao(medicamento, paciente, 3, funcionario);
            repositorioRequisicao.Inserir(r3);

            //action
            var requisicoes = repositorioRequisicao.SelecionarTodos();

            //assert
            //aqui em baixo são 4 porque uma eu crio no construtor
            Assert.AreEqual(4, requisicoes.Count);

            Assert.AreEqual(r1.Data.Date, requisicoes[0].Data.Date);
            Assert.AreEqual(r2.Data.Date, requisicoes[1].Data.Date);
            Assert.AreEqual(r3.Data.Date, requisicoes[2].Data.Date);
        }
    }
}