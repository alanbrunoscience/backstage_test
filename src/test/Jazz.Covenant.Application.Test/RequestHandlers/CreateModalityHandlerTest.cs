using Apps72.Dev.Data.DbMocker;
using Confluent.Kafka;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.RequestHandlers;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Test.RequestHandlers
{
    public  class CreateModalityHandlerTest
    {
        private readonly ICovenantRepository _covenantRepository;
        private readonly IUnitOfWork _unitOfWork;
        

        public CreateModalityHandlerTest()
        {
            _covenantRepository = Substitute.For<ICovenantRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
        }

        [Fact]

        public async void DeveCadastrarModalidade()
        {
            var conn = new MockDbConnection();
            conn.Mocks.When(cmd => cmd.CommandText.Contains("Count(IdentificationModality"))
                      .ReturnsTable(MockTable.WithColumns("Count").AddRow(0));
            var name = "TesteModalidade";
            var identificationModality = 1;
            var createModality = new CreateComandModality(name, identificationModality);
            var createModalityHandler = new CreateModalityHandler(_covenantRepository, _unitOfWork, conn);
            var response = await createModalityHandler.Handle(createModality, CancellationToken.None);
            Assert.IsType<CreateModalitySucess>(response);
        }
        [Fact]

        public async void DeveFalharCadastrarModalidade()
        {
            var conn = new MockDbConnection();
            var name = String.Empty;
            int identificationModality = 0;
            _covenantRepository.RegisterModality(new Domain.Modality()).ReturnsNullForAnyArgs();
            var createModality = new CreateComandModality(name, identificationModality);
            var createModalityHandler = new CreateModalityHandler(_covenantRepository, _unitOfWork, conn);
            var response = await createModalityHandler.Handle(createModality, CancellationToken.None);
            Assert.IsType<CreateModalityFail>(response);
        }

        [Fact]

        public async void DeveNaoCadastraPorqueHaIdentifyModalityRepetida()
        {
            var conn = new MockDbConnection();
            conn.Mocks.When(cmd => cmd.CommandText.Contains("Count(IdentificationModality"))
                      .ReturnsTable(MockTable.WithColumns("Count").AddRow(1));
            var name = "TesteModalidade";
            var identificationModality = 1;
            var createModality = new CreateComandModality(name, identificationModality);
            var createModalityHandler = new CreateModalityHandler(_covenantRepository, _unitOfWork, conn);
            var response = await createModalityHandler.Handle(createModality, CancellationToken.None);
            Assert.Equal(new CreateModalityFail("This value already exists in the base, it must be unique IdentificationModality"), response);
            Assert.IsType<CreateModalityFail>(response);
        }
    }
}
