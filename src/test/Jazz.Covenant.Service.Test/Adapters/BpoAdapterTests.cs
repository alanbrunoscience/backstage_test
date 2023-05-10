using System.Net;
using Bpo;
using Jazz.Core;
using Jazz.Covenant.Base.Builder.Bpo.ConsultaMargemCartao;
using Jazz.Covenant.Base.Builder.Bpo.MarginReserve;
using Jazz.Covenant.Base.Builder.MarginReserve;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Jazz.Covenant.Service.Adapters;
using Jazz.Covenant.Service.BpoService;

namespace Jazz.Covenant.Service.Test.Adapters
{
    public class BpoAdapterTests
    {
        private readonly IBpoService _bpoService;

        public BpoAdapterTests()
        {
            _bpoService = Substitute.For<IBpoService>();
        }

        [Fact]
        public async Task GetMarginCreditCardSucceded()
        {
            // Arrange
            var idCovenant = Guid.NewGuid();
            var cpf = "teste-cpf";
            var matricula = "9898989898";
            var covenantautorization = "teste-covenantautorization";

            var margin = new ConsultaMargemCartaoResponseBuilder().WithMargemCartao(new MargemCartaoBuilder().Build()).Build();

            _bpoService.ConsultMarginCreditCard(idCovenant.ToString(), matricula, cpf, covenantautorization).Returns(new ApiResult<consultaMargemCartaoResponse>(margin,"",HttpStatusCode.OK));

            IEndoserAdapter bpoAdapter = new BpoAdapter(_bpoService);

            // Act
            var dto = await bpoAdapter.ConsultMarginCreditCard(idCovenant.ToString(), matricula, cpf, covenantautorization);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(margin.consultaMargemCartaoReturn.valorMargem, dto.Result.Margin);
        }

        [Fact]
        public async Task MarginReserveSucceded()
        {
            // Arrange
            var identifierInEndoser = "identifierInEndoser";

            var BPOIncluiEmprestimoRequest = new IncluiEmprestimoRequestBuilder().Build();
            var BPOIncluiEmprestimoResponse = new IncluiEmprestimoResponseBuilder().Build();

            var marginReserveDtoRequest = new MarginReserveDtoRequestBuilder().Build();

            _bpoService.MarginReserve(Arg.Any<incluiEmprestimoRequest>(), Arg.Any<string>()).Returns(BPOIncluiEmprestimoResponse);

            IEndoserAdapter bpoAdapter = new BpoAdapter(_bpoService);

            // Act
            var dto = await bpoAdapter.MarginReserve(marginReserveDtoRequest, identifierInEndoser);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(BPOIncluiEmprestimoResponse.incluiEmprestimoReturn.mensagemErro, dto.ErrorMessage);
        }
    }
}
