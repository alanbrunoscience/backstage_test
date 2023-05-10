using AutoFixture;
using Bpo;

namespace Jazz.Covenant.Base.Builder.Bpo.MarginReserve
{
    public class IncluiEmprestimoResponseBuilder
    {
        private incluiEmprestimoResponse _incluiEmprestimoResponse;

        public IncluiEmprestimoResponseBuilder()
        {
            var fixture = new Fixture();

            _incluiEmprestimoResponse = fixture.Create<incluiEmprestimoResponse>();
            _incluiEmprestimoResponse.incluiEmprestimoReturn = fixture.Create<ResultadoInclusaoEmprestimo>();
        }

        public incluiEmprestimoResponse Build()
        {
            return _incluiEmprestimoResponse;
        }
    }
}