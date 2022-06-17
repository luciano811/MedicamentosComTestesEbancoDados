using ControleMedicamentos.Dominio.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class FornecedorTest
    {
        public FornecedorTest()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR");
        }

        [TestMethod]
        public void Nome_deve_ser_obrigatorio()
        {
            var f = new Fornecedor();
            f.Nome = null;

            ValidadorFornecedor validador = new ValidadorFornecedor();

            var resultado = validador.Validate(f);
            //errors.count acusando 10, por que?
            Assert.AreEqual("'Nome' não pode ser nulo.", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Telefone_deve_ser_obrigatorio()
        {
            var f = new Fornecedor();
            f.Nome = "Nome teste";
            f.Telefone = null;

            ValidadorFornecedor validador = new ValidadorFornecedor();

            var resultado = validador.Validate(f);

            Assert.AreEqual("'Telefone' não pode ser nulo.", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Email_deve_ser_obrigatorio()
        {
            var f = new Fornecedor();
            f.Nome = "Nome teste";
            f.Telefone = "Telefone teste";
            f.Email = null;

            ValidadorFornecedor validador = new ValidadorFornecedor();

            var resultado = validador.Validate(f);

            Assert.AreEqual("'Email' não pode ser nulo.", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Cidade_deve_ser_obrigatoria()
        {
            var f = new Fornecedor();
            f.Nome = "Nome teste";
            f.Telefone = "Telefone teste";
            f.Email = "Email teste";
            f.Cidade = null;

            ValidadorFornecedor validador = new ValidadorFornecedor();

            var resultado = validador.Validate(f);

            Assert.AreEqual("'Cidade' não pode ser nulo.", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Estado_deve_ser_obrigatorio()
        {
            var f = new Fornecedor();
            f.Nome = "Nome teste";
            f.Telefone = "Telefone teste";
            f.Email = "Email teste";
            f.Cidade = "Cidade teste";
            f.Estado = null;

            ValidadorFornecedor validador = new ValidadorFornecedor();

            var resultado = validador.Validate(f);

            Assert.AreEqual("'Estado' não pode ser nulo.", resultado.Errors[0].ErrorMessage);
        }

    }
}
