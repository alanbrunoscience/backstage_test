using Jazz.Covenant.Domain.Dto.Adapters;

namespace Jazz.Covenant.Base.Builder.MarginReserve
{
    public class CancelMarginReserveDtoResponseBuilder
    {        
        private CancelMarginReserveDtoResponse _marginReserveDtoResponse;

        public CancelMarginReserveDtoResponseBuilder()
        {
            _marginReserveDtoResponse = new CancelMarginReserveDtoResponse
            {
                Success = true,
                Retriable = false,
                ErrorMessage = "ErrorMessage_test",
                GenericResponse = ""
            };
        }

        public CancelMarginReserveDtoResponseBuilder WithSuccess(bool success)
        {
            _marginReserveDtoResponse.Success = success;
            return this;
        }

        public CancelMarginReserveDtoResponse Build()
        {
            return _marginReserveDtoResponse;
        }
    }
}
