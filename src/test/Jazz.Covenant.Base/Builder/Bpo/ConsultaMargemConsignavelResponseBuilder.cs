
using Bpo;
namespace Jazz.Covenant.Base.Builder.Bpo
{
    public class ConsultaMargemConsignavelResponseBuilder
    {
        private consultaMargemConsignavelResponse consultaMargemConsignavelResponse;

        public ConsultaMargemConsignavelResponseBuilder()
        {
            consultaMargemConsignavelResponse = new consultaMargemConsignavelResponse()
            {
                consultaMargemConsignavelReturn = new[] { new ResultadoConsultaMargemBuilder().Build() }
            };
        }

        public ConsultaMargemConsignavelResponseBuilder WithResultadoConsultaMargens(ResultadoConsultaMargem[] resultadoConsultaMargems)
        {
            consultaMargemConsignavelResponse.consultaMargemConsignavelReturn = resultadoConsultaMargems;
            return this;
        }
        public consultaMargemConsignavelResponse Build()
        {
            return consultaMargemConsignavelResponse;
        }
    }
}
