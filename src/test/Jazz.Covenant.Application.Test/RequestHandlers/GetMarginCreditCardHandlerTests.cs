using System.Net;
using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Base;
using Jazz.Covenant.Domain.Dto.Adapters;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Xunit.Abstractions;

namespace Jazz.Covenant.Application.Test.RequestHandlers
{
    public class GetMarginCreditCardHandlerTests : TestBase
    {
        private readonly IEndoserAdapterService _endoserAdapterService;
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private readonly IEndoserAdapter _endoserAdapter;
        private readonly ICapPublisher _publisher;
        private readonly ApiResult<MarginCreditCardDto> _resultSuccess;
        private readonly ApiResult<MarginCreditCardDto> _resultMarginNegativa;
        private readonly ApiResult<MarginCreditCardDto> _resultMarginZero;


        public GetMarginCreditCardHandlerTests(ITestOutputHelper output)
            : base(output)
        {
            _endoserAdapterService = Substitute.For<IEndoserAdapterService>();
            _createEndoserAdapter = Substitute.For<ICreateEndoserAdapter>();
            _endoserAdapter = Substitute.For<IEndoserAdapter>();
            _publisher = Substitute.For<ICapPublisher>();
            _resultSuccess =new ApiResult<MarginCreditCardDto>(new MarginCreditCardDto(22.33), "", HttpStatusCode.OK);
            _resultMarginNegativa=new ApiResult<MarginCreditCardDto>(new MarginCreditCardDto(-22.33), "", HttpStatusCode.OK);
            _resultMarginZero=new ApiResult<MarginCreditCardDto>(new MarginCreditCardDto(0), "", HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetMarginCreditCardHandlerSucceded()
        {
            // Arrange
            var idCovenant = Guid.NewGuid();
            var cpf = "01289964122";
            var matricula = "teste-matricula";
            var covenantautorization = "teste-covenantautorization";
            var covenantEndoser = new ReadModels.CovenantEndorser() { IdentifierInEndoser = "98", EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO };
            var marginCreditCardDto = new MarginCreditCardDto(22.33);

            _endoserAdapterService.GetEndorserCovenant(idCovenant).Returns(covenantEndoser);

            _endoserAdapter.ConsultMarginCreditCard(covenantEndoser.IdentifierInEndoser, matricula, cpf, covenantautorization).Returns(_resultSuccess);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var find = new FindMarginCreditCardQuery(idCovenant.ToString(), cpf, matricula, covenantautorization);
            PrintPayload(find);

            var handler = new GetMarginCreditCardHandler(_createEndoserAdapter, _endoserAdapterService, _publisher);

            // Act
            var result = await handler.Handle(find, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<MarginCreditCardFound>(result);
            Assert.Equal(((MarginCreditCardFound)result).margin, marginCreditCardDto.Margin);
        }

        [Fact]
        public async Task WhenGetEndorserCovenantReturnNullShouldReturnNotFount()
        {
            var idCovenant = Guid.NewGuid();
            var cpf = "01289964122";
            var matricula = "teste-matricula";
            var covenantautorization = "teste-covenantautorization";

            _endoserAdapterService.GetEndorserCovenant(idCovenant).ReturnsNull();

            var find = new FindMarginCreditCardQuery(idCovenant.ToString(), cpf, matricula, covenantautorization);
            PrintPayload(find);

            var handler = new GetMarginCreditCardHandler(_createEndoserAdapter, _endoserAdapterService, _publisher);

            var result = await handler.Handle(find, CancellationToken.None);
            PrintResultJson(result);

            Assert.IsType<MarginCreditCardNotFound>(result);
        }

        [Fact]
        public async Task WhenConsultMarginCreditCardReturnNullShouldReturnNotFount()
        {
            // Arrange
            var idCovenant = Guid.NewGuid();
            var cpf = "01289964122";
            var matricula = "teste-matricula";
            var covenantautorization = "teste-covenantautorization";
            var covenantEndoser = new ReadModels.CovenantEndorser() { IdentifierInEndoser = "98", EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO };

            _endoserAdapterService.GetEndorserCovenant(idCovenant).ReturnsNull();

            _endoserAdapter.ConsultMarginCreditCard(idCovenant.ToString(), matricula, cpf, covenantautorization).Returns(_resultSuccess);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var find = new FindMarginCreditCardQuery(idCovenant.ToString(), cpf, matricula, covenantautorization);
            PrintPayload(find);

            var handler = new GetMarginCreditCardHandler(_createEndoserAdapter, _endoserAdapterService, _publisher);

            // Act
            var result = await handler.Handle(find, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<MarginCreditCardNotFound>(result);
            Assert.Equal(((MarginCreditCardNotFound)result).Message, $"Covenant with ID {idCovenant} not found.");
        }

        [Fact]
        public async Task WhenExceptionOccurShouldReturnFail()
        {
            // Arrange
            var idCovenant = Guid.NewGuid();
            var cpf = "01289964122";
            var matricula = "teste-matricula";
            var covenantautorization = "teste-covenantautorization";

            _endoserAdapterService.GetEndorserCovenant(idCovenant).Throws(new Exception("teste erro"));

            var find = new FindMarginCreditCardQuery(idCovenant.ToString(), cpf, matricula, covenantautorization);
            PrintPayload(find);

            var handler = new GetMarginCreditCardHandler(_createEndoserAdapter, _endoserAdapterService, _publisher);

            // Act
            var result = await handler.Handle(find, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<GetMarginCreditCardFail>(result);
            //Assert.Equal(((GetMarginCreditCardFail)result), $"Error Getting Margin Credit Card.");
        }

        [Fact]

        public async Task WithMarginNegative()
        {
            // Arrange
            var idCovenant = Guid.NewGuid();
            var cpf = "01289964122";
            var matricula = "teste-matricula";
            var covenantautorization = "teste-covenantautorization";
            var covenantEndoser = new ReadModels.CovenantEndorser() { IdentifierInEndoser = "98", EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO };
            var marginCreditCardDto = new MarginCreditCardDto(-22.33);

            _endoserAdapterService.GetEndorserCovenant(idCovenant).Returns(covenantEndoser);

            _endoserAdapter.ConsultMarginCreditCard(covenantEndoser.IdentifierInEndoser, matricula, cpf, covenantautorization).Returns(_resultMarginNegativa);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var find = new FindMarginCreditCardQuery(idCovenant.ToString(), cpf, matricula, covenantautorization);
            PrintPayload(find);

            var handler = new GetMarginCreditCardHandler(_createEndoserAdapter, _endoserAdapterService, _publisher);

            // Act
            var result = await handler.Handle(find, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<MarginCreditCardFound>(result);
            Assert.Equal(((MarginCreditCardFound)result).margin, marginCreditCardDto.Margin);
        }

        [Fact]

        public async Task WithMarginEqualZero()
        {
            // Arrange
            var idCovenant = Guid.NewGuid();
            var cpf = "01289964122";
            var matricula = "teste-matricula";
            var covenantautorization = "teste-covenantautorization";
            var covenantEndoser = new ReadModels.CovenantEndorser() { IdentifierInEndoser = "98", EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO };
            var marginCreditCardDto = new MarginCreditCardDto(0);

            _endoserAdapterService.GetEndorserCovenant(idCovenant).Returns(covenantEndoser);

            _endoserAdapter.ConsultMarginCreditCard(covenantEndoser.IdentifierInEndoser, matricula, cpf, covenantautorization).Returns(_resultMarginZero);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var find = new FindMarginCreditCardQuery(idCovenant.ToString(), cpf, matricula, covenantautorization);
            PrintPayload(find);

            var handler = new GetMarginCreditCardHandler(_createEndoserAdapter, _endoserAdapterService, _publisher);

            // Act
            var result = await handler.Handle(find, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<MarginCreditCardFound>(result);
            Assert.Equal(((MarginCreditCardFound)result).margin, marginCreditCardDto.Margin);
        }

    }
}
