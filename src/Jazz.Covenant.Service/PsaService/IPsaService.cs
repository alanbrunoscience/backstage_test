using Jazz.Covenant.Service.PsaService.Dto.ConsultMargin;
using Jazz.Covenant.Service.PsaService.Dto.ConsultEmployee;
using Jazz.Covenant.Service.PsaService.Dto.ReservationConfirmation;
using Jazz.Covenant.Service.PsaService.Dto.MarginReserve;
using Jazz.Covenant.Service.PsaService.Dto.Contract;
using Jazz.Covenant.Service.PsaService.Dto.DiscountCode;

namespace Jazz.Covenant.Service.PsaService
{
    public interface IPsaService
    {
        Task<IEnumerable<Employee>> ConsultEmployeeByCpf(int idCovenant, string cpf);
        Task<IEnumerable<Employee>> ConsultEmployeeByEnrollment(int idCovenant, string enrollment);
        Task<IEnumerable<Margin>> ConsultMargin(int idCovenant, string enrollment);
        Task<ResponseMarginReserve> MarginReserve(int idCovenant, RequestMarginReserve requestMarginReserve);
        Task<ResponseReservationConfirmation> ReservationConfirmation(int idCovenant, RequestReservationConfirmation requestReservationConfirmation);
        Task<IEnumerable<Contract>> ListContract(int idCovenant, string enrollment);
        Task<ResponseSettleContract>SettleContract(int idCovenant, RequestSettleContract requestSettleContract);
        Task<IEnumerable< ResponseDiscountCode>> GetDiscountCode(int idCovenant, string enrollment, int modality);
    }
}
