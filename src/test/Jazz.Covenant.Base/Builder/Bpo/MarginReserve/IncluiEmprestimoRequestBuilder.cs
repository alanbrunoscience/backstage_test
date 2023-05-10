using AutoFixture;
using Bpo;

namespace Jazz.Covenant.Base.Builder.Bpo.MarginReserve
{
    public class IncluiEmprestimoRequestBuilder
    {
        private incluiEmprestimoRequest _incluiEmprestimoRequest;

        public IncluiEmprestimoRequestBuilder()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _incluiEmprestimoRequest = fixture.Create<incluiEmprestimoRequest>();
        }

        public IncluiEmprestimoRequestBuilder ComMatricula(string matricula)
        {
            _incluiEmprestimoRequest.matricula = matricula;
            return this;
        }

        public incluiEmprestimoRequest Build()
        {
            return _incluiEmprestimoRequest;
        }
    }
}