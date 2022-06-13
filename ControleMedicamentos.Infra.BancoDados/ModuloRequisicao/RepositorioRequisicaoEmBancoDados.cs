using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoEmBancoDados
    {
        private string enderecoBanco =
                 @"Data Source=(LOCALDB)\MSSQLLOCALDB;
              Initial Catalog=ControleMedicamentosDb;
              Integrated Security=True";

        private const string sqlInserir =
          @"INSERT INTO [TBREQUISICAO] 
                (
                    [FUNCIONARIO_ID],
                    [PACIENTE_ID],
                    [MEDICAMENTO_ID],
                    [QUANTIDADE_MEDICAMENTO],
                    [DATA]
	            )
	            VALUES
                (
                    @FUNCIONARIO_ID,
                    @PACIENTE_ID,
                    @MEDICAMENTO_ID,
                    @QUANTIDADE_MEDICAMENTO,
                    @DATA


                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
           @"UPDATE [TBREQUISICAO]	
		        SET
			        [FUNCIONARIO_ID] = @FUNCIONARIO_ID,
			        [PACIENTE_ID] = @PACIENTE_ID,
			        [MEDICAMENTO_ID] = @MEDICAMENTO_ID,
			        [QUANTIDADE_MEDICAMENTO] = @QUANTIDADE_MEDICAMENTO,
			        [DATA] = @DATA



		        WHERE
			        [ID] = @ID";


        private const string sqlExcluir =
           @"DELETE FROM [TBREQUISICAO]			        
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarPorId =
          @"SELECT 
		            [ID], 
		            [FUNCIONARIO_ID],
                    [PACIENTE_ID],
                    [MEDICAMENTO_ID],
                    [QUANTIDADE_MEDICAMENTO],
                    [DATA]
	            FROM 
		            [TBREQUISICAO]
		        WHERE
                    [ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT 
		            [ID], 
		            [FUNCIONARIO_ID],
                    [PACIENTE_ID],
                    [MEDICAMENTO_ID],
                    [QUANTIDADE_MEDICAMENTO],
                    [DATA]
	            FROM 
		            [TBREQUISICAO]";

        public ValidationResult Inserir(Requisicao requisicao)
        {
            var validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosRequisicao(requisicao, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            requisicao.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Requisicao requisicao)
        {
            var validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosRequisicao(requisicao, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Requisicao requisicao)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", requisicao.Id);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public Requisicao SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            Requisicao requisicao = null;
            if (leitorRequisicao.Read())
                requisicao = ConverterParaRequisicao(leitorRequisicao);

            conexaoComBanco.Close();

            return requisicao;
        }

        public List<Requisicao> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);
            conexaoComBanco.Open();

            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            List<Requisicao> requisicoes = new List<Requisicao>();

            while (leitorRequisicao.Read())
                requisicoes.Add(ConverterParaRequisicao(leitorRequisicao));

            conexaoComBanco.Close();

            return requisicoes;
        }

        private void ConfigurarParametrosRequisicao(Requisicao requisicao, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", requisicao.Id);
            comando.Parameters.AddWithValue("FUNCIONARIO_ID", requisicao.Funcionario.Id);
            comando.Parameters.AddWithValue("PACIENTE_ID", requisicao.Paciente.Id);
            comando.Parameters.AddWithValue("MEDICAMENTO_ID", requisicao.Medicamento.Id);
            comando.Parameters.AddWithValue("QUANTIDADE_MEDICAMENTO", requisicao.QtdMedicamento);
            comando.Parameters.AddWithValue("DATA", requisicao.Data);
        }

        private Requisicao ConverterParaRequisicao(SqlDataReader leitorRequisicao)
        {
            int id = Convert.ToInt32(leitorRequisicao["ID"]);
            int funcionarioId = Convert.ToInt32(leitorRequisicao["FUNCIONARIO_ID"]);
            int pacienteId = Convert.ToInt32(leitorRequisicao["PACIENTE_ID"]);
            int medicamentoId = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_ID"]);
            int quantidadeMedicamento = Convert.ToInt32(leitorRequisicao["QUANTIDADE_MEDICAMENTO"]);
            DateTime data = Convert.ToDateTime(leitorRequisicao["DATA"]);

            RepositorioFuncionarioEmBancoDados repositorioFuncionario = new RepositorioFuncionarioEmBancoDados();
            RepositorioPacienteEmBancoDados repositorioPaciente = new RepositorioPacienteEmBancoDados();
            RepositorioMedicamentoEmBancoDados repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();

            return new Requisicao()
            {
                Id = id,
                Funcionario = repositorioFuncionario.SelecionarPorId(funcionarioId),
                Paciente = repositorioPaciente.SelecionarPorId(pacienteId),
                Medicamento = repositorioMedicamento.SelecionarPorId(medicamentoId),
                QtdMedicamento = quantidadeMedicamento,
                Data = data,
            };
        }
    }
}