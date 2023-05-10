
using Bpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Base.Builder.Bpo
{
    public class ResultadoConsultaMargemBuilder
    {
        private ResultadoConsultaMargem resultadoConsulta;

        public ResultadoConsultaMargemBuilder()
        {
            resultadoConsulta = new ResultadoConsultaMargem()
            {
                cpf = "65656565656",
                dataFimContrato = DateTime.Now,
                matricula = "9898989898",
                nome = "teste",
                secretaria = "testeSecretaria",
                margens = new[] { new MargemInfoBuilder().Build() },
                lotacao = "TesteLotacao"



            };
        }
        public ResultadoConsultaMargemBuilder WithCpf(string cpf)
        {
            resultadoConsulta.cpf = cpf;
            return this;
        }
        public ResultadoConsultaMargemBuilder WithDataFimContrato(DateTime dataFimContrato)
        {
            resultadoConsulta.dataFimContrato = dataFimContrato;
            return this;
        }
        public ResultadoConsultaMargemBuilder WithMatricula(string matricula)
        {
            resultadoConsulta.matricula = matricula;
            return this;
        }
        public ResultadoConsultaMargemBuilder WithSecretaria(string secretaria)
        {
            resultadoConsulta.secretaria = secretaria;
            return this;
        }
        public ResultadoConsultaMargemBuilder WithMargens(MargemInfo[] margens)
        {
            resultadoConsulta.margens = margens;
            return this;
        }
        public ResultadoConsultaMargemBuilder WithLotacao(string lotacao)
        {
            resultadoConsulta.lotacao = lotacao;
            return this;
        }
        public ResultadoConsultaMargem Build()
        {
            return resultadoConsulta;
        }
    }
}
