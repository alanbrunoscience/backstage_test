using Apps72.Dev.Data.DbMocker;
using Jazz.Core;
using Jazz.Covenant.Application.Data.CovenantQuerysTemplates;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.Filter;
using Jazz.Covenant.Application.RequestHandlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Test.RequestHandlers
{
    public class CreateFavoriteCovenantHandlerTest
    {
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IFilterManager _filterManager;

        public CreateFavoriteCovenantHandlerTest()
        {
            _repository = Substitute.For<ICovenantRepository>();
            _uow = Substitute.For<IUnitOfWork>();
          
        }

        [Fact]

        public async Task DeveCadastraCovenantFavorite()
        {
            var conn = new MockDbConnection();
            conn.Mocks.When(cmd => cmd.CommandText.Contains("COUNT(DISTINCT\tCovenants.Id)"))
                    .ReturnsTable(MockTable.WithColumns("Count").AddRow(1));
            conn.Mocks.When(cmd => cmd.CommandText.Contains("SELECT Id FROM"))
                 .ReturnsTable(MockTable.Empty());
            var idCovenant = Guid.NewGuid();
            var favorite = true;
            var cpf = "05479603361";
            var teste = new CreateFavoriteCovenantCommand(favorite, cpf);
            teste.IdCovenant = idCovenant.ToString();
            var handler = new CreateFavoriteCovenantHandler(_repository, _uow, conn);
            var resultado = await handler.Handle(teste, CancellationToken.None);
            Assert.IsType<CreateFavoriteCovenantSucess>(resultado);

        }
        [Fact]

        public async Task DeveAtualizarCovenantFavorite()
        {
            var conn = new MockDbConnection();
            conn.Mocks.When(cmd => cmd.CommandText.Contains("COUNT(DISTINCT\tCovenants.Id)"))
                    .ReturnsTable(MockTable.WithColumns("Count").AddRow(1));
            conn.Mocks.When(cmd => cmd.CommandText.Contains("SELECT Id FROM"))
                 .ReturnsTable(MockTable.WithColumns("Id").AddRow(Guid.NewGuid()));
            var idCovenant = Guid.NewGuid();
            var favorite = true;
            var cpf = "05479603361";
            var teste = new CreateFavoriteCovenantCommand(favorite, cpf);
            teste.IdCovenant = idCovenant.ToString();
            var handler = new CreateFavoriteCovenantHandler(_repository, _uow, conn);
            var resultado = await handler.Handle(teste, CancellationToken.None);
            Assert.IsType<CreateFavoriteCovenantSucess>(resultado);

        }
        [Fact]
        public async Task DeveErroComIdCovenantNaoExisteCovenantFavorite()
        {
            var conn = new MockDbConnection();
            conn.Mocks.When(cmd => cmd.CommandText.Contains("COUNT(DISTINCT\tCovenants.Id)"))
                    .ReturnsTable(MockTable.WithColumns("Count").AddRow(0));
            conn.Mocks.When(cmd => cmd.CommandText.Contains("SELECT Id FROM"))
                 .ReturnsTable(MockTable.WithColumns("Id").AddRow(Guid.NewGuid()));
            var idCovenant = Guid.NewGuid();
            var favorite = true;
            var cpf = "05479603361";
            var teste = new CreateFavoriteCovenantCommand(favorite, cpf);
            teste.IdCovenant = idCovenant.ToString();
            var handler = new CreateFavoriteCovenantHandler(_repository, _uow, conn);
            var resultado = await handler.Handle(teste, CancellationToken.None);
            Assert.IsType<CreateFavoriteCovenantFail>(resultado);
            Assert.Equal(new CreateFavoriteCovenantFail($"Not exist convenant with {idCovenant}"), resultado);
        }
        [Fact]

        public async Task DeveErroCovenantFavorite()
        {
            var conn = new MockDbConnection();
        
            conn.Mocks.When(cmd => cmd.CommandText.Contains("SELECT Id FROM"))
                 .ReturnsTable(MockTable.WithColumns("Id").AddRow(Guid.NewGuid()));
            var idCovenant = Guid.NewGuid();
            var favorite = true;
            var cpf = "05479603361";
            var teste = new CreateFavoriteCovenantCommand(favorite, cpf);
            teste.IdCovenant = idCovenant.ToString();
            var handler = new CreateFavoriteCovenantHandler(_repository, _uow, conn);
            var resultado = await handler.Handle(teste, CancellationToken.None);
            Assert.IsType<CreateFavoriteCovenantFail>(resultado);
            Assert.Equal(new CreateFavoriteCovenantFail("Error creating covenant favorite."), resultado);
        }
    }
}
