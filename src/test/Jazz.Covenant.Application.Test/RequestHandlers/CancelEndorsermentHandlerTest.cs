
using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Base;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Enums;
using Xunit.Abstractions;

namespace Jazz.Covenant.Application.Test.RequestHandlers;
public class CancelEndorsermentHandlerTest : TestBase
{
    private readonly ICapPublisher _capPublisher;
    private readonly ICovenantRepository _covenantRepository;
    private readonly IEndoserAdapterService _endoserAdapterService;
    private readonly IUnitOfWork _unitOfWork;

    public CancelEndorsermentHandlerTest(ITestOutputHelper output)
        : base(output)
    {
        _capPublisher = Substitute.For<ICapPublisher>();
        _covenantRepository = Substitute.For<ICovenantRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _endoserAdapterService = Substitute.For<IEndoserAdapterService>();
    }

    [Fact]
    public async Task DeveCancelarAverbacao()
    {
       
        // Arrange
        var idCovenant = Guid.NewGuid();
        var statusHistorico = StatusEndosament.SUCCESS;
        var cpf = "05479603361";
        var historico = new MarginEndosamentStatusHistory(statusHistorico, idCovenant, "Sucess");
        var endoserment = new CancelEndorsermentCommand()
        {
            Enrollment = "0000ddd",
            TaxId = cpf,
            Value = 10m,
            EndorsementNumber = "dddd",
            IdCovenant = idCovenant.ToString()
        };
        _covenantRepository.GetByIdCovenantCpfStatus(idCovenant, cpf, statusHistorico).Returns(historico);
        var handler = new CancelEndorsermentHandler(_covenantRepository,_unitOfWork,_capPublisher);
        var result = await handler.Handle(endoserment,CancellationToken.None);
        Assert.IsType<CancelEndosermentSuccess>(result);
    }
    [Fact]
    public async Task DeveNAcharContratoCancelarAverbacao()
    {
       
        // Arrange
        var idCovenant = Guid.NewGuid();
        var statusHistorico = StatusEndosament.SUCCESS;
        var cpf = "05479603361";
        var historico = new MarginEndosamentStatusHistory(statusHistorico, idCovenant, "Sucess");
        var endoserment = new CancelEndorsermentCommand()
        {
            Enrollment = "0000ddd",
            TaxId = cpf,
            Value = 10m,
            EndorsementNumber = "dddd",
            IdCovenant = idCovenant.ToString()
        };
        var handler = new CancelEndorsermentHandler(_covenantRepository,_unitOfWork,_capPublisher);
        var result = await handler.Handle(endoserment,CancellationToken.None);
        Assert.IsType<CancelEndosermentNotFound>(result);
    }
    [Fact]
    public async Task DeveErroCancelarAverbacao()
    {
       
        // Arrange
        var idCovenant = Guid.NewGuid();
        var statusHistorico = StatusEndosament.SUCCESS;
        var cpf = "05479603361";
        var historico = new MarginEndosamentStatusHistory(statusHistorico, idCovenant, "Sucess");
        var endoserment = new CancelEndorsermentCommand()
        {
            Enrollment = "0000ddd",
            TaxId = cpf,
            Value = 10m,
            EndorsementNumber = "dddd",
            IdCovenant = idCovenant.ToString()
        };
        var handler = new CancelEndorsermentHandler(_covenantRepository,_unitOfWork,_capPublisher);
        var result = await handler.Handle(null,CancellationToken.None);
        Assert.IsType<CancelEndosermentFail>(result);
    }
}