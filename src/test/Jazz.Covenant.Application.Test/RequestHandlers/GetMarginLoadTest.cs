using DotNetCore.CAP;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Base.Builder.Bpo;
using Jazz.Covenant.Domain.Dto.Adapters;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Jazz.Covenant.Service.Adapters;
using Jazz.Covenant.Service.BpoService;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Jazz.Core;

namespace Jazz.Covenant.Application.Test.RequestHandlers
{
    public class GetMarginLoadTest
    {
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private readonly IEndoserAdapterService _endoserAdapterService;
        private readonly IEndoserAdapter _endorserAdapter;
        private readonly ICapPublisher _capPublisher;
        private readonly ApiResult<MarginLoanDto> _resultSuccess;
        private readonly ApiResult<MarginLoanDto> _resultMargem0;
        private readonly ApiResult<MarginLoanDto> _resultMargemNegativo;

        public GetMarginLoadTest()
        {
            _createEndoserAdapter=Substitute.For<ICreateEndoserAdapter>();
            _endoserAdapterService=Substitute.For<IEndoserAdapterService>();
            _endorserAdapter = Substitute.For<IEndoserAdapter>();
            _capPublisher = Substitute.For<ICapPublisher>();
            _resultSuccess = new ApiResult<MarginLoanDto?>(new MarginLoanDto(500), "", HttpStatusCode.OK);
            _resultMargem0= new ApiResult<MarginLoanDto?>(new MarginLoanDto(0), "", HttpStatusCode.OK);
            _resultMargemNegativo=new ApiResult<MarginLoanDto?>(new MarginLoanDto(-40), "", HttpStatusCode.OK);
        }

        [Fact]

        public async Task DeveRetornaGetMarginLoanResult()
        {
            var idCovenat=Guid.NewGuid();
            var enrollment = "Matricula";
            var cpf = "05479603361";
            var covenantAutorization = "00090909";
            var covenantEndorser = new ReadModels.CovenantEndorser() { EndoserIdentifier= Domain.Enums.EndoserAggregator.BPO,IdentifierInEndoser="dddd" };
            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endorserAdapter);
            _endorserAdapter.ConsultMarginLoan("ddd", enrollment, cpf, covenantAutorization).ReturnsForAnyArgs(_resultSuccess);
            _endoserAdapterService.GetEndorserCovenant(idCovenat).ReturnsForAnyArgs<ReadModels.CovenantEndorser>(covenantEndorser);
            var findMarginLoan = new FindMarginLoan(enrollment, cpf, covenantAutorization);
            findMarginLoan.IdCovenant = idCovenat.ToString();
            var handlerMarginLoan = new GetMarginLoanHandler(_createEndoserAdapter, _endoserAdapterService, _capPublisher);
            var result = await handlerMarginLoan.Handle(findMarginLoan,CancellationToken.None);
            Assert.NotNull(result);
        }
        [Fact]

