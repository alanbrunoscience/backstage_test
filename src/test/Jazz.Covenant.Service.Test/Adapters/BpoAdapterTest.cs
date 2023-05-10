using System.Net;
using Bpo;
using Jazz.Core;
using Jazz.Covenant.Base.Builder.Bpo;
using Jazz.Covenant.Service.Adapters;
using Jazz.Covenant.Service.BpoService;
using NSubstitute.ReturnsExtensions;

namespace Jazz.Covenant.Service.Test.adapters
{
    public class BpoAdapterTest
    {
        private IBpoService _bpoService;

        public BpoAdapterTest()
        {
            _bpoService = Substitute.For<IBpoService>();
        }
        private ConsultaMargemConsignavelResponseBuilder ReturnBuild()
        {
            return new ConsultaMargemConsignavelResponseBuilder()
                      .WithResultadoConsultaMargens(new[]  {

                                            new ResultadoConsultaMargemBuilder().WithMargens(new[]
                                                {
                                                    new MargemInfoBuilder().WithValorDisponivel(500).Build()
                                                    }).Build()
          }
          );
        }
        [Fact]

        public async Task DeveRetornaMargemValor500DoClienteNoConsignavel()
        {

                var resultado = ReturnBuild();
                _bpoService.ConsultMarginConsignableByCPFEnrollmentAutorization("idid", "9898989898", "cpf", "asss").ReturnsForAnyArgs(new ApiResult<consultaMargemConsignavelResponse?>(resultado.Build(),"",HttpStatusCode.OK));
                var bpoAdapter = new BpoAdapter(_bpoService);
                var margem = await bpoAdapter.ConsultMarginLoan("idid", "cpf", "9898989898", "asss");
                Assert.Equal(500, margem.Result.Margin);
        }


        [Fact]
        public async Task DeveRetornaMargemErroDeMatriculaDiferentes()
        {

          var resultado = ReturnBuild();
           _bpoService.ConsultMarginConsignableByCPFEnrollmentAutorization("idid", "9898989898", "cpf", "asss").ReturnsForAnyArgs(new ApiResult<consultaMargemConsignavelResponse?>(resultado.Build(),"",HttpStatusCode.BadRequest));
           var bpoAdapter = new BpoAdapter(_bpoService);
           var result = await bpoAdapter.ConsultMarginLoan("idid", "cpf", "983219389", "dkldsk");
           var domainMessage = new DomainMessage("Enrollment not exist", "Enrollment not exist on this TaxId");
          Assert.True(result.Errors.ToList().Contains(domainMessage));
        }


    }
}
