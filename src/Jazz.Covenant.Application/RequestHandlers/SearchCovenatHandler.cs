using Dapper;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.Data.CovenantQuerysTemplates;
using Jazz.Covenant.Application.Filter;
using Jazz.Covenant.Application.Filter.CovenantFilter;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.RequestHandlers
{
    public class SearchCovenatHandler: IQueryHandler<SearchCovenantQuery, SearchCovenantResult>
    {
        private readonly IFilterManager _filterManager;
        private static readonly ILogger Log = Serilog.Log.ForContext<SearchCovenatHandler>();
        private readonly IDbConnection _connection;

        public SearchCovenatHandler(IFilterManager filterManager, IDbConnection connection)
        {
            _filterManager = filterManager;
            _connection = connection;
        }
        public async Task<SearchCovenantResult> Handle(SearchCovenantQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var builder = ApplySearchCovenantFilters(request,_filterManager);
                var counter = builder.AddTemplate(CovenantQuery.COUNT_SELECT_COVENANT_MODALITY);
                var selector = builder.AddTemplate(CovenantQuery.SELECT_COVENANTS_MODALITYS);
                var count = await _connection.ExecuteScalarAsync<int>(counter.RawSql, counter.Parameters).ConfigureAwait(false);
                var items = await _connection.QueryAsync<ReadModels.Covenant, ReadModels.Modality, ReadModels.Covenant>(selector.RawSql, (c, m) =>
                {
                    c.Modality.Add(m.ModalityName);
                    return c;
                }, splitOn:"ModalityId", param: selector.Parameters) ;
                items = items.GroupBy(i => i.Id).Select(i =>
                {
                    var groupItems = i.First();
                    groupItems.Modality = i.Select(i => i.Modality.First()).ToList();
                    return groupItems;
                });
       
                return SearchCovenantResult.Success(items, request.Skip, request.Size, count);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing query {@Query}", request);
                return SearchCovenantResult.Fail("Error searching covenant.");
            }
        }
        private static SqlBuilder ApplySearchCovenantFilters(SearchCovenantQuery request, IFilterManager filterManager)
        {
            var builder = filterManager
                          .AddFilter(new CovenantByNameFilter(request.NameCovenant))
                          .AddFilter(new CovenantByModalityNameFilter(request.NameModality))
                          .AddFilter(new CovenantByOrganizationPublicFilter(request.OrganizationPublic))
                          .AddFilter(new CovenatByActive(request.Status))
                          .Execute();
            builder = builder.OrderBy("Covenants.Name").AddParameters(new { request.Skip, request.Size });
            return builder;
        }

    }
    public record SearchCovenantQuery(string? NameCovenant, string? NameModality,string? OrganizationPublic ,int Size=10, int Skip=0, bool?Status = true) : PagedQuery(Skip, Size), IQuery<SearchCovenantResult>;
    public record SearchCovenantSuccess : SearchCovenantResult, IPagedResult<ReadModels.Covenant>
    {
        private readonly PagedResult<ReadModels.Covenant> _pagedResult;

        public SearchCovenantSuccess(IEnumerable<ReadModels.Covenant> items, int skip, int size, int totalCount)
        {
            _pagedResult = new PagedResult<ReadModels.Covenant>(items, skip, size, totalCount);
        }

        public IEnumerable<ReadModels.Covenant> Items => _pagedResult.Items;
        public int Skip => _pagedResult.Skip;
        public int Size => _pagedResult.Size;
        public int TotalCount => _pagedResult.TotalCount;
        public int ItemsCount => _pagedResult.ItemsCount;
        public int PageCount => _pagedResult.PageCount;
        public int CurrentPage => _pagedResult.CurrentPage;
    }
    public record SearchCovenantFail(string Errors) : SearchCovenantResult;

    public record SearchCovenantResult
    {
        public static SearchCovenantResult Success(IEnumerable<ReadModels.Covenant> items, int skip, int size, int totalCount) =>
            new SearchCovenantSuccess(items, skip, size, totalCount);

        public static SearchCovenantResult Fail(string errors) => new SearchCovenantFail(errors);
    };


    public class SearchCovenantQueryValidator : AbstractValidator<SearchCovenantQuery>
    {
        public SearchCovenantQueryValidator()
        {
            RuleFor(s => s.Size).NotEmpty();
        }
    }

}
