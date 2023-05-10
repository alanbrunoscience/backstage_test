using AutoMapper;
using Jazz.Common;
using Jazz.Covenant.Domain.Dto.Adapters;

namespace Jazz.Covenant.Application.Mappers;

public static class DomainModelMappers
{
    static DomainModelMappers()
    {
        var config = new MapperConfiguration(
            cfg => 
            {
                cfg.CreateMap<string, NormalizedString>().ConvertUsing(s => NormalizedString.From(s));
                cfg.CreateMap<Covenant.Domain.Covenant, CovenantDto>();
            });

        Mapper = config.CreateMapper();
    }

    private static IMapper Mapper { get; }
}