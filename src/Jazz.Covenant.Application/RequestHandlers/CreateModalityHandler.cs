using Dapper;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.Data.CovenantQuerysTemplates;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Domain;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.RequestHandlers
{
    public class CreateModalityHandler : ICommandHandler<CreateComandModality, CreateModalityResult>
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<CreateComandModality>();
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IDbConnection _connection;


        public CreateModalityHandler(ICovenantRepository repository, IUnitOfWork uow, IDbConnection connection)
        {
            _repository = repository;
            _uow = uow;
            _connection = connection;
        }

        public async Task<CreateModalityResult> Handle(CreateComandModality request, CancellationToken cancellationToken)
        {
            Log.Debug("Received {@Command}", request);
            try
            {
                SqlBuilder builder = new SqlBuilder().Where("IdentificationModality = @IdentificationModality", new { IdentificationModality = request.identificationModality });
                var count = builder.AddTemplate(CovenantQuery.COUNT_SELECT_MODALITY);
                var countModality = await _connection.QuerySingleOrDefaultAsync<int>(count.RawSql, count.Parameters).ConfigureAwait(false);
                if (countModality > 0)
                {
                    return CreateModalityResult.Fail("This value already exists in the base, it must be unique IdentificationModality");
                }
                var modality = new Modality()
                {
                    IdentificationModality = request.identificationModality,
                    Name = request.name,
                };
                await _repository.RegisterModality(modality);
                await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);

                return CreateModalityResult.Sucess();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing command {@request}", request);
                return CreateModalityResult.Fail("Problem with create modality");
            }
        }
    }
    public record CreateComandModality(string name, int identificationModality) : ICommand<CreateModalityResult>;

    public record CreateModalitySucess() : CreateModalityResult();
    public record CreateModalityFail(string errors) : CreateModalityResult();

    public record CreateModalityResult()
    {
        public static CreateModalityResult Sucess() => new CreateModalitySucess();

        public static CreateModalityResult Fail(string errors) => new CreateModalityFail(errors);
    };

    public class CreateModalityValidation : AbstractValidator<CreateComandModality>
    {
        public CreateModalityValidation()
        {
            RuleFor(x => x.identificationModality).NotEmpty();
            RuleFor(x => x.name).NotEmpty();
        }
    }
}
