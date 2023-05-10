using Dapper;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.Data.CovenantQuerysTemplates;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.Filter;
using Jazz.Covenant.Application.Filter.CovenantFavoriteFilter;
using Jazz.Covenant.Application.Filter.CovenantFilter;
using Jazz.Covenant.Application.Validation;
using Jazz.Covenant.Domain;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jazz.Covenant.Application.RequestHandlers
{
    public class CreateFavoriteCovenantHandler : ICommandHandler<CreateFavoriteCovenantCommand, CreateFavoriteCovenantResult>
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<CreateFavoriteCovenantHandler>();
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IDbConnection _connection;
        public CreateFavoriteCovenantHandler(ICovenantRepository repository, IUnitOfWork uow, IDbConnection connection)
        {
            _repository = repository;
            _uow = uow;
            _connection = connection;
         
        }

        public async Task<CreateFavoriteCovenantResult> Handle(CreateFavoriteCovenantCommand request, CancellationToken cancellationToken)
        {
            Log.Debug("Received {@Command}", request);
            try
            {
                var builder = new SqlBuilder().Where($"Covenants.Id ='{request.IdCovenant}'"); 
                var counter = builder.AddTemplate(CovenantQuery.COUNT_SELECT_COVENANT_MODALITY);
                var count = await _connection.ExecuteScalarAsync<int>(counter.RawSql, counter.Parameters).ConfigureAwait(false);
                if(count==0)
                    return CreateFavoriteCovenantResult.Fail($"Not exist convenant with {request.IdCovenant}");
                var builderConvenantFavorite = new SqlBuilder().Where($"CovenantId='{request.IdCovenant}'").Where($"TaxId='{request.TaxId}'");
                var convenantFavoriteBuilder = builderConvenantFavorite.AddTemplate(CovenantQuery.SELECT_CONVENANT_FAVORITE);
                var covenantFavorite = await _connection.QueryFirstOrDefaultAsync<Domain.CovenantFavorite>(convenantFavoriteBuilder.RawSql);
                if(covenantFavorite is not null)
                {
                    covenantFavorite.CovenantId = request.IdCovenant;
                    covenantFavorite.TaxId = request.TaxId;
                    covenantFavorite.Favorite = request.Favorite.Value;
                    await _repository.UpdateConvenantFavorite(covenantFavorite);
                    await _uow.CommitAsync().ConfigureAwait(false);
                    return CreateFavoriteCovenantResult.Sucess();
                    
                }
                var insertCovenantFavorite = new CovenantFavorite()
                {
                    CovenantId = Guid.Parse(request.IdCovenant),
                    Favorite = request.Favorite.Value,
                    TaxId = request.TaxId

                };
                await _repository.RegisterCovenantFavorite(insertCovenantFavorite);
                await _uow.CommitAsync().ConfigureAwait(false);
                return CreateFavoriteCovenantResult.Sucess();
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error processing command {@request}", request);
                return CreateFavoriteCovenantResult.Fail("Error creating covenant favorite.");
            }
        }
    }
    public record CreateFavoriteCovenantCommand(bool? Favorite,Cpf TaxId) : ICommand<CreateFavoriteCovenantResult>
    {
        public string IdCovenant { get; set; } 
    } ;
   
    public record CreateFavoriteCovenantSucess() : CreateFavoriteCovenantResult();
    public record CreateFavoriteCovenantFail(string errors) : CreateFavoriteCovenantResult();

    public record CreateFavoriteCovenantResult()
    {
        public static CreateFavoriteCovenantResult Sucess() => new CreateFavoriteCovenantSucess();

        public static CreateFavoriteCovenantResult Fail(string errors) => new CreateFavoriteCovenantFail(errors);
    };
    public class CreateFavoriteCovenantValidation : AbstractValidator<CreateFavoriteCovenantCommand>
    {
        public CreateFavoriteCovenantValidation()
        {

            RuleFor(x => x.IdCovenant).SetValidator(GuidValidate.GetValidations());
            RuleFor(cmd => cmd.TaxId)
                 .SetValidator(Cpf.GetValidator())
                .WithName(nameof(CreateFavoriteCovenantCommand.TaxId))
                .OverridePropertyName(nameof(CreateFavoriteCovenantCommand.TaxId));
            RuleFor(cmd => cmd.Favorite).NotNull();

        }
    }
}
