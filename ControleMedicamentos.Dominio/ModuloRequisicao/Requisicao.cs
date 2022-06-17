using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class Requisicao : EntidadeBase<Requisicao>
    {
        public Requisicao()
        {
        }

        public Requisicao(Medicamento medicamento, Paciente paciente, int qtdMedicamento, Funcionario funcionario)
        {
            Medicamento = medicamento;
            Paciente = paciente;
            QtdMedicamento = qtdMedicamento;
            Data = DateTime.Now;
            Funcionario = funcionario;
        }
        public ModuloMedicamento.Medicamento Medicamento { get; set; }
        public ModuloPaciente.Paciente Paciente { get; set; }
        public int QtdMedicamento { get; set; }
        public DateTime Data { get; set; }
        public ModuloFuncionario.Funcionario Funcionario { get; set; }
    }
}
