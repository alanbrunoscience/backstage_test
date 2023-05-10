using System.Data;
using Dapper;
using DotNetCore.CAP;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Core;
using Jazz.Covenant.Application.Data.CovenantQuerysTemplates;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.Validation;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.CommandEvent;
using Jazz.Covenant.Domain.Enums;
using Serilog;

namespace Jazz.Covenant.Application.RequestHandlers
{
    public partial class CreateCovenantHandler : ICommandHandler<
        CreateCovenantCommand,
        CreateCovenantResult>
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<CreateCovenantHandler>();
        private readonly IDbConnection _connection;
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICapPublisher _publisher;

        public CreateCovenantHandler(
            ICovenantRepository repository,
            IUnitOfWork uow,
            IDbConnection connection, ICapPublisher publisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _connection = connection;
            _publisher = publisher;
        }


        public async Task<CreateCovenantResult> Handle(CreateCovenantCommand request,
            CancellationToken cancellationToken)
        {
            Log.Debug("Received {@Command}", request);
            try
            {
                var covenantId = CovenantId.Create();
                List<string> listNotExistModality = new List<string>();
                request.Modalities.ForEach(x =>
                {
                    SqlBuilder builder = new SqlBuilder().Where("Modality.Id = @Id", new {Id = Guid.Parse(x)});
                    var selector = builder.AddTemplate(CovenantQuery.COUNT_SELECT_MODALITY_ID);
                    var modality = _connection.ExecuteScalar<int>(selector.RawSql, selector.Parameters);
                    if (modality == 0) listNotExistModality.Add(x);
                });
                if (listNotExistModality.Count > 0)
                    return CreateCovenantResult.Fail("Not exist Modality " +
                                                     string.Join(',', listNotExistModality.Select(e => e).ToArray()));
                var modalityCovenant = request.Modalities.Select(e => new ModalityCovenant()
                    {
                        ModalityId = Guid.Parse(e),
                        CovenantId = covenantId
                    }
                ).ToList();
                SqlBuilder builder = new SqlBuilder().Where("Id = @Id", new {Id = Guid.Parse(request.endoserId)});
                var selector = builder.AddTemplate(CovenantQuery.COUNT_SELECT_ENDOSER_ID);
                var endorserCount = _connection.ExecuteScalar<int>(selector.RawSql, selector.Parameters);
                if (endorserCount == 0)
                    return CreateCovenantResult.Fail("Not exist endoserId " + request.endoserId);
                var covenant = new Domain.Covenant(
                    covenantId,
                    request.name,
                    request.organization,
                    request.level,
                    Guid.Parse(request.endoserId),
                    request.identifierInEndoser,
                    modalityCovenant
                );
                await _repository.SaveAsync(covenant, cancellationToken).ConfigureAwait(false);
                _publisher.PublishAsync("covenant.covenantRegistered",
                  new Events.CovenantRegistered(covenant.Id, covenant.Name));
                await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                return CreateCovenantResult.Success(covenant.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing command {@request}", request);
                return CreateCovenantResult.Fail("Error creating covenant.");
            }
        }
    }

    public record CreateCovenantCommand(
        string name,
        string organization,
        CovenantLevel level,
        string endoserId,
        string identifierInEndoser,
        List<string> Modalities
    ) : ICommand<CreateCovenantResult>;

    public record CreateCovenantSuccess(Guid CompanyId) : CreateCovenantResult;

    public record CreateCovenantFail(string[] Errors) : CreateCovenantResult
    {
        public override string ToString() => string.Join(Environment.NewLine, Errors);
    }

    public abstract record CreateCovenantResult
    {
        public static CreateCovenantResult Success(Guid companyId) => new CreateCovenantSuccess(companyId);

        public static CreateCovenantResult Fail(params string[] errors) => new CreateCovenantFail(errors);
    }

    public class CreateCovenantValidation : AbstractValidator<CreateCovenantCommand>
    {
        public CreateCovenantValidation()
        {
            RuleFor(cmd => cmd.level).IsInEnum();
            RuleFor(cmd => cmd.endoserId).SetValidator(GuidValidate.GetValidations());
            RuleFor(x => x.level).NotEmpty();
            RuleFor(x => x.name).NotEmpty();
            RuleFor(x => x.organization).NotEmpty();
            RuleFor(x => x.Modalities).NotEmpty();
            RuleFor(x => x.Modalities).Custom((e, context) =>
            {
                var index = 0;
                e.ForEach((e) =>
                    {
                        if (!Guid.TryParse(e, out var id))
                        {
                            index++;
                            context.AddFailure($"{index} {e}", "Invalid Guid");
                        }
                    }
                );
            });
        }
    }
}
