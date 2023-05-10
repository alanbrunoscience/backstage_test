
using System.Net;
using FluentValidation.Results;

namespace Jazz.Core
{
    public record ApiResultAgregator
    {
        private List<dynamic> Results { get; set; } = new List<dynamic>();

        public IEnumerable<DomainMessage> Erros
        {
            get
            {
                return Results.SelectMany(x => ((IApiResult)x).Errors);
            }
        }
        public IDictionary<string, string[]> ErrorsDictionary
        {
            get
            {
                var ret = Results.SelectMany(x => ((IApiResult)x).Errors);

                var result = ret.GroupBy(x => x.Code)
                    .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.Message).ToArray()
                );

                return result;
            }
        }
        public bool Success
        {
            get => !Results.Select(x => ((IApiResult)x).Success)
                           .Any(x => x == false);
        }
        public ApiResultAgregator AddApiResult(IApiResult result)
        {
            Results.Add(result);
            return this;
        }
        public ApiResultAgregator AddApiResult(params IApiResult[] results)
        {
            Results.AddRange(results);
            return this;
        }
    }
    public interface IApiResult
    {
        bool Success { get; }
        string ApiName { get; }
        HttpStatusCode Status { get; }
        IEnumerable<DomainMessage> Errors { get; }
    }

    public interface IApiResult<T> : IApiResult
    {
        T Result { get; }
        IApiResult<T> SetResult(HttpStatusCode status, T result = default, params string[] errors);
        IApiResult<T> SetIntegrationError(T result, params string[] errors);
    }

    public record ApiResult<T> : IApiResult<T>
    {
        public T Result { get; private set; }
        public string ApiName { get; private set; }
        public HttpStatusCode Status { get; private set; }
        public IEnumerable<DomainMessage> Errors { get; private set; }
        public bool Success { get => Errors is null ? true : !Errors.Any(); }

        public ApiResult(T result, string apiName, HttpStatusCode status)
        {
            Result = result;
            ApiName = apiName;
            Status = status;
        }

        public ApiResult(T result, string apiName, HttpStatusCode status, params string[] errors)
        {
            Result = result;
            ApiName = apiName;
            Status = status;
            Errors = ErrorMessageBuilder.Build(errors);
        }

        public ApiResult(T result, string apiName, HttpStatusCode status, IEnumerable<DomainMessage> errors)
        {
            Result = result;
            ApiName = apiName;
            Status = status;
            Errors = errors;
        }

        public IApiResult<T> SetResult(HttpStatusCode status, T result = default, params string[] errors)
        {
            Status = status;
            Errors = ErrorMessageBuilder.Build(errors) ?? new DomainMessage[] { };
            Result = result;

            return this;
        }

        public IApiResult<T> SetIntegrationError(T result = default, params string[] errors)
        {
            return SetResult(HttpStatusCode.UnprocessableEntity, result, errors);
        }
    }
    public record DefaultApiResult;
    public record ErrorResult;
    public abstract record ToApiResult<T>
    {
        public static IApiResult<T> Success(string apiName, T result = default) => new ApiResult<T>(result, apiName, HttpStatusCode.OK, new string[] { });
        public static IApiResult<T> Fail(string apiName, T result = default, params string[] errors) => new ApiResult<T>(result, apiName, HttpStatusCode.UnprocessableEntity, errors);
    }

    public record DomainMessage
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public DomainMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public static class ValidationExtensions
    {
        public static string GetErrorsInLine(this ValidationResult that)
        {
            return that == null ? null : string.Join(Environment.NewLine, that.Errors.Select(x => $"{x.ErrorCode} - {x.ErrorMessage}"));
        }

        public static IEnumerable<DomainMessage> GetErrorMessages(this ValidationResult that)
        {
            return ErrorMessageBuilder.Build((that.Errors ?? new List<ValidationFailure> { }).Select(x => x.ErrorMessage).ToArray());
        }
    }

    public static class ErrorMessageBuilder
    {
        public static IEnumerable<DomainMessage> Build(params string[] messages)
        {
            foreach (var message in messages)
            {
                var messageStructure = message.Split('|').ToList();

                if (messageStructure.Count < 2) messageStructure.Insert(0, "00.00");

                yield return new DomainMessage(code: messageStructure[0], message: messageStructure[1]);
            }
        }
    }
}
