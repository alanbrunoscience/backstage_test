using Bpo;

namespace Jazz.Covenant.Base.Builder.Bpo.ConsultaMargemCartao
{
    public class MargemCartaoBuilder
    {
        private MargemCartao _margemCartao;

        public MargemCartaoBuilder()
        {
            _margemCartao = new MargemCartao()
            {
                cpf = "65656565656",
                dataFimContrato = DateTime.Now,
                lotacao = "Teste-lotacao",
                matricula = "9898989898",
                motivoSituacao = "Teste-motivoSituacao",
                nome = "Teste-nome",
                secretaria = "Teste-secretaria",
                situacao = "Teste-situacao",
                tipoServidor = "Teste-tipoServidor",
                valorMargem = 11.22,
                verbaDeDescontos = "Teste-verbaDeDescontos"
            };
        }

        public MargemCartaoBuilder WithValorMargem(double valor)
        {
            _margemCartao.valorMargem = valor;
            return this;
        }

        public MargemCartao Build()
        {
            return _margemCartao;
        }
    }
}