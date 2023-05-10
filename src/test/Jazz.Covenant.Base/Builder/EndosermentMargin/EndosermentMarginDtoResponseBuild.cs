using Jazz.Covenant.Domain.Dto.Adapters;

namespace Jazz.Covenant.Base.Builder.EndosermentMargin;

public class EndosermentMarginDtoResponseBuild
{
    private readonly EndosamentMarginDtoResponse _endosamentMarginDtoResponse;

    public EndosermentMarginDtoResponseBuild()
    {
        _endosamentMarginDtoResponse = new EndosamentMarginDtoResponse
        {
            ErrorMessage = "ErrorMessage_test",
            Retriable = true,
            Success = true,
            GenericResponse = "djmsdkjdsj"
        };
    }

    public EndosermentMarginDtoResponseBuild WithErrorMessage(string erroMessage)
    {
        _endosamentMarginDtoResponse.ErrorMessage = erroMessage;
        return this;
    }

    public EndosamentMarginDtoResponse Build()
    {
        return _endosamentMarginDtoResponse;
    }
}