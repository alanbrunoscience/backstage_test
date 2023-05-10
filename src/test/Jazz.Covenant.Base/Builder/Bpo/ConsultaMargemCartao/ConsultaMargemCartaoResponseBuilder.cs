using Bpo;

namespace Jazz.Covenant.Base.Builder.Bpo.ConsultaMargemCartao
{
    public class ConsultaMargemCartaoResponseBuilder
    {
        private consultaMargemCartaoResponse _consultaMargemConsignavelResponse;

        public ConsultaMargemCartaoResponseBuilder()
        {
            _consultaMargemConsignavelResponse = new consultaMargemCartaoResponse(new MargemCartaoBuilder().Build());
        }

        public ConsultaMargemCartaoResponseBuilder WithMargemCartao(MargemCartao margemCartao)
        {
            _consultaMargemConsignavelResponse.consultaMargemCartaoReturn = margemCartao;
            return this;
        }

        public consultaMargemCartaoResponse Build()
        {
            return _consultaMargemConsignavelResponse;
        }
    }
}