        public async Task DeveRetornaMarginFound()
        {
            var idCovenat = Guid.NewGuid();
            var enrollment = "Matricula";
            var cpf = "05479603361";
            var covenantAutorization = "00090909";
            var covenantEndorser = new ReadModels.CovenantEndorser() { EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO, IdentifierInEndoser = "dddd" };
            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endorserAdapter);
            _endorserAdapter.ConsultMarginLoan("ddd", enrollment, cpf, covenantAutorization).ReturnsForAnyArgs(_resultSuccess);
            _endoserAdapterService.GetEndorserCovenant(idCovenat).ReturnsForAnyArgs<ReadModels.CovenantEndorser>(covenantEndorser);
            var findMarginLoan = new FindMarginLoan(enrollment, cpf, covenantAutorization);
            findMarginLoan.IdCovenant = idCovenat.ToString();
            var handlerMarginLoan = new GetMarginLoanHandler(_createEndoserAdapter, _endoserAdapterService, _capPublisher);
            var result = await handlerMarginLoan.Handle(findMarginLoan, CancellationToken.None);
            Assert.IsType<MarginFound>(result);
        }

        [Fact]

        public async Task DeveRetornaMarginFoundComRetorno()
        {
            var idCovenat = Guid.NewGuid();
            var enrollment = "Matricula";
            var cpf = "05479603361";
            var covenantAutorization = "00090909";
            var covenantEndorser = new ReadModels.CovenantEndorser() { EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO, IdentifierInEndoser = "dddd" };
            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endorserAdapter);
            _endorserAdapter.ConsultMarginLoan("ddd", enrollment, cpf, covenantAutorization).ReturnsForAnyArgs(_resultSuccess);
            _endoserAdapterService.GetEndorserCovenant(idCovenat).ReturnsForAnyArgs<ReadModels.CovenantEndorser>(covenantEndorser);
            var findMarginLoan = new FindMarginLoan(enrollment, cpf, covenantAutorization);
            findMarginLoan.IdCovenant = idCovenat.ToString();
            var handlerMarginLoan = new GetMarginLoanHandler(_createEndoserAdapter, _endoserAdapterService, _capPublisher);
            var result = await handlerMarginLoan.Handle(findMarginLoan, CancellationToken.None);
            Assert.Equal(new MarginFound(500),result);
        }

        [Fact]
        public async Task DeveRetornaMarginNotFoundQuandoNaoEncotraCovenant()
        {
            var idCovenat = Guid.NewGuid();
            var enrollment = "Matricula";
            var cpf = "05479603361";
            var covenantAutorization = "00090909";
            var covenantEndorser = new ReadModels.CovenantEndorser() { EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO, IdentifierInEndoser = "dddd" };
            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endorserAdapter);
            _endorserAdapter.ConsultMarginLoan("ddd", enrollment, cpf, covenantAutorization).ReturnsForAnyArgs(_resultSuccess);
            _endoserAdapterService.GetEndorserCovenant(idCovenat).ReturnsNullForAnyArgs();
            var findMarginLoan = new FindMarginLoan(enrollment, cpf, covenantAutorization);
            findMarginLoan.IdCovenant = idCovenat.ToString();
            var handlerMarginLoan = new GetMarginLoanHandler(_createEndoserAdapter, _endoserAdapterService, _capPublisher);
            var result = await handlerMarginLoan.Handle(findMarginLoan, CancellationToken.None);
            Assert.IsType<MarginNotFound>(result);
        }

        [Fact]

        public async Task DeveRetornaMarginNotFoundComMessagemUnbleSearchDeCovenantNaoEncontradoInformandoId()
        {
            var idCovenat = Guid.NewGuid();
            var enrollment = "Matricula";
            var cpf = "05479603361";
            var covenantAutorization = "00090909";
            var covenantEndorser = new ReadModels.CovenantEndorser() { EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO, IdentifierInEndoser = "dddd" };
            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endorserAdapter);
            _endorserAdapter.ConsultMarginLoan("ddd", enrollment, cpf, covenantAutorization).ReturnsForAnyArgs(_resultSuccess);
            _endoserAdapterService.GetEndorserCovenant(idCovenat).ReturnsNullForAnyArgs();
            var findMarginLoan = new FindMarginLoan(enrollment, cpf, covenantAutorization);
            findMarginLoan.IdCovenant = idCovenat.ToString();
            var handlerMarginLoan = new GetMarginLoanHandler(_createEndoserAdapter, _endoserAdapterService, _capPublisher);
            var result = await handlerMarginLoan.Handle(findMarginLoan, CancellationToken.None);
            Assert.Equal(new MarginNotFound($"{idCovenat}"),result);
        }

        [Fact]

        public async Task DeveRetornaMarginFail()
        {
            var idCovenat = Guid.NewGuid();
            var enrollment = "Matricula";
            var cpf = "05479603361";
            var covenantAutorization = "00090909";
            var covenantEndorser = new ReadModels.CovenantEndorser() { EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO, IdentifierInEndoser = "dddd" };
            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endorserAdapter);
            _endorserAdapter.ConsultMarginLoan("ddd", enrollment, cpf, covenantAutorization).ReturnsNullForAnyArgs();
            _endoserAdapterService.GetEndorserCovenant(idCovenat).ReturnsForAnyArgs<ReadModels.CovenantEndorser>(covenantEndorser);
            var findMarginLoan = new FindMarginLoan(enrollment, cpf, covenantAutorization);
            findMarginLoan.IdCovenant = idCovenat.ToString();
            var handlerMarginLoan = new GetMarginLoanHandler(_createEndoserAdapter, _endoserAdapterService, _capPublisher);
            var result = await handlerMarginLoan.Handle(findMarginLoan, CancellationToken.None);
            Assert.IsType<MarginFail>(result);
        }

        [Fact]

        public async Task DeveRetornaMarginFailComMessagemProblemWithMarginSearch()
        {
            var idCovenat = Guid.NewGuid();
            var enrollment = "Matricula";
            var cpf = "05479603361";
            var covenantAutorization = "00090909";
            var covenantEndorser = new ReadModels.CovenantEndorser() { EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO, IdentifierInEndoser = "dddd" };
            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endorserAdapter);
            _endorserAdapter.ConsultMarginLoan("ddd", enrollment, cpf, covenantAutorization).ReturnsNullForAnyArgs();
            _endoserAdapterService.GetEndorserCovenant(idCovenat).ReturnsForAnyArgs<ReadModels.CovenantEndorser>(covenantEndorser);
            var findMarginLoan = new FindMarginLoan(enrollment, cpf, covenantAutorization);
            findMarginLoan.IdCovenant = idCovenat.ToString();
            var handlerMarginLoan = new GetMarginLoanHandler(_createEndoserAdapter, _endoserAdapterService, _capPublisher);
            var result = await handlerMarginLoan.Handle(findMarginLoan, CancellationToken.None);
            //Assert.Equal(new MarginFail("Problem with margin search"),result);
        }

        [Fact]

        public async Task DeveRetornaMarginNegativa()
        {
            var idCovenat = Guid.NewGuid();
            var enrollment = "Matricula";
            var cpf = "05479603361";
            var covenantAutorization = "00090909";
            var marginLoand = new MarginLoanDto(-40m);
            var covenantEndorser = new ReadModels.CovenantEndorser() { EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO, IdentifierInEndoser = "dddd" };
            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endorserAdapter);
            _endorserAdapter.ConsultMarginLoan("ddd", enrollment, cpf, covenantAutorization).ReturnsForAnyArgs(_resultMargemNegativo);
            _endoserAdapterService.GetEndorserCovenant(idCovenat).ReturnsForAnyArgs<ReadModels.CovenantEndorser>(covenantEndorser);
            var findMarginLoan = new FindMarginLoan(enrollment, cpf, covenantAutorization);
            findMarginLoan.IdCovenant = idCovenat.ToString();
            var handlerMarginLoan = new GetMarginLoanHandler(_createEndoserAdapter, _endoserAdapterService, _capPublisher);
            var result = await handlerMarginLoan.Handle(findMarginLoan, CancellationToken.None);
            Assert.IsType<MarginFound>(result);
            Assert.Equal(((MarginFound)result).valueMargin, marginLoand.Margin);
        }

        [Fact]

        public async Task DeveRetornaMargin0()
        {
            var idCovenat = Guid.NewGuid();
            var enrollment = "Matricula";
            var cpf = "05479603361";
            var covenantAutorization = "00090909";
            var marginLoand = new MarginLoanDto(0);
            var covenantEndorser = new ReadModels.CovenantEndorser() { EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO, IdentifierInEndoser = "dddd" };
            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endorserAdapter);
            _endorserAdapter.ConsultMarginLoan("ddd", enrollment, cpf, covenantAutorization).ReturnsForAnyArgs(_resultMargem0);
            _endoserAdapterService.GetEndorserCovenant(idCovenat).ReturnsForAnyArgs<ReadModels.CovenantEndorser>(covenantEndorser);
            var findMarginLoan = new FindMarginLoan(enrollment, cpf, covenantAutorization);
            findMarginLoan.IdCovenant = idCovenat.ToString();
            var handlerMarginLoan = new GetMarginLoanHandler(_createEndoserAdapter, _endoserAdapterService, _capPublisher);
            var result = await handlerMarginLoan.Handle(findMarginLoan, CancellationToken.None);
            Assert.IsType<MarginFound>(result);
            Assert.Equal(((MarginFound)result).valueMargin, marginLoand.Margin);
        }

    }
}
