// using System.Text.Json;
// using Jazz.Core;
//
// namespace Jazz.Customers.Application.Configuration;
//
// public static class AuditTrailExtensions
// {
//     public static IRepository<TId, TAggregate> AddAudit<TId, TAggregate>(this IRepository<TId, TAggregate> repository)
//         where TId : IEquatable<TId>
//         where TAggregate : class, IAggregateRoot<TId>
//     {
//         if (repository == null) throw new ArgumentNullException(nameof(repository));
//         repository.AggregateRootSaved +=
//             (_, args) =>
//             {
//                 // TODO: Fazer com que o Aggregate implemente uma interface IAuditable e usar atributos para serializar somente o que interessa???
//                 var json = JsonSerializer.Serialize(args.AggregateRoot);
//                 var type = args.AggregateRoot.GetType();
//                 Serilog.Log
//                        .ForContext("AuditTrail", "state-transition")
//                        .ForContext("Aggregate", new { Id = id.GetString(), Type = type })
//                        .Debug(json);
//                 return Task.CompletedTask;
//             };
//
//         return repository;
//     }
// }