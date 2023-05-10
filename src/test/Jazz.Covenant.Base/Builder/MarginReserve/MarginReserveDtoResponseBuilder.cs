using Jazz.Covenant.Domain.Dto.Adapters;

namespace Jazz.Covenant.Base.Builder.MarginReserve
{
    public class MarginReserveDtoResponseBuilder
    {        
        private MarginReserveDtoResponse _marginReserveDtoResponse;

        public MarginReserveDtoResponseBuilder()
        {
            _marginReserveDtoResponse = new MarginReserveDtoResponse
            {
                Success = true,
                Retriable = false,
                ErrorMessage = "ErrorMessage_test",
                GenericResponse = ""
            };
        }

        public MarginReserveDtoResponseBuilder WithSuccess(bool success)
        {
            _marginReserveDtoResponse.Success = success;
            return this;
        }

        public MarginReserveDtoResponse Build()
        {
            return _marginReserveDtoResponse;
        }
    }
}
