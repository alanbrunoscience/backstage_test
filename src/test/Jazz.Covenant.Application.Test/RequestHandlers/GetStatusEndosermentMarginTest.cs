using Apps72.Dev.Data.DbMocker;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Base;
using Xunit.Abstractions;

namespace Jazz.Covenant.Application.Test.RequestHandlers;

public class GetStatusEndosermentMarginTest : TestBase
{
    public GetStatusEndosermentMarginTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task DeveEncontrarStatusDeAverbacao()
    {
        var idEndorsementMargin = Guid.NewGuid();
        var findStatusEndorsementMargin = new FindStatusEndorsementMargin(idEndorsementMargin.ToString());
        var conn = new MockDbConnection();
        conn.Mocks.When(cmd =>
                cmd.CommandText.Contains(@"SELECT StatusEndosament FROM [dbo].[MarginEndosamentStatusHistory]"))
            .ReturnsTable(MockTable.WithColumns("StatusEndosament").AddRow(3));
        PrintPayload(findStatusEndorsementMargin);
        var handler = new GetStatusEndosermentMarginHandler(conn);
        var result = await handler.Handle(findStatusEndorsementMargin, CancellationToken.None);
        PrintResultJson(result);
        Assert.IsType<StatusEndosermentMarginSucess>(result);
    }

    [Fact]
    public async Task DeveNaoEncontraStatusAverbacao()
    {
        var idEndorsementMargin = Guid.NewGuid();
        var findStatusEndorsementMargin = new FindStatusEndorsementMargin(idEndorsementMargin.ToString());
        var conn = new MockDbConnection();
        conn.Mocks.When(cmd =>
                cmd.CommandText.Contains(@"SELECT StatusEndosament FROM [dbo].[MarginEndosamentStatusHistory]"))
            .ReturnsTable(MockTable.Empty());
        PrintPayload(findStatusEndorsementMargin);
        var handler = new GetStatusEndosermentMarginHandler(conn);
        var result = await handler.Handle(findStatusEndorsementMargin, CancellationToken.None);
        PrintResultJson(result);
        Assert.IsType<StatusEndosermentMarginNotFound>(result);
    }

    [Fact]
    public async Task DeveDaProblemaNaBusca()
    {
        var idEndorsementMargin = Guid.NewGuid();
        var conn = new MockDbConnection();
        conn.Mocks.When(cmd =>
                cmd.CommandText.Contains(@"SELECT StatusEndosament FROM [dbo].[MarginEndosamentStatusHistory]"))
            .ReturnsTable(MockTable.Empty());
        var handler = new GetStatusEndosermentMarginHandler(conn);
        var result = await handler.Handle(null, CancellationToken.None);
        PrintResultJson(result);
        Assert.IsType<StatusEndosermentMarginFail>(result);
    }
}