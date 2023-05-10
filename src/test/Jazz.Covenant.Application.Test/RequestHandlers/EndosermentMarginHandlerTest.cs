using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Base;
using Jazz.Covenant.Base.Builder.EndosermentMargin;
using Jazz.Covenant.Domain.Enums;
using NSubstitute.ReturnsExtensions;
using Xunit.Abstractions;

namespace Jazz.Covenant.Application.Test.RequestHandlers;

public class EndosermentMarginHandlerTest : TestBase
{
    private readonly ICapPublisher _capPublisher;
    private readonly ICovenantRepository _covenantRepository;
    private readonly IEndoserAdapterService _endoserAdapterService;
    private readonly IUnitOfWork _unitOfWork;

    public EndosermentMarginHandlerTest(ITestOutputHelper output)
        : base(output)
    {
        _capPublisher = Substitute.For<ICapPublisher>();
        _covenantRepository = Substitute.For<ICovenantRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _endoserAdapterService = Substitute.For<IEndoserAdapterService>();
    }

    [Fact]
    public async Task DeveRealizarAverbacaoDeMargem()
    {
        // Arrange 
        var idCovenant = Guid.NewGuid();
        var covenantEndoser = new ReadModels.CovenantEndorser
            {IdentifierInEndoser = "98", EndoserIdentifier = EndoserAggregator.BPO};
        var endorserment = new EndosermentMarginPayloadBuilder().Build();
        _endoserAdapterService.GetEndorserCovenant(idCovenant).Returns(covenantEndoser);
        var endorsementMarginCommand = new EndorsementMarginCommand(idCovenant.ToString(), endorserment);
        PrintPayload(endorsementMarginCommand);
        var handler =
            new EndorsementMarginHandler(_unitOfWork, _endoserAdapterService, _covenantRepository, _capPublisher);
        //Act
        var result = await handler.Handle(endorsementMarginCommand, CancellationToken.None);
        PrintResultJson(result);
        // Assert
        Assert.IsType<EndorsementMarginSucess>(result);
    }

    [Fact]
    public async Task DeveNaoEncontraIdCovenant()
    {
        // Arrange 
        var idCovenant = Guid.NewGuid();
        var covenantEndoser = new ReadModels.CovenantEndorser
            {IdentifierInEndoser = "98", EndoserIdentifier = EndoserAggregator.BPO};
        var endorserment = new EndosermentMarginPayloadBuilder().Build();
        _endoserAdapterService.GetEndorserCovenant(idCovenant).ReturnsNull();
        var endorsementMarginCommand = new EndorsementMarginCommand(idCovenant.ToString(), endorserment);
        PrintPayload(endorsementMarginCommand);
        var handler =
            new EndorsementMarginHandler(_unitOfWork, _endoserAdapterService, _covenantRepository, _capPublisher);
        //Act
        var result = await handler.Handle(endorsementMarginCommand, CancellationToken.None);
        PrintResultJson(result);
        // Assert
        Assert.IsType<EndorsementMarginNotFound>(result);
        Assert.Equal(new EndorsementMarginNotFound(idCovenant.ToString()), result);
    }

    [Fact]
    public async Task DeveFalharAoTentarAverbar()
    {
        // Arrange 
        var idCovenant = Guid.NewGuid();
        var covenantEndoser = new ReadModels.CovenantEndorser
            {IdentifierInEndoser = "98", EndoserIdentifier = EndoserAggregator.BPO};
        var endorserment = new EndosermentMarginPayloadBuilder().Build();
        _endoserAdapterService.GetEndorserCovenant(idCovenant).Returns(covenantEndoser);
        var endorsementMarginCommand = new EndorsementMarginCommand(idCovenant.ToString(), endorserment);
        PrintPayload(endorsementMarginCommand);
        var handler =
            new EndorsementMarginHandler(_unitOfWork, _endoserAdapterService, _covenantRepository, _capPublisher);
        //Act
        var result = await handler.Handle(null, CancellationToken.None);
        PrintResultJson(result);
        // Assert
        Assert.IsType<EndorsementMarginFail>(result);
    }
}